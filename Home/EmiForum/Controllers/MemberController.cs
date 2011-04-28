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
                shortUserInfo.LastLoginDate = DateTime.MinValue;
                shortUserInfo.LastLoginIp = "";
                shortUserInfo.RegDate = DateTime.Now;
                shortUserInfo.RegIp = Request.UserHostAddress;
                shortUserInfo.Salt = "";
                shortUserInfo.SecQues = "";
                Users.CreateUser(shortUserInfo);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(shortUserInfo);
            }
        }

        [HttpPost]
        public ActionResult Login()
        {
            return View();
        }


    }
}
