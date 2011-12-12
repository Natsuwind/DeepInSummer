using System;
using System.Collections.Generic;
using System.Text;

namespace LiteCMS.Config
{
    [Serializable]
    public class BaseConfigInfo
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public string Dbconnectstring { get; set; }
        public string Dbtype { get; set; }
        /// <summary>
        /// 表前缀
        /// </summary>
        public string Tableprefix { get; set; }
        /// <summary>
        /// 安装目录(相对于应用程序池)
        /// </summary>
        public string ApplictionPath { get; set; }
    }
}
