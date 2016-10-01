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
    }
}