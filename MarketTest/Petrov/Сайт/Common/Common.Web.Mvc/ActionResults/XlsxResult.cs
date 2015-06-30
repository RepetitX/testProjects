using System;
using System.Web;
using System.Web.Mvc;

namespace Common.Web.Mvc
{
    public class XlsxResult : FileContentResult
    {
        public XlsxResult(byte[] data)
            : base(data, "application/vnd.openxmlformat")
        {
            var controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];

            FileDownloadName = string.Format("{0}_{1}.xlsx", controllerName, DateTime.Now.ToString("dd_MM_yyyy_HH_mm"));
        }
    }
}