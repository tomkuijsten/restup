using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restup.Webserver.Models.Contracts;

namespace Restup.Webserver.UnitTests.TestHelpers
{
	public class StaticUsernamePasswordValidator : ICredentialValidator
	{
		public const string Username = "user1";
		public const string Password = "pass123";

		public bool Authenticate(string username, string password)
		{
			return username == Username && password == Password;
		}
	}
}
