using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Principal;
using System.Text;
using Autofac;
using Common.Core.Sorting;
using Common.Core.Pagination;
using Common.Web.Mvc.Autofac;
using System.Web.Mvc;
using System.IO;

namespace Common.Web.Mvc.Controls
{
    public class ActionGrid<TEntity, TGridModel> : IActionGrid
        where TEntity : class
        where TGridModel : class, IGridModel<TEntity>
    {
        private IQueryable<TEntity> _query;
        private IFilter<TEntity> _filter;
        private IGridOptions _options;
        private Func<IQueryable<TEntity>, IGridOptions, IQueryable<TEntity>> _querySorter;
        private Func<IQueryable<TEntity>, IGridModel<TEntity>, string, IQueryable<TEntity>> _querySearcher;
        private IPrincipal _User;
        private readonly bool _showSearchString = true;
        private readonly bool _showGridOptionsString = true;
        private readonly Action<IEnumerable<TEntity>> _postProcessingAction;

        private bool _useCustomPagination;
        private int _customPaginationPageNumber;
        private int _customPaginationPageSize;
        private int _customPaginationTotalItems;

        public ActionGrid(IQueryable<TEntity> query, IGridOptions options)
        {
            _query = query;
            _options = options;
        }

        public ActionGrid(IQueryable<TEntity> query, IGridOptions options, Action<IEnumerable<TEntity>> postProcessingAction)
        {
            _query = query;
            _options = options;
            _postProcessingAction = postProcessingAction;
        }

        public ActionGrid(IQueryable<TEntity> query, IGridOptions options, bool showSearchString, bool showGridOptions)
        {
            _query = query;
            _options = options;
            _showSearchString = showSearchString;
            _showGridOptionsString = showGridOptions;
        }

        public ActionGrid(IQueryable<TEntity> query, IGridOptions options, Action<IEnumerable<TEntity>> postProcessingAction, bool showSearchString, bool showGridOptions)
        {
            _query = query;
            _options = options;
            _postProcessingAction = postProcessingAction;
            _showSearchString = showSearchString;
            _showGridOptionsString = showGridOptions;
        }

        private IQueryable<TEntity> Order(IQueryable<TEntity> query, IGridOptions options)
        {
            if (query == null || options == null || options.SortOptions == null || !options.SortOptions.Any() || options.SortOptions.All(o => string.IsNullOrEmpty(o.Column)))
                return query;

            var orderBy = options.SortOptions.First();
            var orderedQueryable = query.OrderBy(orderBy.Column, orderBy.Direction);

            return options.SortOptions.Skip(1).Aggregate(orderedQueryable, (current, gridSortOptionse) => current.ThenBy(gridSortOptionse.Column, gridSortOptionse.Direction));
        }

        private IQueryable<TEntity> Search(IQueryable<TEntity> query, IGridModel<TEntity> model, string search)
        {
            if (string.IsNullOrEmpty(search))
                return query;

            search = search.Trim();

            var sb = new StringBuilder();

            foreach (var column in (from c in model.Columns where c.Searchable select c))
                sb.Append(sb.Length > 0 ? "||" : "").Append("(").Append(column.SearchName).Append("!= null").Append(" && ").Append(column.SearchName).Append(".ToLower()").Append(".Contains(@0)").Append(")");

            var searchNumber = 0;

            if (int.TryParse(search, out searchNumber))
            {
                foreach (var column in (from c in model.Columns where c.NumericSearchable select c))
                    sb.Append(sb.Length > 0 ? "||" : "").Append(column.SearchName).Append(" == @1");
            }

            if (sb.Length > 0)
                return query.Where(sb.ToString(), search, searchNumber);

            return query;
        }

        public ActionGrid<TEntity, TGridModel> WithFilter(IFilter<TEntity> filter)
        {
            _filter = filter;

            return this;
        }

        public ActionGrid<TEntity, TGridModel> WithQuerySorter(Func<IQueryable<TEntity>, IGridOptions, IQueryable<TEntity>> sorter)
        {
            _querySorter = sorter;

            return this;
        }

        public ActionGrid<TEntity, TGridModel> WithQuerySearcher(Func<IQueryable<TEntity>, IGridModel<TEntity>, string, IQueryable<TEntity>> searcher)
        {
            _querySearcher = searcher;

            return this;
        }

        public ActionGrid<TEntity, TGridModel> WithCustomPagination(int pageNumber, int pageSize, int totalItems)
        {
            _useCustomPagination = true;
            _customPaginationPageNumber = pageNumber;
            _customPaginationPageSize = pageSize;
            _customPaginationTotalItems = totalItems;

            return this;
        }

        public string Render(HtmlHelper html)
        {
            if (_options == null) _options = new GridOptions();

            var model = CreateGridModel(html);

            _query = _querySorter != null ? _querySorter(_query, _options) : Order(_query, _options);
            _query = _querySearcher != null ? _querySearcher(_query, model, _options.SearchString) : Search(_query, model, _options.SearchString);

            var data = _useCustomPagination
                ? _query.AsCustomPagination(_customPaginationPageNumber, _customPaginationPageSize, _customPaginationTotalItems)
                : _query.AsPagination(_options.Page, _options.PageSize);

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            if (_options != null && _options.VisibleColumns != null && _options.VisibleColumns.Count > 0)
            {
                var visibleColumns = _options.VisibleColumns;
                //Сначала скроем все колонки
                foreach (var column in model.Columns)
                {
                    //Возможная ошибка в конфигурации - пустое поле "Name" у колонки. Не будем ее скрывать.
                    if (string.IsNullOrEmpty(column.Name)) continue;

                    ((IGridColumn<TEntity>)column).Visible(false);
                }
                //Потом покажем видимые
                for (int i = 0; i < visibleColumns.Count; i++)
                {
                    var col = model.Columns.FirstOrDefault(c => c.Name == visibleColumns[i]);
                    if (col != null)
                    {
                        ((IGridColumn<TEntity>)col).Visible(true);
                        col.Order = i + 1;
                    }
                }
            }

            var grid = new Grid<TEntity>(data, sw, html.ViewContext).WithFilter(_filter).WithModel(model).Sort(_options.SortOptions);

            if (_postProcessingAction != null)
                grid = grid.PostProcessing(_postProcessingAction);

            if (_showGridOptionsString)
                AppendGridOptions(sb, model);

            if (_showSearchString)
                AppendSearchString(sb);

            grid.Render();

            sb.Append(new Pager(data, html));

            return sb.ToString();
        }

        public MemoryStream XlsxRender()
        {
            if (_options == null) _options = new GridOptions();

            var model = CreateGridModel();

            _query = _querySorter != null ? _querySorter(_query, _options) : Order(_query, _options);
            _query = _querySearcher != null ? _querySearcher(_query, model, _options.SearchString) : Search(_query, model, _options.SearchString);

            var data = _query.ToList();

            if (_options != null && _options.VisibleColumns != null && _options.VisibleColumns.Count > 0)
            {
                var visibleColumns = _options.VisibleColumns;
                //Сначала скроем все колонки
                foreach (var column in model.Columns)
                {
                    //Возможная ошибка в конфигурации - пустое поле "Name" у колонки. Не будем ее скрывать.
                    if (string.IsNullOrEmpty(column.Name)) continue;

                    ((IGridColumn<TEntity>)column).Visible(false);
                }
                //Потом покажем видимые
                for (int i = 0; i < visibleColumns.Count; i++)
                {
                    var col = model.Columns.FirstOrDefault(c => c.Name == visibleColumns[i]);
                    if (col != null)
                    {
                        ((IGridColumn<TEntity>)col).Visible(true);
                        col.Order = i + 1;
                    }
                }
            }

            var grid = new Grid<TEntity>(data).WithModel(model).Sort(_options.SortOptions);

            if (_postProcessingAction != null)
                grid = grid.PostProcessing(_postProcessingAction);

            return grid.XlsxRender();
        }

        public MemoryStream XlsxRender(IXlsxGridRenderer<TEntity> xlsxGridRenderer)
        {
            if (_options == null) _options = new GridOptions();

            var model = CreateGridModel();

            _query = _querySorter != null ? _querySorter(_query, _options) : Order(_query, _options);
            _query = _querySearcher != null ? _querySearcher(_query, model, _options.SearchString) : Search(_query, model, _options.SearchString);

            var data = _query.ToList();

            if (_options != null && _options.VisibleColumns != null && _options.VisibleColumns.Count > 0)
            {
                var visibleColumns = _options.VisibleColumns;
                //Сначала скроем все колонки
                foreach (var column in model.Columns)
                {
                    //Возможная ошибка в конфигурации - пустое поле "Name" у колонки. Не будем ее скрывать.
                    if (string.IsNullOrEmpty(column.Name)) continue;

                    ((IGridColumn<TEntity>)column).Visible(false);
                }
                //Потом покажем видимые
                for (int i = 0; i < visibleColumns.Count; i++)
                {
                    var col = model.Columns.FirstOrDefault(c => c.Name == visibleColumns[i]);
                    if (col != null)
                    {
                        ((IGridColumn<TEntity>)col).Visible(true);
                        col.Order = i + 1;
                    }
                }
            }

            var grid = new Grid<TEntity>(data).WithModel(model).Sort(_options.SortOptions).XlsxRenderUsing(xlsxGridRenderer);

            if (_postProcessingAction != null)
                grid = grid.PostProcessing(_postProcessingAction);

            return grid.XlsxRender();
        }

        private void AppendSearchString(StringBuilder sb)
        {
            var divSearch = new TagBuilder("div");
            divSearch.AddCssClass("col-md-6 col-lg-5 pull-right mvcgrid-search");

            var divInputGroup = new TagBuilder("div");
            divInputGroup.AddCssClass("input-group");

            var input = new TagBuilder("input");

            input.AddCssClass("form-control input-sm");
            input.Attributes["type"] = "text";
            input.Attributes["name"] = "SearchString";
            input.Attributes["value"] = _options.SearchString;

            if (_options.SearchStringPlaceholder != null)
                input.Attributes["placeholder"] = _options.SearchStringPlaceholder;

            divInputGroup.InnerHtml += input.ToString();
            divInputGroup.InnerHtml +=
                "<span class=\"input-group-btn\"><button class=\"btn btn-sm btn-default grid-clear-search-btn\" type=\"button\"><span class=\"glyphicon glyphicon-remove\"></span></button>" +
                "<button class=\"btn btn-sm btn-default grid-search-btn\" type=\"button\"><span class=\"glyphicon glyphicon-search\"></span></button></span>";

            divSearch.InnerHtml += divInputGroup.ToString();

            sb.Append(divSearch.ToString());
        }

        private void AppendGridOptions(StringBuilder sb, IGridModel<TEntity> model)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("grid-options-button");

            var button = new TagBuilder("button");
            button.MergeAttribute("title", "Настроить видимость колонок");
            div.InnerHtml = button.ToString();
            sb.Append(div.ToString());

            var gridoptions = new TagBuilder("div");
            gridoptions.AddCssClass("grid-options");

            var form = new TagBuilder("form");
            var ulholder = new TagBuilder("div");
            ulholder.AddCssClass("checklist");

            var ul = new TagBuilder("ul");

            foreach (var col in model.Columns.OrderBy(c=>c.Order))
            {
                if (string.IsNullOrEmpty(col.Name)) continue;

                var li = new TagBuilder("li");
                var label = new TagBuilder("label");
                var cb = new TagBuilder("input");
                cb.MergeAttribute("type", "checkbox");
                cb.MergeAttribute("value", col.Name);
                cb.MergeAttribute("name", "VisibleColumns");

                if (col.Visible)
                    cb.MergeAttribute("checked", "checked");

                label.InnerHtml += cb.ToString();
                label.InnerHtml += col.DisplayName;

                li.InnerHtml = label.ToString();
                var span = new TagBuilder("span");
                span.AddCssClass("options");
                var a = new TagBuilder("a");
                a.AddCssClass("move");
                span.InnerHtml = a.ToString();
                li.InnerHtml += span.ToString();
                ul.InnerHtml += li.ToString();
            }
            ulholder.InnerHtml = ul.ToString();
            form.InnerHtml = ulholder.ToString();

            var formbottom = new TagBuilder("div");
            formbottom.AddCssClass("form-bottom");

            var apply = new TagBuilder("button");
            apply.SetInnerText("Применить");

            var cancel = new TagBuilder("a");
            cancel.AddCssClass("inline-button");
            cancel.AddCssClass("grid-options-hide");
            cancel.MergeAttribute("href", "javascript:;;");
            cancel.SetInnerText("Отмена");

            formbottom.InnerHtml = apply.ToString();
            formbottom.InnerHtml += cancel.ToString();

            gridoptions.InnerHtml = form.ToString();
            gridoptions.InnerHtml += formbottom.ToString();

            sb.Append(gridoptions.ToString());
        }

        public IGridModel<TEntity> CreateGridModel(HtmlHelper html)
        {
            var tModel = AutofacLifetimeScope.GetInstance<TGridModel>(TypedParameter.From(html), TypedParameter.From(_User));

            if (tModel != null)
                return tModel;

            return new AutoColumnGridModel<TEntity>(new DataAnnotationsModelMetadataProvider());
        }

        public IGridModel<TEntity> CreateGridModel()
        {
            var tModel = AutofacLifetimeScope.GetInstance<TGridModel>(TypedParameter.From(_User));

            if (tModel != null)
                return tModel;

            return new AutoColumnGridModel<TEntity>(new DataAnnotationsModelMetadataProvider());
        }
    }
}
