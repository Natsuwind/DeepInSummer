using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Discuz.Web.Admin;
using Discuz.Common;
using Discuz.Forum;
using System.Data.SqlClient;
using Wysky.Discuz.Plugin.QZoneLogin.BLL.Config;

namespace Wysky.Discuz.Plugin.QZoneLogin.Views.Admin.Plugin
{
    public partial class WyskyQZLoginSetting : AdminPage
    {
        protected internal int isInstall = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isInstall = DNTRequest.GetInt("Install", 0);
                if (isInstall == 1)
                {
                    AddAdminNav();
                    try
                    {
                        BLL.Main.Install();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number != 2714)
                        {
                            throw;
                        }
                    }
                    Response.Write("<script>alert('安装成功，请登录“后台》扩展》QQ 登录”进行设置，并重新生成模板。');location.href='wyskyqzloginsetting.aspx';</script>");
                }
                else
                {
                    QZoneLoginConfigInfo qzlci = QZoneLoginConfigs.GetConfig();
                    this.rbtnEnabled.Checked = qzlci.EnableQQLogin == 1 ? true : false;
                    this.rbtnDisabled.Checked = !this.rbtnEnabled.Checked;

                    this.tbxAppId.Text = qzlci.AppId;
                    this.tbxAppKey.Text = qzlci.AppKey;
                }
            }
        }

        void AddAdminNav()
        {
            if (!MenuManage.FindMenu("扩 展"))
            {
                MenuManage.NewMainMenu("扩 展", "global/global_passportmanage.aspx");
            }

            if (!MenuManage.FindMenu("扩 展", "QQ 登录"))
            {
                MenuManage.NewSubMenu("扩 展", "QQ 登录");
            }

            if (!MenuManage.FindMenu("扩 展", "QQ 登录", "基本设置"))
            {
                MenuManage.NewMenuItem("扩 展", "QQ 登录", "基本设置", "Plugin/WyskyQZLoginSetting.aspx");
            }
            MenuManage.CreateMenuJson();
        }

        protected void btnSaveSetting_Click(object sender, EventArgs e)
        {
            if (this.tbxAppId.Text.Trim() == string.Empty || this.tbxAppKey.Text.Trim() == string.Empty)
            {
                this.lblMessage.Text = "保存设置失败，请填写完整！";
                return;
            }
            QZoneLoginConfigInfo qzlci = QZoneLoginConfigs.GetConfig();
            qzlci.EnableQQLogin = Convert.ToInt32(this.rbtnEnabled.Checked);
            qzlci.AppId = this.tbxAppId.Text.Trim();
            qzlci.AppKey = this.tbxAppKey.Text.Trim();            

            QZoneLoginConfigs.SaveConfig(qzlci);
            this.lblMessage.Text = "保存设置成功！";
        }
    }
}