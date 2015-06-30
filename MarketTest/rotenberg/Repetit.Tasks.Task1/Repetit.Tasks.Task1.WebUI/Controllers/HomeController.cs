using System.Web.Mvc;

namespace Repetit.Tasks.Task1.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ViewResult Secret()
        {
            return View();
        }
    }
}