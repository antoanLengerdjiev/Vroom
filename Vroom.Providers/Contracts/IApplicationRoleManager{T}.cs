using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Vroom.Providers.Contracts
{
    public interface IApplicationRoleManager<TRole> where TRole : IdentityRole
    {
        Task<bool> RoleExistsAsync(string roleName);

        Task<IdentityResult> CreateAsync(TRole role);
    }
}
