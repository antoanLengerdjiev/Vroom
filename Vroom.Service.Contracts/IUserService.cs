using System.Collections.Generic;
using System.Threading.Tasks;
using Vroom.Service.Models;

namespace Vroom.Service.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUserServiceModel>> GetUsersInRoleAsync(string admin);
        Task<PagedUserServiceModel> GetFilteredUsersInRoleAsync(string executive, int pageNumber, int pageSize, string sortOrder, string searchString);
        Task<ApplicationUserServiceModel> GetByIdAsync(string id);
        Task RemoveFromRoleAsync(ApplicationUserServiceModel user, string currentRole);
        Task AddToRoleAsync(ApplicationUserServiceModel user, string newRole);
    }
}
