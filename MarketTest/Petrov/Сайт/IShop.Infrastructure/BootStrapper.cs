using System.Web.Helpers;
using System.Web.Mvc;
using IShop.Infrastructure.Autofac;
using Autofac.Integration.Mvc;
using Common.Web.Mvc;
using Common.Web.Mvc.Autofac;
using Common.Web.Mvc.Controls;
using Module = Autofac.Module;

namespace IShop.Infrastructure
{
    public static class BootStrapper
    {
        public static void ConfigureApplication()
        {
            SetupDependencyInjection();

            ModelBinders.Binders.DefaultBinder = new SmartBinder(new GridOptionsModelBinder());

            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }

        private static void SetupDependencyInjection()
        {
            Module[] modules =
                {
                    new ModuleRegisterContext(),
                    new ModuleRegisterRepository(),
                    new ModuleRegisterAspNetIdentity(),
                    new ModuleRegisterGridModel(),
                    //new ModuleRegisterFilter(), // Сохранение фильтров в админке
                    new ModuleRegisterServices(),
                    new ModuleRegisterControllers()
                };

            var container = new AutofacModulesResolver(modules).Container;

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}