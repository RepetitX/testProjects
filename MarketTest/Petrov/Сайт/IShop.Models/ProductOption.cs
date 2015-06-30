using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Common.Web.Mvc;
using Common.Web.Mvc.Attributes;
using Common.Web.Mvc.Autofac;
using Common.Web.Mvc.Entities;
using Common.Web.Mvc.Resources;

namespace IShop.Models
{
    public class ProductOption : NamedEntityBase<int>
    {
        #region Navigation properties

        #region ProductType

        [Display(Name = "Тип товара")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [DropDownList("ProductTypeDictionary")]
        public int ProductTypeId { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> ProductTypeDictionary
        {
            get
            {
                var productTypeRepository = AutofacLifetimeScope.GetRepository<ProductType, int>();

                var result = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "",
                        Text = ""
                    }
                };

                result.AddRange(productTypeRepository.GetQuery().ToSelectList(pt => pt.Id, pt => pt.Name, pt => ProductTypeId == pt.Id));

                return result;
            }
        }

        [ScaffoldColumn(false)]
        public virtual ProductType ProductType { get; set; }

        #endregion

        [ScaffoldColumn(false)]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Опция товара";
        }

        #endregion
    }
}