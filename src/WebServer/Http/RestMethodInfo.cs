using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System.Reflection;

namespace Devkoes.Restup.WebServer.Http
{
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
}
