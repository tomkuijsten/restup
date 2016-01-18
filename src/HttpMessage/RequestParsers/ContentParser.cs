namespace Devkoes.HttpMessage.RequestParsers
{
    /// <summary>
    /// One bad request scenario we don't cover:
    ///  
    /// If the content has exactly the correct length, but somehow the stream is not done yet, it will stop processing
    /// and be marked as succesfull. Even though new data is being written to the stream, which would make it a bad
    /// request. The reason we don't cover this scenario is that the alternative is even worse, always wait for some timeout
    /// on the stream to be sure it's not writing unexpected data.
    /// </summary>
    internal class ContentParser : HttpRequestPartParser
    {
        private string _content;

        public ContentParser()
        {
            // Incoming data is read entirely, always, so there will never be any unparsed data
            UnparsedData = new byte[0];
        }
        public override void HandleRequestPart(byte[] stream, HttpRequest resultThisFar)
        {
            if (resultThisFar.ContentLength == 0)
            {
                IsFinished = true;
                IsSucceeded = stream.Length == 0;
            }
            else
            {
                _content += resultThisFar.RequestContentEncoding.GetString(stream);
                if (_content.Length == resultThisFar.ContentLength)
                {
                    resultThisFar.Content = _content;
                    IsFinished = true;
                    IsSucceeded = true;
                }
                // else if content is bigger, finished will never be set, badrequest will happen
            }
        }
    }
}
