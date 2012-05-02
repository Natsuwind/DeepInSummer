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
using PostAlbum.PostAlbumService;

namespace PostAlbum
{
    public class Utils
    {       

         /// <summary>
        /// 获取认证信息
        /// </summary>
        /// <returns></returns>
        public static CredentialInfo GetCredentialInfo()
        {
            CredentialInfo _creinfo = new CredentialInfo();
            _creinfo.UserID = TypeConverter.StrToInt(Utils.GetCookie("userid"), 0);
            _creinfo.Password = Utils.GetCookie("password");

            if (App.GetInitParmas.ContainsKey("authToken") && !string.IsNullOrEmpty(App.GetInitParmas["authToken"]))
                _creinfo.AuthToken = App.GetInitParmas["authToken"];

            return _creinfo;
        }

        public static AlbumSoapClient CreateServiceClient()
        {
            var endpointAddr = new EndpointAddress(new Uri(Application.Current.Host.Source, SliderShow.ServiceUrl));
            var binding = new BasicHttpBinding();
            var ctor = typeof(AlbumSoapClient).GetConstructor(new Type[] { typeof(Binding), typeof(EndpointAddress) });
            return (AlbumSoapClient)ctor.Invoke(new object[] { binding, endpointAddr });
        }

        public static string GetCookie(String key)
        {
            if (string.IsNullOrEmpty(HtmlPage.Document.Cookies))
                return null;
            
            //找到想应的cookie键值
            var query = (from cookie in HtmlPage.Document.Cookies.Split(';')
                                  where cookie.Contains(key + "=")
                                  select cookie.Split('&')).FirstOrDefault();
            string result = "";
            if (query != null)
            {
                result = (from c in query where c.Contains(key + "=")
                          select c).FirstOrDefault();
                //System.Windows.Browser.HtmlPage.Window.Alert(HtmlPage.Document.Cookies.Split(';').Length.ToString());
            }
            if (string.IsNullOrEmpty(result))
                return null;

            return result.Substring(result.IndexOf(key + "=") + key.Length + 1);
        }
    }
}
