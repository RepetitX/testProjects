using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Common.Web.Mvc.AspNetIdentity
{
    public class AspNetIdentity<TDbContext, TIdentityUser, TUserManager, TRoleManager> : IAspNetIdentity<TDbContext, TIdentityUser, TUserManager, TRoleManager>
        where TDbContext : DbContext
        where TIdentityUser : IdentityUser
        where TUserManager : UserManager<TIdentityUser>
        where TRoleManager : RoleManager<IdentityRole>
    {
        private readonly Lazy<TDbContext> _dbContext;
        private readonly TUserManager _userManager;
        private readonly TRoleManager _roleManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AspNetIdentity(Lazy<TDbContext> dbContext, TUserManager userManager, TRoleManager roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        }

        #region Users

        public IQueryable<TIdentityUser> GetUsersQuery()
        {
            return _dbContext.Value.Set<TIdentityUser>();
        }

        public void SignIn(string userName, string password, bool isPersistent = false)
        {
            var user = FindUser(userName, password);

            if (user == null)
                throw new AspNetIdentityException("Неправильный логин или пароль.");

            if (_userManager.GetLockoutEnabled(user.Id))
                throw new AspNetIdentityException("Ваш аккаунт заблокирован, обратитесь к администратору.");

            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }

        public async Task SignInAsync(string userName, string password, bool isPersistent = false)
        {
            var user = await FindUserAsync(userName, password);

            if (user == null)
                throw new AspNetIdentityException("Неправильный логин или пароль.");

            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            if (await _userManager.GetLockoutEnabledAsync(identity.GetUserId()))
                throw new AspNetIdentityException("Ваш аккаунт заблокирован, обратитесь к администратору.");

            _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }

        public void SignOut(params string[] authenticationTypes)
        {
            _authenticationManager.SignOut(authenticationTypes);
        }

        public void CreateUser(TIdentityUser user, string password)
        {
            var identityResult = _userManager.Create(user, password);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task CreateUserAsync(TIdentityUser user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public TIdentityUser FindUser(string userName, string password)
        {
            return _userManager.Find(userName, password);
        }

        public async Task<TIdentityUser> FindUserAsync(string userName, string password)
        {
            return await _userManager.FindAsync(userName, password);
        }

        public TIdentityUser FindUserById(string userId)
        {
            return _userManager.FindById(userId);
        }

        public async Task<TIdentityUser> FindUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public TIdentityUser FindUserByName(string userName)
        {
            return _userManager.FindByName(userName);
        }

        public async Task<TIdentityUser> FindUserByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public void AddPassword(string userId, string password)
        {
            var identityResult = _userManager.AddPassword(userId, password);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task AddPasswordAsync(string userId, string password)
        {
            var identityResult = await _userManager.AddPasswordAsync(userId, password);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public void ChangePassword(string userId, string currentPassword, string newPassword)
        {
            var identityResult = _userManager.ChangePassword(userId, currentPassword, newPassword);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var identityResult = await _userManager.ChangePasswordAsync(userId, currentPassword, newPassword);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public string GeneratePasswordResetToken(string userId)
        {
            return _userManager.GeneratePasswordResetToken(userId);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(userId);
        }

        public void ResetPassword(string userId, string token, string newPassword)
        {
            var identityResult = _userManager.ResetPassword(userId, token, newPassword);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var identityResult = await _userManager.ResetPasswordAsync(userId, token, newPassword);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public void ResetPassword(string userId, string newPassword)
        {
            var token = GeneratePasswordResetToken(userId);

            ResetPassword(userId, token, newPassword);
        }

        public async Task ResetPasswordAsync(string userId, string newPassword)
        {
            var token = await GeneratePasswordResetTokenAsync(userId);

            await ResetPasswordAsync(userId, token, newPassword);
        }

        public void RemovePassword(string userId)
        {
            var identityResult = _userManager.RemovePassword(userId);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task RemovePasswordAsync(string userId)
        {
            var identityResult = await _userManager.RemovePasswordAsync(userId);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public string GetEmail(string userId)
        {
            return _userManager.GetEmail(userId);
        }

        public async Task<string> GetEmailAsync(string userId)
        {
            return await _userManager.GetEmailAsync(userId);
        }

        public void SetEmail(string userId, string email)
        {
            var identityResult = _userManager.SetEmail(userId, email);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task SetEmailAsync(string userId, string email)
        {
            var identityResult = await _userManager.SetEmailAsync(userId, email);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public void DeleteUser(string userId)
        {
            var user = FindUserById(userId);

            var identityResult = _userManager.Delete(user);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await FindUserByIdAsync(userId);

            var identityResult = await _userManager.DeleteAsync(user);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public void SendSms(string userId, string message)
        {
            _userManager.SendSms(userId, message);
        }

        public async Task SendSmsAsync(string userId, string message)
        {
            await _userManager.SendSmsAsync(userId, message);
        }

        public void SendEmail(string userId, string subject, string body)
        {
            _userManager.SendEmail(userId, subject, body);
        }

        public async Task SendEmailAsync(string userId, string subject, string body)
        {
            await _userManager.SendEmailAsync(userId, subject, body);
        }

        #endregion

        #region Roles

        public IQueryable<IdentityRole> GetRolesQuery()
        {
            return _dbContext.Value.Set<IdentityRole>();
        }

        public void CreateRole(string roleName)
        {
            var identityResult = _roleManager.Create(new IdentityRole(roleName));

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task CreateRoleAsync(string roleName)
        {
            var identityResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public bool RoleExists(string roleName)
        {
            return _roleManager.RoleExists(roleName);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public void AddUserToRole(string userId, string roleName)
        {
            var identityResult = _userManager.AddToRole(userId, roleName);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task AddUserToRoleAsync(string userId, string roleName)
        {
            var identityResult = await _userManager.AddToRoleAsync(userId, roleName);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public void RemoveUserFromRole(string userId, string roleName)
        {
            var identityResult = _userManager.RemoveFromRole(userId, roleName);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public async Task RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var identityResult = await _userManager.RemoveFromRoleAsync(userId, roleName);

            if (!identityResult.Succeeded)
                throw new AspNetIdentityException(identityResult.Errors);
        }

        public IList<string> GetRolesForUser(string userId)
        {
            return _userManager.GetRoles(userId);
        }

        public async Task<IList<string>> GetRolesForUserAsync(string userId)
        {
            return await _userManager.GetRolesAsync(userId);
        }

        public bool IsUserInRole(string userId, string roleName)
        {
            return _userManager.IsInRole(userId, roleName);
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            return await _userManager.IsInRoleAsync(userId, roleName);
        }

        #endregion
    }
}