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
    public class ParametersController
    {
        public class ParameterValue
        {
            public double Value { get; set; }
        }

        public class ParameterType
        {
            public string Name { get; set; }
            public string Type { get; set; }
        }

        [UriFormat("/channels/{channelId}/nodes/{nodeId}/parameters/{parameterId}")]
        public async Task<GetResponse> ReadParameter(int channelId, int nodeId, int parameterId)
        {
            return await Task.Run(() => new GetResponse(GetResponse.ResponseStatus.OK, new ParameterValue() { Value = 23.0}));
        }

        [UriFormat("/channels/{channelId}/nodes/{nodeId}/parameters/{parameterId}")]
        public PutResponse WriteParameter(int channelId, int nodeId, int parameterId, [FromBody]ParameterValue v)
        {
            return new PutResponse(PutResponse.ResponseStatus.OK);
        }

        [UriFormat("/parameters/{parameterId}")]
        public PostResponse CreateParameter(int parameterId, [FromBody]ParameterType v)
        {
            return new PostResponse(PostResponse.ResponseStatus.Created, $"/parameters/{parameterId}");
        }

        [UriFormat("/parameters/{parameterId}")]
        public DeleteResponse DeleteParameter(int parameterId)
        {
            return new DeleteResponse(DeleteResponse.ResponseStatus.OK);
        }
    }
}
