using IShop.DataAccess.Repository;
using Autofac;
using Common.Repository;


namespace IShop.Infrastructure.Autofac
{
    public class ModuleRegisterRepository : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(IShopRepository<,>)).As(typeof(IRepository<,>)).InstancePerRequest();
        }
    }
}