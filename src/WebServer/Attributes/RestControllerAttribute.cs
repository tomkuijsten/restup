using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServer.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class RestControllerAttribute : Attribute
    {
        public InstanceCreationType InstanceCreationType { get; set; }

        public RestControllerAttribute(InstanceCreationType instanceCreation)
        {
            InstanceCreationType = instanceCreation;
        }
    }
}
