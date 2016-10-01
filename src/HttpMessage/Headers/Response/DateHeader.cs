using System;

namespace Restup.HttpMessage.Headers.Response
{
    internal class DateHeader : HttpHeaderBase
    {
        internal static string NAME = "Date";

        public DateTime Date { get; }

        public DateHeader(DateTime date) : base(NAME, date.ToString("r"))
        {
            Date = date;
        }
    }
}
