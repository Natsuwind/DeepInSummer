using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmiForum.Models;

namespace EmiForum.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public bool IsLogined()
        {
            return (Session["admin"] != null && Session["admin"].ToString() == "admin");
        }
        public ActionResult Index()
        {
            if (IsLogined())
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(string username, string password)
        {
            if (username.Trim() == "adminywen" && password == "123321aa")
            {
                Session["admin"] = "admin";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "用户名或密码错误");
                return View();
            }
        }
        public ActionResult Logout()
        {
            if (IsLogined())
            {
                Session.Abandon();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Index(string SqlScript)
        {
            if (IsLogined())
            {
                if (SqlScript.Trim() != string.Empty)
                {
                    try
                    {
                        Admins.ExecSql(SqlScript);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", string.Format("执行失败，{0}", ex.Message));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "请输入内容！");
                }
            }
            else
            {
                ModelState.AddModelError("", "请登录！");
            }
            return View();
        }
    }
}
