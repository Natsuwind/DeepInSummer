using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace Yuwen.Tools.Httper
{
    public class RegexFunc
    {
        public string GetMatch(string strSource,string strRegex)
        {
            try
            {
                Regex r;
                MatchCollection m;
                r = new Regex(strRegex, RegexOptions.IgnoreCase);
                m = r.Matches(strSource);

                if (m.Count <= 0) return "";

                return m[0].Groups[1].Value;
            }
            catch
            {
                return "";
            }
        }
    }
}
