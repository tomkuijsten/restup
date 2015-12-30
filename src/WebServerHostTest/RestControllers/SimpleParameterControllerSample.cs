using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;

namespace Devkoes.Restup.WebServerHostTest.RestControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public class SimpleParameterControllerSample
    {
        public class DataReceived
        {
            public int ID { get; set; }
            public string PropName { get; set; }
        }

        /// <summary>
        /// Make sure the number of parameters in your UriFormat match the parameters in your method and
        /// the names (case sensitive) and order are respected.
        /// </summary>
        [UriFormat("/simpleparameter/{id}/property/{propName}")]
        public GetResponse GetWithSimpleParameters(int id, string propName)
        {
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                new DataReceived()
                {
                    ID = id,
                    PropName = propName
                });
        }
    }
}
