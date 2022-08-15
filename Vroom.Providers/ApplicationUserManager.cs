using Bytes2you.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Providers.Contracts;

namespace Vroom.Providers
{
    public class ApplicationUserManager<TUser> : IApplicationUserManager<TUser> where TUser: IdentityUser
    {
        private readonly UserManager<TUser> userManager;
        public ApplicationUserManager(UserManager<TUser> userManager)
        {
            Guard.WhenArgument<UserManager<TUser>>(userManager, GlobalConstants.UserManagerNullExceptionMessege).IsNull().Throw();
            this.userManager = userManager;
        }

        public IdentityOptions Options { get => this.userManager.Options; set => this.userManager.Options = value; }

        public async Task<IdentityResult> AddLoginAsync(TUser user, UserLoginInfo login)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<UserLoginInfo>(login, GlobalConstants.GetMemberName(() => login)).IsNull().Throw();

            return await this.userManager.AddLoginAsync(user, login);
        }

        public async Task<IdentityResult> AddToRoleAsync(TUser user, string role)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<string>(role, GlobalConstants.GetMemberName(() => role)).IsNull().Throw();

            return await this.userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> ChangeEmailAsync(TUser user, string newEmail, string token)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<string>(newEmail, GlobalConstants.GetMemberName(() => newEmail)).IsNullOrEmpty().Throw();
            Guard.WhenArgument<string>(token, GlobalConstants.GetMemberName(() => token)).IsNullOrEmpty().Throw();

            return await this.userManager.ChangeEmailAsync(user, newEmail, token);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(TUser user, string token)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<string>(token, GlobalConstants.GetMemberName(() => token)).IsNullOrEmpty().Throw();

            return await this.userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<string>(password, GlobalConstants.GetMemberName(() => password)).IsNullOrEmpty().Throw();

            return await this.userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> CreateAsync(TUser user)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();

            return await this.userManager.CreateAsync(user);
        }

        public async Task<TUser> FindByEmailAsync(string email)
        {
            Guard.WhenArgument<string>(email, GlobalConstants.GetMemberName(() => email)).IsNullOrEmpty().Throw();

            return await this.userManager.FindByEmailAsync(email);
        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            Guard.WhenArgument<string>(userId, GlobalConstants.UserIdNullExceptionMessage).IsNullOrEmpty().Throw();
            return await this.userManager.FindByIdAsync(userId);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(TUser user)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();

            return await this.userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(TUser user)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();

            return await this.userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string> GetUserIdAsync(TUser user)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();

            return await this.userManager.GetUserIdAsync(user);
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName)
        {
            Guard.WhenArgument<string>(roleName, GlobalConstants.GetMemberName(() => roleName)).IsNullOrEmpty().Throw();

            return await this.userManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<bool> IsEmailConfirmedAsync(TUser user)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();

            return await this.userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<string>(role, GlobalConstants.GetMemberName(() => role)).IsNullOrEmpty().Throw();

            return await this.userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<string>(token, GlobalConstants.GetMemberName(() => token)).IsNullOrEmpty().Throw();
            Guard.WhenArgument<string>(newPassword, GlobalConstants.GetMemberName(() => newPassword)).IsNullOrEmpty().Throw();

            return await this.userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> SetUserNameAsync(TUser user, string userName)
        {
            Guard.WhenArgument<TUser>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<string>(userName, GlobalConstants.GetMemberName(() => userName)).IsNullOrEmpty().Throw();

            return await this.userManager.SetUserNameAsync(user, userName);
        }
    }
}
