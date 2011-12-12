using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Natsuhime.Common
{
    public class Utils
    {
        /// <summary>
        /// �Ƿ�Ϊip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// ����Ƿ���SqlΣ���ַ�
        /// </summary>
        /// <param name="str">Ҫ�ж��ַ���</param>
        /// <returns>�жϽ��</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// ��õ�ǰ����·��
        /// </summary>
        /// <param name="strPath">ָ����·��</param>
        /// <returns>����·��</returns>
        public static string GetMapPath(string strPath)
        {
            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.Server.MapPath(strPath);
            }
            else //��web��������
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
        /// ʱ���ת��ΪDateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            //1 ��ϵͳʱ��ת����UNIXʱ���
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970,1,1));
            //DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            //TimeSpan toNow = dtNow.Subtract(dtStart);
            //string timeStamp = toNow.Ticks.ToString();
            //timeStamp = timeStamp.Substring(0,timeStamp.Length - 7);
            //2��UNIXʱ���ת����ϵͳʱ��
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timestamp * 10000000);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        public static string UnicodeCharToChineseChar(string str)
        {
            ////����תΪUNICODE�ַ�
            //string str = "����";
            //string outStr = "";
            //if (!string.IsNullOrEmpty(str))
            //{
            //    for (int i = 0; i < str.Length; i++)
            //    {
            //        //�������ַ�תΪ10����������Ȼ��תΪ16����unicode�ַ�
            //        outStr += "\\u" + ((int)str[i]).ToString("x");
            //    }
            //}

            //UNICODE�ַ�תΪ����
            //string str = "\\u4e2d\\u6587";
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //��unicode�ַ�תΪ10����������Ȼ��תΪchar�����ַ�
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
        /// MD5����(32λ)
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <returns>32λ���ܺ���ַ���</returns>
        public static string MD5(string str)
        {
            return MD5(str, false);
        }
        /// <summary>
        /// MD5����(32/16λ)
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="cry16">�Ƿ����16λ���ܷ�ʽ</param>
        /// <returns>���ܺ���ַ���</returns>
        public static string MD5(string str, bool cry16)
        {
            if (cry16) //16λMD5���ܣ�ȡ32λ���ܵ�9~25�ַ��� 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }
            else//32λ���� 
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
