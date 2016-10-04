using System;

namespace Restup.Webserver.Rest
{
    internal class UriParameter
    {
        public string Name { get; }
        public string Value { get; }

        public UriParameter(string name)
        {
            Name = name;
        }

        public UriParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        protected bool Equals(UriParameter other)
        {
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UriParameter) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0)*397) ^ (Value?.GetHashCode() ?? 0);
            }
        }
    }
}