using Devkoes.HttpMessage;
using Devkoes.HttpMessage.Models.Schemas;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Devkoes.Restup.WebServer.Http
{
    internal class ContentSerializer
    {
        internal string GetContentString(string charset, byte[] rawContent)
        {
            return Encoding.GetEncoding(charset).GetString(rawContent);
        }

        internal object FromContent(string content, MediaType contentMediaType, Type contentType)
        {
            if (contentMediaType == MediaType.JSON)
            {
                return JsonConvert.DeserializeObject(content, contentType);
            }
            else if (contentMediaType == MediaType.XML)
            {
                return XmlDeserializeObject(content, contentType);
            }

            throw new NotImplementedException();
        }

        internal string ToContent(object contentObject, HttpServerRequest req)
        {
            if (contentObject == null)
            {
                return null;
            }

            var suppTypeHiQuality = req.AcceptMediaTypes.FirstOrDefault(r => r != MediaType.Unsupported);
            suppTypeHiQuality = suppTypeHiQuality == MediaType.Unsupported ? Configuration.Default.AcceptType : suppTypeHiQuality;

            if (suppTypeHiQuality == MediaType.JSON)
            {
                return JsonConvert.SerializeObject(contentObject);
            }
            else if (suppTypeHiQuality == MediaType.XML)
            {
                return XmlSerializeObject(contentObject);
            }

            return null;
        }

        private static string XmlSerializeObject(object toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        private static object XmlDeserializeObject(string content, Type toType)
        {
            var serializer = new XmlSerializer(toType);
            object result;

            using (TextReader reader = new StringReader(content))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
