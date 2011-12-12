using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.UI;
using System.IO;
using System.Xml;
using System.IO.Compression;
using System.Xml.Serialization;

namespace Natsuhime.Common
{
    //http://topic.csdn.net/u/20090108/18/328870AF-4F27-458A-B5BB-94FC949F8973.html
    //下面是本人写的几个序列化反序列化，以及压缩的类，其实就是把很多已有的方法归类了下，共享出来给个参考： 
    public static class BinaryFormatSerializer
    {
        private static readonly BinaryFormatter InnerBinaryFormatter = new BinaryFormatter();
        private readonly static MemoryStreamStack MemoryStreamStacker = new MemoryStreamStack();

        public static byte[] Serialize(object obj, bool compress)
        {
            if (obj == null)
            {
                return null;
            }
            byte[] info;
            MemoryStream stream = MemoryStreamStacker.GetMemoryStream();
            try
            {
                InnerBinaryFormatter.Serialize(stream, obj);
                stream.SetLength(stream.Position);
                info = stream.ToArray();
                if (compress)
                {
                    info = CompressHelper.CompressBytes(info);
                }
            }
            finally
            {
                MemoryStreamStacker.ReleaseMemoryStream(stream);
            }
            return info;
        }
        public static object Deserialize(byte[] info, bool decompress)
        {
            if (info == null || info.Length <= 0)
            {
                return info;
            }
            object ret;
            if (decompress)
            {
                info = CompressHelper.DeCompressBytes(info);
            }
            MemoryStream stream = MemoryStreamStacker.GetMemoryStream();
            try
            {
                stream.Write(info, 0, info.Length);
                stream.Position = 0L;
                ret = InnerBinaryFormatter.Deserialize(stream);
            }
            finally
            {
                MemoryStreamStacker.ReleaseMemoryStream(stream);
            }
            return ret;
        }
        public static byte[] Serialize(object obj)
        {
            return Serialize(obj, false);
        }
        public static object Deserialize(byte[] info)
        {
            return Deserialize(info, false);
        }
        public static string SerializeToBase64String(object data, bool compress)
        {
            byte[] buffer = Serialize(data, compress);
            return Convert.ToBase64String(buffer);
        }
        public static object DeserializeFromBase64String(string baseString, bool decompress)
        {
            if (String.IsNullOrEmpty(baseString))
                return null;

            byte[] buffer = Convert.FromBase64String(baseString);
            return Deserialize(buffer, decompress);
        }
        public static string SerializeToBase64String(object data)
        {
            return SerializeToBase64String(data, false);
        }
        public static object DeserializeFromBase64String(string baseString)
        {
            return DeserializeFromBase64String(baseString, false);
        }
    }
    //上面是二进制的，这个是datdacontract序列化成二进制的： 
    /*
    public static class DataContractFormatSerializer
    {
        public static string SerializeToBase64String<T>(T obj, bool compress)
        {
            byte[] ret = Serialize<T>(obj, compress);
            return Convert.ToBase64String(ret);
        }
        public static byte[] Serialize<T>(T obj, bool compress)
        {
            if (obj == null)
            {
                return null;
            }
            byte[] info;
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                using (XmlDictionaryWriter binaryDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(stream))
                {
                    serializer.WriteObject(binaryDictionaryWriter, obj);
                    binaryDictionaryWriter.Flush();
                }
                info = stream.ToArray();
                if (compress)
                {
                    info = CompressHelper.CompressBytes(info);
                }
            }
            return info;
        }
        public static T DeserializeFromBase64String<T>(string baseString, bool decompress)
        {
            if (String.IsNullOrEmpty(baseString))
                return default(T);

            byte[] buffer = Convert.FromBase64String(baseString);
            return Deserialize<T>(buffer, decompress);
        }
        public static T Deserialize<T>(byte[] info, bool decompress)
        {
            T ret = default(T);
            if (info == null || info.Length <= 0)
            {
                return ret;
            }
            if (decompress)
            {
                info = CompressHelper.DeCompressBytes(info);
            }
            using (MemoryStream stream = new MemoryStream(info))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                using (XmlDictionaryReader binaryDictionaryReader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    ret = (T)serializer.ReadObject(binaryDictionaryReader);
                }
            }
            return ret;
        }
    }
    */
    /*
    //这个是datacontract序列化为xml的： 
    public static class DataContractXmlSerializer
    {
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            string ret = "";
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                using (XmlDictionaryWriter binaryDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8))
                {
                    serializer.WriteObject(binaryDictionaryWriter, obj);
                    binaryDictionaryWriter.Flush();
                }
                ret = Encoding.UTF8.GetString(stream.ToArray());
            }
            return ret;
        }
        public static T Deserialize<T>(string xml)
        {
            T ret = default(T);
            if (string.IsNullOrEmpty(xml))
            {
                return ret;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0L;
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                using (XmlDictionaryReader binaryDictionaryReader = XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    ret = (T)serializer.ReadObject(binaryDictionaryReader);
                }
            }
            return ret;
        }
    }
    */
    //这个是viewstate序列化使用的类 
    public static class ObjectStateFormatSerializer
    {
        private static readonly ObjectStateFormatter InnerStateFormatter = new ObjectStateFormatter();
        private readonly static MemoryStreamStack MemoryStreamStacker = new MemoryStreamStack();
        public static string SerializeToBase64String(object obj, bool compress)
        {
            byte[] bytes = Serialize(obj, compress);
            return Convert.ToBase64String(bytes);
        }
        public static T DeserializeFromBase64String<T>(string baseString, bool compress)
        {
            if (String.IsNullOrEmpty(baseString))
                return default(T);
            byte[] buf = Convert.FromBase64String(baseString);
            return Deserialize<T>(buf, compress);
        }
        public static byte[] Serialize(object obj, bool compress)
        {
            if (obj == null)
            {
                return null;
            }
            byte[] buf = null;
            MemoryStream stream = MemoryStreamStacker.GetMemoryStream();
            try
            {
                InnerStateFormatter.Serialize(stream, obj);
                stream.SetLength(stream.Position);
                buf = stream.ToArray();
                if (compress)
                {
                    buf = CompressHelper.CompressBytes(buf);
                }
            }
            finally
            {
                MemoryStreamStacker.ReleaseMemoryStream(stream);
            }
            return buf;
        }
        public static T Deserialize<T>(byte[] info, bool compress)
        {
            T ret = default(T);
            if (info == null || info.Length <= 0)
            {
                return ret;
            }
            if (compress)
            {
                info = CompressHelper.DeCompressBytes(info);
            }
            MemoryStream stream = MemoryStreamStacker.GetMemoryStream();
            try
            {
                stream.Write(info, 0, info.Length);
                stream.Position = 0L;
                ret = (T)InnerStateFormatter.Deserialize(stream);
            }
            finally
            {
                MemoryStreamStacker.ReleaseMemoryStream(stream);
            }
            return ret;
        }
    }

    //这个是普通xml序列化的： 
    public static class XmlSerializationHelper
    {
        public static string Serialize(object obj, string xsdNs, string xsiNs)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("xsd", xsdNs);
            ns.Add("xsi", xsiNs);

            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            StringBuilder sb = new StringBuilder();
            TextWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, obj, ns);

            sb.Replace("utf-16", "utf-8");

            return sb.ToString();
        }
        public static object Deserialize(string xmlText, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            TextReader reader = new StringReader(xmlText);
            object result = serializer.Deserialize(reader);

            return result;
        }
        public static string RemoveEmptyTags(string sXML)
        {
            System.Text.StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<xsl:stylesheet ");
            sb.Append("     version=\"1.0\" ");
            sb.Append("     xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\"");
            sb.Append("     xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\">");
            sb.Append("     <xsl:output method=\"xml\" version=\"1.0\" encoding=\"UTF-8\"/>");
            sb.Append("   <!-- Whenever you match any node or any attribute -->");
            sb.Append("   <xsl:template match=\"node()|@*\">");
            sb.Append("      <!-- Copy the current node -->");
            sb.Append("     <xsl:if test=\"normalize-space(.) != '' or normalize-space(./@*) != \'' \">");
            sb.Append("          <xsl:copy>");
            sb.Append("              <!-- Including any attributes it has and any child nodes -->");
            sb.Append("               <xsl:apply-templates select=\"@*|node()\"/>");
            sb.Append("          </xsl:copy>");
            sb.Append("     </xsl:if>");
            sb.Append("   </xsl:template>");
            sb.Append("</xsl:stylesheet>");
            return TransXMLStringThroughXSLTString(sXML, sb.ToString());
        }
        private static string TransXMLStringThroughXSLTString(string sXML, string sXSLT)
        {
            //This is the logic of the application.
            //XslCompiledTransform objTransform = new XslCompiledTransform();
            //XmlDocument objDocument = new XmlDocument();
            //StringWriter objStream = new StringWriter();

            ////objDocument.Load(strAppPath+"BookFair.xml");
            //objDocument.LoadXml(sXML);

            //StringReader stream = new StringReader(sXSLT);
            //XmlReader xmlR = new XmlTextReader(stream);

            //objTransform.Load(xmlR, null, null);

            //objTransform.Transform(xmlR, null,
            //     objStream, null);

            //return objStream.ToString().Replace(@"encoding=""utf-16""?>", @"encoding=""utf-8""?>");
            throw new NotImplementedException();
        }
        public static string PrettyPrint(string xmlText)
        {
            String result = string.Empty;
            try
            {
                MemoryStream ms = new MemoryStream();
                XmlTextWriter w = new XmlTextWriter(ms, Encoding.Unicode);
                XmlDocument d = new XmlDocument();

                // Load the XmlDocument with the XML.
                d.LoadXml(xmlText);

                w.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                d.WriteContentTo(w);
                w.Flush();
                ms.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                ms.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sr = new StreamReader(ms);

                // Extract the text from the StreamReader.
                String formattedXML = sr.ReadToEnd();

                result = formattedXML;

                ms.Close();
                w.Close();
            }
            catch (Exception err)
            {
                Exception e = new Exception(err.Message + " " + "XML Input: " + xmlText, err.InnerException);
                throw (e);
            }

            return result;
        }
    }
    //这个是压缩二进制的类： 
    public static class CompressHelper
    {
        private readonly static MemoryStreamStack MemoryStreamStacker = new MemoryStreamStack();
        public static string CompressBytesToBase64String(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return "";
            }
            byte[] compressedData = CompressBytes(buffer);
            return System.Convert.ToBase64String(compressedData, 0, compressedData.Length);
        }
        public static string ConvertBytesToBase64String(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return "";
            }
            return System.Convert.ToBase64String(buffer, 0, buffer.Length);
        }
        public static byte[] ConvertBase64StringToBytes(string deCompressString)
        {
            if (string.IsNullOrEmpty(deCompressString))
            {
                return new byte[0];
            }
            byte[] buffer = System.Convert.FromBase64String(deCompressString);
            return buffer;
        }
        public static byte[] DeCompressBase64StringToBytes(string deCompressString)
        {
            if (string.IsNullOrEmpty(deCompressString))
            {
                return new byte[0];
            }
            byte[] buffer = System.Convert.FromBase64String(deCompressString);

            byte[] ret = DeCompressBytes(buffer);
            return ret;
        }
        public static byte[] CompressBytes(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return buffer;
            }
            byte[] compressedData;
            MemoryStream ms = MemoryStreamStacker.GetMemoryStream();
            try
            {
                using (DeflateStream compressedzipStream = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    compressedzipStream.Write(buffer, 0, buffer.Length);
                }
                ms.SetLength(ms.Position);
                //如果得到的结果长度比原来还大,则不需要压宿,返回原来的,并带上一位标识数据是否已压宿过
                if (ms.Length >= buffer.Length)
                {
                    compressedData = new byte[buffer.Length + 1];
                    buffer.CopyTo(compressedData, 0);
                    compressedData[compressedData.Length - 1] = 0;
                }
                else
                {
                    compressedData = new byte[ms.Length + 1];
                    ms.ToArray().CopyTo(compressedData, 0);
                    compressedData[compressedData.Length - 1] = 1;
                }
                //ms.Close();
            }
            finally
            {
                MemoryStreamStacker.ReleaseMemoryStream(ms);
            }
            return compressedData;
        }
        private static MemoryStream DeCompressMemoryToMemory(MemoryStream ms)
        {
            MemoryStream data = MemoryStreamStacker.GetMemoryStream();
            using (DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress))
            {
                byte[] writeData = new byte[8192];
                // Use the ReadAllBytesFromStream to read the stream.
                while (true)
                {
                    int size = zipStream.Read(writeData, 0, writeData.Length);
                    if (size > 0)
                    {
                        data.Write(writeData, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return data;
        }
        public static byte[] DeCompressBytes(byte[] buffer)
        {
            if (buffer == null || buffer.Length <= 0)
            {
                return buffer;
            }
            byte[] bytes = new byte[buffer.Length - 1];
            Array.Copy(buffer, bytes, bytes.Length);

            //如果最后一位是0,说明没有被压宿,那么也不需要解压速 
            if (buffer[buffer.Length - 1] == 0)
            {
                return bytes;
            }
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                MemoryStream stream = null;
                try
                {
                    stream = DeCompressMemoryToMemory(ms);
                    stream.SetLength(stream.Position);
                    bytes = stream.ToArray();
                }
                finally
                {
                    MemoryStreamStacker.ReleaseMemoryStream(stream);
                }
            }
            return bytes;
        }
    }

    //这个是这个序列化系列里用到memorystream的栈的类： 
    public class MemoryStreamStack 
    { 
        private Stack<MemoryStream> _streams = new Stack<MemoryStream>(); 
        public MemoryStream GetMemoryStream()
        {
            MemoryStream stream = null; 
            if (_streams.Count > 0)
            { 
                lock (_streams)
                { 
                    if (_streams.Count > 0)
                    {
                        stream = (MemoryStream)_streams.Pop();
                    } 
                }
            } 
            if (stream == null) 
            { 
                stream = new MemoryStream(0x800);
            } 
            return stream; 
        }
        
        public void ReleaseMemoryStream(MemoryStream stream)
        { 
            if (stream == null) 
            {
                return; 
            } 
            stream.Position = 0L; 
            stream.SetLength(0L); 
            lock (_streams) 
            { 
                _streams.Push(stream); 
            } 
        } 
        ~MemoryStreamStack()
        {
            foreach (MemoryStream memory in _streams)
            { 
                memory.Dispose();
            } 
            _streams.Clear();
            _streams = null;
        } 
    }
}
