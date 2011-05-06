using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmiForum.Models.Entity;
using EmiForum.Models;
using System.Web.Security;

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

                        Users.SetLoginStatus(shortUserInfo.Email);
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
                    //FormsAuthentication.SetAuthCookie(loginedUserInfo.Username, true);
                    FormsAuthentication.RedirectFromLoginPage(loginedUserInfo.Username, true);
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
    }
}
