using System.Web.Mvc;

namespace IShop.Controllers.Areas.AZ
{
    public class AZAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AZ";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DefaultAdmin",
                "AZ/{controller}/{action}/{id}",
                new { controller = "Order", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
