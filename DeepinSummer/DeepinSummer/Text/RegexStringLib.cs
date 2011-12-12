using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Text
{
    public class RegexStringLib
    {
        /// <summary>
        /// 取得获取网页中文件的正则表达式
        /// </summary>
        /// <param name="exts">需要获取的扩展名,用|分割.(比如"jpg|jpeg|png|ico|bmp|gif")</param>
        /// <returns></returns>
        public static string GetFileUrlRegexString(string exts)
        {
            return string.Format(@"((http(s)?://)?)+(((/?)+[\w-.]+(/))*)+[\w-./]+\.+({0})", exts);
        }
    }
}
