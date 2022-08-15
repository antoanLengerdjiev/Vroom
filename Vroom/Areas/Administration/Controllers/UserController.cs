using AutoMapper;
using Bytes2you.Validation;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vroom.Areas.Administration.Models;
using Vroom.Common;
using Vroom.Data.Common;
using Vroom.Data.Models;
using Vroom.Helpers;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Vroom.Service.Models;

namespace Vroom.Areas.Administration.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [Area("Administration")]
    public class UserController : Controller
    {
        private readonly IMapper mapper;
        private readonly IApplicationUserManager<ApplicationUser> userManager;
        private readonly IUserService userService;
        public UserController(IMapper mapper, IApplicationUserManager<ApplicationUser> userManager, IUserService userService)
        {
            Guard.WhenArgument<IUserService>(userService, GlobalConstants.UserServiceNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IApplicationUserManager<ApplicationUser>>(userManager, GlobalConstants.UserManagerNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IMapper>(mapper, GlobalConstants.MapperProviderNullExceptionMessege).IsNull().Throw();

            this.mapper = mapper;
            this.userManager = userManager;
            this.userService = userService;
        }
        public async Task<IActionResult> Index(string searchString,string sortOrder ,int pageNumber=1, int pageSize=2)
        {
            this.ViewBag.CurrentSearchString = searchString;
            this.ViewBag.CurrentSortOrder = sortOrder;
            this.ViewBag.SortOrderParam = string.IsNullOrEmpty(sortOrder) ? "order_dsc" : "";
            var skip = (pageNumber * pageSize) - pageSize;

            var admins = await this.userService.GetUsersInRoleAsync(Roles.Admin);
            var normalUsers = await this.userService.GetFilteredUsersInRoleAsync(Roles.Executive, pageNumber, pageSize,sortOrder,searchString);

            var adminsViewModels = this.mapper.Map<List<ApplicationUserServiceModel>, List<AppUserViewModel>>(admins.ToList());
            var normalUserViewModels = this.mapper.Map<List<ApplicationUserServiceModel>, List<AppUserViewModel>>(normalUsers.Users.ToList());

            var result = new PagedResult<IndexAppUsersViewModel>
            {
                Data = new List<IndexAppUsersViewModel> { new IndexAppUsersViewModel { Admins = adminsViewModels, Users = normalUserViewModels } },
                TotalItems = normalUsers.TotalSize,
                PageNumber = pageNumber,
                PageSize = pageSize
            };


            adminsViewModels.ForEach(x => x.IsAdmin = true);

            return View(result);
        }

        public async Task<IActionResult> Update(string id, bool isAdmin)
        {

            // var user = await this.vroomDbContext.Users.FindAsync(id);

            var user = await this.userService.GetByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            var currentRole = isAdmin ? Roles.Admin : Roles.Executive;
            var newRole = isAdmin ? Roles.Executive : Roles.Admin;
            await this.userService.RemoveFromRoleAsync(user, currentRole);
            await this.userService.AddToRoleAsync(user, newRole);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
