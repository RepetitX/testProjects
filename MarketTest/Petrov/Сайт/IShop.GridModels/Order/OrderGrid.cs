using System.Web.Mvc;
using IShop.Models;
using Common.Web.Mvc.Controls;

namespace IShop.GridModels
{
    public class OrderGrid : GridModel<Order>
    {
        public OrderGrid(HtmlHelper html)
        {
            Column.For(m => html.DeleteLinkWithAntiForgery(m.Id)).Attributes(@class => "mvcgrid-options-column").DoNotEncode();
            Column.For(m => html.EditLink(m.Id, string.Concat(m.Surname, " ", m.Name, " ", m.Patronymic), null)).Sortable("Surname").Named("ФИО Заказчика").DoNotEncode();
            Column.For(m => m.CreateDateTime).Sortable("CreateDateTime").Named("Дата заказа").DoNotEncode();
        }
    }
}