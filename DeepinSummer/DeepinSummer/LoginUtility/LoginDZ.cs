using System;
using System.Net;

namespace Natsuhime.LoginUtility
{
    public enum LoginNameType
    {
        UserName,
        Email,
        UID 
    }
    public class LoginDZ
    {
        /// <summary>
        /// ��¼DZ
        /// </summary>
        /// <param name="Url">logging.php��ַ.�븽��?action=login���</param>
        /// <param name="LoginName">��¼�����û������������uid��</param>
        /// <param name="LoginNameType">��¼�����ͣ��û������������uid��</param>
        /// <param name="Password">����</param>
        /// <param name="VCode">��֤��(��ʱδ֧��)</param>
        /// <param name="Questionid">��¼��ʾ����id</param>
        /// <param name="Answer">��</param>
        /// <param name="Charset">��ҳ����</param>
        /// <param name="Proxy">����(��ʹ���봫��null)</param>
        /// <returns></returns>
        public static CookieContainer Login(string Url, string LoginName, LoginNameType LoginType, string Password, string VCode, string Questionid, string Answer, string Charset, WebProxy Proxy)
        {
            string returnData = "";
            string formhash = "";
            CookieContainer objCookie = new CookieContainer();
            Httper objPostHttper = new Httper();
            objPostHttper.Url = Url;
            objPostHttper.Charset = Charset;
            objPostHttper.Cookie = objCookie;
            if (Proxy != null)
            {
                objPostHttper.Proxy = Proxy;
            }

            try
            {
                formhash = RegexUtility.GetMatch(objPostHttper.HttpGet(), "formhash=(.*)\"");

                objPostHttper.PostData = string.Format("&formhash={0}&referer=index.php&loginfield={5}&username={1}&password={2}&questionid={3}&answer={4}&cookietime=2592000&loginsubmit=%CC%E1%BD%BB"
                    , formhash, LoginName, Password, Questionid, Answer, LoginType.ToString().ToLower());
                returnData = objPostHttper.HttpPost();
                if (returnData.IndexOf("��ӭ������") > 0)
                {
                    //��¼�ɹ�,����cookie
                    return objCookie;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
