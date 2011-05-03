using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmiForum.Models.Entity;
using EmiForum.Models;

namespace EmiForum.Controllers
{
    public class MemberController : Controller
    {
        //
        // GET: /Me/

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ShortUserInfo shortUserInfo)
        {
            if (ModelState.IsValid)
            {
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


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ShortUserInfo shortUserInfo)
        {
            return View();
        }


    }
}
