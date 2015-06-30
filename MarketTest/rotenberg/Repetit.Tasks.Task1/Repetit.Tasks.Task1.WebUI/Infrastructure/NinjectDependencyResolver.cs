using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Mvc;
using Ninject.Web.Common;
using Repetit.Tasks.Task1.Domain.Abstract;
using Repetit.Tasks.Task1.Domain.Concrete;

namespace Repetit.Tasks.Task1.WebUI.Infrastructure
{
    public class NinjectDependencyResolver :IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IOrdersRepository>().To<OrdersRepository>().InRequestScope().WithConstructorArgument("connectionString", WebConfigurationManager.ConnectionStrings["TestMarketDb"].ConnectionString);
        }
    }
}