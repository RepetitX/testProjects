using System;

// Чтоб работал конструктор EF
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Core.Models
{
    public class Name : IEquatable<Name>
    {
        private Name()
        {

        }

        public Name(string last, string first, string middle)
        {
            Last = last;
            First = first;
            Middle = middle;
        }

        public string First { get; private set; }
        public string Middle { get; private set; }
        public string Last { get; private set; }

        public bool Equals(Name other)
        {
            if (First != other.First) return false;
            if (Middle != other.First) return false;
            if (Last != other.Last) return false;
            return true;
        }
    }
}