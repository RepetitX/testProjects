using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Security.Principal;
using System.Xml.Linq;

namespace Common.Web.Mvc
{
    public static class MenuHelper
    {
        public static string Menu(this HtmlHelper html)
        {
            return MenuHelper.Menu(html, null, null);
        }

        public static string Menu(this HtmlHelper html, string startNodeId)
        {
            return MenuHelper.Menu(html, startNodeId, null);
        }

        public static string Menu(this HtmlHelper html, IPrincipal user)
        {
            return MenuHelper.Menu(html, null, user);
        }

        public static string Menu(this HtmlHelper html, string startNodeId, IPrincipal user)
        {
            if (html.ViewContext.HttpContext.Cache.Get("sitemap") == null)
            {
                html.ViewContext.HttpContext.Cache.Add("sitemap", XDocument.Load(html.ViewContext.HttpContext.Server.MapPath("~/web.sitemap")), null, DateTime.Now.AddYears(1),Cache.NoSlidingExpiration, CacheItemPriority.Normal,null);
            }

            var sb = new StringBuilder();
            XDocument doc = html.ViewContext.HttpContext.Cache.Get("sitemap") as XDocument;
            string cssclass = string.IsNullOrEmpty(startNodeId) ? "menu" : "submenu";
            sb.Append("<ul class=\"").Append(cssclass).Append("\">");
            var root = doc.Root;
            if (!string.IsNullOrEmpty(startNodeId))
                root = doc.Root.Elements("mvcSiteMapNode").SingleOrDefault(n => n.Attribute("id") != null && n.Attribute("id").Value == startNodeId);
            if (root == null) return string.Empty;

            foreach (var node in root.Elements("mvcSiteMapNode"))
            {
                if (!IsAccessibleToUser(user, node)) continue;

                var currentController = html.ViewContext.RouteData.Values["controller"].ToString().ToUpper();
                bool current = (currentController == node.Attribute("controller").Value.ToString().ToUpper()) || node.Elements("mvcSiteMapNode").Any(e => e.Attribute("controller").Value.ToString().ToUpper() == currentController);
                string cls = current ? " class=\"selected\"" : "";
                sb.AppendFormat("<li {0}>", cls);
                var rvd = new RouteValueDictionary();
                foreach (var attr in node.Attributes())
                {
                    if (attr.Name == "title" || attr.Name == "id" || attr.Name == "roles") continue;
                    rvd.Add(attr.Name.ToString(), attr.Value.ToString());
                }
                sb.Append(html.RouteLink(node.Attribute("title").Value.ToString(), rvd));
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        private static bool IsAccessibleToUser(IPrincipal user, XElement node)
        {
            if (node.Attribute("roles") != null)
            {
                var roles = node.Attribute("roles").Value.ToString().Split(',');
                foreach (var role in roles)
                    if (user.IsInRole(role))
                        return true;
                return false;
            }
            return true;
        }
    }
}
