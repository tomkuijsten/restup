using Restup.HttpMessage;
using Restup.HttpMessage.Models.Schemas;
using Restup.Webserver.Models.Contracts;
using Restup.WebServer.Models.Contracts;
using Restup.WebServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restup.WebServer.Http
{
	public class BasicAuthorizationProvider : IAuthorizationProvider
	{
		private ICredentialValidator _credentialValidator;
		private string _realm;

		public BasicAuthorizationProvider(string realm, ICredentialValidator credentialValidator)
		{
			_realm = realm;
			_credentialValidator = credentialValidator;
		}

		public string Realm { get { return _realm; } }

		public HttpResponseStatus Authorize(IHttpServerRequest request)
		{
			if (request.Headers.Any(h => h.Name == "Authorization"))
			{
				var authValue = request.Headers.Single(h => h.Name == "Authorization").Value;
				var authValueArray = authValue.Split(' ');
				if(authValueArray.Length!=2 || authValueArray[0] != "Basic")
				{
					return HttpResponseStatus.BadRequest;
				}

				string[] pair = null;
				try
				{
					pair = Base64.DecodeFrom64(authValueArray[1]).Split(':');
				}
				catch (FormatException)
				{
					return HttpResponseStatus.BadRequest;
				}
				var user = pair[0];
				var pass = pair[1];

				if (!_credentialValidator.Authenticate(user, pass))
				{
					return HttpResponseStatus.Unauthorized;
				}
				return HttpResponseStatus.OK;
			}
			else // ask for authentication headers
			{
				return HttpResponseStatus.Unauthorized;
			}
		}
	}
}
