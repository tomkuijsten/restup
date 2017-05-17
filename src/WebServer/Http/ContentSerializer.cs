using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Restup.Webserver.Models.Schemas;

namespace Restup.Webserver.Http
{
    internal class ContentSerializer
    {
        internal object FromContent(byte[] content, MediaType contentMediaType, Type contentType, Encoding contentEncoding)
        {
            switch (contentMediaType)
            {
                case MediaType.JSON:
                    var contentAsString = contentEncoding.GetString(content);
                    return JsonConvert.DeserializeObject(contentAsString, contentType);
                case MediaType.XML:
                    return XmlDeserializeObject(content, contentType, contentEncoding);
            }

            throw new NotImplementedException();
        }

        internal byte[] ToAcceptContent(object contentObject, MediaType acceptMediaType, Encoding acceptEncoding)
        {
            if (contentObject == null)
            {
                return new byte[0];
            }
            
            switch (acceptMediaType)
            {
                case MediaType.JSON:
                    return acceptEncoding.GetBytes(JsonConvert.SerializeObject(contentObject));
                case MediaType.XML:
                    return XmlSerializeObject(contentObject, acceptEncoding);
            }

            throw new NotImplementedException();
        }

        private static byte[] XmlSerializeObject(object toSerialize, Encoding acceptEncoding)
        {
            var xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (var memoryStream = new MemoryStream())
            {
                // setting the encoding in the XmlWriter will ensure the correct encoding is output in the xml
                using (var xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings { Encoding = acceptEncoding }))
                {
                    xmlSerializer.Serialize(xmlWriter, toSerialize);
                }

                return memoryStream.ToArray();
            }
        }

        private static object XmlDeserializeObject(byte[] content, Type toType, Encoding contentEncoding)
        {
            var serializer = new XmlSerializer(toType);
            using (var memoryStream = new MemoryStream(content))
            {
                // note: setting the content encoding here will not guarantee that this encoding will be used to read the xml
                //       once the reader hits the encoding="" attribute it will immediately switch to that encoding, but this 
                //       is still the right thing to do because the encoding="" might not have been specified
                using (var reader = new StreamReader(memoryStream, contentEncoding))
                {
                    return serializer.Deserialize(reader);
                }
            }
        }
    }
}
