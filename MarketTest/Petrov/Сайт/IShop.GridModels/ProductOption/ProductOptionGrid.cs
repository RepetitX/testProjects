using System.Web.Mvc;
using IShop.Models;
using Common.Web.Mvc.Controls;

namespace IShop.GridModels
{
    public class ProductOptionGrid : GridModel<ProductOption>
    {
        public ProductOptionGrid(HtmlHelper html)
        {
            Column.For(m => html.DeleteLinkWithAntiForgery(m.Id)).Attributes(@class => "mvcgrid-options-column").DoNotEncode();
            Column.For(m => html.EditLink(m.Id, m.Name, "Edit", new[] { "modal-form" })).Sortable("Name").Named("Название").DoNotEncode();
            Column.For(m => m.ProductType.Name).Sortable("ProductType.Name").Named("Название товара").DoNotEncode();
        }
    }
}