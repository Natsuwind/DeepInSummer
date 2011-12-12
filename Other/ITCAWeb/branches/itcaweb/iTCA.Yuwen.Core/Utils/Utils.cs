using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace iTCA.Yuwen.Core
{
    public class Utils
    {
        /// <summary>
        /// �жϸ������ַ����е������ǲ��Ƕ�Ϊ��ֵ��(�ַ����ж��ŷָ�)
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumericString(string strNumber)
        {
            string[] numbers = strNumber.Split(',');
            return IsNumericArray(numbers);
        }
        /// <summary>
        /// �жϸ������ַ�������(strNumber)�е������ǲ��Ƕ�Ϊ��ֵ��
        /// </summary>
        /// <param name="strNumber">Ҫȷ�ϵ��ַ�������</param>
        /// <returns>���򷵼�true �����򷵻� false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// �ж϶����Ƿ�ΪInt32���͵�����
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
            {
                return IsNumeric(expression.ToString());
            }
            return false;

        }

        /// <summary>
        /// �ж϶����Ƿ�ΪInt32���͵�����
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && System.Text.RegularExpressions.Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }


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
    }
}
