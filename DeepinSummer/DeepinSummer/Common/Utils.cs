using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Natsuhime.Common
{
    public class Utils
    {
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.TrimStart('~').Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        /// <summary>
        /// 时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            //1 将系统时间转换成UNIX时间戳
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970,1,1));
            //DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            //TimeSpan toNow = dtNow.Subtract(dtStart);
            //string timeStamp = toNow.Ticks.ToString();
            //timeStamp = timeStamp.Substring(0,timeStamp.Length - 7);
            //2将UNIX时间戳转换成系统时间
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timestamp * 10000000);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        public static string UnicodeCharToChineseChar(string str)
        {
            ////中文转为UNICODE字符
            //string str = "中文";
            //string outStr = "";
            //if (!string.IsNullOrEmpty(str))
            //{
            //    for (int i = 0; i < str.Length; i++)
            //    {
            //        //将中文字符转为10进制整数，然后转为16进制unicode字符
            //        outStr += "\\u" + ((int)str[i]).ToString("x");
            //    }
            //}

            //UNICODE字符转为中文
            //string str = "\\u4e2d\\u6587";
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            return outStr;
        }

        /// <summary>
        /// MD5加密(32位)
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>32位加密后的字符串</returns>
        public static string MD5(string str)
        {
            return MD5(str, false);
        }
        /// <summary>
        /// MD5加密(32/16位)
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="cry16">是否采用16位加密方式</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string str, bool cry16)
        {
            if (cry16) //16位MD5加密（取32位加密的9~25字符） 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }
            else//32位加密 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }
        }


        private static char[] constant ={
                                            '0','1','2','3','4','5','6','7','8','9',
                                            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                                            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
                                        };
        public enum RandomType
        {
            All,
            Number,
            Uppercased,
            Lowercased,
            NumberAndUppercased,
            NumberAndLowercased,
            UppercasedAndLowercased,
        }
        public static string GenerateRandom(int Length, RandomType rt)
        {
            int initsize = 0;
            int beginsize = 0;
            int endsize = 0;
            Boolean IsCross = false;
            switch (rt)
            {
                case RandomType.All:
                    {
                        initsize = 62;
                        beginsize = 1;
                        endsize = 62;
                        //IsCross = false;
                        break;
                    }
                case RandomType.Lowercased:
                    {
                        initsize = 26;
                        beginsize = 11;
                        endsize = 36;
                        //IsCross = false;
                        break;
                    }
                case RandomType.Uppercased:
                    {
                        initsize = 26;
                        beginsize = 37;
                        endsize = 62;
                        // IsCross = false;
                        break;
                    }
                case RandomType.Number:
                    {
                        initsize = 10;
                        beginsize = 1;
                        endsize = 10;
                        //IsCross = false;
                        break;
                    }
                case RandomType.UppercasedAndLowercased:
                    {
                        initsize = 52;
                        beginsize = 11;
                        endsize = 62;
                        //IsCross = false;
                        break;
                    }
                case RandomType.NumberAndLowercased:
                    {
                        initsize = 36;
                        beginsize = 1;
                        endsize = 36;
                        //IsCross = false;
                        break;
                    }
                case RandomType.NumberAndUppercased:
                    {
                        IsCross = true;
                        break;
                    }
            }


            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(initsize);
            Random rd = new Random();
            if (!IsCross)
            {
                for (int i = 0; i < Length; i++)
                {
                    newRandom.Append(constant[rd.Next(beginsize, endsize)]);
                }
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    newRandom.Append(constant[rd.Next(1, 10)]);
                    newRandom.Append(constant[rd.Next(37, 62)]);
                }
            }

            return newRandom.ToString();
        }

    }
}
