using IShop.Controllers;
using Autofac;
using Autofac.Integration.Mvc;
using IShop.Controllers.Areas.AZ.Controllers;

namespace IShop.Infrastructure.Autofac
{
    public class ModuleRegisterControllers : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(ProductTypeController).Assembly);
        }
    }
}