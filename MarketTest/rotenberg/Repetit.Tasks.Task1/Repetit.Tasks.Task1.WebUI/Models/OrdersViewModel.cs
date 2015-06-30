using System.Collections.Generic;
using Repetit.Tasks.Task1.Domain.Entities;

namespace Repetit.Tasks.Task1.WebUI.Models
{
    public class OrdersViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public PageData PageInfo { get; set; }
        public AppUser CurrenUser { get; set; }
    }
}