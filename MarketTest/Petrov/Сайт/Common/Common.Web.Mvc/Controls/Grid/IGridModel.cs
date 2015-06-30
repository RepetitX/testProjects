using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using Common.Core.Sorting;
using Common.Web.Mvc.Controls.Syntax;

namespace Common.Web.Mvc.Controls
{
	/// <summary>
	/// Defines a grid model
	/// </summary>
	public interface IGridModel<T> where T: class 
	{
		IGridRenderer<T> Renderer { get; set; }
        IXlsxGridRenderer<T> XlsxRenderer { get; set; }
		ICollection<GridColumn<T>> Columns { get; }
		IGridSections<T> Sections { get; }
		string EmptyText { get; set; }
		IDictionary<string, object> Attributes { get; set; }
        IEnumerable<GridSortOptions> SortOptions { get; set; }
	}
}