using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Repetit.Tasks.Task1.WebUI.Infrastructure;
using Repetit.Tasks.Task1.WebUI.Models;
using System.Collections;
using System.Collections.Generic;

namespace Repetit.Tasks.Task1.WebUI.Controllers
{
    public class BaseController : Controller
    {
        public AppUser CurrentUser
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>().FindByName(HttpContext.User.Identity.Name);
            }
        }
     
    }
}