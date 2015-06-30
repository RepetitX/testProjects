using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOne(int id);
        Task<IEnumerable<Order>> GetMany(int pageSize, int offset);
        Task<Order> Store(Order newOrder);
        Task Delete(int id);
    }
}