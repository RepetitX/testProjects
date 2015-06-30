using System.ComponentModel.DataAnnotations;
using Common.Web.Mvc.Resources;

namespace IShop.ViewModels
{
    public class AdminUserManageViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "EmailIncorrect")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public override string ToString()
        {
            return "Профиль";
        }
    }
}