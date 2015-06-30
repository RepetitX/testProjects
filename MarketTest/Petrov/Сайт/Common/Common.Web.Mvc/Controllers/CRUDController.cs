using System.Web.Mvc;
using Common.Data;
using Common.Web.Mvc.Controls;
using Common.Web.Mvc.Services;

namespace Common.Web.Mvc
{
    public class CRUDController<TEntity, TKey, TEntityGrid, TEntityGridOptions> : ControllerBase
        where TEntity : class, IEntityBase<TKey>
        where TEntityGrid : class, IGridModel<TEntity>
    {
        protected readonly IBaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions> _service;

        public CRUDController(IBaseService<TEntity, TKey, TEntityGrid, TEntityGridOptions> service)
        {
            _service = service;
        }

        public virtual ActionResult Index()
        {
            return View(model: this.ToString());
        }

        public virtual ActionResult BatchGrid(TKey[] ids, TEntityGridOptions options)
        {
            return View(_service.GetActionGrid(ids, options, User));
        }

        public virtual ActionResult Grid(TEntityGridOptions options)
        {
            return View(_service.GetActionGrid(options, User));
        }

        public virtual ActionResult BatchXlsxGrid(TKey[] ids, TEntityGridOptions options)
        {
            return new XlsxResult(_service.GetActionGrid(ids, options, User).XlsxRender().ToArray());
        }

        public virtual ActionResult XlsxGrid(TEntityGridOptions options)
        {
            return new XlsxResult(_service.GetActionGrid(options, User).XlsxRender().ToArray());
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View(_service.Create(User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(TEntity create)
        {
            return ExecuteSavingMethod<TEntity, TKey>(create, () => _service.Create(create, User), () => _service.BeforeGet(create, User));
        }

        [HttpGet]
        public virtual ActionResult Edit(TKey id)
        {
            return View(_service.Edit(id, User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(TEntity edit)
        {
            return ExecuteSavingMethod<TEntity, TKey>(edit, () => _service.Edit(edit, User), () => _service.BeforeGet(edit, User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual JsonResult Delete(TKey id)
        {
            return ExecuteMethod(() => _service.Delete(id, User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual JsonResult BatchDelete(TKey[] ids)
        {
            return ExecuteMethod(() => _service.Delete(ids, User));
        }

        [HttpGet]
        public virtual JsonResult Sort(TKey[] ids)
        {
            return ExecuteMethod(() => _service.Sort(ids));
        }

        public override string ToString()
        {
            return GetType().Name.Replace("Controller", "");
        }
    }
}