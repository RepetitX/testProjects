using System;
using System.Collections.Generic;
using System.Linq;
using Common.Core.Sorting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Common.Web.Mvc.Controls
{
    [Serializable]
    public class GridOptions : IGridOptions
    {
        public IEnumerable<GridSortOptions> SortOptions { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int FilterId { get; set; }
        public string SearchString { get; set; }
        public string SearchStringPlaceholder { get; set; }
        public IList<string> VisibleColumns { get; set; }
        public IList<FilterConditionValue> FilterConditionValues { get; set; }

        public GridOptions()
        {
            Page = 1;
            PageSize = 30;

            SortOptions = new List<GridSortOptions>{new GridSortOptions {Column = "Id", Direction = SortDirection.Ascending}};
            VisibleColumns = new List<string>();
            FilterConditionValues = new List<FilterConditionValue>();
        }

        public FilterConditionValue GetFilterConditionValue(string key)
        {
            return FilterConditionValues.SingleOrDefault(c => !string.IsNullOrEmpty(c.Key) && c.Key.ToLower() == key.ToLower());
        }

        public static GridOptions Deserialize(string serialized)
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream(Convert.FromBase64String(serialized));
            
            try{
                return bf.Deserialize(ms) as GridOptions;
            }
            finally{
                ms.Close();
            }
        }

        public string Serialize()
        {
            var bf = new BinaryFormatter();
            var memStr = new MemoryStream();

            var copy = MemberwiseClone() as GridOptions;
            copy.Page = 1;
            try
            {
                bf.Serialize(memStr, copy);
                memStr.Position = 0;

                return Convert.ToBase64String(memStr.ToArray());
            }
            finally
            {
                memStr.Close();
            }
        }
    }
}
