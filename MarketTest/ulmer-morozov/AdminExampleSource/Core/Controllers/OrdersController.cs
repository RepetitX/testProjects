using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Interfaces;
using Core.Models;

namespace Core.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        public async Task<IEnumerable<Order>> Get(int pageSize, int offset)
        {
            var orders = await _orderRepository.GetMany(pageSize, offset);
            return orders;
        }

        public async Task<Order> Get(int id)
        {
            var order = await _orderRepository.GetOne(id);
            return order;
        }

        public async Task<Order> Put(Order order)
        {
            var storedOrder = await _orderRepository.Store(order);
            return storedOrder;
        }

        public async Task Delete(int id)
        {
            await _orderRepository.Delete(id);
        }
    }
}