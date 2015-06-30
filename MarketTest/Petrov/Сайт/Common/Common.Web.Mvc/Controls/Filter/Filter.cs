using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Common.Web.Mvc.Controls.Filter;
using Common.Web.Mvc.Services;

namespace Common.Web.Mvc.Controls
{
    public class Filter<TEntity> : IFilter<TEntity>
        where TEntity: class
    {
        protected IFilterService FilterService;
        protected IList<IFilterCondition<TEntity>> FilterConditions;

        public bool AllowSave { get { return FilterService != null; } }
        public int FilterId { get; private set; }
        public FilterEdit FilterEdit { get; private set; }
        public IPrincipal User { get; set; }

        public IEnumerable<SavedFilter> SavedFilters { get; private set; }
        public IEnumerable<IFilterCondition> Conditions { get { return FilterConditions.Cast<IFilterCondition>(); } }

        public Filter()
        {
            FilterConditions = new List<IFilterCondition<TEntity>>();
        }

        public Filter<TEntity> Configure(IFilterService filterService, IPrincipal user)
        {
            FilterService = filterService;

            return Configure(user);
        }

        public Filter<TEntity> Configure(IPrincipal user)
        {
            User = user;

            return Configure();
        }

        protected virtual Filter<TEntity> Configure()
        {
            return this;
        }

        public void AddCondition(IFilterCondition<TEntity> condition)
        {
            FilterConditions.Add(condition);
        }

        public Filter<TEntity> Init(IGridOptions options)
        {
            return Init(options, null);
        }

        public Filter<TEntity> Init(IGridOptions options, string gridKey)
        {
            if (options != null)
            {
                foreach (var fc in FilterConditions)
                    fc.Value = options.GetFilterConditionValue(fc.Key);

                FilterId = options.FilterId;
            }

            if (AllowSave && !string.IsNullOrEmpty(gridKey) && User != null)
            {
                SavedFilters = FilterService.GetSavedFilters(gridKey, User);

                var filters = new List<SelectListItem> { new SelectListItem { Text = "Новый фильтр", Value = "0" } };
                filters.AddRange(SavedFilters.ToSelectList(f => f.Id, f => f.Name, f => f.Id == FilterId));

                FilterEdit = new FilterEdit { Id = FilterId, Filters = filters };

                var currentFilter = SavedFilters.FirstOrDefault(f => f.Id == FilterId);

                if (currentFilter != null)
                {
                    FilterEdit.Name = currentFilter.Name;
                    FilterEdit.IsShared = currentFilter.UserId == null;
                }
            }

            return this;
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            if (FilterConditions != null)
            {
                foreach (var condition in FilterConditions)
                    query = condition.Apply(query);
            }

            return query;
        }
    }
}
