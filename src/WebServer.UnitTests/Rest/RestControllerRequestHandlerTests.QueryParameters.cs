using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using Restup.Webserver.UnitTests.TestHelpers;
using System;
using System.Threading.Tasks;

namespace Restup.Webserver.UnitTests.Rest
{
    [TestClass]
    public class RestControllerRequestHandlerTests_QueryParameters
    {
        [TestMethod]
        public async Task HandleRequest_MoreQueryParametersThenDefined_IgnoreExtraParameters()
        {
            var restHandler = new RestControllerRequestHandler();
            restHandler.RegisterController<QueryParamsTestController>();

            var request = await restHandler.HandleRequestAsync(
                Utils.CreateRestServerRequest(uri: new Uri("/query?val=test&val2=test2&_=129283174928743", UriKind.Relative)));

            Assert.IsNotNull(request);
            Assert.AreEqual((int)GetResponse.ResponseStatus.OK, request.StatusCode);

            var asGet = request as IGetResponse;

            Assert.AreEqual(asGet.ContentData, "test");
        }

        [RestController(InstanceCreationType.Singleton)]
        public class QueryParamsTestController
        {
            [UriFormat("/query?val={val}&val2={val2}")]
            public IGetResponse SimpleQuery(string val, string val2)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, val);
            }
        }
    }
}
