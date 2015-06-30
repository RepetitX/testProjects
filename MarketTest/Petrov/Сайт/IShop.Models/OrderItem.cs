using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using Common.Web.Mvc;
using Common.Web.Mvc.Attributes;
using Common.Web.Mvc.Autofac;
using Common.Web.Mvc.Entities;
using Common.Web.Mvc.Resources;

namespace IShop.Models
{
    public class OrderItem : EntityBase<int>
    {
        #region Navigation properties

        #region Order

        [HiddenInput(DisplayValue = false)]
        public Guid OrderId { get; set; }

        [ScaffoldColumn(false)]
        public virtual Order Order { get; set; }

        #endregion

        #region ProductType

        [Display(Name = "Тип товара")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [DropDownList("ProductTypeDictionary")]
        public int ProductTypeId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ProductType ProductType { get; set; }

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

        #endregion

        #region ProductOption

        [NotMapped]
        [ScaffoldColumn(false)]
        public List<int> ProductOptionList { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> ProductOptionDictionary { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<ProductOption> ProductOptions { get; set; }

        #endregion

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Позиция в заказе"; //Прошу прощения лучше названия не придумал :)
        }

        #endregion
    }
}