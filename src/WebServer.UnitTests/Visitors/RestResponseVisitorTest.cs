using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Visitors;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.UnitTests.Visitors
{
    [TestClass]
    public class RestResponseVisitorTest
    {
        [TestMethod]
        public void Visit_Delete_DefaultResponse()
        {
            RestResponseVisitor v = new RestResponseVisitor(null);
            v.Visit(new DeleteResponse(DeleteResponse.ResponseStatus.OK));

            StringAssert.Contains(v.HttpResponse.Response, "200 OK");
            StringAssert.Contains(v.HttpResponse.Response, "Connection: ");
            StringAssert.Contains(v.HttpResponse.Response, "Date: ");
        }

        //todo, add more
    }
}
