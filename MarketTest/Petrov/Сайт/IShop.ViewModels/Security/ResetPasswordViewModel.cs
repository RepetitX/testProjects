using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Web.Mvc.Resources;

namespace IShop.ViewModels
{
    public class ResetPasswordViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [UIHint("Text")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(100, ErrorMessage = "Длина поля не должна быть минимум {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль еще раз")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}