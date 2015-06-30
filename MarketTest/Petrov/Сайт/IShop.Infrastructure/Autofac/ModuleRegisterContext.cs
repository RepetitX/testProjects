using IShop.DataAccess.Context;
using Autofac;

namespace IShop.Infrastructure.Autofac
{
    public class ModuleRegisterContext : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IShopContext>().AsSelf().InstancePerRequest();
        }
    }
}