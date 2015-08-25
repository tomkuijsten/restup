using System;

namespace Devkoes.Restup.WebServer.Attributes
{
    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class FromBodyAttribute : Attribute
    {
        // acts as a mark
    }
}
