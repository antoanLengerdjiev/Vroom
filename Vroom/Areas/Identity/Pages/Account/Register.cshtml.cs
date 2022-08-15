using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Vroom.Data.Models;
using Vroom.Helpers;
using Vroom.Providers.Contracts;

namespace Vroom.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    //[Authorize(Roles = Roles.Admin)]
    public class RegisterModel : PageModel
    {
        private readonly IApplicationSignInManager<ApplicationUser> _signInManager;
        private readonly IApplicationUserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;
        private readonly IApplicationRoleManager<IdentityRole> roleManager;
        private readonly IEncodingProvider encodingProvider;
        private readonly IWebEncodersProvider webEncodersProvider;

        public RegisterModel(
            IApplicationUserManager<ApplicationUser> userManager,
            IApplicationSignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            //IEmailSender emailSender,
            IApplicationRoleManager<IdentityRole> roleManager,
            IEncodingProvider encodingProvider,
            IWebEncodersProvider webEncodersProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
            this.roleManager = roleManager;
            this.webEncodersProvider = webEncodersProvider;
            this.encodingProvider = encodingProvider;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required]
            [Display(Name = "User Name")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }


            [Required]
            [Phone]
            [Display(Name = "Office Phone Number")]
            public string PhoneNumber2 { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public bool IsAdmin { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Username, Email = Input.Email, PhoneNumber = Input.PhoneNumber, PhoneNumber2 = Input.PhoneNumber2 };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    //Create Roles if didnt exist
                    if (!await this.roleManager.RoleExistsAsync("Admin"))
                    {
                        await this.roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    if (!await this.roleManager.RoleExistsAsync("Executive"))
                    {
                        await this.roleManager.CreateAsync(new IdentityRole("Executive"));
                    }
                    await this._userManager.AddToRoleAsync(user, "Executive");
                    // Asign user to a role as per the checbox is selection
                    //if (Input.IsAdmin)
                    //{
                    //    await this._userManager.AddToRoleAsync(user, "Admin");
                    //}
                    //else
                    //{
                    //    
                    //}

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = this.webEncodersProvider.Base64UrlEncode(this.encodingProvider.UTF8GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { username = Input.Username, email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        //return LocalRedirect(returnUrl);
                        return this.RedirectToAction("Index");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
