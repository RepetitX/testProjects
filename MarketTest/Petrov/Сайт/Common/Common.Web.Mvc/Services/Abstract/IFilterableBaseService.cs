using System.Security.Principal;
using Common.Web.Mvc.Controls;

namespace Common.Web.Mvc.Services
{
    public interface IFilterableBaseService<TEntity, TKey, TEntityGrid, in TEntityGridOptions, TEntityFilter>
        where TEntity : class
        where TEntityGridOptions : GridOptions
    {
        IFilterService FilterService { get; }
        Filter<TEntity> GetFilter(TEntityGridOptions options, string gridKey, IPrincipal principal);
        Filter<TEntity> GetFilterUser(TEntityGridOptions options, string gridKey, IPrincipal principal);
    }
}
