using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Orders = new HashSet<Order>();
        }

        public ApplicationUser(string userName)
            : base(userName)
        {
            Orders = new HashSet<Order>();
        }

        #region Navigation properties

        [ScaffoldColumn(false)]
        public virtual ICollection<Order> Orders { get; set; }

        #endregion
    }
}