using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Natsuhime.Common
{
    public class TypeParse
    {
        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]*$");
        }
        public static bool IsDouble(object expression)
        {
            if (expression != null)
            {
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }
        /// <summary>
        /// 判断给定的字符串中的数据是不是都为数值型(字符串中逗号分隔)
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumericString(string strNumber)
        {
            string[] numbers = strNumber.Split(',');
            return IsNumericArray(numbers);
        }
        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
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
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            string str = expression.ToString();
            if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
            {
                if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                string str = expression.ToString();
                if (string.Compare(str, "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(str, "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object expression, int defValue)
        {
            if (expression != null)
            {
                string str = expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return Convert.ToInt32(str);
                    }
                }
                return defValue;
            }
            return defValue;
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object expression, float defValue)
        {
            if (expression != null)
            {
                string strValue = expression.ToString();

                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    return Convert.ToSingle(strValue);
                }
                return defValue;
            }
            return defValue;
        }

        /// <summary>
        /// 时间戳转换成DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Timestamp2Date(string s)
        {
            string timeStamp = s;
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// DateTime转换成时间戳
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DateTimeTimestamp(string s)
        {
            DateTime dtNow = DateTime.Parse(s);
            return DateTime2TimeStamp(dtNow);
        }

        private static string DateTime2TimeStamp(DateTime dtNow)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            return timeStamp.Substring(0, timeStamp.Length - 7);
        }

        /// <summary>
        /// 取得当前的时间戳.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUnixTimestamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            return timeStamp.Substring(0, timeStamp.Length - 7);
        }

        /// <summary>
        /// 取得某个时间的时间戳
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetTimestamp(int year, int month, int day)
        {
            DateTime timeStamp0 = new DateTime(1970, 1, 1);
            DateTime timeStamp = new DateTime(year, month, day);
            long a = (timeStamp.Ticks - timeStamp0.Ticks) / 10000000 - 8 * 60 * 60;
            return a.ToString();
        }
    }
}
