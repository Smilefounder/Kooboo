using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Kooboo.Sites.Logistics
{
    public class XmlSerializerUtilis
    {
        public static string SerializeXML<T>(XmlWriterSettings settings, XmlSerializerNamespaces ns, T request)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            string xml = "";
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, request, ns);
                }
                xml = textWriter.ToString();
            }

            return xml;
        }


        public static T DeserializeXML<T>(XmlReaderSettings settings, string response)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            // No settings need modifying here

            using (StringReader textReader = new StringReader(response))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }
    }
}
