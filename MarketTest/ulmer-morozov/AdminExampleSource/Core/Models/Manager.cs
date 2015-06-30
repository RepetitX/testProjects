using System;

// Для работы конструктора EF
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Core.Models
{
    public class Manager : IEquatable<Manager>
    {
        private Manager() { }

        public Manager(int id, Name name, string image)
        {
            Id = id;
            Name = name;
            Image = image;
        }

        public int Id { get; private set; }
        public Name Name { get; private set; }
        public string Image { get; private set; }

        public static Manager Create(int id, string lastName, string firstName, string middleName, string image)
        {
            var manager = new Manager(id, new Name(lastName, firstName, middleName), image);
            return manager;
        }

        public bool Equals(Manager other)
        {
            if (!Id.Equals(other.Id)) return false;
            if (!Name.Equals(other.Name)) return false;
            if (!Image.Equals(other.Image)) return false;
            return true;
        }
    }
}