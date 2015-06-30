using Core.Interfaces;
using Data;
using Ninject.Modules;

namespace AdminExample.DI
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Kernel
                .Bind<IProductRepository>()
                .To<EfProductRepository>()
                .InSingletonScope();

            Kernel
                .Bind<IManagerRepository>()
                .To<EfManagerRepository>()
                .InSingletonScope();

            Kernel
                .Bind<IOrderRepository>()
                .To<EfOrderRepository>()
                .InSingletonScope();

        }
    }
}