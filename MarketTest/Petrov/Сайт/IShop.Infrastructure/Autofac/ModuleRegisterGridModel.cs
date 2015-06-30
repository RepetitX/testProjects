using IShop.GridModels;
using Autofac;

namespace IShop.Infrastructure.Autofac
{
    public class ModuleRegisterGridModel : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AdminUserGrid).Assembly);
        }
    }
}