using System.Web.Mvc;

namespace IShop.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(int? statusCode, string az)
        {
            ViewBag.AZ = !string.IsNullOrEmpty(az) && az.ToLower() == "az";

            Response.StatusCode = statusCode ?? 500;

            return View();
        }
    }
}
