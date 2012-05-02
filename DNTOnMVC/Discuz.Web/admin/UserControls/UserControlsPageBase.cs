using System;
using System.Collections.Generic;
using System.Web;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public class UserControlsPageBase : System.Web.UI.UserControl
    {
        protected internal GeneralConfigInfo config;

        public UserControlsPageBase()
        {
            config = GeneralConfigs.GetConfig();

            // 如果IP访问列表有设置则进行判断
            if (config.Adminipaccess.Trim() != "")
            {
                string[] regctrl = Utils.SplitString(config.Adminipaccess, "\n");
                if (!Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                {
                    Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return;
                }
            }

            // 获取用户信息
            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
            UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(oluserinfo.Groupid);
            if (oluserinfo.Userid <= 0 || usergroupinfo.Radminid != 1)
            {
                Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return;
            }

            string secques = Users.GetUserInfo(oluserinfo.Userid).Secques;

            // 管理员身份验证
            if (Context.Request.Cookies["dntadmin"] == null || Context.Request.Cookies["dntadmin"]["key"] == null ||
                ForumUtils.GetCookiePassword(Context.Request.Cookies["dntadmin"]["key"].ToString(), config.Passwordkey) != (oluserinfo.Password + secques + oluserinfo.Userid.ToString()))
            {
                Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return;
            }
            else
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["dntadmin"];
                cookie.Values["key"] = ForumUtils.SetCookiePassword(oluserinfo.Password + secques + oluserinfo.Userid.ToString(), config.Passwordkey);
                cookie.Values["userid"] = oluserinfo.Userid.ToString();
                cookie.Expires = DateTime.Now.AddMinutes(30);
                HttpContext.Current.Response.AppendCookie(cookie);
            }            
        }
    }
}