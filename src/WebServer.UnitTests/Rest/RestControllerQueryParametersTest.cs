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
    public class RestControllerQueryParametersTest
    {
        [TestMethod]
        public async Task HandleRequest_MoreQueryParametersThenDefined_IgnoreExtraParameters()
        {
            var restHandler = new RestControllerRequestHandler();
            restHandler.RegisterController<QueryParamsTestController>();

            var request = await restHandler.HandleRequestAsync(
                Utils.CreateRestServerRequest(uri: new Uri("/query?val=test&_=129283174928743", UriKind.Relative)));

            Assert.IsNotNull(request);
            Assert.AreEqual((int)GetResponse.ResponseStatus.OK, request.StatusCode);

            var asGet = request as IGetResponse;

            Assert.AreEqual(asGet.ContentData, "test");
        }

        [RestController(InstanceCreationType.Singleton)]
        public class QueryParamsTestController
        {
            [UriFormat("/query?val={val}")]
            public IGetResponse SimpleQuery(string val)
            {
                return new GetResponse(GetResponse.ResponseStatus.OK, val);
            }
        }
    }
}
