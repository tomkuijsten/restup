using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restup.Webserver.Models.Contracts
{
	public interface ICredentialValidator
	{
		bool Authenticate(string username, string password);
	}
}
