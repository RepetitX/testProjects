using System.Web.Mvc;
using System.Web.Mvc.Html;
using IShop.ViewModels;
using Common.Web.Mvc.Controls;

namespace IShop.GridModels
{
    public class AdminUserGrid : GridModel<EditAdminUserViewModel>
    {
        public AdminUserGrid(HtmlHelper html)
        {
            Column.For(m => html.DeleteLinkWithAntiForgery(m.Id)).Attributes(@class => "mvcgrid-options-column").DoNotEncode();
            Column.For(m => html.EditLink(m.Id, m.UserName, "Edit", new [] { "modal-form" })).Sortable("UserName").Named("Имя пользователя").DoNotEncode();
            Column.For(m => m.RoleName).Sortable(false).Named("Роль");
            Column.For(m => m.Email).Named("E-mail");
            Column.For(m => m.LockoutEnabled ? "<span class=\"label label-danger\">Заблокирован</span>" : "<span class=\"label label-success\">Активен</span>").Sortable("LockoutEnabled").Named("Статус аккаунта").DoNotEncode();
            //Column.For(m => html.ActionLink("Сбросить пароль", "ResetPassword", "Security", new { area = "AZ", id = m.Id }, new { @class = "btn btn-mb modal-form" })).DoNotEncode();
        }
    }
}