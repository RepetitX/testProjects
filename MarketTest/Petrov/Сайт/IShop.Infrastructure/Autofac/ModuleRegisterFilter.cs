using IShop.DataAccess.Repository;
using Autofac;
using Common.Repository;
using Common.Web.Mvc.Controls.Filter;
using Common.Web.Mvc.Services;

namespace IShop.Infrastructure.Autofac
{
    public class ModuleRegisterFilter : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IShopRepository<SavedFilter, int>>().As<IRepository<SavedFilter, int>>().InstancePerRequest();
            builder.RegisterType<FilterService>().As<IFilterService>().InstancePerRequest();
        }
    }
}