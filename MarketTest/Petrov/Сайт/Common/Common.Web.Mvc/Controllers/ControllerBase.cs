using System;
using System.Web.Mvc;
using Common.Data;

namespace Common.Web.Mvc
{
    public abstract class ControllerBase : Controller
    {
        protected JsonResult ExecuteMethod(Action method)
        {
            if (!ViewData.ModelState.IsValid)
                return Json(new CommandResult(ViewData.ModelState));

            try
            {
                method.Invoke();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Global", e.Message);
            }

            return Json(new CommandResult(ModelState), JsonRequestBehavior.AllowGet);
        }

        protected JsonResult ExecuteMethod(Func<object> method)
        {
            if (!ViewData.ModelState.IsValid)
                return Json(new CommandResult(ModelState));

            object data = null;

            try
            {
                data = method.Invoke();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Global", e.Message);
            }

            return Json(new CommandResult(ModelState, data), JsonRequestBehavior.AllowGet);
        }

        protected ActionResult ExecuteSavingMethod<TEntity, TKey>(TEntity entity, Action savingMethod, Func<TEntity> fillingMethod)
            where TEntity : class, IEntityBase<TKey>
        {
            if (!ViewData.ModelState.IsValid)
                return View(fillingMethod.Invoke());

            try
            {
                savingMethod.Invoke();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Global", e.Message);

                return View(fillingMethod.Invoke());
            }

            return RedirectToAction("Edit", new { id = entity.Id });
        }
    }
}