using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IShop.Infrastructure;

namespace IShop
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BootStrapper.ConfigureApplication();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    var routeData = new RouteData();

        //    var areaName = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"] as string;

        //    IController errorController = new ErrorController();

        //    Response.Clear();

        //    routeData.Values.Add("controller", "Error");

        //    var ex = Server.GetLastError() as HttpException;

        //    if (ex != null)
        //    {
        //        switch (ex.GetHttpCode())
        //        {
        //            case 404:
        //                routeData.Values.Add("action", "Error");
        //                routeData.Values.Add("statusCode", "404");

        //                if (areaName != null)
        //                    routeData.Values.Add("az", areaName);
        //                break;
        //            case 500:
        //                routeData.Values.Add("action", "Error");
        //                routeData.Values.Add("statusCode", "500");

        //                if (areaName != null)
        //                    routeData.Values.Add("az", areaName);
        //                break;
        //            default:
        //                return;
        //        }
        //    }
        //    else
        //    {
        //        routeData.Values.Add("action", "Error");
        //    }

        //    Server.ClearError();
        //    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        //}

        protected void Application_BeginRequest()
        {
            if (Context.Request.Url.PathAndQuery.ToLower().EndsWith("/rnum"))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var fileInfo = new FileInfo(assembly.Location);
                var version = assembly.GetName().Version;

                var rnum = string.Format("{0}.{1}", version.Major, version.Minor);
                var bnum = string.Format("{0}.{1}", version.Build, version.Revision);
                var date = fileInfo.LastWriteTime.ToString("g");
                var result = string.Format("Release Number:{0}     Build Number:{1}     Date:{2}     Developer:Common", rnum, bnum, date);

                Context.Response.Write(result);
                Context.Response.End();
            }
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }
    }
}
