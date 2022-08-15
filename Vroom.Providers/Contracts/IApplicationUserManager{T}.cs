using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vroom.Providers.Contracts
{
    public interface IApplicationUserManager<TUser> where TUser : IdentityUser
    {
        IdentityOptions Options { get; set; }
        Task<TUser> FindByIdAsync(string userId);

        Task<IdentityResult> CreateAsync(TUser user, string password);

        Task<IdentityResult> ConfirmEmailAsync(TUser user, string token);

        Task<IdentityResult> ChangeEmailAsync(TUser user, string newEmail, string token);

        Task<IdentityResult> SetUserNameAsync(TUser user, string userName);

        Task<IdentityResult> CreateAsync(TUser user);

        Task<IdentityResult> AddLoginAsync(TUser user, UserLoginInfo login);

        Task<string> GetUserIdAsync(TUser user);

        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);

        Task<TUser> FindByEmailAsync(string email);

        Task<bool> IsEmailConfirmedAsync(TUser user);

        Task<string> GeneratePasswordResetTokenAsync(TUser user);

        Task<IdentityResult> AddToRoleAsync(TUser user, string role);
        Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword);

        Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role);

        Task<IList<TUser>> GetUsersInRoleAsync(string roleName);
    }
}
