using System;

namespace Restup.Webserver.Rest
{
    public class PathPart
    {
        public PathPartType PartType { get; }
        public string Value { get; }

        public enum PathPartType
        {
            Path,
            Argument,
        }

        public PathPart(PathPartType pathPartType, string value)
        {
            PartType = pathPartType;
            Value = value;
        }

        protected bool Equals(PathPart other)
        {
            return PartType == other.PartType && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PathPart)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)PartType * 397) ^ (Value?.GetHashCode() ?? 0);
            }
        }
    }
}