using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.HttpMessage.Plumbing;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Models.Contracts;
using Devkoes.Restup.WebServer.Models.Schemas;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Devkoes.Restup.WebServer.Rest
{
    internal class RestToHttpResponseConverter : IRestResponseVisitor<RestServerRequest, HttpServerResponse>
    {
        private ContentSerializer _contentSerializer;

        public RestToHttpResponseConverter()
        {
            _contentSerializer = new ContentSerializer();
        }

        public HttpServerResponse Visit(DeleteResponse response, RestServerRequest restReq)
        {
            return GetDefaultResponse(response);
        }

        public HttpServerResponse Visit(PostResponse response, RestServerRequest restReq)
        {
            var serverResponse = GetDefaultContentResponse(response, restReq);

            if (response.Status == PostResponse.ResponseStatus.Created)
                serverResponse.Location = new Uri(response.LocationRedirect, UriKind.RelativeOrAbsolute);

            return serverResponse;
        }

        public HttpServerResponse Visit(GetResponse response, RestServerRequest restReq)
        {
            return GetDefaultContentResponse(response, restReq);
        }

        public HttpServerResponse Visit(PutResponse response, RestServerRequest restReq)
        {
            return GetDefaultContentResponse(response, restReq);
        }

        public HttpServerResponse Visit(StatusOnlyResponse response, RestServerRequest restReq)
        {
            return GetDefaultResponse(response);
        }

        public HttpServerResponse Visit(MethodNotAllowedResponse methodNotAllowedResponse, RestServerRequest restReq)
        {
            var serverResponse = GetDefaultResponse(methodNotAllowedResponse);
            serverResponse.Allow = methodNotAllowedResponse.Allows;

            return serverResponse;
        }

        private HttpServerResponse GetDefaultContentResponse(IContentRestResponse response, RestServerRequest restReq)
        {
            var defaultResponse = GetDefaultResponse(response);

            if (response.ContentData != null)
            {
                defaultResponse.ContentType = restReq.AcceptMediaType;
                defaultResponse.ContentCharset = restReq.AcceptCharset;
                defaultResponse.Content = _contentSerializer.ToAcceptContent(response.ContentData, restReq);
            }

            return defaultResponse;
        }

        private HttpServerResponse GetDefaultResponse(IRestResponse response)
        {
            var serverResponse = HttpServerResponse.Create(response.StatusCode);
            serverResponse.Date = DateTime.Now;
            serverResponse.IsConnectionClosed = true;

            return serverResponse;
        }

        public HttpServerResponse Visit(SendFile response, RestServerRequest restReq)
        {
            return PrepToSendFile(response, restReq);
        }

        private HttpServerResponse PrepToSendFile(SendFile response, RestServerRequest restReq)
        {
            FileStream fileToServe = null;
            try
            {
                Task<string> t = GetFilePath(response.file.FileName, response.file.FilePath);
                t.Wait();
                fileToServe = new FileStream(t.Result, FileMode.Open, FileAccess.Read);
                long fileLength = fileToServe.Length;

                HttpServerResponse mResp = HttpServerResponse.Create(response.StatusCode);
                mResp.Content = new byte[fileLength];
                fileToServe.Read(mResp.Content, 0, (int)fileLength);
                fileToServe.Dispose();

                return mResp;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception sending file: {0}", e.ToString());
                if (fileToServe != null)
                {
                    try
                    {
                        fileToServe.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception closing file after first exception: {0}", ex.ToString());
                    }
                }
            }
            //throw new Exception("Exception encoding file");
            return HttpServerResponse.Create(HttpResponseStatus.NotFound);
        }

        async Task<string> GetFilePath(string filename, string filepath)
        {
            try
            {
                StorageFolder localFolder = filepath != "" ? await StorageFolder.GetFolderFromPathAsync(filepath) : ApplicationData.Current.LocalFolder;
                //replace / by \ as the URL can be relative, same in the file system
                filename = filename.Replace('/','\\');
                var file = await localFolder.GetFileAsync(filename);
                if (file != null)
                    return file.Path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("File not found: {0}", ex.Message));
            }
            return ""; 
        }
    }
}
