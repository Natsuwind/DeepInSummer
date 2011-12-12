using System;
using System.Data;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace DianPing
{
    /*
*  Author:Heilong05@163.com
*  如果您改进了这个类请发一份代码给我(heilong05 在163.com)
*/
    /// <summary>
    /// 网页类
    /// </summary>
    public class WebPage
    {

        #region 私有成员
        private Uri m_uri;   //网址
        private List<Link> m_links;    //此网页上的链接
        private string m_title;        //此网页的标题
        private string m_html;         //此网页的HTML代码
        private string m_outstr;       //此网页可输出的纯文本
        private bool m_good;           //此网页是否可用
        private int m_pagesize;       //此网页的大小
        private static Dictionary<string, CookieContainer> webcookies = new Dictionary<string, CookieContainer>();//存放所有网页的Cookie
        private string m_post;  //此网页的登陆页需要的POST数据
        private string m_loginurl;  //此网页的登陆页
        #endregion


        #region 私有方法
        /// <summary>
        /// 这私有方法从网页的HTML代码中分析出链接信息
        /// </summary>
        /// <returns>List<Link></returns>
        private List<Link> getLinks()
        {
            if (m_links.Count == 0)
            {
                Regex[] regex = new Regex[2];
                regex[0] = new Regex("(?m)<a[^><]+href=(\"|')?(?<url>([^>\"'\\s)])+)(\"|')?[^>]*>(?<text>(\\w|\\W)*?)</", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                regex[1] = new Regex("<[i]*frame[^><]+src=(\"|')?(?<url>([^>\"'\\s)])+)(\"|')?[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                for (int i = 0; i < 2; i++)
                {
                    Match match = regex[i].Match(m_html);
                    while (match.Success)
                    {
                        try
                        {
                            string url = new Uri(m_uri, match.Groups["url"].Value).AbsoluteUri;
                            string text = "";
                            if (i == 0) text = new Regex("(<[^>]+>)|(\\s)|(&nbsp;)|&|\"", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(match.Groups["text"].Value, "");
                            Link link = new Link(url, text);
                            m_links.Add(link);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); };
                        match = match.NextMatch();
                    }
                }
            }
            return m_links;
        }

        /// <summary>
        /// 此私有方法从一段HTML文本中提取出一定字数的纯文本
        /// </summary>
        /// <param name="instr">HTML代码</param>
        /// <param name="firstN">提取从头数多少个字</param>
        /// <param name="withLink">是否要链接里面的字</param>
        /// <returns>纯文本</returns>
        private string getFirstNchar(string instr, int firstN, bool withLink)
        {
            if (m_outstr == "")
            {
                m_outstr = instr.Clone() as string;
                m_outstr = new Regex(@"(?m)<script[^>]*>(\w|\W)*?</script[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(m_outstr, "");
                m_outstr = new Regex(@"(?m)<style[^>]*>(\w|\W)*?</style[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(m_outstr, "");
                m_outstr = new Regex(@"(?m)<select[^>]*>(\w|\W)*?</select[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(m_outstr, "");
                if (!withLink) m_outstr = new Regex(@"(?m)<a[^>]*>(\w|\W)*?</a[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(m_outstr, "");
                Regex objReg = new System.Text.RegularExpressions.Regex("(<[^>]+?>)|&nbsp;", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                m_outstr = objReg.Replace(m_outstr, "");
                Regex objReg2 = new System.Text.RegularExpressions.Regex("(\\s)+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                m_outstr = objReg2.Replace(m_outstr, " ");
            }
            return m_outstr.Length > firstN ? m_outstr.Substring(0, firstN) : m_outstr;
        }

        /// <summary>
        /// 此私有方法返回一个IP地址对应的无符号整数
        /// </summary>
        /// <param name="x">IP地址</param>
        /// <returns></returns>
        private uint getuintFromIP(IPAddress x)
        {
            Byte[] bt = x.GetAddressBytes();
            uint i = (uint)(bt[0] * 256 * 256 * 256);
            i += (uint)(bt[1] * 256 * 256);
            i += (uint)(bt[2] * 256);
            i += (uint)(bt[3]);
            return i;
        }

        #endregion


        #region 公有文法
        /// <summary>
        /// 此公有方法提取网页中一定字数的纯文本，包括链接文字
        /// </summary>
        /// <param name="firstN">字数</param>
        /// <returns></returns>
        public string getContext(int firstN)
        {
            return getFirstNchar(m_html, firstN, true);
        }

        /// <summary>
        /// 此公有方法提取网页中一定字数的纯文本，不包括链接文字
        /// </summary>
        /// <param name="firstN"></param>
        /// <returns></returns>
        public string getContextWithOutLink(int firstN)
        {
            return getFirstNchar(m_html, firstN, false);
        }

        /// <summary>
        /// 此公有方法从本网页的链接中提取一定数量的链接，该链接的URL满足某正则式
        /// </summary>
        /// <param name="pattern">正则式</param>
        /// <param name="count">返回的链接的个数</param>
        /// <returns>List<Link></returns>
        public List<Link> getSpecialLinksByUrl(string pattern, int count)
        {
            if (m_links.Count == 0) getLinks();
            List<Link> SpecialLinks = new List<Link>();
            List<Link>.Enumerator i;
            i = m_links.GetEnumerator();
            int cnt = 0;
            while (i.MoveNext() && cnt < count)
            {
                if (new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(i.Current.url).Success)
                {
                    SpecialLinks.Add(i.Current);
                    cnt++;
                }
            }
            return SpecialLinks;
        }



        /// <summary>
        /// 此公有方法从本网页的链接中提取一定数量的链接，该链接的文字满足某正则式
        /// </summary>
        /// <param name="pattern">正则式</param>
        /// <param name="count">返回的链接的个数</param>
        /// <returns>List<Link></returns>
        public List<Link> getSpecialLinksByText(string pattern, int count)
        {
            if (m_links.Count == 0) getLinks();
            List<Link> SpecialLinks = new List<Link>();
            List<Link>.Enumerator i;
            i = m_links.GetEnumerator();
            int cnt = 0;
            while (i.MoveNext() && cnt < count)
            {
                if (new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(i.Current.text).Success)
                {
                    SpecialLinks.Add(i.Current);
                    cnt++;
                }
            }
            return SpecialLinks;
        }
        /// <summary>
        /// 此公有方法获得所有链接中在一定IP范围的链接
        /// </summary>
        /// <param name="_ip_start">起始IP</param>
        /// <param name="_ip_end">终止IP</param>
        /// <returns></returns>
        public List<Link> getSpecialLinksByIP(string _ip_start, string _ip_end)
        {
            IPAddress ip_start = IPAddress.Parse(_ip_start);
            IPAddress ip_end = IPAddress.Parse(_ip_end);
            if (m_links.Count == 0) getLinks();
            List<Link> SpecialLinks = new List<Link>();
            List<Link>.Enumerator i;
            i = m_links.GetEnumerator();
            while (i.MoveNext())
            {
                IPAddress ip;
                try
                {
                    ip = Dns.GetHostEntry(new Uri(i.Current.url).Host).AddressList[0];
                }
                catch { continue; }
                if (getuintFromIP(ip) >= getuintFromIP(ip_start) && getuintFromIP(ip) <= getuintFromIP(ip_end))
                {
                    SpecialLinks.Add(i.Current);
                }
            }
            return SpecialLinks;
        }

        /// <summary>
        /// 这公有方法提取本网页的纯文本中满足某正则式的文字
        /// </summary>
        /// <param name="pattern">正则式</param>
        /// <returns>返回文字</returns>
        public string getSpecialWords(string pattern)
        {
            if (m_outstr == "") getContext(Int16.MaxValue);
            Regex regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            Match mc = regex.Match(m_outstr);
            if (mc.Success)
                return mc.Groups[1].Value;
            return string.Empty;
        }
        #endregion


        #region 构造函数

        private void Init(string _url)
        {

            try
            {
                m_uri = new Uri(_url);
                m_links = new List<Link>();
                m_html = "";
                m_outstr = "";
                m_title = "";
                m_good = true;
                if (_url.EndsWith(".rar") || _url.EndsWith(".dat") || _url.EndsWith(".msi"))
                {
                    m_good = false;
                    return;
                }
                HttpWebRequest rqst = (HttpWebRequest)WebRequest.Create(m_uri);
                rqst.AllowAutoRedirect = true;
                rqst.MaximumAutomaticRedirections = 3;
                rqst.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
                rqst.KeepAlive = true;
                rqst.Timeout = 30000;
                lock (WebPage.webcookies)
                {
                    if (WebPage.webcookies.ContainsKey(m_uri.Host))
                        rqst.CookieContainer = WebPage.webcookies[m_uri.Host];
                    else
                    {
                        CookieContainer cc = new CookieContainer();
                        WebPage.webcookies[m_uri.Host] = cc;
                        rqst.CookieContainer = cc;
                    }
                }

                HttpWebResponse rsps = (HttpWebResponse)rqst.GetResponse();

                Stream sm = rsps.GetResponseStream();
                if (!rsps.ContentType.ToLower().StartsWith("text/") || rsps.ContentLength > 1 << 22)
                {
                    rsps.Close();
                    m_good = false;
                    return;
                }
                Encoding cding = System.Text.Encoding.Default;
                string contenttype = rsps.ContentType.ToLower();
                int ix = contenttype.IndexOf("charset=");
                if (ix != -1)
                {

                    try
                    {
                        cding = System.Text.Encoding.GetEncoding(rsps.ContentType.Substring(ix + "charset".Length + 1));
                    }
                    catch
                    {
                        cding = Encoding.Default;
                    }
                    m_html = new StreamReader(sm, cding).ReadToEnd();
                }
                else
                {
                    m_html = new StreamReader(sm, cding).ReadToEnd();
                    Regex regex = new Regex("charset=(?<cding>[^=]+)?\"", RegexOptions.IgnoreCase);
                    string strcding = regex.Match(m_html).Groups["cding"].Value;
                    try
                    {
                        cding = Encoding.GetEncoding(strcding);
                    }
                    catch
                    {
                        cding = Encoding.Default;
                    }
                    byte[] bytes = Encoding.Default.GetBytes(m_html.ToCharArray());
                    m_html = cding.GetString(bytes);
                    if (m_html.Split('?').Length > 100)
                    {
                        m_html = Encoding.Default.GetString(bytes);
                    }
                }


                m_pagesize = m_html.Length;
                m_uri = rsps.ResponseUri;
                rsps.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + m_uri.ToString());
                m_good = false;

            }
        }

        public WebPage(string _url)
        {
            string uurl = "";
            try
            {
                uurl = Uri.UnescapeDataString(_url);
                _url = uurl;
            }
            catch { };
            Regex re = new Regex("(?<h>[^\x00-\xff]+)");
            Match mc = re.Match(_url);
            if (mc.Success)
            {
                string han = mc.Groups["h"].Value;
                _url = _url.Replace(han, System.Web.HttpUtility.UrlEncode(han, Encoding.GetEncoding("GB2312")));
            }

            Init(_url);
        }

        public WebPage(string _url, string _loginurl, string _post)
        {
            string uurl = "";
            try
            {
                uurl = Uri.UnescapeDataString(_url);
                _url = uurl;
            }
            catch { };
            Regex re = new Regex("(?<h>[^\x00-\xff]+)");
            Match mc = re.Match(_url);
            if (mc.Success)
            {
                string han = mc.Groups["h"].Value;
                _url = _url.Replace(han, System.Web.HttpUtility.UrlEncode(han, Encoding.GetEncoding("GB2312")));
            }
            if (_loginurl.Trim() == "" || _post.Trim() == "" || WebPage.webcookies.ContainsKey(new Uri(_url).Host))
            {
                Init(_url);
            }
            else
            {
                #region 登陆
                string indata = _post;
                m_post = _post;
                m_loginurl = _loginurl;
                byte[] bytes = Encoding.Default.GetBytes(_post);
                CookieContainer myCookieContainer = new CookieContainer();
                try
                {

                    //新建一个CookieContainer来存放Cookie集合

                    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(_loginurl);
                    //新建一个HttpWebRequest
                    myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    myHttpWebRequest.AllowAutoRedirect = false;
                    myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
                    myHttpWebRequest.Timeout = 60000;
                    myHttpWebRequest.KeepAlive = true;
                    myHttpWebRequest.ContentLength = bytes.Length;
                    myHttpWebRequest.Method = "POST";
                    myHttpWebRequest.CookieContainer = myCookieContainer;
                    //设置HttpWebRequest的CookieContainer为刚才建立的那个myCookieContainer
                    Stream myRequestStream = myHttpWebRequest.GetRequestStream();
                    myRequestStream.Write(bytes, 0, bytes.Length);
                    myRequestStream.Close();
                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                    foreach (Cookie ck in myHttpWebResponse.Cookies)
                    {
                        myCookieContainer.Add(ck);
                    }
                    myHttpWebResponse.Close();
                }
                catch
                {
                    Init(_url);
                    return;
                }

                #endregion

                #region 登陆后再访问页面
                try
                {
                    m_uri = new Uri(_url);
                    m_links = new List<Link>();
                    m_html = "";
                    m_outstr = "";
                    m_title = "";
                    m_good = true;
                    if (_url.EndsWith(".rar") || _url.EndsWith(".dat") || _url.EndsWith(".msi"))
                    {
                        m_good = false;
                        return;
                    }
                    HttpWebRequest rqst = (HttpWebRequest)WebRequest.Create(m_uri);
                    rqst.AllowAutoRedirect = true;
                    rqst.MaximumAutomaticRedirections = 3;
                    rqst.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
                    rqst.KeepAlive = true;
                    rqst.Timeout = 30000;
                    rqst.CookieContainer = myCookieContainer;
                    lock (WebPage.webcookies)
                    {
                        WebPage.webcookies[m_uri.Host] = myCookieContainer;
                    }
                    HttpWebResponse rsps = (HttpWebResponse)rqst.GetResponse();

                    Stream sm = rsps.GetResponseStream();
                    if (!rsps.ContentType.ToLower().StartsWith("text/") || rsps.ContentLength > 1 << 22)
                    {
                        rsps.Close();
                        m_good = false;
                        return;
                    }
                    Encoding cding = System.Text.Encoding.Default;
                    int ix = rsps.ContentType.ToLower().IndexOf("charset=");
                    if (ix != -1)
                    {
                        try
                        {
                            cding = System.Text.Encoding.GetEncoding(rsps.ContentType.Substring(ix + "charset".Length + 1));
                        }
                        catch
                        {
                            cding = Encoding.Default;
                        }
                    }


                    m_html = new StreamReader(sm, cding).ReadToEnd();


                    m_pagesize = m_html.Length;
                    m_uri = rsps.ResponseUri;
                    rsps.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + m_uri.ToString());
                    m_good = false;

                }
                #endregion
            }

        }

        #endregion


        #region 属性

        /// <summary>
        /// 通过此属性可获得本网页的网址，只读
        /// </summary>
        public string URL
        {
            get
            {
                return m_uri.AbsoluteUri;
            }
        }

        /// <summary>
        /// 通过此属性可获得本网页的标题，只读
        /// </summary>
        public string Title
        {
            get
            {
                if (m_title == "")
                {
                    Regex reg = new Regex(@"(?m)<title[^>]*>(?<title>(?:\w|\W)*?)</title[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    Match mc = reg.Match(m_html);
                    if (mc.Success)
                        m_title = mc.Groups["title"].Value.Trim();
                }
                return m_title;
            }
        }


        /// <summary>
        /// 此属性获得本网页的所有链接信息，只读
        /// </summary>
        public List<Link> Links
        {
            get
            {
                if (m_links.Count == 0) getLinks();
                return m_links;
            }
        }


        /// <summary>
        /// 此属性返回本网页的全部纯文本信息，只读
        /// </summary>
        public string Context
        {
            get
            {
                if (m_outstr == "") getContext(Int16.MaxValue);
                return m_outstr;
            }
        }

        /// <summary>
        /// 此属性获得本网页的大小
        /// </summary>
        public int PageSize
        {
            get
            {
                return m_pagesize;
            }
        }
        /// <summary>
        /// 此属性获得本网页的所有站内链接
        /// </summary>
        public List<Link> InsiteLinks
        {
            get
            {
                return getSpecialLinksByUrl("^http://" + m_uri.Host, Int16.MaxValue);
            }
        }

        /// <summary>
        /// 此属性表示本网页是否可用
        /// </summary>
        public bool IsGood
        {
            get
            {
                return m_good;
            }
        }
        /// <summary>
        /// 此属性表示网页的所在的网站
        /// </summary>
        public string Host
        {
            get
            {
                return m_uri.Host;
            }
        }


        /// <summary>
        /// 此网页的登陆页所需的POST数据
        /// </summary>
        public string PostStr
        {
            get
            {
                return m_post;
            }
        }
        /// <summary>
        /// 此网页的登陆页
        /// </summary>
        public string LoginURL
        {
            get
            {
                return m_loginurl;
            }
        }

        /// <summary>
        /// HTML Content
        /// </summary>
        public string Html
        {
            get { return m_html; }
            set { m_html = value; }
        }
        #endregion
    }

    /// <summary>
    /// 链接类
    /// </summary>
    public class Link
    {
        public string url;   //链接网址
        public string text;  //链接文字
        public Link(string _url, string _text)
        {
            url = _url;
            text = _text;
        }
    }
}