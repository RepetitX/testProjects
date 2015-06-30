using IShop.Services;
using Autofac;
using Common.Web.Mvc.Services;

namespace IShop.Infrastructure.Autofac
{
    public class ModuleRegisterServices : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(FilterableBaseService<,,,,>)).As(typeof(IFilterableBaseService<,,,,>)).InstancePerRequest();

            builder.RegisterGeneric(typeof(BaseService<,,,>)).As(typeof(IBaseService<,,,>)).InstancePerRequest();

            builder.RegisterType<AdminUsersService>().As<IAdminUsersService>().InstancePerRequest();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerRequest();
        }
    }
}