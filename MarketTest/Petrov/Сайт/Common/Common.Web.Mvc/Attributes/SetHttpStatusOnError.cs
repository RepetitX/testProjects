using System.Web.Mvc;

namespace IntraVision.Web.Mvc
{
    public class SetHttpStatusOnError : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
                filterContext.HttpContext.Response.StatusCode = 500;
        }
    }
}
