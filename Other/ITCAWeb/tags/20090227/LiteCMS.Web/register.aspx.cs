using System;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class register : BasePage
    {
        protected override void Page_Show()
        {
            pagetitle = "注册用户";
            UserInfo userinfo = GetUserInfo();
            if (userinfo != null)
            {
                ShowError("注册用户", "您已经登录了,请不要重复注册帐号!", "", "usercontrolpanel.aspx");
            }
            if (ispost)
            {
                string email = YRequest.GetString("email");
                string password = YRequest.GetString("password");
                string username = YRequest.GetString("username");
                string secquestion = YRequest.GetString("secretquestion");
                string secanswer = YRequest.GetString("secretanswer");

                if (email != string.Empty && password != string.Empty && username != string.Empty)
                {
                    if (Users.GetUserInfo(username, 1) != null)
                    {
                        ShowError("注册用户", "注册失败,用户名已存在!", "", "");
                    }
                    else if (Users.GetUserInfo(email, 0) != null)
                    {
                        ShowError("注册用户", "注册失败,Email已存在!", "", "");
                    }
                    if (secquestion == string.Empty || secanswer == string.Empty)
                    {
                        ShowError("注册用户", "注册失败,找回密码提示或答案为空.请填写完整以保障帐号安全!", "", "");
                    }
                    UserInfo info = new UserInfo();
                    info.Adminid = 0;
                    info.Articlecount = 0;
                    info.Bdday = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    info.Del = 0;
                    info.Email = email;
                    info.Secquestion = secquestion;
                    info.Secanswer = Natsuhime.Common.Utils.MD5(secanswer);
                    info.Groupid = 1;
                    info.Hi = "";
                    info.Lastlogdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    info.Lastlogip = "";
                    info.Msn = "";
                    info.Nickname = username;
                    info.Password = Natsuhime.Common.Utils.MD5(password);
                    info.Qq = "";
                    info.Realname = "";
                    info.Regdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    info.Regip = YRequest.GetIP();
                    info.Replycount = 0;
                    info.Topiccount = 0;
                    info.Username = username;

                    Users.AddUser(info);
                    ShowMsg("注册用户", "注册帐号成功,跳转到用户中心.", "", "usercontrolpanel.aspx");
                }
            }

        }
    }
}
