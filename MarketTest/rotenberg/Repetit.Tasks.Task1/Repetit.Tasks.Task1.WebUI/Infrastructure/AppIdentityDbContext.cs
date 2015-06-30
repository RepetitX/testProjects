using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repetit.Tasks.Task1.WebUI.Models;

namespace Repetit.Tasks.Task1.WebUI.Infrastructure
{
    public class AppIdentityDbContext :IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext()
            : base("TestMarketDb")
        {
        }

        static AppIdentityDbContext()
        {
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }
    }

    public class IdentityDbInit :  DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            InitialSetup(context);
            base.Seed(context);
        }

        private void InitialSetup(AppIdentityDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));
            string roleName = "Administrators";
            string userName = "admin";
            string password = "1234567";
            string email = "admin@example.com";
            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new AppRole(roleName));
            }
            AppUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(new AppUser { UserName = userName, Email = email, PhoneNumber = "89267222630", City = "Москва" },
                password);
                user = userMgr.FindByName(userName);
                if (!userMgr.IsInRole(user.Id, roleName))
                {
                    userMgr.AddToRole(user.Id, roleName);
                }
            }
        }
    }
}