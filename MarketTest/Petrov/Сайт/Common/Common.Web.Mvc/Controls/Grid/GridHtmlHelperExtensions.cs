using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Linq.Expressions;
using Microsoft.Web.Mvc;
using Common.Web.Mvc.Controls.Syntax;

namespace Common.Web.Mvc.Controls
{
    public static class GridHtmlHelperExtensions
    {
        /// <summary>
        /// Создает чекбокс для выделения строки
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string RowCheckbox(this HtmlHelper html, object id)
        {
            var checkboxTag = new TagBuilder("input");
            checkboxTag.MergeAttribute("type", "checkbox");
            checkboxTag.MergeAttribute("value", id.ToString());
            checkboxTag.MergeAttribute("id", "row-checkbox_" + id.ToString());
            checkboxTag.AddCssClass("row-checkbox");

            return checkboxTag.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Создает ссылку для удаления элемента (с иконкой мусорного ведра)
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteLink(this HtmlHelper html, object id)
        {
            var aTag = new TagBuilder("a");
            var url = new UrlHelper(html.ViewContext.RequestContext);
            aTag.MergeAttribute("href", url.Action("Delete", new { id = id }));
            aTag.MergeAttribute("id", "del_" + id.ToString());
            aTag.MergeAttribute("title", "Удалить");
            aTag.AddCssClass("mvcgrid-delete-link");

            aTag.InnerHtml = "<span class=\"glyphicon glyphicon-trash\"></span>";

            return aTag.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Создает ссылку для удаления элемента с токеном безопасности против CSRF атак (с иконкой мусорного ведра)
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteLinkWithAntiForgery(this HtmlHelper html, object id)
        {
            return AntiForgery.GetHtml() + DeleteLink(html, id);
        }

        /// <summary>
        /// Создает ссылку для удаления элемента (с иконкой мусорного ведра)
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string DeleteLink(this HtmlHelper html, object id, string action)
        {
            var aTag = new TagBuilder("a");
            var url = new UrlHelper(html.ViewContext.RequestContext);
            aTag.MergeAttribute("href", url.Action(action, new { id = id }));
            aTag.MergeAttribute("id", "del_" + id.ToString());
            aTag.MergeAttribute("title", "Удалить");
            aTag.AddCssClass("mvcgrid-delete-link");

            aTag.InnerHtml = "<span class=\"glyphicon glyphicon-trash\"></span>";

            return aTag.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Создает ссылку для удаления элемента с токеном безопасности против CSRF атак (с иконкой мусорного ведра)
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string DeleteLinkWithAntiForgery(this HtmlHelper html, object id,string action)
        {
            return AntiForgery.GetHtml() + DeleteLink(html, id, action);
        }

        /// <summary>
        /// Создает ссылку для редактирования элемента (с иконкой)
        /// </summary>
        public static string Link(this HtmlHelper html, object id, string text, string action, string controller, IEnumerable<string> cssClass = null)
        {
            var aTag = new TagBuilder("a");
            var url = new UrlHelper(html.ViewContext.RequestContext);
            aTag.MergeAttribute("href", url.Action(action, controller, new { id = id }));
            aTag.MergeAttribute("id", "edit_" + id);

            aTag.SetInnerText(text);

            if (cssClass != null)
            {
                foreach (var css in cssClass)
                    aTag.AddCssClass(css);
            }

            return aTag.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Создает ссылку для редактирования элемента (с иконкой)
        /// </summary>
        public static string EditLink(this HtmlHelper html, object id, IEnumerable<string> cssClass = null)
        {
            var aTag = new TagBuilder("a");
            var url = new UrlHelper(html.ViewContext.RequestContext);
            aTag.MergeAttribute("href", url.Action("Edit", new { id = id }));
            aTag.MergeAttribute("id", "edit_" + id);
            aTag.MergeAttribute("title", "Редактировать");
            aTag.AddCssClass("mvcgrid-edit-link");

            if (cssClass != null)
            {
                foreach (var css in cssClass)
                    aTag.AddCssClass(css);
            }

            aTag.InnerHtml = "<span class=\"glyphicon glyphicon-pencil\"></span>";

            return aTag.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Создает ссылку для редактирования элемента (с текстом)
        /// </summary>
        public static string EditLink(this HtmlHelper html, object id, string text, IEnumerable<string> cssClass = null)
        {
            var aTag = new TagBuilder("a");
            var url = new UrlHelper(html.ViewContext.RequestContext);
            aTag.MergeAttribute("href", url.Action("Edit", new { id = id }));
            aTag.MergeAttribute("id", "edit_" + id);

            aTag.SetInnerText(text);

            if (cssClass != null)
            {
                foreach (var css in cssClass)
                    aTag.AddCssClass(css);
            }

            return aTag.ToString(TagRenderMode.Normal);
        }

        public static string EditLink(this HtmlHelper html, object id, string text, string action, IEnumerable<string> cssClass = null)
        {
            var aTag = new TagBuilder("a");
            var url = new UrlHelper(html.ViewContext.RequestContext);
            aTag.MergeAttribute("href", url.Action(action, new { id = id }));
            aTag.MergeAttribute("id", "edit_" + id);

            aTag.SetInnerText(text);

            if (cssClass != null)
            {
                foreach (var css in cssClass)
                    aTag.AddCssClass(css);
            }

            return aTag.ToString(TagRenderMode.Normal);
        }

        public static string EditLink(this HtmlHelper html, object id, string text, string action, string controller, IEnumerable<string> cssClass = null)
        {
            var aTag = new TagBuilder("a");
            var url = new UrlHelper(html.ViewContext.RequestContext);
            aTag.MergeAttribute("href", url.Action(action, controller, new { id = id }));
            aTag.MergeAttribute("id", "edit_" + id);

            aTag.SetInnerText(text);

            if (cssClass != null)
            {
                foreach (var css in cssClass)
                    aTag.AddCssClass(css);
            }

            return aTag.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Создает картинку для перетаскивания строк в таблице
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string DragHandleImage(this HtmlHelper html)
        {
            var aTag = new TagBuilder("a");

            aTag.MergeAttribute("title", "Отсортировать");
            aTag.AddCssClass("mvcgrid-move-link");

            aTag.InnerHtml = "<span class=\"glyphicon glyphicon-sort\"></span>";

            return aTag.ToString(TagRenderMode.Normal);
        }

        public static string SearchString<TController>(this HtmlHelper html, Expression<Action<TController>> action, string updatePanelId)
            where TController: Controller
        {
            var div = new TagBuilder("div");
            div.AddCssClass("search");

            var form = new TagBuilder("form");
            form.MergeAttribute("action", LinkBuilder.BuildUrlFromExpression(html.ViewContext.RequestContext, html.RouteCollection, action));
            form.MergeAttribute("target", updatePanelId);
            form.AddCssClass("ajax-form");

            var input = new TagBuilder("input");
            input.MergeAttribute("name", "SearchString");

            var button = new TagBuilder("button");
            button.MergeAttribute("type", "submit");
            
            form.InnerHtml += input.ToString();
            form.InnerHtml += button.ToString();
            div.InnerHtml = form.ToString();

            return div.ToString();
        }

        public static IGrid<T> GridView<T>(this HtmlHelper helper, IEnumerable<T> dataSource) where T : class
        {
            return new Grid<T>(dataSource, helper.ViewContext.Writer, helper.ViewContext);
        }
    }
}
