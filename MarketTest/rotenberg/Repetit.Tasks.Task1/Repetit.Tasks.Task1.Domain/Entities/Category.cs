using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Repetit.Tasks.Task1.Domain.Entities
{
  [Table(Name="Categories")]
  public  class Category
    {
       [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int CategoryID { get; set; }

        [Column]
        public string Name { get; set; }

        [Association(OtherKey = "CategoryID")]
        private EntitySet<Product> _products;

       public List<Product> Products
        {
            get
            {
                return _products.ToList();
            }
        }
    }
}
