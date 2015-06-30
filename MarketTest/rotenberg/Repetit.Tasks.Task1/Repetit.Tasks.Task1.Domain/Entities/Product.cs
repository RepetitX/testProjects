using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Repetit.Tasks.Task1.Domain.Entities
{
    [Table(Name="Products")]
   public class Product
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int ProductID { get; set; }

        [Column]
        [Required]
        public string Name { get; set; }

        [Column]
        [Required]
        public int CategoryID { get; set; }

        [Column]
        [Required]
        public decimal Price { get; set; }

        private EntityRef<Category> _category;

        [System.Data.Linq.Mapping.Association(ThisKey = "CategoryID", Storage = "_category", IsForeignKey = true)]
        public Category Category
        {
            get
            {
                return _category.Entity;
            }
            set
            {
                _category.Entity = value;
                CategoryID = value.CategoryID;
            }
        }
        [System.Data.Linq.Mapping.Association(OtherKey = "ProductID")]
        private EntitySet<Option> _options;

        public List<Option> Options
        {
            get
            {
                return _options.ToList();
            }
        }

        [System.Data.Linq.Mapping.Association(OtherKey = "ProductID")]
        private EntitySet<OrderItem> _orderItems;

        public List<OrderItem> OrderItems
        {
            get
            {
                return _orderItems.ToList();
            }
        } 
    }
}
