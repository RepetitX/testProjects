using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Common.Repository;

namespace Common.Web.Mvc.Autofac
{
    public static class AutofacLifetimeScope
    {
        public static IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class
        {
            var lifetimeScope = DependencyResolver.Current.GetService<ILifetimeScope>();

            if (lifetimeScope != null)
            {
                var repository = lifetimeScope.Resolve<IRepository<TEntity, TKey>>();

                if (repository != null)
                    return repository;
            }

            return null;
        }

        public static TInstance GetInstance<TInstance>(params Parameter[] parameters)
            where TInstance : class
        {
            var lifetimeScope = DependencyResolver.Current.GetService<ILifetimeScope>();

            if (lifetimeScope != null)
            {
                var instance = lifetimeScope.Resolve<TInstance>(parameters);

                if (instance != null)
                    return instance;
            }

            return null;
        }
    }
}
