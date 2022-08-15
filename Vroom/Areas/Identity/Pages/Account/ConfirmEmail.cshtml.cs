using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Vroom.Data.Models;
using Vroom.Providers.Contracts;

namespace Vroom.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly IApplicationUserManager<ApplicationUser> _userManager;
        private readonly IEncodingProvider encodingProvider;
        private readonly IWebEncodersProvider webEncodersProvider;

        public ConfirmEmailModel(IApplicationUserManager<ApplicationUser> userManager, IEncodingProvider encodingProvider, IWebEncodersProvider webEncodersProvider)
        {
            _userManager = userManager;
            this.webEncodersProvider = webEncodersProvider;
            this.encodingProvider = encodingProvider;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = this.encodingProvider.UTF8GetString(this.webEncodersProvider.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}
