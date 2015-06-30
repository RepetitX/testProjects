using System.Collections.Generic;
using Repetit.Tasks.Task1.Domain.Entities;

namespace Repetit.Tasks.Task1.WebUI.Models
{
    public class ListItemsModel
    {
        public IEnumerable<OrderItem> Items { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
    }
}