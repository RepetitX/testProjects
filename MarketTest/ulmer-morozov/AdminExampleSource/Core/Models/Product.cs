using System;
using System.Collections.Generic;
using System.Linq;

// Чтобы Entity Framework мог создавать экземпляры
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Core.Models
{
    public class Product :  IEquatable<Product>
    {
        private Product()
        {
            Accessorises = new List<Product>();
        }

        public Product(int id, string title, string description, string image, int price, ProductType type = ProductType.Product)
        {
            Id = id;
            Title = title;
            Description = description;
            Image = image;
            Price = price;
            Type = type;
            Accessorises = new List<Product>();
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }
        public int Price { get; private set; }
        public ProductType Type { get; private set; }

        public virtual List<Product> Accessorises { get; private set; }

        public void AddAccessorise(Product product)
        {
            Accessorises.Add(product);
        }

        public bool HaveAccessorises { get { return Accessorises.Any(); } }

        public bool Equals(Product other)
        {
            if (!Id.Equals(other.Id)) return false;
            if (!Title.Equals(other.Title)) return false;
            if (!Description.Equals(other.Description)) return false;
            if (!Price.Equals(other.Price)) return false;
            return true;
        }
    }
}