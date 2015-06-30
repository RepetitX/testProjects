using System.ComponentModel.DataAnnotations;
using Common.Web.Mvc.Resources;

namespace IShop.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}