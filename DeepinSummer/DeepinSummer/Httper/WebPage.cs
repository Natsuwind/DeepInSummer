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
*  ������Ľ���������뷢һ�ݴ������(heilong05 ��163.com)
*/
    /// <summary>
    /// ��ҳ��
    /// </summary>
    public class WebPage
    {

        #region ˽�г�Ա
        private Uri m_uri;   //��ַ
        private List<Link> m_links;    //����ҳ�ϵ�����
        private string m_title;        //����ҳ�ı���
        private string m_html;         //����ҳ��HTML����
        private string m_outstr;       //����ҳ������Ĵ��ı�
        private bool m_good;           //����ҳ�Ƿ����
        private int m_pagesize;       //����ҳ�Ĵ�С
        private static Dictionary<string, CookieContainer> webcookies = new Dictionary<string, CookieContainer>();//���������ҳ��Cookie
        private string m_post;  //����ҳ�ĵ�½ҳ��Ҫ��POST����
        private string m_loginurl;  //����ҳ�ĵ�½ҳ
        #endregion


        #region ˽�з���
        /// <summary>
        /// ��˽�з�������ҳ��HTML�����з�����������Ϣ
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
        /// ��˽�з�����һ��HTML�ı�����ȡ��һ�������Ĵ��ı�
        /// </summary>
        /// <param name="instr">HTML����</param>
        /// <param name="firstN">��ȡ��ͷ�����ٸ���</param>
        /// <param name="withLink">�Ƿ�Ҫ�����������</param>
        /// <returns>���ı�</returns>
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
        /// ��˽�з�������һ��IP��ַ��Ӧ���޷�������
        /// </summary>
        /// <param name="x">IP��ַ</param>
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


        #region �����ķ�
        /// <summary>
        /// �˹��з�����ȡ��ҳ��һ�������Ĵ��ı���������������
        /// </summary>
        /// <param name="firstN">����</param>
        /// <returns></returns>
        public string getContext(int firstN)
        {
            return getFirstNchar(m_html, firstN, true);
        }

        /// <summary>
        /// �˹��з�����ȡ��ҳ��һ�������Ĵ��ı�����������������
        /// </summary>
        /// <param name="firstN"></param>
        /// <returns></returns>
        public string getContextWithOutLink(int firstN)
        {
            return getFirstNchar(m_html, firstN, false);
        }

        /// <summary>
        /// �˹��з����ӱ���ҳ����������ȡһ�����������ӣ������ӵ�URL����ĳ����ʽ
        /// </summary>
        /// <param name="pattern">����ʽ</param>
        /// <param name="count">���ص����ӵĸ���</param>
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
        /// �˹��з����ӱ���ҳ����������ȡһ�����������ӣ������ӵ���������ĳ����ʽ
        /// </summary>
        /// <param name="pattern">����ʽ</param>
        /// <param name="count">���ص����ӵĸ���</param>
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
        /// �˹��з������������������һ��IP��Χ������
        /// </summary>
        /// <param name="_ip_start">��ʼIP</param>
        /// <param name="_ip_end">��ֹIP</param>
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
        /// �⹫�з�����ȡ����ҳ�Ĵ��ı�������ĳ����ʽ������
        /// </summary>
        /// <param name="pattern">����ʽ</param>
        /// <returns>��������</returns>
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


        #region ���캯��

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
                #region ��½
                string indata = _post;
                m_post = _post;
                m_loginurl = _loginurl;
                byte[] bytes = Encoding.Default.GetBytes(_post);
                CookieContainer myCookieContainer = new CookieContainer();
                try
                {

                    //�½�һ��CookieContainer�����Cookie����

                    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(_loginurl);
                    //�½�һ��HttpWebRequest
                    myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    myHttpWebRequest.AllowAutoRedirect = false;
                    myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
                    myHttpWebRequest.Timeout = 60000;
                    myHttpWebRequest.KeepAlive = true;
                    myHttpWebRequest.ContentLength = bytes.Length;
                    myHttpWebRequest.Method = "POST";
                    myHttpWebRequest.CookieContainer = myCookieContainer;
                    //����HttpWebRequest��CookieContainerΪ�ղŽ������Ǹ�myCookieContainer
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

                #region ��½���ٷ���ҳ��
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


        #region ����

        /// <summary>
        /// ͨ�������Կɻ�ñ���ҳ����ַ��ֻ��
        /// </summary>
        public string URL
        {
            get
            {
                return m_uri.AbsoluteUri;
            }
        }

        /// <summary>
        /// ͨ�������Կɻ�ñ���ҳ�ı��⣬ֻ��
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
        /// �����Ի�ñ���ҳ������������Ϣ��ֻ��
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
        /// �����Է��ر���ҳ��ȫ�����ı���Ϣ��ֻ��
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
        /// �����Ի�ñ���ҳ�Ĵ�С
        /// </summary>
        public int PageSize
        {
            get
            {
                return m_pagesize;
            }
        }
        /// <summary>
        /// �����Ի�ñ���ҳ������վ������
        /// </summary>
        public List<Link> InsiteLinks
        {
            get
            {
                return getSpecialLinksByUrl("^http://" + m_uri.Host, Int16.MaxValue);
            }
        }

        /// <summary>
        /// �����Ա�ʾ����ҳ�Ƿ����
        /// </summary>
        public bool IsGood
        {
            get
            {
                return m_good;
            }
        }
        /// <summary>
        /// �����Ա�ʾ��ҳ�����ڵ���վ
        /// </summary>
        public string Host
        {
            get
            {
                return m_uri.Host;
            }
        }


        /// <summary>
        /// ����ҳ�ĵ�½ҳ�����POST����
        /// </summary>
        public string PostStr
        {
            get
            {
                return m_post;
            }
        }
        /// <summary>
        /// ����ҳ�ĵ�½ҳ
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
    /// ������
    /// </summary>
    public class Link
    {
        public string url;   //������ַ
        public string text;  //��������
        public Link(string _url, string _text)
        {
            url = _url;
            text = _text;
        }
    }
}