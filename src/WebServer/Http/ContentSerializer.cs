﻿using Devkoes.HttpMessage.Models.Schemas;
using Devkoes.Restup.WebServer.Models.Schemas;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Devkoes.Restup.WebServer.Http
{
    internal class ContentSerializer
    {
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
            else if (contentMediaType == MediaType.HTML)
            {
                return content.ToString();
            }

            throw new NotImplementedException();
        }

        internal byte[] ToAcceptContent(object contentObject, RestServerRequest req)
        {
            if (contentObject == null)
            {
                return new byte[0];
            }

            if (req.AcceptMediaType == MediaType.JSON)
            {
                return req.AcceptEncoding.GetBytes(JsonConvert.SerializeObject(contentObject));
            }
            else if (req.AcceptMediaType == MediaType.XML)
            {
                return req.AcceptEncoding.GetBytes(XmlSerializeObject(contentObject));
            }
            else if (req.AcceptMediaType == MediaType.HTML)
            {
                return req.AcceptEncoding.GetBytes(contentObject.ToString());
            }

            return new byte[0];
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
