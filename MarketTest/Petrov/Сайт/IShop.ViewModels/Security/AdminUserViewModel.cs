using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Web.Mvc.Attributes;
using Common.Web.Mvc.Resources;

namespace IShop.ViewModels
{
    public class AdminUserViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "Имя пользователя", Order = 0)]
        public string UserName { get; set; }

        [ScaffoldColumn(false)]
        public string RoleName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [DropDownList("RolesDictionary")]
        [Display(Name = "Роль", Order = 3)]
        public string RoleId { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> RolesDictionary { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "EmailIncorrect")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "E-mail", Order = 4)]
        public string Email { get; set; }

        public override string ToString()
        {
            return "Пользователь";
        }
    }

    public class CreateAdminUserViewModel : AdminUserViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(100, ErrorMessage = "Длина поля не должна быть минимум {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Order = 1)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль еще раз", Order = 2)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditAdminUserViewModel : AdminUserViewModel
    {
        [Display(Name = "Заблокировать пользователя", Order = 6)]
        public bool LockoutEnabled { get; set; }
    }
}