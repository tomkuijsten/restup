using Restup.Webserver.Models.Contracts;
using System.Collections.Generic;

namespace Restup.Webserver.Models.Schemas
{
    internal class StatusOnlyResponse : IRestResponse
    {
        public IEnumerable<IHeader> Headers { get; }

        public int StatusCode { get; private set; }

        internal StatusOnlyResponse(int statusCode)
        {
            Headers = new List<IHeader>();
            StatusCode = statusCode;
        }


        public IRestResponse addHeader(IHeader headerToAdd)
        {
            ((List<IHeader>)Headers).Add(headerToAdd);

            return this;
        }

        public IRestResponse addHeader(string headerName, string headerValue)
        {
            return addHeader(new Header(headerName, headerValue));
        }
    }
}
