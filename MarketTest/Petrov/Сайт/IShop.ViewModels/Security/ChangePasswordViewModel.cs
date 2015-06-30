using System.ComponentModel.DataAnnotations;
using Common.Web.Mvc.Resources;

namespace IShop.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(100, ErrorMessage = "Длина поля не должна быть минимум {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль еще раз")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
