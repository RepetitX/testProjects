using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Repetit.Tasks.Task1.Domain.Entities
{
  [Table(Name = "Options")]
   public class Option
    {
     [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
     public int OptionID { get; set; }

     [Column]
     public int ProductID { get; set; }

      private EntityRef<Product> _product;

      [Association(ThisKey = "ProductID", Storage = "_product", IsForeignKey = true)]
      public Product Product
      {
          get
          {
              return _product.Entity;
          }
          set 
          { 
              _product.Entity = value;
              ProductID = value.ProductID;
          }
      }

      [Column]
      public string Name { get; set; }

      [Column]
      public decimal Price { get; set; }
   
    }
}
