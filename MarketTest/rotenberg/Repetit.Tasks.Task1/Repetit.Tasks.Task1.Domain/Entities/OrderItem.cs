using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Repetit.Tasks.Task1.Domain.Entities
{
    [Table(Name = "OrderItems")]
   public class OrderItem
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int OrderItemID { get; set; }

        [Column]
        [Required(ErrorMessage = "Необходимо выбрать продукт")]
        [Range(1, int.MaxValue, ErrorMessage = "Необходимо выбрать продукт")]
        public int ProductID { get; set; }

        [Column]
        [Required]
        public int OrderID { get; set; }

        private EntityRef<Product> _product;

        [System.Data.Linq.Mapping.Association(ThisKey = "ProductID", Storage = "_product", IsForeignKey = true)]
        public Product Product
        {
            get { return _product.Entity; }
            set {
                try
                {
                    ProductID = value.ProductID;
                    _product.Entity.ProductID = value.ProductID;
                }
                catch
                {
                }
            }
        }

        private EntityRef<Order> _order;

        [System.Data.Linq.Mapping.Association(ThisKey = "OrderID", Storage = "_order", IsForeignKey = true)]
        public Order Order
        {
            get
            {
                return _order.Entity;
            }
            set { OrderID = value.OrderID;
                _order.Entity.OrderID = value.OrderID;
            }
        }

        [Column]
        [Required(ErrorMessage = "Необходимо выбрать кол-во")]
        public int Number { get; set; }

        [System.Data.Linq.Mapping.Association(OtherKey = "OrderItemID")]
        private EntitySet<OrderOption> _orderOptions;

        public List<OrderOption> OrderOptions
        {
            get { return _orderOptions.ToList(); }
        } 
    }
}
