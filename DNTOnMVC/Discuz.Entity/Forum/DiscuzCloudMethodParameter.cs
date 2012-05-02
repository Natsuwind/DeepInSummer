using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;

namespace Discuz.Entity
{
    public class DiscuzCloudMethodParameter
    {
        public const string PARAMKEYTEMP = "args%5B{0}%5D";

        private List<DiscuzCloudKeyValue> _paramList;

        private bool _inArgs = true;

        public DiscuzCloudMethodParameter()
        {
            _paramList = new List<DiscuzCloudKeyValue>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inArgs">是否给postname添加args前缀</param>
        public DiscuzCloudMethodParameter(bool inArgs)
        {
            _inArgs = inArgs;
            _paramList = new List<DiscuzCloudKeyValue>();
        }

        public void Add(string key, string value)
        {
            _paramList.Add(DiscuzCloudKeyValue.Create(key, value));
        }

        public string Find(string key)
        {
            foreach (DiscuzCloudKeyValue kv in _paramList)
            {
                if (key == kv.Name)
                    return kv.Value;
            }
            return null;
        }

        public string GetPostData()
        {
            StringBuilder result = new StringBuilder();
            _paramList.Sort();
            foreach (DiscuzCloudKeyValue kv in _paramList)
            {
                result.AppendFormat("&{0}={1}", _inArgs ? string.Format(PARAMKEYTEMP, kv.Name) : kv.Name, Utils.PHPUrlEncode(kv.Value));
            }
            return result.ToString();
        }
    }

    public class DiscuzCloudKeyValue : IComparable
    {
        private string name;
        private object value;

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value
        {
            get
            {
                if (value is Array)
                    return ConvertArrayToString(value as Array);
                else
                    return value.ToString();
            }
        }

        protected DiscuzCloudKeyValue(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Utils.UrlEncode(Value));
        }

        /// <summary>
        /// 创建参数对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DiscuzCloudKeyValue Create(string name, object value)
        {
            return new DiscuzCloudKeyValue(name, value);
        }

        /// <summary>
        /// .Sort()排序规则,区分大小写的排序
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (!(obj is DiscuzCloudKeyValue))
                return -1;

            char[] a = this.name.ToCharArray();
            char[] b = (obj as DiscuzCloudKeyValue).name.ToCharArray();
            int length = a.Length > b.Length ? b.Length : a.Length;
            int i = -1;
            while (++i < length)
            {
                if (a[i] == b[i])
                    continue;
                return a[i] < b[i] ? -1 : 1;
            }
            return -1;
        }

        /// <summary>
        /// 将数组转为字符串
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static string ConvertArrayToString(Array a)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < a.Length; i++)
            {
                if (i > 0)
                    builder.Append(",");

                builder.Append(a.GetValue(i).ToString());
            }

            return builder.ToString();
        }
    }
}
