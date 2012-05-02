using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace HaoRan.WebCam
{
    public static class Constants
    {
        /// <summary>
        /// 可能的状态
        /// </summary>
        public enum FileStates
        {
            /// <summary>
            /// 暂停
            /// </summary>
            Pending = 0,
            /// <summary>
            /// 上传中
            /// </summary>
            Uploading = 1,
            /// <summary>
            /// 结束
            /// </summary>
            Finished = 2,
            /// <summary>
            /// 移除
            /// </summary>
            Deleted = 3,
            /// <summary>
            /// 错误
            /// </summary>
            Error = 4
        }
    }
}
