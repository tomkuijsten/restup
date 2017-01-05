using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restup.Webserver.Models.Contracts;
using Windows.Foundation;

namespace Restup.Webserver.UnitTests.TestHelpers
{
	public class StaticUsernamePasswordValidator : ICredentialValidator
	{
		public const string Username = "user1";
		public const string Password = "pass123";

		public IAsyncOperation<bool> AuthenticateAsync(string username, string password)
		{
			return AuthenticateHelper(username, password).AsAsyncOperation();
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		private async Task<bool> AuthenticateHelper(string username, string password)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{
			return username == Username && password == Password;
		}
	}
}
