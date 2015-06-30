using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Common.Core.Pagination;
using System.Web.Mvc;
using System.IO;

namespace Common.Web.Mvc.Controls
{
    public class FastActionGrid<TEntity> : IActionGrid
        where TEntity : class
    {
        private IQueryable<TEntity> _Query;
        private IGridOptions _Options;
        private Type _TModel;
        private IPrincipal _User;

        public FastActionGrid(IQueryable<TEntity> query, IGridOptions options)
        {
            _Query = query;
            _Options = options;
        }

        public FastActionGrid<TEntity> WithModelType(Type tmodel)
        {
            return WithModelType(tmodel, null);
        }

        public FastActionGrid<TEntity> WithModelType(Type tmodel, IPrincipal user)
        {
            if (!typeof(IGridModel<TEntity>).IsAssignableFrom(tmodel))
                throw new ArgumentException("Model type must implement IGridModel<T> interface!");

            _TModel = tmodel;
            _User = user;
            return this;
        }

        public string Render(HtmlHelper html)
        {
            if (_Options == null) _Options = new GridOptions();

            var model = CreateGridModel(html);
            var data = _Query.AsPagination(1, _Options.PageSize);

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            if (_Options.VisibleColumns != null && _Options.VisibleColumns.Count > 0)
            {
                var visibleColumns = _Options.VisibleColumns;
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

            new Grid<TEntity>(data, sw, html.ViewContext).WithModel(model).Sort(_Options.SortOptions).Render();
            sb.Append(new FastPager(data, _Options, html));

            return sb.ToString();
        }

        public IGridModel<TEntity> CreateGridModel(HtmlHelper html)
        {
            if(_TModel != null )
            {
                if (_User != null)
                    return (IGridModel<TEntity>)Activator.CreateInstance(_TModel, html, _User);
                else
                    return (IGridModel<TEntity>)Activator.CreateInstance(_TModel, html);
            }

            return new AutoColumnGridModel<TEntity>(new DataAnnotationsModelMetadataProvider());
        }
    }
}
