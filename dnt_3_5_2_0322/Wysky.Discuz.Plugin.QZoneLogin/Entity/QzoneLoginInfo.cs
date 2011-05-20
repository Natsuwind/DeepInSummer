using System;
using System.Collections.Generic;
using System.Web;

namespace Wysky.Discuz.Plugin.QZoneLogin.Entity
{
    [Serializable]
    public class QzoneLoginInfo
    {
        /// <summary>
        /// 返回码 
        /// </summary>
        public int ret { get; set; }
        /// <summary>
        /// 如果ret<0，会有相应的错误信息提示，返回数据全部用UTF-8编码 
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 昵称 
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 头像URL
        /// </summary>
        public string figureurl { get; set; }
        public string figureurl_1 { get; set; }
        public string figureurl_2 { get; set; }
    }
}