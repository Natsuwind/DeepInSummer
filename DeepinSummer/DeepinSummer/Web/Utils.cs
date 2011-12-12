using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Natsuhime.Web
{
    public class Utils
    {
        /// <summary>
        /// ���α��̬ҳ����ʾ����
        /// </summary>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <param name="pagecount">��ҳ��</param>
        /// <param name="pageurl">��������ҳ���ַ(�ļ���)</param>
        /// <param name="pageextname">��������ҳ���ַ(��չ��,��.��)</param>
        /// <param name="extendpage">�ܱ�ҳ����ʾ��������</param>
        /// <returns>html����</returns>
        public static string GetStaticPageNumbersHtml(int currentpage, int pagecount, string pageurl, string pageextname, int extendpage)
        {
            int startPage = 1;
            int endPage = 1;

            string t1 = "<a href=\"" + pageurl + "-1" + pageextname + "\">��ҳ&laquo;</a>";
            string t2 = "<a href=\"" + pageurl + "-" + pagecount + pageextname + "\">&raquo;ĩҳ</a>";

            if (pagecount < 1) pagecount = 1;
            if (extendpage < 3) extendpage = 2;

            if (pagecount > extendpage)
            {
                if (currentpage - (extendpage / 2) > 0)
                {
                    if (currentpage + (extendpage / 2) < pagecount)
                    {
                        startPage = currentpage - (extendpage / 2);
                        endPage = startPage + extendpage - 1;
                    }
                    else
                    {
                        endPage = pagecount;
                        startPage = endPage - extendpage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendpage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = pagecount;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == currentpage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(pageurl);
                    s.Append("-");
                    s.Append(i);
                    s.Append(pageextname);
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }
        /// <summary>
        /// ���ҳ����ʾ����
        /// </summary>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <param name="pagecount">��ҳ��</param>
        /// <param name="pageurl">�������ӵ�ַ(���ļ����͹��ò���Ĳ���)</param>
        /// <param name="extendpage">�ܱ�ҳ����ʾ��������</param>
        /// <param name="pagepramname">ҳ��������</param>
        /// <param name="anchor">ê��</param>
        /// <returns>Html����</returns>
        public static string GetPageNumbersHtml(int currentpage, int pagecount, string pageurl, int extendpage, string pagepramname, string anchor)
        {
            if (pagepramname == "")
                pagepramname = "page";
            int startPage = 1;
            int endPage = 1;

            if (pageurl.IndexOf("?") > 0)
            {
                pageurl = pageurl + "&";
            }
            else
            {
                pageurl = pageurl + "?";
            }

            string t1 = "<a href=\"" + pageurl + "&" + pagepramname + "=1";
            string t2 = "<a href=\"" + pageurl + "&" + pagepramname + "=" + pagecount;
            if (anchor != null)
            {
                t1 += anchor;
                t2 += anchor;
            }
            t1 += "\">��ҳ&laquo;</a>";
            t2 += "\">&raquo;ĩҳ</a>";

            if (pagecount < 1)
                pagecount = 1;
            if (extendpage < 3)
                extendpage = 2;

            if (pagecount > extendpage)
            {
                if (currentpage - (extendpage / 2) > 0)
                {
                    if (currentpage + (extendpage / 2) < pagecount)
                    {
                        startPage = currentpage - (extendpage / 2);
                        endPage = startPage + extendpage - 1;
                    }
                    else
                    {
                        endPage = pagecount;
                        startPage = endPage - extendpage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendpage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = pagecount;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == currentpage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(pageurl);
                    s.Append(pagepramname);
                    s.Append("=");
                    s.Append(i);
                    if (anchor != null)
                    {
                        s.Append(anchor);
                    }
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }


        /// <summary>
        /// �Ƴ�Html���
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// ����HTML�еĲ���ȫ��ǩ
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }

        /// <summary>
        /// ���� HTML �ַ����ı�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>������</returns>
        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// ���� HTML �ַ����Ľ�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>������</returns>
        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// ���� URL �ַ����ı�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>������</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str, Encoding.Default);
        }
        public static string UrlEncode(string str, string encodingName)
        {
            return HttpUtility.UrlEncode(str, Encoding.GetEncoding(encodingName));
        }
        public static string UrlEncode(string str, Encoding e)
        {
            return HttpUtility.UrlEncode(str, e);
        }

        /// <summary>
        /// ���� URL �ַ����ı�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns>������</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str, Encoding.Default);
        }
        public static string UrlDecode(string str, string encodingName)
        {
            return HttpUtility.UrlDecode(str, Encoding.GetEncoding(encodingName));
        }
        public static string UrlDecode(string str, Encoding e)
        {
            return HttpUtility.UrlDecode(str, e);
        }

        /// <summary>
        /// ȡ��HTML������ͼƬ�� URL��
        /// </summary>
        /// <param name="sHtmlText">HTML����</param>
        /// <returns>ͼƬ��URL�б�</returns>
        public static string[] GetImageUrls(string sourcehtml)
        {
            //ƥ��img��ǩ
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            //����ƥ���ַ���
            MatchCollection mc = regImg.Matches(sourcehtml);

            int i = 0;
            string[] urls = new string[mc.Count];

            // ȡ��ƥ�����б�
            foreach (Match match in mc)
                urls[i++] = match.Groups["imgUrl"].Value;

            return urls;
        }


        public static string CompleteRelativeUrl(string baseUrl, string RelativeUrl)
        {
            ////��·������
            //if (RelativeUrl.IndexOf("http://") == -1 && RelativeUrl.IndexOf("https://") == -1)
            //    RelativeUrl = (RelativeUrl[0] == '/' ? domainName : dnDir) + RelativeUrl;

            if (RelativeUrl.IndexOf("http://") == -1 && RelativeUrl.IndexOf("https://") == -1)
            {
                if (RelativeUrl[0] == '.')
                {
                    RelativeUrl = baseUrl + RelativeUrl.TrimStart('.').TrimStart('/');
                }
                if (RelativeUrl[0] == '/')
                {
                    RelativeUrl = baseUrl + RelativeUrl.TrimStart('/');
                }

                //www.abc.com/1.jpg��������,images/1.jpg��Ҫ���
                if (RelativeUrl.Split('.').Length < 3)
                {
                    RelativeUrl = baseUrl + RelativeUrl;
                }
            }
            return RelativeUrl;
        }
    }
}
