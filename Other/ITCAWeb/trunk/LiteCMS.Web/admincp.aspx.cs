using System;
using Natsuhime.Web;
using System.Web;
using LiteCMS.Entity;
using LiteCMS.Core;

namespace LiteCMS.Web
{
    public partial class admincp : BasePage
    {
        protected string url;
        protected bool isadminlogined;
        protected override void Page_Show()
        {
            if (YRequest.GetQueryString("action") == "logout")
            {
                HttpCookie admincookie = currentcontext.Request.Cookies["cmsntadmin"];
                if (admincookie != null)
                {
                    admincookie.Expires = DateTime.Now.AddYears(-1);
                    currentcontext.Response.AppendCookie(admincookie);
                }
                ShowMsg("注销管理状态", "管理员登录注销成功,跳转到前台首页.", "", "../cms/index.aspx");
            }
            UserInfo userinfo = GetUserInfo();
            if (userinfo == null)
            {
                ShowError("后台登陆", "发生错误,请先登录前台,然后再访问此页.", "", "../member/login.aspx");
            }

            IsAdminLogined();
            if (admininfo != null)
            {
                string action = YRequest.GetString("action") == string.Empty ? "default" : YRequest.GetString("action");
                int id = YRequest.GetInt("id", 0);

                url = string.Format("frame.aspx?action={0}&id={1}", action, id);
            }
            else
            {
                url = "";
                if (ispost)
                {
                    //todo adminlogin
                    string name = YRequest.GetFormString("loginname");
                    string password = YRequest.GetFormString("password");
                    string path = YRequest.GetFormString("path");
                    admininfo = Admins.GetAdminInfo(name, Natsuhime.Common.Utils.MD5(password));

                    if (admininfo != null && admininfo.Uid == userinfo.Uid)
                    {
                        HttpCookie admincookie = new HttpCookie("cmsntadmin");
                        admincookie.Values["adminid"] = admininfo.Adminid.ToString();
                        admincookie.Values["password"] = admininfo.Password;
                        admincookie.Values["path"] = path;
                        admincookie.Expires = DateTime.Now.AddMinutes(20d);
                        currentcontext.Response.AppendCookie(admincookie);

                        ShowMsg("后台登陆", "登录成功!开始跳转到后台首页", "", "../admincp.aspx");
                    }
                    else
                    {
                        ShowError("后台登陆", "登录失败,用户名或密码错误!", "", "");
                    }
                }
            }
        }
    }
}
