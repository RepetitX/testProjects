using System.Web.Mvc;
using IShop.GridModels;
using IShop.Models;
using Common.Web.Mvc;
using Common.Web.Mvc.Services;

namespace IShop.Controllers.Areas.AZ.Controllers
{
    [Authorize]
    public class ProductTypeController : CRUDController<ProductType, int, ProductTypeGrid, ProductTypeGridOptions>
    {
        public ProductTypeController(IBaseService<ProductType, int, ProductTypeGrid, ProductTypeGridOptions> service)
            : base(service)
        {
        }

        public override string ToString()
        {
            return "Типы товаров";
        }
    }
}