using System.Web.Mvc;
using IShop.GridModels;
using IShop.Models;
using Common.Web.Mvc;
using Common.Web.Mvc.Services;

namespace IShop.Controllers.Areas.AZ.Controllers
{
    [Authorize]
    public class ProductOptionController : FiltrateCRUDController<ProductOption, int, ProductOptionGrid, ProductOptionGridOptions, ProductOptionFilter>
    {
        public ProductOptionController(IFilterableBaseService<ProductOption, int, ProductOptionGrid, ProductOptionGridOptions, ProductOptionFilter> service)
            : base(service)
        {
        }

        public override string ToString()
        {
            return "Опции товаров";
        }
    }
}