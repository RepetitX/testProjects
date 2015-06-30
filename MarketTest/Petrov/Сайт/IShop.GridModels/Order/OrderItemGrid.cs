using System.Web.Mvc;
using IShop.Models;
using Common.Web.Mvc.Controls;

namespace IShop.GridModels
{
    public class OrderItemGrid : GridModel<OrderItem>
    {
        public OrderItemGrid(HtmlHelper html)
        {
            Column.For(m => html.DeleteLinkWithAntiForgery(m.Id, "OrderItemsDelete")).Attributes(@class => "mvcgrid-options-column").DoNotEncode();
            Column.For(m => html.EditLink(m.Id, m.ProductType.Name, "OrderItemsEdit", new[] { "modal-form" })).Sortable("ProductType.Name").Named("Название товара").DoNotEncode();
        }
    }
}