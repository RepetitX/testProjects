using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Web.Mvc.Entities;

namespace IShop.Models
{
    public class ProductType : NamedEntityBase<int>
    {
        #region Navigation properties

        [ScaffoldColumn(false)]
        public virtual ICollection<ProductOption> ProductOptions { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Тип товара";
        }

        #endregion
    }
}