using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using Common.Core;
using Common.Data;
using Common.Web.Mvc;
using Common.Web.Mvc.Attributes;
using IShop.Common.Enums;
using IShop.Models.Attributes;
using Common.Web.Mvc.Resources;

namespace IShop.Models
{
    public class Order : IEntityBase<Guid>
    {
        [Display(Name = "Номер заказа")]
        public Guid Id { get; set; }

        #region Контактные данные

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [DropDownList("GendersDictionary")]
        [Display(Name = "Обращение")]
        public Gender Gender { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> GendersDictionary
        {
            get
            {
                return Enum.GetValues(typeof(Gender)).Cast<Gender>().ToSelectList(g => g, g => g.GetDescription(), g => g == Gender);
            }
        }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [EmailRequired(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")] //Обязательно для заполнения что-то одно либо телефон либо Email
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "EmailIncorrect")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [PhoneRequired(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")] //Обязательно для заполнения что-то одно либо телефон либо Email
        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        #endregion

        [Display(Name = "Адрес доставки")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(500, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        public string Address { get; set; }

        [Display(Name = "Комментарии")]
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(500, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "StringLengthIncorrect")]
        public string Comments { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreateDateTime { get; set; }

        [ScaffoldColumn(false)]
        public string UserManagerId { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string UserManagerName { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> UserManagerDictionary { get; set; }

        #region Navigation properties

        [ScaffoldColumn(false)]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        [ScaffoldColumn(false)]
        public virtual ApplicationUser UserManager { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Заказ";
        }

        #endregion


    }
}