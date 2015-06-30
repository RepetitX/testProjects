using System.ComponentModel.DataAnnotations;

namespace Repetit.Tasks.Task1.WebUI.Models
{
    public class AddItemModel
    {
        [Required]
        public int OrderID { get;set;}

        [Required(ErrorMessage = "Необходимо выбрать продукт")]
        [Range(1, int.MaxValue, ErrorMessage="Необходимо выбрать продукт")]
        public int ProductID { get; set; }

         [Required(ErrorMessage = "Необходимо выбрать кол-во")]
         [Range(1, int.MaxValue, ErrorMessage = "Необходимо выбрать кол-во")]
        public int Number { get; set; }
    }
}