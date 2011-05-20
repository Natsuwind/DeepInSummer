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
        protected override void ShowPage()
        {
            if (DNTRequest.GetInt("install", 0) == 1 && useradminid == 1)
            {
                if (BLL.Main.Install() > 0)
                {
                    System.Web.HttpContext.Current.Response.Write("<script>alert('安装成功');</script>");
                    System.Web.HttpContext.Current.Response.Write("<a href=\"qzonelogin.aspx\">点击刷新本页！</a>");
                    return;
                }
                System.Web.HttpContext.Current.Response.Write("<script>alert('安装失败');</script>");
                return;
            }
            if (userid != -1)
            {
                SetUrl(BaseConfigs.GetForumPath);
                AddMsgLine("您已经登录，无须重复登录");
            }

            if (DNTRequest.GetInt("callback", 0) == 0)
            {
                pagetitle = "使用QQ帐号登录";
                var context = new QzoneSDK.Context.QzoneContext(key, secret);
#if DEBUG
                var callbackUrl = "http://localhost:4487/QZoneLogin.aspx?callback=1"; //"/qzone/account/QQCallback.aspx";
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
                if (oauth_token != null && oauth_vericode != null && requestTokenSecret != null)
                {
                    QzoneSDK.Qzone qzone = new QzoneSDK.Qzone(key, secret, oauth_token, requestTokenSecret, oauth_vericode);
                    userid = BLL.Main.GetUIDByQqOpenid(qzone.OpenID);
                    if (userid != -1)
                    {
                        UserCredits.UpdateUserCredits(userid);
                        ForumUtils.WriteUserCookie(userid, -1, config.Passwordkey);
                        OnlineUsers.UpdateAction(olid, UserAction.Register.ActionID, 0, config.Onlinetimeout);
                        //MsgForward("login_succeed");
                        //AddMsgLine("登录成功, 返回登录前页面");
                        //SetUrl("index.aspx");
                        //System.Web.HttpContext.Current.Response.Redirect("index.aspx");
                        System.Web.HttpContext.Current.Response.Write("<script type=\"text/javascript\">window.opener.location.href='index.aspx';window.close();</script>");
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
            }
            else
            {
                string qqopenid = Utils.GetCookie("wysky_qzlogin", "qqopenid");
                if (qqopenid == string.Empty)
                {
                    AddErrLine("激活超时，请重新登录完成激活！");
                    return;
                }
                UserInfo userInfo = BLL.Main.CreateUser(DNTRequest.GetString("username"), DNTRequest.GetString("email"));

                if (userInfo == null)
                {
                    AddErrLine("添加激活数据失败，请联系管理员检查，谢谢～");
                    return;
                }
                BLL.Main.CreateQqUserInfo(qqopenid, userInfo.Uid);

                SetUrl("index.aspx");
                SetShowBackLink(false);
                SetMetaRefresh(config.Regverify == 0 ? 2 : 5);
                Statistics.ReSetStatisticsCache();

                UserCredits.UpdateUserCredits(userInfo.Uid);
                ForumUtils.WriteUserCookie(userInfo, -1, config.Passwordkey);
                OnlineUsers.UpdateAction(olid, UserAction.Register.ActionID, 0, config.Onlinetimeout);
                //MsgForward("register_succeed");
                //AddMsgLine("注册成功, 返回登录页");
                System.Web.HttpContext.Current.Response.Write("<script type=\"text/javascript\">window.opener.location.href='index.aspx';window.close();</script>");
                return;
            }
        }

    }
}