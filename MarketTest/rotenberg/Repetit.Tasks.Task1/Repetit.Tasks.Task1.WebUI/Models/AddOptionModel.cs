using System.ComponentModel.DataAnnotations;

namespace Repetit.Tasks.Task1.WebUI.Models
{
    public class AddOptionModel
    {
        [Required(ErrorMessage="Обязательное поле")]
        public int OrderItemID { get; set; }

        [Required(ErrorMessage = "Вы не выбрали опцию")]
        public int OptionID { get; set; }

        [Required(ErrorMessage="Вы не указали количество")]
        public int Number { get; set; }

        [Required(ErrorMessage="Отсутствует номер заказа")]
        public int OrderID {get;set;}

        [Required(ErrorMessage = "Продукт-обязательное поле")]
        public int ProductID { get; set; }
    }
}