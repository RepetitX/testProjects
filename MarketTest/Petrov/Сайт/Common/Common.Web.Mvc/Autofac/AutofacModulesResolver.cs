using Autofac;
using Autofac.Integration.Mvc;

namespace Common.Web.Mvc.Autofac
{
    public class AutofacModulesResolver
    {
        private readonly ContainerBuilder _containerBuilder;

        public IContainer Container { get { return _containerBuilder.Build(); } }

        public AutofacModulesResolver(params Module[] modules)
        {
            _containerBuilder = new ContainerBuilder();
            _containerBuilder.RegisterModule(new AutofacWebTypesModule()); // Register web abstractions like HttpContextBase.
            _containerBuilder.RegisterSource(new ViewRegistrationSource()); // Enable property injection in view pages.
            //_containerBuilder.RegisterFilterProvider(); // Enable property injection into action filters.

            foreach (var module in modules)
                _containerBuilder.RegisterModule(module);
        }
    }
}
