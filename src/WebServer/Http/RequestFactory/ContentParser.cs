using Devkoes.Restup.WebServer.Models.Schemas;

namespace Devkoes.Restup.WebServer.Http.RequestFactory
{
    /// <summary>
    /// One bad request scenario we don't cover:
    ///  
    /// If the content has exactly the correct length, but somehow the stream is not done yet, it will stop processing
    /// and be marked as succesfull. Even though new data is being written to the stream, which would make it a bad
    /// request. The reason we don't cover this scenario is that the alternative is even worse, always wait for some timeout
    /// one the stream to be sure it's writing unexpected data.
    /// </summary>
    internal class ContentParser : RequestPipelinePart
    {
        private string _content;

        public override void HandleRequestPart(byte[] stream, HttpRequest resultThisFar)
        {
            _content += resultThisFar.RequestContentEncoding.GetString(stream);
            if (_content.Length == resultThisFar.ContentLength)
            {
                resultThisFar.Content = _content;
                Finished = true;
            }
            // else if content is bigger, finished will never be set, badrequest will happen
        }
    }
}
