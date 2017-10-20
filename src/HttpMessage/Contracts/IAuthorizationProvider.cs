using Restup.HttpMessage;
using Restup.HttpMessage.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restup.WebServer.Models.Contracts
{
	public interface IAuthorizationProvider
	{
		string Realm { get; }
		Task<HttpResponseStatus> AuthorizeAsync(IHttpServerRequest request);
	}
}
