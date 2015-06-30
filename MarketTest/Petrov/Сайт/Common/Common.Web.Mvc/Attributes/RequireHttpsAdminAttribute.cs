using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Common.Web.Mvc.Attributes
{
    public class RequireHttpsAdminAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // Ssl в админке можно включить установив параметр RequireHttpsAdmin в web.config равным true
            var requireHttpsAdminSetting = WebConfigurationManager.AppSettings.Get("RequireHttpsAdmin");
            bool requireHttpsAdmin;

            if (!string.IsNullOrEmpty(requireHttpsAdminSetting) && bool.TryParse(requireHttpsAdminSetting, out requireHttpsAdmin) && requireHttpsAdmin)
            {
                //присутствие BaseDNS в web.config будет озаначать, что админка должна работать только по этому DNS
                var baseDns = WebConfigurationManager.AppSettings["BaseDNS"];

                if (string.IsNullOrEmpty(baseDns) || HttpContext.Current.Request.Url.Host == baseDns)
                {
                    base.OnAuthorization(filterContext);
                }
                else
                {
                    HttpContext.Current.Response.Redirect("https://" + baseDns + "/Admin");
                }
            }
        }
    }
}
