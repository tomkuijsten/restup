using Restup.HttpMessage.Headers.Request;
using Restup.HttpMessage.Plumbing;
using System;
using System.Linq;

namespace Restup.HttpMessage.ServerRequestParsers
{
    /// <summary>
    /// Some http header spec rules (http://www.w3.org/Protocols/rfc2616/rfc2616-sec4.html#sec4.2)
    /// - Each header field consists of a name followed by a colon (":") and the field value
    /// - Field names are case-insensitive
    /// - The field value MAY be preceded by any amount of LWS (any space, tab, newline), though a single SP is preferred.
    /// - Header fields can be extended over multiple lines by preceding each extra line with at least one SP or HT (tab)
    /// - The field-content does not include any leading or trailing LWS: linear white space occurring before the first 
    ///   non-whitespace character of the field-value or after the last non-whitespace character of the field-value. Such 
    ///   leading or trailing LWS MAY be removed without changing the semantics of the field value.
    /// - Any LWS that occurs between field-content MAY be replaced with a single SP before interpreting the field value 
    ///   or forwarding the message downstream
    /// - Multiple message-header fields with the same field-name MAY be present in a message if and only if the entire 
    ///   field-value for that header field is defined as a comma-separated list.
    /// - If a request contains a message-content and a Content-Length is not given, the server SHOULD respond with 400 (bad
    ///   request) if it cannot determine the length of the message, or with 411 (length required) if it wishes to insist 
    ///   on receiving a valid Content-Length. 
    /// </summary>
    internal class HeadersParser : HttpRequestPartParser
    {
        private RequestHeaderFactory _headerFactory;

        public HeadersParser()
        {
            _headerFactory = new RequestHeaderFactory();
        }

        /// <summary>
        /// A regex would be nice :)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="resultThisFar"></param>
        public override void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar)
        {
            UnparsedData = stream;

            string headerName = null;
            var bytesLeft = stream.Length;
            int headerValueStartIndex = 0;
            int headerValueEndIndex = 0;
            for (var i = 0; i < stream.Length; i++, bytesLeft--)
            {
                byte currentByte = stream[i];
                if (headerName == null && currentByte == Constants.ColonByte)
                {
                    headerName = GetHeaderString(stream, headerValueEndIndex, i);
                    headerValueStartIndex = i + 1;
                }
                else if (stream[i] == Constants.CRByte)
                {
                    // Special case when /r/n is received or /r/ncontentdata
                    if (i == 0 && bytesLeft >= 2 && stream[1] == Constants.LFByte)
                    {
                        IsFinished = true;
                        IsSucceeded = true;
                        headerValueEndIndex = 2;
                        break;
                    }

                    if (headerName != null &&
                        bytesLeft >= 2 &&
                        stream[i + 1] == Constants.LFByte)
                    {
                        // Handle end of one header scenario
                        var headerValue = GetHeaderString(stream, headerValueStartIndex, i);
                        headerValue = headerValue.TrimWhitespaces();
                        try
                        {
                            resultThisFar.AddHeader(_headerFactory.Create(headerName, headerValue));
                        }
                        catch (Exception)
                        {
                            IsFinished = true;
                            IsSucceeded = false;
                            break;
                        }
                        finally
                        {
                            headerName = null;
                            headerValueStartIndex = 0;
                            headerValueEndIndex = i + 2;
                        }
                    }

                    if (bytesLeft >= 4 &&
                        stream[i + 1] == Constants.LFByte &&
                        stream[i + 2] == Constants.CRByte &&
                        stream[i + 3] == Constants.LFByte)
                    {
                        // Handle end of headers scenario
                        headerValueEndIndex = i + 4;
                        IsFinished = true;
                        IsSucceeded = true;
                        break;
                    }
                }
            }

            UnparsedData = stream.Skip(headerValueEndIndex).ToArray();
        }

        private string GetHeaderString(byte[] stream, int unparsedIndex, int markerIndex)
        {
            int length = markerIndex - unparsedIndex;
            var data = new byte[length];
            Array.Copy(stream, unparsedIndex, data, 0, length);

            return Constants.DefaultHttpEncoding.GetString(data);
        }
    }
}