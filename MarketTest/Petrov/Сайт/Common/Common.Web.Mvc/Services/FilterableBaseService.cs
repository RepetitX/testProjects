using System;
using System.Linq;
using System.Security.Principal;
using Common.Repository;
using Common.Web.Mvc.Controls;

namespace Common.Web.Mvc.Services
{
    public class FilterableBaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions, TEntityFilter> : BaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions>, IFilterableBaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions, TEntityFilter>
        where TEntity : class, new()
        where TEntityGridOptions : GridOptions
        where TEntityFilter : Filter<TEntity>, new()
        where TEntityGrid : class, IGridModel<TEntity>
    {
        protected readonly IFilterService _filterService;

        public IFilterService FilterService
        {
            get { return _filterService; }
        }

        public FilterableBaseService(Lazy<IRepository<TEntity, TKey>> repository)
            : this(repository, null)
        {
        }

        public FilterableBaseService(Lazy<IRepository<TEntity, TKey>> repository, IFilterService filterService)
            : base(repository)
        {
            _filterService = filterService;
        }

        public override ActionGrid<TEntity, TEntityGrid> GetActionGrid(TKey[] keys, TEntityGridOptions options, IPrincipal principal)
        {
            var filter = new TEntityFilter().Configure(_filterService, principal).Init(options);
            var query = filter.Apply(GetQuery(principal).Where(_repository.Value.ExpPrimaryKeyContainsIn(keys)));

            return new ActionGrid<TEntity, TEntityGrid>(query, options, false, false).WithFilter(filter);
        }

        public override ActionGrid<TEntity, TEntityGrid> GetActionGrid(TEntityGridOptions options, IPrincipal principal)
        {
            var filter = new TEntityFilter().Configure(_filterService, principal).Init(options);
            var query = filter.Apply(GetQuery(principal));

            return new ActionGrid<TEntity, TEntityGrid>(query, options, false, false).WithFilter(filter);
        }

        public virtual Filter<TEntity> GetFilter(TEntityGridOptions options, string gridKey, IPrincipal principal)
        {
            return GetFilterUser(options, gridKey, principal);
        }

        public virtual Filter<TEntity> GetFilterUser(TEntityGridOptions options, string gridKey, IPrincipal principal)
        {
            return new TEntityFilter().Configure(_filterService, principal).Init(options, gridKey);
        }
    }
}
