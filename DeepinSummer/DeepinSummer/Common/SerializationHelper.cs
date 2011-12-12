using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace Natsuhime.Common
{
    public class SerializationHelper
    {
        /// <summary>
        /// XML 序列化
        /// </summary>
        /// <param name="o"></param>
        /// <param name="t"></param>
        /// <param name="fullXmlPath"></param>
        public static void SaveXml(object o, string fullXmlPath)
        {
            //FileStream fs = null;
            XmlTextWriter xw = null;
            try
            {
                //fs = new FileStream(fullXmlPath, FileMode.OpenOrCreate);
                xw = new XmlTextWriter(fullXmlPath, Encoding.GetEncoding("gb2312"));
                XmlSerializer xs = new XmlSerializer(o.GetType());


                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                xs.Serialize(xw, o, ns);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //if (fs != null)
                //    fs.Close();
                if (xw != null)
                {
                    xw.Close();
                }
            }
        }
        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fullXmlPath"></param>
        /// <returns></returns>
        public static object LoadXml(Type t, string fullXmlPath)
        {
            FileStream fs = null;
            object o = null;

            try
            {
                fs = new FileStream(fullXmlPath, FileMode.Open);
                o = XmlDeSerialize(t, fs);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return o;
        }
        /// <summary>
        /// XML反序列化(重载读入流,记得手动关闭流)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object XmlDeSerialize(Type t, Stream s)
        {
            XmlSerializer xs = new XmlSerializer(t);
            return xs.Deserialize(s);
        }


        public static void SaveBinary(object o, string fullFilePath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fullFilePath, FileMode.OpenOrCreate);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, o);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        public static object LoadBinary(string fullFilePath)
        {
            FileStream fs = null;
            object o = null;

            try
            {
                fs = new FileStream(fullFilePath, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                o = formatter.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return o;
        }


        public static void SaveJson(object o, string fullFilePath)
        {
            string json = JavaScriptConvert.SerializeObject(o);
            File.WriteAllText(fullFilePath, json, new UTF8Encoding(true, true));
        }
        public static object LoadJson(string fullFilePath, Type t)
        {
            string oldJson = File.ReadAllText(fullFilePath, new UTF8Encoding(true, true));
            if (oldJson.Trim() != string.Empty)
            {
                return JavaScriptConvert.DeserializeObject(oldJson, t);
            }
            else
            {
                return null;
            }
        }
    }
}
