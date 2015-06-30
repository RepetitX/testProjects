using System;
using System.Web.Mvc;
using System.Text;
using Repetit.Tasks.Task1.WebUI.Models;

namespace Repetit.Tasks.Task1.WebUI.Helpers
{
    public static class PagingHelper
    {
       // private const string bus_ancor_format = "/{bustype_code_9}/{man}/id_man {id_manufacture}/profile{profile_code_11}/height{height_12}/diameter{diameter_4}/sezon{sezon_1}/onpage{PageSize}/page{Page}";
        private const string bus_ancor_format_man = "/Bus/Index/{0}/" + "{1}/" + "id_man{2}/" + "profile-{3}/" + "height-{4}/" + "diameter-{5}/" + "sezon-{6}/" + "spike-{7}/" + "onpage{8}/" + "page{9}";
        private const string bus_ancor_format = "/Bus/Index/{0}/" + "id_man{1}/" + "profile-{2}/" + "height-{3}/" + "diameter-{4}/" + "sezon-{5}/" + "spike-{6}/" + "onpage{7}/" + "page{8}";
        private const string bus_ancor_format_type = "/Bus/Index/{0}/onpage{1}";
        private const string bus_ancor_ps = "/Bus/Index/{0}/" + "{1}/" + "id_man{2}/" + "profile-{3}/" + "height-{4}/" + "diameter-{5}/" + "sezon-{6}/" + "spike-{7}";
        //                                                                         0              1             2           3                4             5         6        7             8             9

        private const string bus_ancor_format_man_full = "/Bus/Full/{0}/" + "{1}/" + "id_man{2}/" + "profile-{3}/" + "height-{4}/" + "diameter-{5}/" + "sezon-{6}/" + "spike-{7}/" + "onpage{8}/" + "page{9}";
        private const string bus_ancor_format_full = "/Bus/Full/{0}/" + "id_man{1}/" + "profile-{2}/" + "height-{3}/" + "diameter-{4}/" + "sezon-{5}/" + "spike-{6}/" + "onpage{7}/" + "page{8}";
        private const string bus_ancor_format_type_full = "/Bus/Full/{0}/onpage{1}";
        private const string bus_ancor_ps_full = "/Bus/Full/{0}/" + "{1}/" + "id_man{2}/" + "profile-{3}/" + "height-{4}/" + "diameter-{5}/" + "sezon-{6}/" + "spike-{7}";

        private const string disk_ancor_format = "/Web/Pages/DiscCatalog.aspx/{0}/man{1}/{2}/mark-{3}/diameter{4}/width{5}/fix{6}/pcd{7}/onpage{8}/page{9}";
        private const string disk_ancor_format_ps = "/Web/Pages/DiscCatalog.aspx/{0}/man{1}/{2}/mark-{3}/diameter{4}/width{5}/fix{6}/pcd{7}";
        public static  MvcHtmlString PageLinks(this HtmlHelper html, PageData pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder sb = new StringBuilder();
            if (pageInfo!=null && pageInfo.TotalItems > pageInfo.PageSize)
            {
                UrlHelper url = new UrlHelper(html.ViewContext.RequestContext);
                TagBuilder back = new TagBuilder("A");
                back.MergeAttribute("href", pageInfo.Page > 1 ? pageUrl(pageInfo.Page - 1) : "javascript:void(0);");
               
                TagBuilder arrow_back = new TagBuilder("IMG");
                if (pageInfo.Page == 1)
                {
                    back.MergeAttribute("class", "no-await btn btn-default btn-primary selected");
                    arrow_back.MergeAttribute("src", url.Content("~/Content/img/back_p.gif"));
                }
                else
                {
                    arrow_back.MergeAttribute("src", url.Content("~/Content/img/b_p.png"));
                    arrow_back.AddCssClass("btn btn-default");
                }
                arrow_back.MergeAttribute("title", "Предыдущая");
                arrow_back.MergeAttribute("alt", "Предыдущая");
                back.InnerHtml = arrow_back.ToString();
           
                TagBuilder first = new TagBuilder("A");
                first.MergeAttribute("href", pageUrl(1));
                if (pageInfo.IsFirstPage())
                    first.MergeAttribute("class", "btn-primary selected");
                first.AddCssClass("btn btn-default");
                first.InnerHtml = "1";

                sb.Append(first);

                if (pageInfo.Page > 5)
                {
                    for (int dot = 1; dot < 4; dot++)
                    {
                        TagBuilder span = new TagBuilder("span");
                        span.InnerHtml = ".";
                        sb.Append(span);
                    }
                }

                for (int i = pageInfo.Page - 3; i <= pageInfo.Page; i++)
                {
                    if (i > 1)
                    {
                        TagBuilder tag = new TagBuilder("A");
                        tag.MergeAttribute("href", pageUrl(i));
                        if (i == pageInfo.Page)
                        {
                            tag.AddCssClass("selected");
                            tag.AddCssClass("btn-primary");
                        }
                        tag.AddCssClass("btn btn-default");
                        tag.InnerHtml = i.ToString();
                        sb.Append(tag);
                    }
                }

                for (int i = pageInfo.Page + 1; i <= pageInfo.Page + 3; i++)
                {
                    if (i < pageInfo.Pages)
                    {
                        TagBuilder tag = new TagBuilder("A");
                        tag.MergeAttribute("href", pageUrl(i));
                        tag.InnerHtml = i.ToString();
                        tag.AddCssClass("btn btn-default");
                        sb.Append(tag);
                    }
                }

                if (pageInfo.Pages - pageInfo.Page > 4)
                {
                    for (int dot = 1; dot < 4; dot++)
                    {
                        TagBuilder span = new TagBuilder("span");
                        span.InnerHtml = ".";
                        sb.Append(span);
                    }
                }

                if (!pageInfo.IsLastPage())
                {
                    TagBuilder last = new TagBuilder("A");

                    last.MergeAttribute("href", pageUrl(pageInfo.Pages));
                    last.InnerHtml = pageInfo.Pages.ToString();
                    last.AddCssClass("btn btn-default");
                    sb.Append(last);
                }
                TagBuilder next = new TagBuilder("A");
                next.MergeAttribute("href",
                    pageInfo.Page < pageInfo.Pages ? pageUrl(pageInfo.Page + 1) : "javascript:void(0);");
                if (pageInfo.Page == pageInfo.Pages)
                {
                    next.MergeAttribute("class", "no-await");
                }
                TagBuilder arrow_next = new TagBuilder("IMG");
                if (pageInfo.Page == pageInfo.Pages)
                {
                    arrow_next.MergeAttribute("src", url.Content("~/Content/img/next_p.gif"));
                }
                else
                {
                    arrow_next.MergeAttribute("src", url.Content("~/Content/img/n_p.png"));
                }
                arrow_next.MergeAttribute("title", "Следующая");
                arrow_next.MergeAttribute("alt", "Следующая");
                next.InnerHtml = arrow_next.ToString();
            //    sb.Append(next);
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString ThePageLinks(this HtmlHelper html, PageData pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder sb = new StringBuilder();
            if (pageInfo.TotalItems > pageInfo.PageSize)
            {

                if (pageInfo != null)
                {
                    UrlHelper url = new UrlHelper(html.ViewContext.RequestContext);
                    TagBuilder back = new TagBuilder("A");
                    back.MergeAttribute("href", pageInfo.Page > 1 ? pageUrl(pageInfo.Page - 1) : pageUrl(1));
                    TagBuilder arrow_back = new TagBuilder("IMG");
                    if (pageInfo.Page == 1)
                    {
                        back.MergeAttribute("class", "no-await");
                        arrow_back.MergeAttribute("src", url.Content("~/Content/img/back_p.gif"));
                    }
                    else
                    {
                        arrow_back.MergeAttribute("src", url.Content("~/Content/img/b_p.png"));
                    }
                    arrow_back.MergeAttribute("title", "Предыдущая");
                    arrow_back.MergeAttribute("alt", "Предыдущая");
                    back.InnerHtml = arrow_back.ToString();
                    sb.Append(back);

                    TagBuilder first = new TagBuilder("A");
                    first.MergeAttribute("href", pageUrl(1));
                    if (pageInfo.IsFirstPage())
                        first.MergeAttribute("class", "selected");

                    first.InnerHtml = "1";

                    sb.Append(first);

                    if (pageInfo.Page > 5)
                    {
                        for (int dot = 1; dot < 4; dot++)
                        {
                            TagBuilder span = new TagBuilder("span");
                            span.InnerHtml = ".";
                            sb.Append(span);
                        }
                    }

                    for (int i = pageInfo.Page - 3; i <= pageInfo.Page; i++)
                    {
                        if (i > 1)
                        {
                            TagBuilder tag = new TagBuilder("A");
                            tag.MergeAttribute("href", pageUrl(i));
                            if (i == pageInfo.Page)
                            {
                                tag.MergeAttribute("class", "selected");
                            }
                            tag.InnerHtml = i.ToString();
                            sb.Append(tag);
                        }
                    }

                    for (int i = pageInfo.Page + 1; i <= pageInfo.Page + 3; i++)
                    {
                        if (i < pageInfo.Pages)
                        {
                            TagBuilder tag = new TagBuilder("A");
                            tag.MergeAttribute("href", pageUrl(i));
                            tag.InnerHtml = i.ToString();
                            sb.Append(tag);
                        }
                    }

                    if (pageInfo.Pages - pageInfo.Page > 4)
                    {
                        for (int dot = 1; dot < 4; dot++)
                        {
                            TagBuilder span = new TagBuilder("span");
                            span.InnerHtml = ".";
                            sb.Append(span);
                        }
                    }

                    if (!pageInfo.IsLastPage())
                    {
                        TagBuilder last = new TagBuilder("A");


                        last.MergeAttribute("href", pageUrl(pageInfo.Pages));
                        last.InnerHtml = pageInfo.Pages.ToString();

                        sb.Append(last);
                    }
                    TagBuilder next = new TagBuilder("A");
                    next.MergeAttribute("href", pageInfo.Page < pageInfo.Pages ? pageUrl(pageInfo.Page + 1) : pageUrl(pageInfo.Pages));
                    if (pageInfo.Page == pageInfo.Pages)
                    {
                        next.MergeAttribute("class", "no-await");
                    }
                    TagBuilder arrow_next = new TagBuilder("IMG");
                    if (pageInfo.Page == pageInfo.Pages)
                    {
                        arrow_next.MergeAttribute("src", url.Content("~/Content/img/next_p.gif"));
                    }
                    else
                    {
                        arrow_next.MergeAttribute("src", url.Content("~/Content/img/n_p.png"));
                    }
                    arrow_next.MergeAttribute("title", "Следующая");
                    arrow_next.MergeAttribute("alt", "Следующая");
                    next.InnerHtml = arrow_next.ToString();
                    sb.Append(next);
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString PageButtons(this HtmlHelper html, PageData pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder sb = new StringBuilder();
            if (pageInfo.TotalItems > pageInfo.PageSize)
            {
                if (pageInfo != null)
                {
                    UrlHelper url = new UrlHelper(html.ViewContext.RequestContext);
                    TagBuilder back = new TagBuilder("A");
                    back.MergeAttribute("href", pageInfo.Page > 1 ? pageUrl(pageInfo.Page - 1) : pageUrl(1));
                    TagBuilder arrow_back = new TagBuilder("IMG");
                    arrow_back.MergeAttribute("src", url.Content("~/Content/images/back_p.gif"));
                    arrow_back.MergeAttribute("title", "Предыдущая");
                    arrow_back.MergeAttribute("alt", "Предыдущая");
                    back.InnerHtml = arrow_back.ToString();
                    sb.Append(back);

                    TagBuilder first = new TagBuilder("A");
                    first.MergeAttribute("href", pageUrl(1));
                    if (pageInfo.IsFirstPage())
                        first.MergeAttribute("class", "selected");

                    first.InnerHtml = "1";

                    sb.Append(first);

                    if (pageInfo.Page > 5)
                    {
                        for (int dot = 1; dot < 4; dot++)
                        {
                            TagBuilder span = new TagBuilder("span");
                            span.InnerHtml = ".";
                            sb.Append(span);
                        }
                    }

                    for (int i = pageInfo.Page - 3; i <= pageInfo.Page; i++)
                    {
                        if (i > 1)
                        {
                            TagBuilder tag = new TagBuilder("A");
                            tag.MergeAttribute("href", pageUrl(i));
                            if (i == pageInfo.Page)
                            {
                                tag.MergeAttribute("class", "selected");
                            }
                            tag.InnerHtml = i.ToString();
                            sb.Append(tag);
                        }
                    }

                    for (int i = pageInfo.Page + 1; i <= pageInfo.Page + 3; i++)
                    {
                        if (i < pageInfo.Pages)
                        {
                            TagBuilder tag = new TagBuilder("A");
                            tag.MergeAttribute("href", pageUrl(i));
                            tag.InnerHtml = i.ToString();
                            sb.Append(tag);
                        }
                    }

                    if (pageInfo.Pages - pageInfo.Page > 4)
                    {
                        for (int dot = 1; dot < 4; dot++)
                        {
                            TagBuilder span = new TagBuilder("span");
                            span.InnerHtml = ".";
                            sb.Append(span);
                        }
                    }

                    if (!pageInfo.IsLastPage())
                    {
                        TagBuilder last = new TagBuilder("A");


                        last.MergeAttribute("href", pageUrl(pageInfo.Pages));
                        last.InnerHtml = pageInfo.Pages.ToString();

                        sb.Append(last);
                    }
                    TagBuilder next = new TagBuilder("A");
                    next.MergeAttribute("href", pageInfo.Page < pageInfo.Pages ? pageUrl(pageInfo.Page + 1) : pageUrl(pageInfo.Pages));
                    TagBuilder arrow_next = new TagBuilder("IMG");
                    arrow_next.MergeAttribute("src", url.Content("~/Content/images/next_p.gif"));
                    arrow_next.MergeAttribute("title", "Следующая");
                    arrow_next.MergeAttribute("alt", "Следующая");
                    next.InnerHtml = arrow_next.ToString();
                    sb.Append(next);
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
   }
}