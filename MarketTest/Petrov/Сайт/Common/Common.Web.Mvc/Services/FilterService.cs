using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Common.Repository;
using Common.Web.Mvc.Controls;
using Common.Web.Mvc.Controls.Filter;
using Microsoft.AspNet.Identity;

namespace Common.Web.Mvc.Services
{
    public class FilterService : IFilterService
    {
        private readonly Lazy<IRepository<SavedFilter, int>> _savedFiltersRepository;

        public FilterService(Lazy<IRepository<SavedFilter, int>> savedFiltersRepository)
        {
            _savedFiltersRepository = savedFiltersRepository;
        }

        public virtual List<SavedFilter> GetSavedFilters(string filterKey, IPrincipal user)
        {
            var userId = user.Identity.GetUserId();

            return _savedFiltersRepository.Value
                .Where(f => f.FilterKey == filterKey && (f.UserId == userId || f.UserId == null))
                .OrderBy(f => f.Name)
                .ToList();
        }

        public virtual void SaveFilter(FilterEdit filterEdit, string filterKey, IGridOptions options, IPrincipal user)
        {
            if (filterEdit.Id != 0)
            {
                var userId = user.Identity.GetUserId();

                var filter = _savedFiltersRepository.Value.First(f => f.Id == filterEdit.Id && (f.UserId == userId || f.UserId == null));

                filter.Name = filterEdit.Name;
                filter.UserId = !filterEdit.IsShared ? user.Identity.GetUserId() : null;
                filter.GridOptions = options.Serialize();

                _savedFiltersRepository.Value.Update(filter);
            }
            else
            {
                var filter = new SavedFilter
                    {
                        Id = filterEdit.Id,
                        Name = filterEdit.Name,
                        FilterKey = filterKey,
                        UserId = !filterEdit.IsShared ? user.Identity.GetUserId() : null,
                        GridOptions = options.Serialize()
                    };

                _savedFiltersRepository.Value.Add(filter);
            }

            _savedFiltersRepository.Value.SaveChanges();
        }

        public virtual void DeleteFilter(int id, IPrincipal user)
        {
            var userId = user.Identity.GetUserId();

            _savedFiltersRepository.Value.Delete(f => f.Id == id && (f.UserId == userId || f.UserId == null));
        }

        public virtual GridOptions LoadFilter(int id, IPrincipal user)
        {
            var userId = user.Identity.GetUserId();

            var filter = _savedFiltersRepository.Value.First(f => f.Id == id && (f.UserId == userId || f.UserId == null));

            var gridOptions = GridOptions.Deserialize(filter.GridOptions);
            gridOptions.FilterId = filter.Id;

            return gridOptions;
        }
    }
}