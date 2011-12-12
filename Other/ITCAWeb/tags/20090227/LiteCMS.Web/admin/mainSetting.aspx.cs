using System;
using System.IO;
using System.Web;
using LiteCMS.Config;
using Natsuhime.Common;

namespace LiteCMS.Web.Admin
{
    public partial class mainsetting : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //读取
                MainConfigInfo mainconfiginfo = MainConfigs.Load();
                tbxWebSiteName.Text = mainconfiginfo.Websitename;
                tbxSEOTitle.Text = mainconfiginfo.Seotitle;
                ckbxUrlRewrite.Checked = mainconfiginfo.Urlrewrite == 1 ? true : false;
                tbxUrlRewriteExtName.Text = mainconfiginfo.Urlrewriteextname;
                tbxGlobalCacheTimeOut.Text = mainconfiginfo.Globalcachetimeout.ToString();
                tbxAnalyticsCode.Text = mainconfiginfo.Analyticscode.Trim();
            }
            else
            {
                string websitename = tbxWebSiteName.Text.Trim();
                string seotitle = tbxSEOTitle.Text.Trim();
                if (websitename != string.Empty && seotitle != string.Empty)
                {
                    MainConfigInfo info = new MainConfigInfo();
                    info.Closed = 0;
                    info.Closedreason = "";
                    info.ApplictionSecKey = "";
                    info.Cookiedomain = "";
                    info.Urlrewrite = Convert.ToInt32(ckbxUrlRewrite.Checked);
                    info.Urlrewriteextname = tbxUrlRewriteExtName.Text.Trim();
                    info.Globalcachetimeout = Convert.ToInt32(tbxGlobalCacheTimeOut.Text.Trim());
                    info.Websitename = websitename;
                    info.Seotitle = seotitle;
                    info.Analyticscode = tbxAnalyticsCode.Text.Trim();
                    MainConfigs.Save(info);
                    MainConfigs.ResetConfig();
                }
            }
        }        
    }
}
