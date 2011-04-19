using System;
using System.Xml.Serialization;

namespace Natsuhime.Data
{
    [Serializable, XmlRoot("BaseConfigInfo")]
    public class DbConfigInfo
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public string Dbconnectstring { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string Dbtype { get; set; }
    }
}
