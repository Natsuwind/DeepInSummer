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
        /// 登录DZ
        /// </summary>
        /// <param name="Url">logging.php地址.请附带?action=login添加</param>
        /// <param name="LoginName">登录名（用户名、邮箱或者uid）</param>
        /// <param name="LoginNameType">登录名类型（用户名、邮箱或者uid）</param>
        /// <param name="Password">密码</param>
        /// <param name="VCode">验证码(暂时未支持)</param>
        /// <param name="Questionid">登录提示问题id</param>
        /// <param name="Answer">答案</param>
        /// <param name="Charset">网页编码</param>
        /// <param name="Proxy">代理(不使用请传入null)</param>
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
                if (returnData.IndexOf("欢迎您回来") > 0)
                {
                    //登录成功,返回cookie
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
