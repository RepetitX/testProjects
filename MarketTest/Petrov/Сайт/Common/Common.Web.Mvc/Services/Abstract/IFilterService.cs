using System.Collections.Generic;
using System.Security.Principal;
using Common.Web.Mvc.Controls;
using Common.Web.Mvc.Controls.Filter;

namespace Common.Web.Mvc.Services
{
    public interface IFilterService
    {
        List<SavedFilter> GetSavedFilters(string filterKey, IPrincipal user);
        void SaveFilter(FilterEdit filterEdit, string filterKey, IGridOptions options, IPrincipal user);
        void DeleteFilter(int id, IPrincipal user);
        GridOptions LoadFilter(int id, IPrincipal user);
    }
}