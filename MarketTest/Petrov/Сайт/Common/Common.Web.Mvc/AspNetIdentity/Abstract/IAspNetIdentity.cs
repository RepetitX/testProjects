using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Common.Web.Mvc.AspNetIdentity
{
    public interface IAspNetIdentity<TDbContext, TIdentityUser, TUserManager, TRoleManager>
        where TDbContext : DbContext
        where TIdentityUser : IdentityUser
        where TUserManager : UserManager<TIdentityUser>
        where TRoleManager : RoleManager<IdentityRole>
    {
        IQueryable<TIdentityUser> GetUsersQuery();
        void SignIn(string userName, string password, bool isPersistent = false);
        Task SignInAsync(string userName, string password, bool isPersistent = false);
        void SignOut(params string[] authenticationTypes);
        void CreateUser(TIdentityUser user, string password);
        Task CreateUserAsync(TIdentityUser user, string password);
        TIdentityUser FindUser(string userName, string password);
        Task<TIdentityUser> FindUserAsync(string userName, string password);
        TIdentityUser FindUserById(string userId);
        Task<TIdentityUser> FindUserByIdAsync(string userId);
        TIdentityUser FindUserByName(string userName);
        Task<TIdentityUser> FindUserByNameAsync(string userName);
        void AddPassword(string userId, string password);
        Task AddPasswordAsync(string userId, string password);
        void ChangePassword(string userId, string currentPassword, string newPassword);
        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        string GeneratePasswordResetToken(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        void ResetPassword(string userId, string token, string newPassword);
        Task ResetPasswordAsync(string userId, string token, string newPassword);
        void ResetPassword(string userId, string newPassword);
        Task ResetPasswordAsync(string userId, string newPassword);
        void RemovePassword(string userId);
        Task RemovePasswordAsync(string userId);
        string GetEmail(string userId);
        Task<string> GetEmailAsync(string userId);
        void SetEmail(string userId, string email);
        Task SetEmailAsync(string userId, string email);
        void DeleteUser(string userId);
        Task DeleteUserAsync(string userId);
        void SendSms(string userId, string message);
        Task SendSmsAsync(string userId, string message);
        void SendEmail(string userId, string subject, string body);
        Task SendEmailAsync(string userId, string subject, string body);

        IQueryable<IdentityRole> GetRolesQuery();
        void CreateRole(string roleName);
        Task CreateRoleAsync(string roleName);
        bool RoleExists(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        void AddUserToRole(string userId, string roleName);
        Task AddUserToRoleAsync(string userId, string roleName);
        void RemoveUserFromRole(string userId, string roleName);
        Task RemoveUserFromRoleAsync(string userId, string roleName);
        IList<string> GetRolesForUser(string userId);
        Task<IList<string>> GetRolesForUserAsync(string userId);
        bool IsUserInRole(string userId, string roleName);
        Task<bool> IsUserInRoleAsync(string userId, string roleName);
    }
}