using Bytes2you.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Providers.Contracts;

namespace Vroom.Providers
{
    public class ApplicationSignInManager<TUser> : IApplicationSignInManager<TUser> where TUser: IdentityUser
    {
        private readonly SignInManager<TUser> _signInManager;
        public ApplicationSignInManager(SignInManager<TUser> _signInManager)
        {
            Guard.WhenArgument<SignInManager<TUser>>(_signInManager, GlobalConstants.SignInManagerNullExceptionMessege).IsNull().Throw();

            this._signInManager = _signInManager;
        }
        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null)
        {
            Guard.WhenArgument<string>(provider, GlobalConstants.GetMemberName(() => provider)).IsNullOrEmpty().Throw();
            Guard.WhenArgument<string>(redirectUrl, GlobalConstants.GetMemberName(() => redirectUrl)).IsNullOrEmpty().Throw();

            return this._signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userId);
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            Guard.WhenArgument<string>(loginProvider, GlobalConstants.GetMemberName(() => loginProvider)).IsNullOrEmpty().Throw();
            Guard.WhenArgument<string>(providerKey, GlobalConstants.GetMemberName(() => providerKey)).IsNullOrEmpty().Throw();


            return await this._signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return await this._signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
        {
            return await this._signInManager.GetExternalLoginInfoAsync(expectedXsrf);
        }

        public async Task<TUser> GetTwoFactorAuthenticationUserAsync()
        {
            return await this._signInManager.GetTwoFactorAuthenticationUserAsync();
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            Guard.WhenArgument<string>(userName, GlobalConstants.GetMemberName(() => userName)).IsNullOrEmpty().Throw();
            Guard.WhenArgument<string>(password, GlobalConstants.GetMemberName(() => password)).IsNullOrEmpty().Throw();

            return await this._signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }

        public async Task RefreshSignInAsync(TUser user)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();

            await this._signInManager.RefreshSignInAsync(user);
        }

        public async Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();

            await this._signInManager.SignInAsync(user, isPersistent, authenticationMethod);
        }

        public async Task SignOutAsync()
        {
            await this._signInManager.SignOutAsync();
        }

        public async Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient)
        {
            Guard.WhenArgument<string>(code, GlobalConstants.GetMemberName(() => code)).IsNullOrEmpty().Throw();

            return await this._signInManager.TwoFactorAuthenticatorSignInAsync(code, isPersistent, rememberClient);
        }

        public async Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode)
        {
            Guard.WhenArgument<string>(recoveryCode, GlobalConstants.GetMemberName(() => recoveryCode)).IsNullOrEmpty().Throw();

            return await this._signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
        }
    }
}
