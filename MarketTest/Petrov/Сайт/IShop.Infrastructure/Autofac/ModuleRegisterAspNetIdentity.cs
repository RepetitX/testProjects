using System.Web.Mvc;
using IShop.AspNetIdentity;
using IShop.DataAccess.Context;
using IShop.Models;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Common.Web.Mvc.AspNetIdentity;

namespace IShop.Infrastructure.Autofac
{
    public class ModuleRegisterAspNetIdentity : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new UserStore<ApplicationUser>(DependencyResolver.Current.GetService<IShopContext>())).AsImplementedInterfaces().InstancePerRequest();
            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager> { DataProtectionProvider = new DpapiDataProtectionProvider("IShop") }).AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.Register(c => new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(DependencyResolver.Current.GetService<IShopContext>()))).AsSelf().InstancePerRequest();

            builder.RegisterGeneric(typeof(AspNetIdentity<,,,>)).As(typeof(IAspNetIdentity<,,,>)).InstancePerRequest();
        }
    }
}