using Restup.Webserver.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;

namespace Restup.WebServer.Http
{
	public class PassThroughCredentialValidator : ICredentialValidator
	{
		private string _forwardUrl;

		public PassThroughCredentialValidator(string forwardUrl)
		{
			_forwardUrl = forwardUrl;
		}

		public IAsyncOperation<bool> AuthenticateAsync(string username, string password)
		{
			return AuthenticateHelper(username, password).AsAsyncOperation();
		}

		public async Task<bool> AuthenticateHelper(string username, string password)
		{
			var client = new HttpClient();
			var buffer = Windows.Security.Cryptography.CryptographicBuffer.ConvertStringToBinary(username + ":" + password, Windows.Security.Cryptography.BinaryStringEncoding.Utf16LE);
			string base64token = Windows.Security.Cryptography.CryptographicBuffer.EncodeToBase64String(buffer);
			client.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Basic", base64token);
			var result = await client.GetAsync(new Uri(_forwardUrl));
			return result.StatusCode == HttpStatusCode.Ok;
		}
	}
}
