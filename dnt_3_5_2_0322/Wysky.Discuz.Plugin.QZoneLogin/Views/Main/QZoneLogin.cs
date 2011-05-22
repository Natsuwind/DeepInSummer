using System;
using System.Collections.Generic;
using System.Web;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Wysky.Discuz.Plugin.QZoneLogin.Entity;

namespace Wysky.Discuz.Plugin.QZoneLogin.Views.Main
{
    public class QZoneLogin : PageBase
    {
        string key = "206141";
        string secret = "b10ce1bfae6641996fc40f6cdb70a188";
        protected internal string email = "";
        protected internal string wysky_page_msg = "";
        protected override void ShowPage()
        {
            if (DNTRequest.GetInt("install", 0) == 1)
            {
                pagetitle = "初始化插件数据";
                if (useradminid != 1)
                {
                    wysky_page_msg = "此操作需要管理员权限，请登录管理员账号再访问此页面！";
                    return;
                }
                try
                {
                    BLL.Main.Install();
                    wysky_page_msg = "<script>alert('安装成功');location.href='index.aspx'</script>";
                    return;
                }
                catch
                {
                    throw;
                }
            }
            if (userid != -1)
            {
                pagetitle = "您已经登录";
                wysky_page_msg = "<script type=\"text/javascript\">alert('您已经登录了，请不要重复登录！');window.opener.location.reload();window.close();</script>";
                return;
            }

            if (DNTRequest.GetInt("callback", 0) == 0)
            {
                pagetitle = "使用QQ帐号登录";
                var context = new QzoneSDK.Context.QzoneContext(key, secret);
#if DEBUG
                var callbackUrl = "http://nt.wysky.org/QZoneLogin.aspx?callback=1"; //"/qzone/account/QQCallback.aspx";
#else
                var callbackUrl = "http://nt.wysky.org/QZoneLogin.aspx?callback=1";
#endif
                var requestToken = context.GetRequestToken(callbackUrl);
                var authenticationUrl = context.GetAuthorizationUrl(requestToken, callbackUrl);
                Utils.WriteCookie("wysky_qzlogin", "requesttokensecret", requestToken.TokenSecret);
                System.Web.HttpContext.Current.Response.Redirect(authenticationUrl);
            }
            else
            {
                Callback();
            }
        }

        void Callback()
        {
            if (DNTRequest.GetInt("createuser", 0) == 0)
            {
                string oauth_token = DNTRequest.GetString("oauth_token");
                string oauth_vericode = DNTRequest.GetString("oauth_vericode");
                string requestTokenSecret = Utils.GetCookie("wysky_qzlogin", "requesttokensecret");
                if (oauth_token == string.Empty || oauth_vericode == string.Empty || requestTokenSecret == string.Empty)
                {
                    pagetitle = "发生错误";
                    wysky_page_msg = "<script type=\"text/javascript\">alert('模块参数有空值，请回到登录页面重新登录！');window.opener.location.href='login.aspx';window.close();</script>";
                    return;
                }
                QzoneSDK.Qzone qzone = new QzoneSDK.Qzone(key, secret, oauth_token, requestTokenSecret, oauth_vericode);
                userid = BLL.Main.GetUIDByQqOpenid(qzone.OpenID);
                if (userid != -1)
                {
                    UserCredits.UpdateUserCredits(userid);
                    ForumUtils.WriteUserCookie(userid, -1, config.Passwordkey);
                    OnlineUsers.UpdateAction(olid, UserAction.Register.ActionID, 0, config.Onlinetimeout);

                    pagetitle = "登录成功";
                    wysky_page_msg = "<script type=\"text/javascript\">window.opener.location.reload();window.close();</script>";
                    return;
                }
                else
                {
                    var currentUser = qzone.GetCurrentUser();
                    var user = (QzoneLoginInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(currentUser, typeof(QzoneLoginInfo));
                    if (null != user)
                    {
                        username = user.nickname;
                        Utils.WriteCookie("wysky_qzlogin", "qqopenid", qzone.OpenID);
                    }
                }


            }
            else
            {
                username = DNTRequest.GetString("username");
                email = DNTRequest.GetString("email");
                if (username == string.Empty || email == string.Empty)
                {
                    AddErrLine("用户名和Email邮箱地址必须填写！");
                    return;
                }
                if (Users.GetUserId(username) > 0)
                {
                    AddErrLine(string.Format("此用户名（{0}）已被使用，请重新换一个！", username));
                    return;
                }
                string qqopenid = Utils.GetCookie("wysky_qzlogin", "qqopenid");
                if (qqopenid == string.Empty)
                {
                    AddErrLine("激活超时，请重新登录完成激活！");
                    return;
                }
                UserInfo userInfo = BLL.Main.CreateUser(username, email);

                if (userInfo == null || userInfo.Uid < 1)
                {
                    AddErrLine("添加激活数据失败，请联系管理员检查数据表，谢谢～");
                    return;
                }
                BLL.Main.CreateQqUserInfo(qqopenid, userInfo.Uid);
                
                Statistics.ReSetStatisticsCache();
                UserCredits.UpdateUserCredits(userInfo.Uid);
                ForumUtils.WriteUserCookie(userInfo, -1, config.Passwordkey);
                OnlineUsers.UpdateAction(olid, UserAction.Register.ActionID, 0, config.Onlinetimeout);
                //MsgForward("register_succeed");
                //AddMsgLine("注册成功, 返回登录页");

                pagetitle = "激活成功";
                wysky_page_msg = "<script type=\"text/javascript\">window.opener.location.reload();window.close();</script>";
                return;
            }
        }

    }
}