using System.Data.Linq.Mapping;

namespace Repetit.Tasks.Task1.Domain.Entities
{
   [Table(Name="AspNetUsers")]
   public class AspNetUser
    {
       [Column(IsPrimaryKey=true, IsDbGenerated = false, AutoSync= AutoSync.Always)]
       public string Id { get; set; }

        [Column]
        public string FirstName { get; set; }

        [Column]
        public string LastName { get; set; }
    
        [Column]
        public string City { get; set; }

        [Column]
        public string UserName { get; set; }

        [Column]
        public string PhoneNumber { get; set; }
    }
}
