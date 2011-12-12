using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Natsuhime.Web.Plugin.Discuz
{
    public class TextAnalyze
    {
        public static MatchCollection GetThreadsInBoard(string sourceHtml)
        {
            string regexstring = RegexStringLib.GetThreadsInBoard();
            MatchCollection mc = RegexUtility.GetMatchFull(sourceHtml, regexstring);
            return mc;
        }
        public static Dictionary<string, string> GetThreadsInBoard(string sourceHtml, string baseUrl)
        {
            Dictionary<string, string> threads = new Dictionary<string, string>();
            MatchCollection mc = GetThreadsInBoard(sourceHtml);

            if (mc != null)
            {
                foreach (Match m in mc)
                {
                    string fullUrl = Utils.CompleteRelativeUrl(baseUrl, m.Groups[1].Value);
                    if (!threads.ContainsKey(fullUrl))
                    {
                        threads.Add(
                            fullUrl,
                            m.Groups[2].Value
                            );
                    }
                }
            }
            return threads;
        }

        public static List<string> GetFilesInPost(string sourceHtml, string baseUrl)
        {
            List<string> fileUrls = new List<string>();

            string regexString = Natsuhime.Text.RegexStringLib.GetFileUrlRegexString("jpg|jpeg|png|ico|bmp|gif");
            List<string> imageShortUrls = Natsuhime.Text.TextAnalyze.GetUrlList(sourceHtml, regexString, baseUrl);

            if (imageShortUrls.Count > 0)
            {
                foreach (string url in imageShortUrls)
                {
                    string fullUrl = Utils.CompleteRelativeUrl(baseUrl, url);
                    fileUrls.Add(fullUrl);
                }
            }
            return fileUrls;
        }

        public static int GetBoardPageCount(string sourceHtml)
        {
            return GetBoardPageCountPro(sourceHtml);

            #region 不用的匹配方式
            string regexstring = RegexStringLib.GetBoardPageCount();
            string result = RegexUtility.GetMatch(sourceHtml, regexstring);

            int pageCount;
            if (int.TryParse(result, out pageCount))
            {
                return pageCount;
            }
            else
            {
                return -1;
            }
            #endregion
        }

        public static int GetBoardPageCountPro(string sourceHtml)
        {
            string regexString = "(<div\\s+class=\"pages\".*?>([<.*?>]*)</div>)";
            string result = RegexUtility.GetMatch(sourceHtml, regexString).Replace("</a>","</a>|").Replace("<a","|<a");
            string nums = Web.Utils.RemoveHtml(result);

            MatchCollection mc = RegexUtility.GetMatchFull(nums, "[0-9]+");

            string pageString = string.Empty;
            if (mc.Count > 0)
            {
                pageString = mc[mc.Count - 1].Groups[0].Value;
            }

            int pageCount;
            if (int.TryParse(pageString, out pageCount))
            {
                return pageCount;
            }
            else
            {
                return -1;
            }
        }
    }
}
