using Restup.Webserver.Models.Contracts;
using Restup.WebServer.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Restup.DemoControllers.Authentication
{
	public sealed class DemoCredentialValidator : ICredentialValidator
	{
		public IAsyncOperation<bool> AuthenticateAsync(string username, string password)
		{
			return AuthenticateHelper(username, password).AsAsyncOperation();
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		private async Task<bool> AuthenticateHelper(string username, string password)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{
			return (username == "user" && password == "pass");
		}
	}
}
