using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Interfaces;
using Core.Models;

namespace Core.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        // GET api/values
        public async Task<IEnumerable<Product>> Get(int? forProductId = null)
        {
            if (!forProductId.HasValue)
            {
                //возвращаем обычные товары
                var products = await _productRepository.GetMany(x => x.Type == ProductType.Product);
                return products;
            }
            //возвращаем аксессуары к заданному товару
            var product = await _productRepository.GetOne(forProductId.Value);
            if (product == null)
            {
                return new List<Product>();
            }
            return product.Accessorises;
        }
    }
}
