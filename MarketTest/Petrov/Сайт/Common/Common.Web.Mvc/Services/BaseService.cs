using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Common.Repository;
using Common.Web.Mvc.Controls;

namespace Common.Web.Mvc.Services
{
    public class BaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions> : IBaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions>
        where TEntity : class, new()
        where TEntityGrid : class, IGridModel<TEntity>
        where TEntityGridOptions : IGridOptions
    {
        protected readonly Lazy<IRepository<TEntity, TKey>> _repository;

        public BaseService(Lazy<IRepository<TEntity, TKey>> repository)
        {
            _repository = repository;
        }

        public virtual IQueryable<TEntity> GetQuery(IPrincipal principal)
        {
            return _repository.Value.GetQuery();
        }

        public virtual IList<TEntity> GetList(IPrincipal principal)
        {
            return _repository.Value.GetQuery().ToList();
        }

        public virtual ActionGrid<TEntity, TEntityGrid> GetActionGrid(TKey[] keys, TEntityGridOptions options, IPrincipal principal)
        {
            var query = GetQuery(principal).Where(_repository.Value.ExpPrimaryKeyContainsIn(keys));

            return new ActionGrid<TEntity, TEntityGrid>(query, options, false, false);
        }

        public virtual ActionGrid<TEntity, TEntityGrid> GetActionGrid(TEntityGridOptions options, IPrincipal principal)
        {
            var query = GetQuery(principal);

            return new ActionGrid<TEntity, TEntityGrid>(query, options, false, false);
        }

        public virtual TEntity Get(TKey key)
        {
            return _repository.Value.Single(key);
        }

        public virtual TEntity Create(IPrincipal principal)
        {
            return BeforeGet(new TEntity(), principal);
        }

        public virtual void Create(TEntity entity, IPrincipal principal)
        {
            _repository.Value.Add(BeforeSave(entity, principal));
            _repository.Value.SaveChanges();
        }

        public virtual TEntity Edit(TKey key, IPrincipal principal)
        {
            return BeforeGet(Get(key), principal);
        }

        public virtual void Edit(TEntity entity, IPrincipal principal)
        {
            _repository.Value.Update(BeforeSave(entity, principal));
            _repository.Value.SaveChanges();
        }

        public virtual TEntity BeforeGet(TEntity entity, IPrincipal principal)
        {
            return entity;
        }

        public virtual TEntity BeforeSave(TEntity entity, IPrincipal principal)
        {
            return entity;
        }

        public virtual void Delete(TKey key, IPrincipal principal)
        {
            Delete(new[] { key }, principal);
        }

        public virtual void Delete(TKey[] keys, IPrincipal principal)
        {
            _repository.Value.Delete(_repository.Value.ExpPrimaryKeyContainsIn(keys));
        }

        public virtual void Sort(TKey[] keys)
        {
            foreach (var key in keys)
            {
                var sortOrder = Array.IndexOf(keys, key);

                _repository.Value.Update(_repository.Value.ExpPrimaryKeyEquals(key), _repository.Value.ExpUpdateSortOrder(sortOrder));
            }
        }
    }
}