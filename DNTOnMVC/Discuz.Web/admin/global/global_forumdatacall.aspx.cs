using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Plugin.Mail;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑邮件配置
    /// </summary>
    public partial class ForumDataCall : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }

        #endregion

    }
}