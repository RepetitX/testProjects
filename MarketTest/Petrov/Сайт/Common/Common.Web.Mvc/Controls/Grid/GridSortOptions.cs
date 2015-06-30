using System;
using System.Linq;
using Common.Core.Sorting;
using System.Collections.Generic;

namespace Common.Web.Mvc.Controls
{
	/// <summary>
	/// Sorting information for use with the grid.
	/// </summary>
	[Serializable]
	public class GridSortOptions
	{
		public string Column { get; set; }
		public SortDirection Direction { get; set; }
	}

    public static class GridSortHelper
    {
        public static string GetSortExpression(IEnumerable<GridSortOptions> options)
        {
            if (options == null || options.Count() == 0)
                return "";

            return string.Join(",", (options.Select(o => o.Column + " " + o.Direction.ToString())));
        }
    }
}