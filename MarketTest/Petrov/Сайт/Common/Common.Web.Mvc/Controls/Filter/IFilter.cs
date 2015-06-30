using System.Collections.Generic;
using System.Linq;
using Common.Web.Mvc.Controls.Filter;

namespace Common.Web.Mvc.Controls
{
    public interface IFilter
    {
        int FilterId { get; }
        bool AllowSave { get; }
        FilterEdit FilterEdit { get; }

        IEnumerable<SavedFilter> SavedFilters { get; }
        IEnumerable<IFilterCondition> Conditions { get; }
    }

    public interface IFilter<TEntity> : IFilter
        where TEntity : class
    {
        IQueryable<TEntity> Apply(IQueryable<TEntity> query);
    }
}