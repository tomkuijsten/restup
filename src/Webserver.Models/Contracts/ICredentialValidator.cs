using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Restup.Webserver.Models.Contracts
{
	public interface ICredentialValidator
	{
		IAsyncOperation<bool> AuthenticateAsync(string username, string password);
	}
}
