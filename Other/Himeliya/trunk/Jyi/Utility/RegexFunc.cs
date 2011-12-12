using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace Jyi.Utility
{
    class RegexFunc
    {
        public string GetMatch(string strSource,string strRegex)
        {
            try
            {

                Regex r;
                MatchCollection m;
                r = new Regex(strRegex, RegexOptions.IgnoreCase);
                m = r.Matches(strSource);

                if (m.Count <= 0) return string.Format("Æ¥ÅäÊ§°Ü\r\n{0}", strSource);

                return m[0].Groups[1].Value;
            }
            catch
            {
                return "";
            }
        }

        public MatchCollection GetMatchFull(string strSource, string strRegex)
        {
            try
            {

                Regex r;
                MatchCollection m;
                r = new Regex(strRegex, RegexOptions.IgnoreCase);
                m = r.Matches(strSource);

                if (m.Count <= 0) return null;

                return m;
            }
            catch
            {
                return null;
            }
        }
    }
}
