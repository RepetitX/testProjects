using System;

// для работы конструктора EF
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Core.Models
{
    public class Address : IEquatable<Address>
    {
        private Address() { }

        public Address(string country, string town, string street, int building)
        {
            Country = country;
            Town = town;
            Street = street;
            Building = building;
        }

        public string Country { get; private set; }
        public string Town { get; private set; }
        public string Street { get; private set; }
        public int Building { get; private set; }

        public bool Equals(Address other)
        {
            if (!Country.Equals(other.Country)) return false;
            if (!Town.Equals(other.Town)) return false;
            if (!Street.Equals(other.Street)) return false;
            if (!Building.Equals(other.Building)) return false;
            return true;
        }
    }
}