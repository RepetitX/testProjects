using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Web.Mvc.Attributes;
using Common.Web.Mvc.Resources;

namespace Common.Web.Mvc.Controls.Filter
{
    public class FilterEdit
    {
        [DropDownList("Filters")]
        [Display(Name = "Сохранить как")]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> Filters { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(255, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "Название фильтра")]
        public string Name { get; set; }

        [Display(Name = "Общий фильтр")]
        public bool IsShared { get; set; }
    }
}