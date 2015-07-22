using Devkoes.Restup.WebServer.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Devkoes.Restup.WebServer.Attributes;

namespace Devkoes.Restup.WebServer
{
    public enum RestVerb
    {
        POST,
        GET,
        PUT,
        DELETE
    }

    public class RestMethodInfo
    {
        public MethodInfo MethodInfo { get; set; }
        public RestVerb Verb { get; set; }
        public IInstanceCreator<IRestController> Instantiator { get; set; }

        public RestMethodInfo(
            MethodInfo methodInfo,
            string uriFormat)
        {
            InitializeVerb(methodInfo);

            // create regular expression to extract parameters
            // create regular expression to see if an uri matches
        }

        private void InitializeVerb(MethodInfo mi)
        {
            var restVerbAttr = mi.ReturnType.GetTypeInfo().GetCustomAttribute<RestVerbAttribute>();
            Verb = restVerbAttr.Verb;
        }

        public bool Match(RestVerb verb, string uri)
        {
            return this.Verb == verb && UriMatches(uri);
        }

        private bool UriMatches(string uri)
        {
            return true;
        }

        public object[] GetParametersFromUri(string uri)
        {
            return null;
        }
    }

    public class RestControllerRequestHandler
    {
        private List<RestMethodInfo> _restMethodCollection;

        public RestControllerRequestHandler()
        {
            _restMethodCollection = new List<RestMethodInfo>();
        }

        public void RegisterController<T>() where T : IRestController
        {
            var tType = typeof(T);
            foreach (var methodDef in tType.GetRuntimeMethods())
            {
                if(methodDef.ReturnType == typeof(IRestResponse))
                {
                    if(methodDef.IsDefined(typeof(UriFormatAttribute)))
                    {
                        var uriFormat = methodDef.GetCustomAttribute(typeof(UriFormatAttribute)) as UriFormatAttribute;
                        _restMethodCollection.Add(new RestMethodInfo(methodDef, uriFormat.UriFormat));
                    }
                }
            }
        }

        public IRestResponse HandleRequest(RestVerb verb, string uri)
        {
            var restMethod = _restMethodCollection.SingleOrDefault(r => r.Match(verb, uri)))
            if(restMethod != null)
            {
                return (IRestResponse)restMethod.MethodInfo.Invoke(restMethod.Instantiator.Create(), restMethod.GetParametersFromUri(uri));
            }

            return null;
        }
    }
}
