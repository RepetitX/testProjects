using System;
using System.Web.Configuration;
using IShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartupAttribute(typeof(IShop.AspNetIdentity.Startup))]
namespace IShop.AspNetIdentity
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var loginPathSettings = WebConfigurationManager.AppSettings["LoginPath"];

            app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString(loginPathSettings ?? "/AZ/Security/SignIn"),
                    Provider = new CookieAuthenticationProvider
                        {
                            OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                                TimeSpan.FromMinutes(30),
                                (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie)),
                        }
                });
        }
    }
}