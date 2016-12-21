using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.WebServer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restup.DemoControllers
{
	[Authorize]
	[RestController(InstanceCreationType.PerCall)]
	public sealed class AuthenticatedPerCallControllerSample
	{
		private long _totalNrOfCallsHandled;

		private class CallInfo
		{
			public long TotalCallsHandled { get; set; }
		}

		/// <summary>
		/// This will always return a TotalCallsHandled of one, no matter how many times you request this uri.
		/// </summary>
		/// <returns></returns>
		[UriFormat("/authpercall")]
		public IGetResponse GetPerCallSampleValue()
		{
			return new GetResponse(
				GetResponse.ResponseStatus.OK,
				new CallInfo() { TotalCallsHandled = ++_totalNrOfCallsHandled });
		}
	}
}
