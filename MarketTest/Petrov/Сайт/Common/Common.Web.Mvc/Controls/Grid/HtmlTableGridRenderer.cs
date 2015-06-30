using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Common.Core.Sorting;

namespace Common.Web.Mvc.Controls
{
	/// <summary>
	/// Renders a grid as an HTML table.
	/// </summary>
    public class HtmlTableGridRenderer<T> : GridRenderer<T> where T : class
    {
        /// <summary>
        /// Класс mvcgrid нужен для javascript. Класс table - для дизайна (css).
        /// </summary>
        private const string DefaultCssClass = "table table-striped table-condensed table-hover";

        public HtmlTableGridRenderer(ViewEngineCollection engines)
            : base(engines)
        {

        }
        public HtmlTableGridRenderer() { }

        protected override void RenderHeaderCellEnd()
        {
            RenderText("</th>");
        }

        protected virtual void RenderEmptyHeaderCellStart()
        {
            RenderText("<th>");
        }

        protected override void RenderHeaderCellStart(GridColumn<T> column)
        {
            var attributes = new Dictionary<string, object>(column.HeaderAttributes);

            if (IsSortingEnabled && column.Sortable)
            {
                var options = GridModel.SortOptions.Select((o, i) => new { index = i, option = o });
                var optionColumn = options.FirstOrDefault(o => o.option.Column == column.SearchName);

                if (optionColumn != null)
                {
                    bool isSortedByThisColumn = optionColumn.option.Column == column.SortName;
                    if (isSortedByThisColumn)
                    {
                        string sortClass = optionColumn.option.Direction == SortDirection.Ascending ? "sort_asc " : "sort_desc ";
                        if (attributes.ContainsKey("class")) sortClass = sortClass + attributes["class"];
                        attributes["class"] = sortClass;
                        attributes["sort_index"] = optionColumn.index;
                    }
                }
            }

            string attrs = BuildHtmlAttributes(attributes);

            if (attrs.Length > 0)
                attrs = " " + attrs;

            RenderText(string.Format("<th{0}>", attrs));
        }

        protected override void RenderHeaderText(GridColumn<T> column)
        {
            if (IsSortingEnabled && column.Sortable)
            {
                var options = GridModel.SortOptions.Select((o, i) => new { index = i, option = o });
                var optionColumn = options.FirstOrDefault(o => o.option.Column == column.SortName);

                var sortOptions = new GridSortOptions
                {
                    Column = column.SortName
                };

                var sortIndicator = "";

                if (optionColumn != null)
                {
                    var isSortedByThisColumn = optionColumn.option.Column == column.SortName;

                    if (isSortedByThisColumn)
                    {
                        sortOptions.Direction = (optionColumn.option.Direction == SortDirection.Ascending)
                            ? SortDirection.Descending
                            : SortDirection.Ascending;
                        sortIndicator = " " + ((sortOptions.Direction == SortDirection.Ascending) ? char.ConvertFromUtf32(0x00002193) : char.ConvertFromUtf32(0x00002191));
                    }
                }

                var routeValues = new RouteValueDictionary();

                foreach (var queryString in Context.RequestContext.HttpContext.Request.QueryString.AllKeys)
                {
                    routeValues[queryString] = Context.RequestContext.HttpContext.Request.QueryString[queryString];
                }

                routeValues["SortOptions[0].Column"] = sortOptions.Column;
                routeValues["SortOptions[0].Direction"] = sortOptions.Direction;

                var link = HtmlHelper.GenerateLink(Context.RequestContext, RouteTable.Routes, column.DisplayName + sortIndicator, null, null, null, routeValues, null);
                RenderText(link);
            }
            else
            {
                base.RenderHeaderText(column);
            }
        }

        protected override void RenderRowStart(GridRowViewData<T> rowData)
        {
            var attributes = GridModel.Sections.Row.Attributes(rowData);

            if (!attributes.ContainsKey("class"))
            {
                attributes["class"] = rowData.IsAlternate ? "gridrow_alternate" : "gridrow";
            }

            string attributeString = BuildHtmlAttributes(attributes);

            if (attributeString.Length > 0)
            {
                attributeString = " " + attributeString;
            }

            RenderText(string.Format("<tr{0}>", attributeString));
        }

        protected override void RenderRowEnd()
        {
            RenderText("</tr>");
        }

        protected override void RenderEndCell()
        {
            RenderText("</td>");
        }

        protected override void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowData)
        {
            string attrs = BuildHtmlAttributes(column.Attributes(rowData));
            if (attrs.Length > 0)
                attrs = " " + attrs;

            RenderText(string.Format("<td{0}>", attrs));
        }

        protected override void RenderHeadStart()
        {
            string attributes = BuildHtmlAttributes(GridModel.Sections.HeaderRow.Attributes(new GridRowViewData<T>(null, false)));
            if (attributes.Length > 0)
            {
                attributes = " " + attributes;
            }


            RenderText(string.Format("<thead><tr{0}>", attributes));
        }

        protected override void RenderHeadEnd()
        {
            RenderText("</tr></thead>");
        }

        protected override void RenderGridStart()
        {
            if (!GridModel.Attributes.ContainsKey("class"))
            {
                GridModel.Attributes["class"] = DefaultCssClass;
            }

            GridModel.Attributes["class"] += " mvcgrid";

            string attrs = BuildHtmlAttributes(GridModel.Attributes);

            if (attrs.Length > 0)
                attrs = " " + attrs;

            RenderText(string.Format("<div class=\"mvcgridscroll\"><table{0}>", attrs));
        }

        protected override void RenderGridEnd(bool isEmpty)
        {
            RenderText("</table></div>");
        }

        protected override void RenderEmpty()
        {
            RenderHeadStart();
            RenderEmptyHeaderCellStart();
            RenderHeaderCellEnd();
            RenderHeadEnd();
            RenderBodyStart();
            RenderText("<tr><td>" + GridModel.EmptyText + "</td></tr>");
            RenderBodyEnd();
        }

        protected override void RenderBodyStart()
        {
            RenderText("<tbody>");
        }

        protected override void RenderBodyEnd()
        {
            RenderText("</tbody>");
        }

        /// <summary>
        /// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
        /// </summary>
        /// <returns></returns>
        private static string BuildHtmlAttributes(IDictionary<string, object> attributes)
        {
            if (attributes == null || attributes.Count == 0)
            {
                return string.Empty;
            }

            const string attributeFormat = "{0}=\"{1}\"";

            return string.Join(" ",
                   attributes.Select(pair => string.Format(attributeFormat, pair.Key, pair.Value)).ToArray()
            );
        }

        protected override void RenderQuickFilterCellEnd()
        {
            RenderText("</td>");
        }

        protected override void RenderQuickFilterCellStart(GridColumn<T> column)
        {
            RenderText(string.Format("<td class=\"QuickFilter_{0}\">", column.FilterName));
        }

        protected override void RenderQuickFilterStart()
        {
            RenderText("<tr class=\"QuickFilter\">");
        }

        protected override void RenderQuickFilterEnd()
        {
            RenderText("</tr>");
        }

        protected override void RenderQuickFilterHtml(GridColumn<T> column)
        {
            var condition = Filter.Conditions.FirstOrDefault(c => c.Key == column.FilterName);
            if (condition != null)
            {
                string type = condition.GetType().Name;
                string viewName = type.IndexOf('`') > 0 ? type.Substring(0, type.Length - 2) : type;


                using (var sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(Context.Controller.ControllerContext, viewName);
                    if (viewResult.View != null)
                    {
                        var viewContext = new ViewContext(Context.Controller.ControllerContext, viewResult.View, new ViewDataDictionary(), new TempDataDictionary(), sw);
                        
                        viewContext.ViewData.Model = condition;
                        viewContext.ViewData["index"] = 1;
                        
                        viewResult.View.Render(viewContext, sw);
                        
                        RenderText(sw.GetStringBuilder().ToString());
                    }
                }
            }
        }
    }
}