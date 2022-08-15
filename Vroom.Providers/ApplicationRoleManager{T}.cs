using Bytes2you.Validation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Providers.Contracts;

namespace Vroom.Providers
{
    public class ApplicationRoleManager<TRole> : IApplicationRoleManager<TRole> where TRole : IdentityRole
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public ApplicationRoleManager(RoleManager<IdentityRole> roleManager)
        {
            Guard.WhenArgument<RoleManager<IdentityRole>>(roleManager, GlobalConstants.RoleManagerNullExceptionMessege).IsNull().Throw();

            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateAsync(TRole role)
        {
            Guard.WhenArgument<TRole>(role, GlobalConstants.RoleNullExceptionMessege).IsNull().Throw();

            return await this.roleManager.CreateAsync(role);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            Guard.WhenArgument<string>(roleName, GlobalConstants.RoleNullOrEmptyStringExceptionMessege).IsNullOrEmpty().Throw();

            return await this.roleManager.RoleExistsAsync(roleName);
        }
    }
}
