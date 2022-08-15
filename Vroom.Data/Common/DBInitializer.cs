using Bytes2you.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vroom.Common;
using Vroom.Data.Contracts;
using Vroom.Data.Models;

namespace Vroom.Data.Common
{
    public class DBInitializer : IDBInitializer
    {
        private readonly VroomDbContext vroomDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DBInitializer(VroomDbContext vroomDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            Guard.WhenArgument<VroomDbContext>(vroomDbContext, GlobalConstants.DbContextNullExceptionMessege)
                 .IsNull()
                 .Throw();

            Guard.WhenArgument<UserManager<ApplicationUser>>(userManager, GlobalConstants.UserManagerNullExceptionMessege)
                 .IsNull()
                 .Throw();

            Guard.WhenArgument<RoleManager<IdentityRole>>(roleManager, GlobalConstants.RoleManagerNullExceptionMessege)
                 .IsNull()
                 .Throw();

            this.vroomDbContext = vroomDbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public void Initialize()
        {
            if(vroomDbContext.Database.GetPendingMigrations().Count() > 0)
            {
                vroomDbContext.Database.Migrate();
            }
            
            if(vroomDbContext.Roles.Any(x => x.Name == "Admin"))
            {
                return;
            }

            roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();

            userManager.CreateAsync(new ApplicationUser { UserName = "Admin", Email = "Admin@abv.bg", EmailConfirmed = true, }, "Admin@123").GetAwaiter().GetResult();

            userManager.AddToRoleAsync(userManager.FindByNameAsync("Admin").GetAwaiter().GetResult(), "Admin").GetAwaiter().GetResult();

        }
    }
}
