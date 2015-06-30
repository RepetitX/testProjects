using System;
using System.Linq;
using System.Text;
using Common.Core.Sorting;
using Common.Core.Pagination;
using System.Web.Mvc;
using System.IO;

namespace Common.Web.Mvc.Controls
{
    public class GridViewModelException : Exception
    {
        public GridViewModelException(string message) : base(message) { }
    }

    public class GridViewModel<TEntity> : IGridViewModel<TEntity>
        where TEntity : class
    {
        protected IQueryable<TEntity> _Query;
        protected IPagination<TEntity> _Data;
        protected IFilter<TEntity> _Filter;
        protected IGridModel<TEntity> _Model;

        public GridOptions GridOptions { get; set; }
        public IPagination<TEntity> Data
        {
            get
            {
                if (_Data == null) throw new GridViewModelException(Resources.Grid.GridMustBePrepared);
                return _Data;
            }
        }

        public GridViewModel(GridOptions options, IQueryable<TEntity> query)
        {
            GridOptions = options ?? new GridOptions();
            _Query = query;
        }

        protected virtual IGridModel<TEntity> GetModel(HtmlHelper html)
        {
            if(_Model == null )
                _Model = new AutoColumnGridModel<TEntity>(new DataAnnotationsModelMetadataProvider());
            
            return _Model;
        }

        public IGridModel<TEntity> PrepareModel(HtmlHelper html, GridOptions options)
        {
            var model = GetModel(html);

            //Установить видимость колонок
            if (GridOptions != null && GridOptions.VisibleColumns != null && GridOptions.VisibleColumns.Count > 0)
            {
                var visibleColumns = GridOptions.VisibleColumns;
                foreach (var column in model.Columns)
                {
                    //Возможная ошибка в конфигурации - пустое поле "Name" у колонки. Не будем ее скрывать.
                    if (string.IsNullOrEmpty(column.Name)) continue;

                    var icolumn = column as IGridColumn<TEntity>;
                    icolumn.Visible(false);
                    if (visibleColumns.Contains(column.Name)) icolumn.Visible(true);
                }
            }
            return model;
        }

        public IGridViewModel<TEntity> WithFilter(IFilter<TEntity> filter)
        {
            _Filter = filter;
            return this;
        }

        protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> query)
        {
            if (_Filter != null) return _Filter.Apply(query);
            return query;
        }

        protected virtual IQueryable<TEntity> Order(IQueryable<TEntity> query)
        {
            if (query == null || GridOptions == null || GridOptions.SortOptions == null || !GridOptions.SortOptions.Any() || GridOptions.SortOptions.All(o => string.IsNullOrEmpty(o.Column)))
                return query;

            return GridOptions.SortOptions.Aggregate(query, (current, gridSortOptionse) => current.OrderBy(gridSortOptionse.Column, gridSortOptionse.Direction));
        }

        public virtual IGridViewModel<TEntity> Prepare()
        {
            var query = _Query;

            query = Filter(query);
            query = Order(query);

            _Data = query.AsPagination(GridOptions.Page, GridOptions.PageSize);
            return this;
        }

        public virtual string Render(HtmlHelper html)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("updatepanel");
            div.GenerateId(this.GetType().Name);

            var url = new UrlHelper(html.ViewContext.RequestContext);
            div.MergeAttribute("data-action", url.Action("Grid"));

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            
            new Grid<TEntity>(Data, sw, html.ViewContext).WithModel(PrepareModel(html,GridOptions)).Sort(GridOptions.SortOptions).Render();
            
            div.InnerHtml += sb.ToString();
            div.InnerHtml += html.Pager(Data);

            return div.ToString();
        }


        public IFilter GetFilter()
        {
            throw new NotImplementedException();
        }
    }
}
