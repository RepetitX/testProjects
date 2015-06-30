using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Common.Web.Mvc.Controls;

namespace Common.Web.Mvc.Services
{
    public interface IBaseService<TEntity, in TKey, TEntityGrid, in TEntityGridOptions>
        where TEntity : class
        where TEntityGrid : class, IGridModel<TEntity>
    {
        IQueryable<TEntity> GetQuery(IPrincipal principal);
        IList<TEntity> GetList(IPrincipal principal);
        ActionGrid<TEntity, TEntityGrid> GetActionGrid(TKey[] keys, TEntityGridOptions options, IPrincipal principal);
        ActionGrid<TEntity, TEntityGrid> GetActionGrid(TEntityGridOptions options, IPrincipal principal);
        TEntity Get(TKey key);
        TEntity Create(IPrincipal principal);
        void Create(TEntity entity, IPrincipal principal);
        TEntity Edit(TKey key, IPrincipal principal);
        void Edit(TEntity entity, IPrincipal principal);
        TEntity BeforeGet(TEntity entity, IPrincipal principal);
        TEntity BeforeSave(TEntity entity, IPrincipal principal);
        void Delete(TKey key, IPrincipal principal);
        void Delete(TKey[] keys, IPrincipal principal);
        void Sort(TKey[] keys);
    }
}