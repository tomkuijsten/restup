using System;

namespace Restup.Webserver.Attributes
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class UriFormatAttribute : Attribute
    {
        public string UriFormat { get; }

        public UriFormatAttribute(string uriFormat)
        {
            this.UriFormat = uriFormat;
        }
    }
}
