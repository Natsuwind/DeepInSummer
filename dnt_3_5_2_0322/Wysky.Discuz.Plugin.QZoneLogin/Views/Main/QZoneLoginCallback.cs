using System;
using System.Collections.Generic;
using System.Web;
using Discuz.Forum;
using Discuz.Common;

namespace Wysky.Discuz.Plugin.QZoneLogin.Views.Main
{
    public class QZoneLoginCallback : PageBase
    {
        string key = "203249";
        string secret = "37d107b7b10be60fbb3c7ef8574b9c87";
        protected override void ShowPage()
        {
            pagetitle = "激活 QQ 登录功能";

            var context = new QzoneSDK.Context.QzoneContext(key, secret);
#if DEBUG
            var callbackUrl = "http://localhost:4487/QZoneLoginCallback.aspx"; //"/qzone/account/QQCallback.aspx";
#else            
            var callbackUrl = "http://nt.wysky.org/QZoneLoginCallback.aspx"; 
#endif
            var requestToken = context.GetRequestToken(callbackUrl);
            Session["requesttokensecret"] = requestToken.TokenSecret;
            var authenticationUrl = context.GetAuthorizationUrl(requestToken, callbackUrl);

            Response.Redirect(authenticationUrl);
        }

        void Callback()
        {
            string oauth_token = DNTRequest.GetString("oauth_token");
            string oauth_vericode = DNTRequest.GetString("oauth_vericode");
            if (oauth_token != null && oauth_vericode != null && Session["requesttokensecret"] != null)
            {
                var requestTokenSecret = Session["requesttokensecret"].ToString();
                QzoneSDK.Qzone qzone = new QzoneSDK.Qzone(key, secret, oauth_token, requestTokenSecret, oauth_vericode);
                //ShortUserInfo userinfo = Users.GetUserInfoByQqOpenid(qzone.OpenID);
                //if (userinfo != null)
                //{
                //    FormsAuthentication.SetAuthCookie(userinfo.Username, true);
                //    Response.Write("<script type=\"text/javascript\">window.opener.location.reload();window.close();</script>");
                //    return View();
                //}

                var currentUser = qzone.GetCurrentUser();
                //Response.Write(currentUser);
                //var user = (BasicProfile)JsonConvert.DeserializeObject(currentUser, typeof(BasicProfile));
                //if (null != user)
                //{
                //    userinfo = new ShortUserInfo();
                //    userinfo.Username = user.Nickname;
                //    Session["qqopenid"] = qzone.OpenID;

                //    //Response.Write("<br />" + user.Nickname + "<br />" + qzone.OpenID);
                //    return View(userinfo);
                //}
            }
            Response.Redirect("Login.aspx");
        }
    }
}