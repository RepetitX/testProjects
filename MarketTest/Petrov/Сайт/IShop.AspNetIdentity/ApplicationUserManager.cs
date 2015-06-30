using IShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace IShop.AspNetIdentity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IdentityFactoryOptions<ApplicationUserManager> options)
            : base(store)
        {
            UserValidator = new UserValidator<ApplicationUser>(this)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = false
                };

            PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false
                };

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
                UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        }
    }
}