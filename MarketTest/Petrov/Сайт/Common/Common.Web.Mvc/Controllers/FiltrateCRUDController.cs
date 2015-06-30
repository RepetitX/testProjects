using System.Web.Mvc;
using Common.Data;
using Common.Web.Mvc.Controls;
using Common.Web.Mvc.Controls.Filter;
using Common.Web.Mvc.Services;

namespace Common.Web.Mvc
{
    public class FiltrateCRUDController<TEntity, TKey, TEntityGrid, TEntityGridOptions, TEntityFilter> : CRUDController<TEntity, TKey, TEntityGrid, TEntityGridOptions>
        where TEntity : class, IEntityBase<TKey>
        where TEntityGrid : class, IGridModel<TEntity>
        where TEntityGridOptions : GridOptions
        where TEntityFilter : Filter<TEntity>, new()
    {
        protected readonly IFilterableBaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions, TEntityFilter> _filterableService;

        public FiltrateCRUDController(IFilterableBaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions, TEntityFilter> filterableService)
            : base((IBaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions>)filterableService)
        {
            _filterableService = filterableService;
        }

        public override ActionResult Index()
        {
            return View("IndexWithFilter", model: this.ToString());
        }

        public virtual ActionResult Filter(TEntityGridOptions options)
        {
            var filterKey = GridOptionsModelBinder.GridKey(ControllerContext);

            return View(_filterableService.GetFilter(options, filterKey, User));
        }

        [HttpPost]
        public virtual JsonResult SaveFilter(FilterEdit filterEdit, TEntityGridOptions options)
        {
            var filterKey = GridOptionsModelBinder.GridKey(ControllerContext);

            return ExecuteMethod(() => _filterableService.FilterService.SaveFilter(filterEdit, filterKey, options, User));
        }

        [HttpGet]
        public virtual ActionResult LoadFilter(int id)
        {
            var gridOptions = _filterableService.FilterService.LoadFilter(id, User);

            GridOptionsModelBinder.SaveGridOptions(ControllerContext, gridOptions);

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public virtual JsonResult DeleteFilter(int id)
        {
            return ExecuteMethod(() => _filterableService.FilterService.DeleteFilter(id, User));
        }

        [HttpGet]
        public virtual ActionResult ClearFilter()
        {
            GridOptionsModelBinder.ClearGridOptions(ControllerContext);

            return RedirectToAction("Index");
        }
    }
}