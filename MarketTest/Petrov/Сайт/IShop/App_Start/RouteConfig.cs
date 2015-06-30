using System.Web.Mvc;
using System.Web.Routing;

namespace IShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Error", action = "Error", id = UrlParameter.Optional }
            );
        }
    }
}
