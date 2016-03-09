using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devkoes.Restup.DemoControllers
{
    [RestController(InstanceCreationType.Singleton)]
    public class HTMLControllerSample
    {
        [UriFormat("/html")]
        public GetResponse HTMLExample()
        {
            string strResp = "";
            strResp += "<!DOCTYPE html><html><head><meta charset=\"UTF-8\"><title>HTML Page</title>";
            strResp += "</head><body><h1>This is a, HTML example</h1><p>";
            strResp += "This page is returned thru a server running on Windows 10 IOT!";
            strResp += "<p>it does illustrate the text/html MediaType";
            strResp += "</body></html>";
            return new GetResponse(
                GetResponse.ResponseStatus.OK, strResp);
        }
    }
}
