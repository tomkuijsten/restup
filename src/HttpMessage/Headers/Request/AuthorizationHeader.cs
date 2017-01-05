using System;
using System.Collections.Generic;
using System.Text;
using Restup.HttpMessage.Models.Contracts;

namespace Restup.HttpMessage.Headers.Request
{
	public class AuthorizationHeader : HttpRequestHeaderBase
	{
		public AuthorizationHeader(string username, string password) : base("Authorization", $"Basic {WebServer.Utils.Base64.EncodeTo64($"{username}:{password}")}")
		{
		}

		public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
		{
			
		}
	}
}
