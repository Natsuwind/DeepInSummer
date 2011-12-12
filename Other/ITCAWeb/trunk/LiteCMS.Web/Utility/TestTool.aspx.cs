using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using LiteCMS.Config;
using Natsuhime;
using Natsuhime.Common;

namespace LiteCMS.Web.Utility
{
    public partial class TestTool : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            BaseConfigInfo info = new BaseConfigInfo();
            info.Dbconnectstring = @"Data Source=F:\Doctments\Works\Mine\iTCAWeb\trunk\LiteCMS.Web\config\db.config";
            info.Dbtype = "Sqlite";
            info.Tableprefix = "wy_";
            info.ApplictionPath = "/";
            BaseConfigs.Save(info);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            MainConfigInfo info = new MainConfigInfo();
            info.Closed = 0;
            info.Closedreason = "";
            info.ApplictionSecKey = Guid.NewGuid().ToString();
            info.Cookiedomain = "";
            info.Urlrewrite = 0;
            info.Urlrewriteextname = ".aspx";
            info.Websitename = "盛夏之地";
            info.Seotitle = "技术同好会";

            MainConfigs.Save(info);
        }
    }
}
