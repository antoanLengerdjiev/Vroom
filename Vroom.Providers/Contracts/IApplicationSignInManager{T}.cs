

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vroom.Providers.Contracts
{
    public interface IApplicationSignInManager<TUser> where TUser : IdentityUser
    {
        Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null);
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();

        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);

        Task RefreshSignInAsync(TUser user);

        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null);


        Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null);

        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);

        Task<TUser> GetTwoFactorAuthenticationUserAsync();

        Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient);


        Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode);

        Task SignOutAsync();
    }
}
