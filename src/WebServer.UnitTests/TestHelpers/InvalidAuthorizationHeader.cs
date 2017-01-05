using System;
using System.Collections.Generic;
using System.Text;
using Restup.HttpMessage.Models.Contracts;

namespace Restup.HttpMessage.Headers.Request
{
	public class InvalidAuthorizationHeaderNotABase64String : HttpRequestHeaderBase
	{
		public InvalidAuthorizationHeaderNotABase64String() : base("Authorization", "Basic thisisnotbase64")
		{
		}

		public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
		{
			
		}
	}

	public class InvalidAuthorizationHeaderMissingPassword : HttpRequestHeaderBase
	{
		public InvalidAuthorizationHeaderMissingPassword(string username, string password) : base("Authorization", $"Basic {WebServer.Utils.Base64.EncodeTo64($"{username}:")}")
		{
		}

		public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
		{

		}
	}

	public class InvalidAuthorizationHeaderWrongMethod : HttpRequestHeaderBase
	{
		public InvalidAuthorizationHeaderWrongMethod(string username, string password) : base("Authorization", "NTLM invalid")
		{
		}

		public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
		{

		}
	}
}
