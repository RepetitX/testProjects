using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Repetit.Tasks.Task1.Domain.Entities
{
    [Table(Name = "OrderOptions")]
   public class OrderOption
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int OrderOptionsID { get; set; }

        [Column]
        public int OrderItemID { get; set; }

        [Column]
        public int OptionID { get; set; }

        [Column]
        [Range(1,int.MaxValue,ErrorMessage="Не менее одной единицы")]
        [Required(ErrorMessage="Вы не указали количество")]
        public int Number { get; set; }

        private EntityRef<OrderItem> _orderItem;

        [System.Data.Linq.Mapping.Association(ThisKey = "OrderItemID", IsForeignKey = true, Storage = "_orderItem")]
        public OrderItem OrderItem
        {
            get { return _orderItem.Entity; }
        }

        private EntityRef<Option> _option;

        [System.Data.Linq.Mapping.Association(ThisKey = "OptionID", IsForeignKey = true, Storage = "_option")]
        public Option Option
        {
            get { return _option.Entity; }
        }
    }
}
