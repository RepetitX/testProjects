using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Data;
using Common.Web.Mvc.Resources;

namespace Common.Web.Mvc.Entities
{
    public class NamedEntityBase<TKey> : INamedEntityBase<TKey>
    {
        [HiddenInput(DisplayValue = false)]
        public TKey Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "Название", Order = 0)]
        public string Name { get; set; }
    }
}
