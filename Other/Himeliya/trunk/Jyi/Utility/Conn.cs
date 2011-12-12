using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Jyi.Utility
{
    class Conn
    {
        public static string PostData(string Url, string Charset, string param, string referer)
        {
            return PostData(Url, Charset, param, referer, 0, null, null);
        }
        public static string PostData(string Url, string Charset, string param, string referer, int timeOut, WebProxy webProxy)
        {
            return PostData(Url, Charset, param, referer, timeOut, webProxy, null);
        }
        public static string PostData(string Url, string Charset, string param, string referer, CookieContainer cookieContainer)
        {
            return PostData(Url, Charset, param, referer, 0, null, cookieContainer);
        }

        /// <summary>
        /// Post数据,返回内容
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Charset"></param>
        /// <param name="param">经过UrlEncode后的参数</param>
        /// <param name="referer"></param>
        /// <param name="timeOut"></param>
        /// <param name="webProxy"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static string PostData(string Url, string Charset, string param, string referer, int timeOut, WebProxy webProxy, CookieContainer cookieContainer)
        {
            byte[] bs = Encoding.ASCII.GetBytes(param);

            HttpWebRequest objRequest = (HttpWebRequest)HttpWebRequest.Create(Url);
            objRequest.Method = "POST";
            objRequest.Referer = referer;
            objRequest.ContentType = "application/x-www-form-urlencoded";
            objRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight, */*";
            objRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";

            objRequest.ContentLength = bs.Length;
            if (timeOut > 0)
            {
                objRequest.Timeout = timeOut;
            }
            if (cookieContainer != null)
            {
                objRequest.CookieContainer = cookieContainer;
            }

            if (webProxy != null)
            {

                objRequest.Proxy = webProxy;
            }

            
            try
            {
                using (Stream reqStream = objRequest.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }

                using (WebResponse wr = objRequest.GetResponse())
                {
                    Stream s = wr.GetResponseStream();
                    StreamReader sr = new StreamReader(s, Encoding.GetEncoding(Charset));
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "Jyi链接失败\r\n" + ex.ToString();
            }
        }





        public static string GetData(string Url, string Charset, string referer, int timeOut, WebProxy webProxy, CookieContainer cookieContainer)
        {
            HttpWebRequest objRequest = (HttpWebRequest)HttpWebRequest.Create(Url);            
            objRequest.Method = "GET";
            objRequest.Referer = referer;
            objRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight, */*";
            objRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";

            if (timeOut > 0)
            {
                objRequest.Timeout = timeOut;
            }
            if (cookieContainer != null)
            {
                objRequest.CookieContainer = cookieContainer;
            }

            if (webProxy != null)
            {

                objRequest.Proxy = webProxy;
            }

            try
            {
                WebResponse wr2 = objRequest.GetResponse();
                Stream s = wr2.GetResponseStream();
                //Stream s = objRequest.GetResponse().GetRequestStream();
                StreamReader sr = new StreamReader(s, Encoding.GetEncoding(Charset));
                return sr.ReadToEnd();
            }
            catch (System.Net.WebException)
            {
                return "链接失败!";
            }
        }
    }
}
