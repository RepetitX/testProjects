using System.Web.Mvc;
using IShop.Models;
using Common.Web.Mvc.Controls;

namespace IShop.GridModels
{
    public class ProductTypeGrid : GridModel<ProductType>
    {
        public ProductTypeGrid(HtmlHelper html)
        {
            Column.For(m => html.DeleteLinkWithAntiForgery(m.Id)).Attributes(@class => "mvcgrid-options-column").DoNotEncode();
            Column.For(m => html.EditLink(m.Id, m.Name, "Edit", new [] { "modal-form" })).Sortable("UserName").Named("Название").DoNotEncode();
        }
    }
}