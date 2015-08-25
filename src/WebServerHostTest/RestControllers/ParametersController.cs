using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServerHostTest.RESTControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public class ParametersController : IRestController
    {
        public class ParameterValue
        {
            public double Value { get; set; }
        }

        [UriFormat("/channels/{channelId}/nodes/{nodeId}/parameters/{parameterId}")]
        public GetResponse ReadParameter(int channelId, int nodeId, int parameterId)
        {
            return new GetResponse(GetResponse.GetResponseStatus.OK, new ParameterValue() { Value = 23.0});
        }
    }
}
