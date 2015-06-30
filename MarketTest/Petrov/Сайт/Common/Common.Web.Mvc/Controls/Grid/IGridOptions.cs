using System.Collections.Generic;

namespace Common.Web.Mvc.Controls
{
    public interface IGridOptions
    {
        IList<FilterConditionValue> FilterConditionValues { get; set; }
        FilterConditionValue GetFilterConditionValue(string key);

        int Page { get; set; }
        int PageSize { get; set; }
        int FilterId { get; set; }
        string SearchString { get; set; }
        string SearchStringPlaceholder { get; set; }
        IEnumerable<GridSortOptions> SortOptions { get; set; }
        IList<string> VisibleColumns { get; set; }

        string Serialize();
    }
}
