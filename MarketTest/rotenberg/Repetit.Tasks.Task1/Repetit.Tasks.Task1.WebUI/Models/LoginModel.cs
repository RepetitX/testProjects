using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repetit.Tasks.Task1.WebUI.Models
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}