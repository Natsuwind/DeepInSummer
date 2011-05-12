using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmiForum.Models.Entity;
using EmiForum.Models;
using System.Web.Security;
using QzoneSDK.Models;
using Newtonsoft.Json;

namespace EmiForum.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Create(ShortUserInfo shortUserInfo)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                if ((shortUserInfo.Email == null || shortUserInfo.Email.Trim() == string.Empty)
                    || (shortUserInfo.Username == null || shortUserInfo.Username.Trim() == string.Empty)
                    || shortUserInfo.Password == null
                    )
                {
                    ModelState.AddModelError("", "请填写完整的注册信息！");
                    return View(shortUserInfo);
                }
                switch (Users.IsExits(shortUserInfo.Username, shortUserInfo.Email))
                {
                    case 0:
                        shortUserInfo.Password =
                            System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(shortUserInfo.Password, "MD5");
                        shortUserInfo.LastLoginDate = DateTime.MinValue;
                        shortUserInfo.LastLoginIp = "";
                        shortUserInfo.RegDate = DateTime.Now;
                        shortUserInfo.RegIp = Request.UserHostAddress;
                        shortUserInfo.Salt = "";
                        shortUserInfo.SecQues = "";
                        Users.CreateUser(shortUserInfo);

                        FormsAuthentication.SetAuthCookie(shortUserInfo.Username, true);
                        return RedirectToAction("Index", "Home");
                    case 3:
                        ModelState.AddModelError("", "用户名和邮箱已被其他人注册，请重新填写。（<a href=\"#\">[忘记密码？]</a>）");
                        return View(shortUserInfo);
                    case 2:
                        ModelState.AddModelError("", "邮箱已被其他人注册,请重新填写。（<a href=\"#\">[忘记密码？]</a>）");
                        return View(shortUserInfo);
                    case 1:
                        ModelState.AddModelError("", "用户名已被其他人注册,请重新填写。（<a href=\"#\">[忘记密码？]</a>）");
                        return View(shortUserInfo);
                    default:
                        ModelState.AddModelError("", "注册发生错误！");
                        return View(shortUserInfo);
                }

            }
            else
            {
                return View(shortUserInfo);
            }
        }


        public ActionResult LogOn()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(ShortUserInfo shortUserInfo)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (shortUserInfo.Email != null && shortUserInfo.Password != null && shortUserInfo.Email.Trim() != string.Empty && shortUserInfo.Password != string.Empty)
            {
                ShortUserInfo loginedUserInfo = Users.CheckUserLogin(shortUserInfo.Email, shortUserInfo.Password);
                if (loginedUserInfo != null)
                {
                    FormsAuthentication.SetAuthCookie(loginedUserInfo.Username, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email或密码错误！");
                    return View(shortUserInfo);
                }
            }
            else
            {
                ModelState.AddModelError("", "Email和密码必须填写！");
                return View();
            }
        }

        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }



        string key = "203249";
        string secret = "37d107b7b10be60fbb3c7ef8574b9c87";
        public ActionResult LogOnByQQ()
        {
            var context = new QzoneSDK.Context.QzoneContext(key, secret);
#if DEBUG
            var callbackUrl = "http://localhost:4487/Home/Index"; //"/qzone/account/QQCallback.aspx";
#else            
            var callbackUrl = "http://wysky.org/Account/LogOnByQQCallback"; 
#endif
            var requestToken = context.GetRequestToken(callbackUrl);
            Session["requesttokensecret"] = requestToken.TokenSecret;
            var authenticationUrl = context.GetAuthorizationUrl(requestToken, callbackUrl);

            return Redirect(authenticationUrl);
        }
        public ActionResult LogOnByQQCallback(string oauth_token, string oauth_vericode)
        {
            if (oauth_token != null && oauth_vericode != null && Session["requesttokensecret"] != null)
            {
                var requestTokenSecret = Session["requesttokensecret"].ToString();
                QzoneSDK.Qzone qzone = new QzoneSDK.Qzone(key, secret, oauth_token, requestTokenSecret, oauth_vericode);
                ShortUserInfo userinfo = Users.GetUserInfoByQqOpenid(qzone.OpenID);
                if (userinfo != null)
                {
                    FormsAuthentication.SetAuthCookie(userinfo.Username, true);
                    Response.Write("<script type=\"text/javascript\">window.opener.location.reload();window.close();</script>");
                    return View();
                }

                var currentUser = qzone.GetCurrentUser();
                //Response.Write(currentUser);
                var user = (BasicProfile)JsonConvert.DeserializeObject(currentUser, typeof(BasicProfile));
                if (null != user)
                {
                    userinfo = new ShortUserInfo();
                    userinfo.Username = user.Nickname;
                    Session["qqopenid"] = qzone.OpenID;

                    //Response.Write("<br />" + user.Nickname + "<br />" + qzone.OpenID);
                    return View(userinfo);
                }
            }
            return RedirectToAction("LogOn", "Account");
        }

        [HttpPost]
        public ActionResult LogOnByQQCallback(ShortUserInfo shortUserInfo)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                if ((shortUserInfo.Email == null || shortUserInfo.Email.Trim() == string.Empty)
                    || (shortUserInfo.Username == null || shortUserInfo.Username.Trim() == string.Empty)
                    || Session["qqopenid"] == null
                    )
                {
                    ModelState.AddModelError("", "必填项缺失，或者激活超时。请重新激活QQ登录！");
                    return View(shortUserInfo);
                }
                switch (Users.IsExits(shortUserInfo.Username, shortUserInfo.Email))
                {
                    case 0:
                        shortUserInfo.Password = "";
                        shortUserInfo.LastLoginDate = DateTime.MinValue;
                        shortUserInfo.LastLoginIp = "";
                        shortUserInfo.RegDate = DateTime.Now;
                        shortUserInfo.RegIp = Request.UserHostAddress;
                        shortUserInfo.Salt = "";
                        shortUserInfo.SecQues = "";
                        shortUserInfo.QqOpenId = Session["qqopenid"].ToString();
                        Users.CreateUser(shortUserInfo);

                        FormsAuthentication.SetAuthCookie(shortUserInfo.Username, true);
                        Response.Write("<script type=\"text/javascript\">window.opener.location.reload();window.close();</script>");
                        return View();
                    case 3:
                        ModelState.AddModelError("", "用户名和邮箱已被其他人注册，请重新填写。（<a href=\"#\">[忘记密码？]</a>）");
                        return View(shortUserInfo);
                    case 2:
                        ModelState.AddModelError("", "邮箱已被其他人注册,请重新填写。（<a href=\"#\">[忘记密码？]</a>）");
                        return View(shortUserInfo);
                    case 1:
                        ModelState.AddModelError("", "用户名已被其他人注册,请重新填写。（<a href=\"#\">[忘记密码？]</a>）");
                        return View(shortUserInfo);
                    default:
                        ModelState.AddModelError("", "注册发生错误！");
                        return View(shortUserInfo);
                }

            }
            else
            {
                return View(shortUserInfo);
            }
        }
    }
}
