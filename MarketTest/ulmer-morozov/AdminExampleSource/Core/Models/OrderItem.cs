using System;

// Для работы конструктора Entity
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Core.Models
{
    public class OrderItem : IEquatable<OrderItem>
    {
        private OrderItem() { }

        public OrderItem(int orderId, Product product, int count)
        {
            OrderId = orderId;
            Product = product;
            ProductId = product.Id;
            Count = count;
        }

        public virtual Product Product { get; private set; }
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public int Count { get; private set; }

        public bool Equals(OrderItem other)
        {
            if (!OrderId.Equals(other.OrderId)) return false;
            if (!ProductId.Equals(other.ProductId)) return false;
            if (!Count.Equals(other.Count)) return false;
            return true;
        }


        public void ClearEntity()
        {
            Product = null;
        }

        public void SetOrderId(int id)
        {
            OrderId = id;
        }
    }
}