using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace Common.Web.Mvc.Controls
{
    /// <summary>
    /// A renderer responsible for rendering a grid.
    /// </summary>
	public interface IXlsxGridRenderer<T> where T:class 
	{
		/// <summary>
		/// Renders a grid
		/// </summary>
		/// <param name="gridModel">The grid model to render</param>
		/// <param name="dataSource">Data source for the grid</param>
		/// <param name="output">The TextWriter to which the grid should be rendered/</param>
		/// <param name="viewContext">View context</param>
		MemoryStream Render(IGridModel<T> gridModel, IEnumerable<T> dataSource);
	}
}