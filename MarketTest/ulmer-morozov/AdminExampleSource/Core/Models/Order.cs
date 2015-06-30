using System;
using System.Collections.Generic;
using System.Linq;

// Для работы конструктора Entity
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Core.Models
{
    public class Order : IEquatable<Order>
    {
        protected Order()
        {
            Items = new List<OrderItem>();
        }

        public Order(int id, Name clientName, DateTime creationDateTime, Manager manager, Address address, string contacts, string note)
        {
            Id = id;
            CreationDateTime = creationDateTime;
            Address = address;
            Contacts = contacts;
            Note = note;
            ClientName = clientName;
            Manager = manager;
            Items = new List<OrderItem>();
        }

        public int Id { get; private set; }
        public Name ClientName { get; private set; }
        public DateTime CreationDateTime { get; private set; }
        public Address Address { get; private set; }
        public string Contacts { get; private set; }
        public string Note { get; private set; }

        //Сложные свойства, которые заполняет Entity
        public virtual Manager Manager { get; private set; }
        public virtual List<OrderItem> Items { get; private set; }

        public Order AddOrderItem(Product product, int count)
        {
            var orderItem = new OrderItem(Id, product, count);
            Items.Add(orderItem);
            return this;
        }

        public void SetManager(Manager manager)
        {
            Manager = manager;
        }

        public void SetCreationDateTime(DateTime date)
        {
            CreationDateTime = date;
        }

        public bool Equals(Order other)
        {
            if (!Id.Equals(other.Id)) return false;
            if (!CreationDateTime.Equals(other.CreationDateTime)) return false;
            if (!Address.Equals(other.Address)) return false;
            if (!Contacts.Equals(other.Contacts)) return false;
            if (!Note.Equals(other.Note)) return false;
            if (!Manager.Equals(other.Manager)) return false;
            if (!CreationDateTime.Equals(other.CreationDateTime)) return false;
            if (!Items.SequenceEqual(other.Items)) return false;
            return true;
        }

        public void SetItems(IEnumerable<OrderItem> orderItems)
        {
            Items = orderItems.ToList();
        }
    }
}