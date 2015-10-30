using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
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

        [UriFormat("/networks/{networkId}/channels/{channelId}/parameters/{parameterId}")]
        public Task<GetResponse> ReadParameter(int networkId, int channelId, int parameterId)
        {
            return Task.FromResult(new GetResponse(GetResponse.ResponseStatus.OK, new ParameterValue() { Value = 23.0 }));
        }

        [UriFormat("/networks/{networkId}/channels/{channelId}/parameters/{parameterId}")]
        public Task<PutResponse> UpdateParameter(int networkId, int channelId, int parameterId, [FromBody]ParameterValue v)
        {
            return Task.FromResult(new PutResponse(PutResponse.ResponseStatus.OK));
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
