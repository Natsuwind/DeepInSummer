using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 用户权限设置
    /// </summary>
    public partial class cloudindex : AdminPage
    {
        public DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();

        public string iFrameUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            iFrameUrl = config.Cloudenabled == 1 ? DiscuzCloud.GetCloudAppListIFrame(userid) : string.Empty;
        }
    }
}