using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common.Web.Mvc;
using Common.Web.Mvc.Autofac;
using Common.Web.Mvc.Controls;
using IShop.Models;

namespace IShop.GridModels
{
    public class ProductOptionFilter : Filter<ProductOption>
    {
        protected override Filter<ProductOption> Configure()
        {
            #region ProductType

            var productTypeRepository = AutofacLifetimeScope.GetRepository<ProductType, int>();

            var productTypes = new List<SelectListItem>();

            productTypes.AddRange(productTypeRepository.GetQuery().OrderBy(c => c.Name).ToSelectList(i => i.Id, i => i.Name).ToList());

            if (productTypes.Any())
                AddCondition(new SelectFilterCondition<ProductOption, int>(c => c.ProductTypeId, productTypes, "Тип товара"));

            #endregion

            #region Name

            AddCondition(new StringFilterCondition<ProductOption, string>(c => c.Name, "Название"));

            #endregion

            return this;
        }
    }
}
