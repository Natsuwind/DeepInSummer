using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Natsuhime
{
    public class RegexUtility
    {
        public static string GetMatch(string strSource, string strRegex)
        {
            try
            {
                Regex r = new Regex(strRegex, RegexOptions.IgnoreCase);
                MatchCollection m = r.Matches(strSource);

                if (m.Count <= 0)
                    return string.Empty;
                else
                    return m[0].Groups[1].Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static MatchCollection GetMatchFull(string strSource, string strRegex)
        {
            return GetMatchFull(strSource, strRegex, RegexOptions.IgnoreCase);
        }
        public static MatchCollection GetMatchFull(string strSource, string strRegex, RegexOptions options)
        {
            try
            {
                Regex r = new Regex(strRegex, options);
                MatchCollection m = r.Matches(strSource);

                if (m.Count <= 0)
                    return null;
                else
                    return m;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 正则替换
        /// </summary>
        /// <param name="pattern">替换规则</param>
        /// <param name="input">原始字符串</param>
        /// <param name="replacement">替换为</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceRegex(string pattern, string input, string replacement)
        {
            return ReplaceRegex(pattern, input, replacement, RegexOptions.IgnoreCase);
        }

        public static string ReplaceRegex(string pattern, string input, string replacement, RegexOptions options)
        {
            // Regex search and replace
            Regex regex = new Regex(pattern, options);
            return regex.Replace(input, replacement);
        }
    }
}
