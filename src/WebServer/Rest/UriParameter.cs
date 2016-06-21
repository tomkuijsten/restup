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
    }
}