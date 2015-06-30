using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Common.Core.Sorting;

namespace Common.Web.Mvc.Controls
{
	public class XlsxGridRenderer<T> : IXlsxGridRenderer<T> where T : class
	{
        protected IGridModel<T> GridModel { get; private set; }
        protected IEnumerable<T> DataSource { get; private set; }

        protected List<AutoFilter> AutoFilterList { get; set; }

        public MemoryStream Render(IGridModel<T> gridModel, IEnumerable<T> dataSource)
        {
            GridModel = gridModel;
            DataSource = dataSource;
            return GetStreamReport();
        }

        protected virtual MemoryStream GetStreamReport()
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Report");

            var dataTable = GetTable();
            //ws.Cell(1, 1).Value = DateTime.Now;

            //ws.Range(1, 1, 1, dataTable.Columns.Count).Merge().AddToNamed("Titles");
            var xlTable = ws.Cell(1, 1).InsertTable(dataTable.AsEnumerable());

            ws.Columns().AdjustToContents();

            var ms = new MemoryStream();

            wb.SaveAs(ms);

            return ms;
        }

        protected IEnumerable<GridColumn<T>> VisibleColumns()
        {
            return GridModel.Columns.Where(x => x.Visible);
                }

        private DataTable GetTable()
                {
            var table = new DataTable();

            foreach (var column in VisibleColumns().OrderBy(c => c.Order))
                table.Columns.Add(column.DisplayName, column.ColumnType ?? typeof(string));

            foreach (var item in DataSource)
                table.Rows.Add(VisibleColumns().OrderBy(c => c.Order).Select(column => column.GetValue(item)).ToArray());

            return table;
        }
    }
}