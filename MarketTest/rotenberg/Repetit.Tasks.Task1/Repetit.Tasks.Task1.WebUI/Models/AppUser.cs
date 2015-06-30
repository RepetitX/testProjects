using Microsoft.AspNet.Identity.EntityFramework;

namespace Repetit.Tasks.Task1.WebUI.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }
    }
}