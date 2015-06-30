using System.Linq;
using Repetit.Tasks.Task1.Domain.Entities;

namespace Repetit.Tasks.Task1.Domain.Abstract
{
   public interface IOrdersRepository
    {
       IQueryable<Order> Orders { get; }
       IQueryable<Category> Categories { get; }
       IQueryable<Product> Products { get; }
       bool UpdateOrder(Order newOrder);
       bool AddProduct(int ProductID, int Number, int OrderID);
       IQueryable<OrderItem> OrderItems { get; }
       bool EditProduct(OrderItem item);
       bool DeleteProductFromOrder(int OrderItemID);
       bool DeleteOption(int OrderOptionsID);
       bool EditOption(OrderOption option);
       bool AddOptionToItem(int OrderItemID, int OptionID, int Number, int ProductID, int OrderID);
       IQueryable<OrderOption> OrderOptions { get; }
       IQueryable<Option> Options { get; }
    }
}
