using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restup.WebServer.Models.Schemas
{
	internal class InternalServerErrorResponse : StatusOnlyResponse
	{
		internal InternalServerErrorResponse() : base((int)HttpResponseStatus.InternalServerError)
		{

		}
	}
}
