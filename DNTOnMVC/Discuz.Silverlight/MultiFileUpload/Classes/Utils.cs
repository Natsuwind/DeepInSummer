using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Browser;
using System.Linq;
using MultiFileUpload.UploadServiceAsmx;

namespace MultiFileUpload.Classes
{
    public class Utils
    {
        public static string GetCookie(String key)
        {
            if (string.IsNullOrEmpty(HtmlPage.Document.Cookies))
                return null;

            //找到想应的cookie键值
            string result = (from c in
                                 (from cookie in HtmlPage.Document.Cookies.Split(';')
                                  where cookie.Contains(key + "=")
                                  select cookie.Split('&')).FirstOrDefault()
                             where c.Contains(key + "=")
                             select c).FirstOrDefault().ToString();

            if(string.IsNullOrEmpty(result))
                return null;

            return result.Substring(result.IndexOf(key + "=") + key.Length + 1);
        }

        public static bool Exists(String key, String value)
        {
            return HtmlPage.Document.Cookies.Contains(String.Format("{0}={1}", key, value));
        }

        public static MixObjectsSoapClient CreateServiceClient()
        {
            var endpointAddr = new EndpointAddress(new Uri(Application.Current.Host.Source, Page.ServiceUrl));
            var binding = new BasicHttpBinding();
            var ctor = typeof(MixObjectsSoapClient).GetConstructor(new Type[] { typeof(Binding), typeof(EndpointAddress) });
            return (MixObjectsSoapClient)ctor.Invoke(new object[] { binding, endpointAddr });
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (str == null)
                return defValue;

            if (str.Length > 0 && str.Length <= 11 && System.Text.RegularExpressions.Regex.IsMatch(str, @"^[-]?[0-9]*$"))
            {
                if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    return Convert.ToInt32(str);
            }
            return defValue;
        }

   
        /// <summary>
        /// 获取认证信息
        /// </summary>
        /// <returns></returns>
        public static CredentialInfo GetCredentialInfo()
        {
                CredentialInfo _creinfo = new CredentialInfo();
                _creinfo.UserID = Utils.StrToInt(Utils.GetCookie("userid"), 0);
                _creinfo.Password = Utils.GetCookie("password");

                if (App.GetInitParmas.ContainsKey("authToken") && !string.IsNullOrEmpty(App.GetInitParmas["authToken"]))
                    _creinfo.AuthToken = App.GetInitParmas["authToken"];

                if (App.GetInitParmas.ContainsKey("forumid") && !string.IsNullOrEmpty(App.GetInitParmas["forumid"]))
                    _creinfo.ForumID = StrToInt(App.GetInitParmas["forumid"], 0);

                return _creinfo;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                        startIndex = startIndex - length;
                }

                if (startIndex > str.Length)
                    return "";
            }
            else
            {
                if (length < 0)
                    return "";
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                        return "";
                }
            }

            if (str.Length - startIndex < length)
                length = str.Length - startIndex;

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }
    }
}
