﻿using System;
using System.Collections.Generic;
using System.Web;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Wysky.Discuz.Plugin.QZoneLogin.Entity;
using Wysky.Discuz.Plugin.QZoneLogin.BLL.Config;

namespace Wysky.Discuz.Plugin.QZoneLogin.Views.Main
{
    public class QZoneLogin : PageBase
    {
        //string key = "206141";
        //string secret = "b10ce1bfae6641996fc40f6cdb70a188";
        protected internal string email = "";
        protected internal string wysky_page_msg = "";
        protected internal QZoneLoginConfigInfo qzlConfig = QZoneLoginConfigs.GetConfig();
        protected override void ShowPage()
        {
            var callbackUrlParam = "";

            #region 检查是否开启插件
            if (qzlConfig.EnableQQLogin != 1)
            {
                pagetitle = "QQ 登录插件没有开启";
                wysky_page_msg = "<script type=\"text/javascript\">alert('QQ 登录插件没有开启，请联系管理员！');window.close();</script>";
                return;
            }
            #endregion


            #region 检查登录状态
            var isBind = DNTRequest.GetInt("bind", 0);
            if (isBind > 0)
            {
                //如果是绑定/解绑，则要求登录状态才能使用
                if (userid == -1)
                {
                    pagetitle = "请登录后再进行 QQ 绑定";
                    wysky_page_msg = "<script type=\"text/javascript\">alert('请登录后再进行 QQ 绑定！');window.opener.location.href='login.aspx';window.close();</script>";
                    return;
                }
                if (isBind == 2)
                {
                    //解绑
                    BLL.Main.DeleteQqLoginInfo("", userid);
                    pagetitle = "已经解除绑定";
                    wysky_page_msg = "<script type=\"text/javascript\">alert('已经解除 QQ 帐号绑定！');window.opener.location.reload();window.close();</script>";
                    return;
                }
                callbackUrlParam += "&bind=" + isBind;
            }
            else
            {
                //如果不是绑定，即为QQ登录，则要求在非登录状态
                if (userid != -1)
                {
                    pagetitle = "您已经登录";
                    wysky_page_msg = "<script type=\"text/javascript\">alert('您已经登录了，请不要重复登录！');window.opener.location.reload();window.close();</script>";
                    return;
                }
            }
            #endregion

            #region 开始接入
            if (DNTRequest.GetInt("callback", 0) == 0)
            {
                //第一次发送
                pagetitle = "使用QQ帐号登录";
                var context = new QzoneSDK.Context.QzoneContext(qzlConfig.AppId, qzlConfig.AppKey);
                var callbackUrl = string.Format(
                    "http://{0}/QZoneLogin.aspx?callback=1{1}",
                    DNTRequest.GetCurrentFullHost(),
                    callbackUrlParam
                    );
                var requestToken = context.GetRequestToken(callbackUrl);
                var authenticationUrl = context.GetAuthorizationUrl(requestToken, callbackUrl);
                Utils.WriteCookie("wysky_qzlogin", "requesttokensecret", requestToken.TokenSecret);
                System.Web.HttpContext.Current.Response.Redirect(authenticationUrl);
            }
            else
            {
                //验证后回传
                Callback();
            }
            #endregion
        }

        void Callback()
        {
            //创建用户
            if (DNTRequest.GetInt("createuser", 0) == 1)
            {
                username = DNTRequest.GetString("username");
                email = DNTRequest.GetString("email");
                string qqopenid = Utils.GetCookie("wysky_qzlogin", "qqopenid");
                CreateUser(username, email, qqopenid);
                return;
            }
            else
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

                QzoneSDK.Qzone qzone = new QzoneSDK.Qzone(qzlConfig.AppId, qzlConfig.AppKey, oauth_token, requestTokenSecret, oauth_vericode);

                if (DNTRequest.GetInt("bind", 0) > 0)
                {
                    //要求登录
                    if (userid < 0)
                    {
                        pagetitle = "请先登录论坛才能绑定/解绑 QQ 帐号！";
                        wysky_page_msg = "<script type=\"text/javascript\">alert('请先登录论坛才能绑定/解绑 QQ 帐号！');window.opener.location.href='login.aspx';window.close();</script>";
                        return;
                    }

                    //开始绑定
                    if (DNTRequest.GetInt("bind", 0) == 1)
                    {
                        //清理历史记录
                        BLL.Main.DeleteQqLoginInfo(qzone.OpenID, userid);
                        BLL.Main.CreateQqUserInfo(qzone.OpenID, userid);
                        pagetitle = "绑定成功";
                        wysky_page_msg = "<script type=\"text/javascript\">alert('绑定 QQ 帐号成功！');window.opener.location.reload();window.close();</script>";
                        return;
                    }
                    return;
                }

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
        }

        void CreateUser(string newUsername, string newUserEmail, string qqopenid)
        {
            if (newUsername == string.Empty || newUserEmail == string.Empty)
            {
                AddErrLine("用户名和Email邮箱地址必须填写！");
                return;
            }
            if (Users.GetUserId(newUsername) > 0)
            {
                AddErrLine(string.Format("此用户名（{0}）已被使用，请重新换一个！", newUsername));
                return;
            }

            if (!Utils.IsValidEmail(newUserEmail))
            {
                AddErrLine("Email格式不正确");
                return;
            }
            if (!Users.ValidateEmail(newUserEmail))
            {
                AddErrLine("Email: \"" + newUserEmail + "\" 已经被其它用户注册使用");
                return;
            }

            if (qqopenid == string.Empty)
            {
                AddErrLine("激活超时，请重新登录完成激活！");
                return;
            }
            UserInfo userInfo = BLL.Main.CreateUser(newUsername, newUserEmail);

            if (userInfo == null || userInfo.Uid < 1)
            {
                AddErrLine("添加激活数据失败，请联系管理员检查数据表，谢谢～");
                return;
            }
            BLL.Main.DeleteQqLoginInfo(qqopenid, userInfo.Uid);
            BLL.Main.CreateQqUserInfo(qqopenid, userInfo.Uid);

            Statistics.ReSetStatisticsCache();
            UserCredits.UpdateUserCredits(userInfo.Uid);
            ForumUtils.WriteUserCookie(userInfo, -1, config.Passwordkey);
            OnlineUsers.UpdateAction(olid, UserAction.Register.ActionID, 0, config.Onlinetimeout);
            //MsgForward("register_succeed");
            //AddMsgLine("注册成功, 返回登录页");

            pagetitle = "激活成功";
            wysky_page_msg = "<script type=\"text/javascript\">window.opener.location.reload();window.close();</script>";
        }
    }
}