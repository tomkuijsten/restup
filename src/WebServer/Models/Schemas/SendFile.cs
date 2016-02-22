using System;
using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Contracts;

namespace Devkoes.Restup.WebServer.Models.Schemas
{
    public class File
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }

        public File(string filename, string filepath)
        {
            FileName = filename;
            FilePath = filepath;
        }
    }


    [RestVerb(HttpMethod.GET)]
    public struct SendFile : IContentRestResponse
    {
        public enum ResponseStatus : int
        {
            OK = 200,
            NotFound = 404
        };
        public ResponseStatus Status { get; }

        public int StatusCode
        {
            get
            {
                return (int)Status;
            }
        }

        public File file { set; get; }

        public object ContentData { get; }

        public SendFile(ResponseStatus status, object data)
        {
            ContentData = data;
            Status = status;
            if (data.GetType().Name.ToLower() == "string")
            {
                file = new File((string)data, "");
            } else
            {
                file = new File(((File)data).FileName, ((File)data).FilePath);
            }


        }
        T IRestResponse.Visit<P, T>(IRestResponseVisitor<P, T> visitor, P param)
        {
            return visitor.Visit(this, param);
        }
    }
}
