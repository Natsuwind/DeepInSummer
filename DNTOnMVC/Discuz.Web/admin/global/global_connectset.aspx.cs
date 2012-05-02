using System;
using System.Web;
using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// Connect设置
    /// </summary>
    public partial class connectset : AdminPage
    {
        public DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();

        public int upload = DNTRequest.GetInt("upload", 0);

        public string uploadLogoUrl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DiscuzCloud.GetCloudServiceEnableStatus("connect"))
            {
                ResponseScript("<script>alert('站点 " + (config.Cloudenabled == 1 ? "QQ 登录服务" : "云平台") +
                    "未开启');window.location.href='" + (config.Cloudenabled == 1 ? " global_cloudindex.aspx" : "global_cloudset.aspx") + "';</script>");
            }

            if (DNTRequest.GetInt("save", 0) > 0)
            {
                config.Allowconnectregister = DNTRequest.GetInt("enablereg", 0);
                config.Allowuseqzavater = DNTRequest.GetInt("enablecatchqzavatar", 0);
                int bindCount = DNTRequest.GetInt("maxbindcount", -1);

                if (bindCount < 0)
                {
                    ResponseScript("<script>alert('允许QQ号注册数量填写错误');window.location.href='global_connectset.aspx';</script>");
                    return;
                }
                config.Maxuserbindcount = bindCount;
                DiscuzCloudConfigs.SaveConfig(config);
                DiscuzCloudConfigs.ResetConfig();
                ResponseScript("<script>alert('操作成功');window.location.href='global_connectset.aspx';</script>");
            }

            uploadLogoUrl = DiscuzCloud.GetCloudUploadLogoIFrame(userid);
        }

        private void ResponseScript(string script)
        {
            HttpContext.Current.Response.Write(script);
            HttpContext.Current.Response.End();
        }
    }
}