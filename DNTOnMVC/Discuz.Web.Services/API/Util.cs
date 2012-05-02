using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;

using Discuz.Common;
using Newtonsoft.Json;

namespace Discuz.Web.Services.API
{
    public class Util
    {
#if NET1
        private static Hashtable serializer_dict = new Hashtable();
#else
        private static Dictionary<int, XmlSerializer> serializer_dict = new Dictionary<int, XmlSerializer>();
#endif
        private static ErrorDetails _errorDetails = new ErrorDetails();

        private static Regex[] r = new Regex[3];
        private const string REGEX_FORMAT = @"(\<{0}\>)([\s\S]+?)(\<\/{0}\>)";
        static Util()
        {
            r[0] = new Regex(string.Format(REGEX_FORMAT, "message"), RegexOptions.IgnoreCase);
            r[1] = new Regex(string.Format(REGEX_FORMAT, "title"), RegexOptions.IgnoreCase);
            r[2] = new Regex(string.Format(REGEX_FORMAT, "signature"), RegexOptions.IgnoreCase);
        }

        #region Comment out

        //private static XmlSerializer ErrorSerializer
        //{
        //    get
        //    {
        //        return GetSerializer(typeof(Error));
        //    }
        //}

        /// <summary>
        /// 获得远程页面内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        //public static byte[] GetResponseBytes(string url)
        //{
        //    WebRequest request = HttpWebRequest.Create(url);
        //    WebResponse response = null;

        //    try
        //    {
        //        response = request.GetResponse();
        //        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
        //        {
        //            return Encoding.UTF8.GetBytes(reader.ReadToEnd());
        //        }
        //    }
        //    finally
        //    {
        //        if (response != null)
        //            response.Close();
        //    }
        //}

        /// <summary>
        /// 获得序列器
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        //public static XmlSerializer GetSerializer(Type t)
        //{
        //    int type_hash = t.GetHashCode();

        //    if (!serializer_dict.ContainsKey(type_hash))
        //        serializer_dict.Add(type_hash, new XmlSerializer(t));

        //    return serializer_dict[type_hash] as XmlSerializer;
        //}

        /// <summary>
        /// 将字符型转为浮点型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //        internal static float GetFloatFromString(string input)
        //        {
        //            float returnValue;
        //#if NET1
        //            try
        //            {
        //                returnValue = Convert.ToSingle(input);
        //            }
        //            catch 
        //            {
        //                returnValue = -1;
        //            }
        //#else
        //            float.TryParse(input, out returnValue);
        //#endif
        //            return returnValue;
        //        }

        #endregion

        /// <summary>
        /// 去除xml中的空节点
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>整理过的xml字符串</returns>
        internal static string RemoveEmptyNodes(string xml, string whitelist)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList nodes = doc.SelectNodes("//node()");

            foreach (XmlNode node in nodes)
                if (node.ChildNodes.Count == 0 && node.InnerText == string.Empty && node.ParentNode.Name != "#document" && !Utils.InArray(node.Name, whitelist))
                    node.ParentNode.RemoveChild(node);
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xw.Formatting = System.Xml.Formatting.Indented;
            xw.Indentation = 2;
            doc.PreserveWhitespace = true;
            doc.WriteTo(xw);
            xml = sw.ToString();
            sw.Close();
            xw.Close();
            return xml;
        }

        /// <summary>
        /// 去除json的空属性
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal static string RemoveJsonNull(string json)
        {
            //return System.Text.RegularExpressions.Regex.Replace(json, @",?""\w*"":null,?", string.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @",""\w*"":null", string.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @"""\w*"":null,", string.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @"""\w*"":null", string.Empty);
            return json;
        }

        /// <summary>
        /// 为message添加cdata标记
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static string AddMessageCDATA(string xml)
        {
            return AddCDATA(xml, r[0]);
        }

        /// <summary>
        /// 为title添加cdata标记
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static string AddTitleCDATA(string xml)
        {
            return AddCDATA(xml, r[1]);
        }

        /// <summary>
        /// 为signature添加cdata标记
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        internal static string AddSignatureCDATA(string xml)
        {
            return AddCDATA(xml, r[2]);
        }

        /// <summary>
        /// 为元素内容添加CDATA
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        internal static string AddCDATA(string xml, Regex r)
        {
            Match m;

            for (m = r.Match(xml); m.Success; m = m.NextMatch())
            {
                xml = xml.Replace(m.Groups[0].ToString(), string.Format("{0}<![CDATA[\r\n{1}\r\n]]>{2}", m.Groups[1].ToString(), m.Groups[2].ToString(), m.Groups[3].ToString()));
            }
            return xml;
        }

        /// <summary>
        /// 判断参数是否为空或者为0
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        internal static bool AreParamsNullOrZeroOrEmptyString(params object[] parms)
        {
            foreach (object obj in parms)
            {
                if (obj == null)
                    return true;

                if (obj.GetType() == typeof(int) && Convert.ToInt32(obj) == 0)
                    return true;

                if (obj.GetType() == typeof(string) && obj.ToString() == string.Empty)
                    return true;
            }
            return false;
        }

        internal static string CreateErrorMessage(ErrorType error, List<DNTParam> paramList)
        {
            if (error == ErrorType.API_EC_NONE)
                return string.Empty;
            Error errorObj = new Error();
            errorObj.ErrorCode = (int)error;
            errorObj.ErrorMsg = _errorDetails[errorObj.ErrorCode].ToString();

            ArrayList list = new ArrayList();
            foreach (DNTParam param in paramList)
            {
                list.Add(new Arg(param.Name, param.Value));
            }
            if (list.Count > 0)
            {
                ArgResponse ar = new ArgResponse();
                ar.Args = (Arg[])list.ToArray(typeof(Arg));
                ar.List = true;
                errorObj.Args = ar;
            }
            return DNTRequest.GetString("format").Trim().ToLower() == "json" ?
                JavaScriptConvert.SerializeObject(errorObj) : SerializationHelper.Serialize(errorObj);
        }
    }
}
