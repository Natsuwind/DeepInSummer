using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Natsuhime.Web
{
    public class YCookies
    {
        #region 静态方法
        //取得cookie对象
        public static HttpCookie GetCookie(string cookiename)
        {
            return HttpContext.Current.Request.Cookies[cookiename];
        }
        //创建新的cookie对象
        public static HttpCookie CreateCookie(string cookiename)
        {
            return new HttpCookie(cookiename);
        }
        //保存cookie对象
        public static void SaveCookie(HttpCookie cookie)
        {
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        //增加cookie有效期(可以写负数表示过期)
        public static void AddCookieExpiresTime(int minute, string cookiename)
        {
            HttpCookie cookie = GetCookie(cookiename);
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(minute);
                SaveCookie(cookie);
            }
        }
        //设置cookie作用域
        public static void SetCookieDomain(string domain, string cookiename)
        {
            HttpCookie cookie = GetCookie(cookiename);
            if (cookie != null)
            {
                cookie.Domain = domain;
                SaveCookie(cookie);
            }
        }
        //写入cookie键值
        public static void WriteCookieValue(string key, string value, string cookiename)
        {
            HttpCookie cookie = GetCookie(cookiename);
            if (cookie == null)
            {
                cookie = CreateCookie(cookiename);
            }
            cookie.Values[key] = Utils.UrlEncode(value);
            SaveCookie(cookie);
        }
        //读取cookie中键的值.如果为空返回"".
        public static string GetCookieStringValue(string key, string cookiename)
        {
            HttpCookie cookie = GetCookie(cookiename);
            if (cookie != null)
            {
                if (cookie.Values[key] != null)
                {
                    return Utils.UrlDecode(cookie.Values[key]);
                }
            }
            return "";
        }
        public static int GetCookieIntValue(string key, string cookiename, int defValue)
        {
            return Common.TypeParse.StrToInt(GetCookieStringValue(key, cookiename), defValue);
        }
        #endregion

        public HttpCookie Cookie { get; set; }
        public string CookieDomain { get; set; }
        string cookieName;
        public YCookies(string cookiename)
        {
            this.cookieName = cookiename;
            Cookie = GetCookie(this.cookieName);
        }
        public void WriteCookieValue(string key, string value)
        {
            if (Cookie == null)
            {
                Cookie = CreateCookie(cookieName);
            }
            Cookie.Values[key] = Utils.UrlEncode(value);
        }
        public string GetCookieStringValue(string key)
        {
            if (Cookie != null)
            {
                if (Cookie.Values[key] != null)
                {
                    return Utils.UrlDecode(Cookie.Values[key]);
                }
            }
            return "";
        }
        public int GetCookieIntValue(string key, int defValue)
        {
            return Common.TypeParse.StrToInt(this.GetCookieStringValue(key), defValue);
        }
        public void AddCookieExpiresTime(int minute)
        {
            if (Cookie != null)
            {
                Cookie.Expires = DateTime.Now.AddMinutes(minute);
            }
        }
        public void SaveCookie()
        {
            if (CookieDomain != null && CookieDomain.Trim() != string.Empty)
            {
                Cookie.Domain = CookieDomain.Trim();
            }
            if (Cookie != null)
            {
                HttpContext.Current.Response.AppendCookie(Cookie);
            }
        }
    }
}
