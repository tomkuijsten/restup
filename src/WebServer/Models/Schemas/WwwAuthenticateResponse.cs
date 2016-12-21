using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restup.WebServer.Models.Schemas
{
	internal class WwwAuthenticateResponse : StatusOnlyResponse
	{
		internal WwwAuthenticateResponse(string realm) : base((int)HttpResponseStatus.Unauthorized, new Dictionary<string, string>() { { "WWW-Authenticate", $"Basic realm=\"{realm}\"" } })
		{

		}
	}
}
