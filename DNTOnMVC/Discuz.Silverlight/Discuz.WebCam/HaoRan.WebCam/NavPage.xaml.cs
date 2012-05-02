using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace HaoRan.WebCam
{
    public partial class NavPage : UserControl
    {
        public NavPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NavPage_Loaded);
        }

        #region 注册 js对象和相关事件handler声明
        void NavPage_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.RegisterScriptableObject("JavaScriptObject", javaScriptableObject);
        }

        [ScriptableType]
        public class AvatarEventArgs : EventArgs
        {
            [ScriptableMember]
            public string AvatarUrl { get; set; }
        }

        public static JavaScriptableObject javaScriptableObject = new JavaScriptableObject();

        /// <summary>
        /// 要注册并在页面中使用的js调用脚本对象
        /// </summary>
        [ScriptableType]
        public class JavaScriptableObject
        {
            /// <summary>
            /// js捆绑的事件处理器
            /// </summary>
            [ScriptableMember]
            public event EventHandler<AvatarEventArgs> CloseAvatar;

            public void OnCloseAvatar(string eventArgs)
            {
                if (CloseAvatar != null)
                {
                    CloseAvatar(this, new AvatarEventArgs()
                    {
                        AvatarUrl = eventArgs
                    });
                }
            }
        }
        #endregion
    }
}
