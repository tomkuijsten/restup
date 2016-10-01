using Restup.Webserver.Models.Schemas;
using System;

namespace Restup.Webserver.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RestControllerAttribute : Attribute
    {
        public InstanceCreationType InstanceCreationType { get; }

        public RestControllerAttribute(InstanceCreationType instanceCreation)
        {
            InstanceCreationType = instanceCreation;
        }
    }
}
