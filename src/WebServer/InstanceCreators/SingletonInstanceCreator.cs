using Devkoes.Restup.WebServer.Models.Contracts;
using System;

namespace Devkoes.Restup.WebServer.InstanceCreators
{
    public class SingletonInstanceCreator<T> : IInstanceCreator<T> where T : class
    {
        public T Create(params object[] args)
        {
            return Activator.CreateInstance<T>();
        }
    }
}
