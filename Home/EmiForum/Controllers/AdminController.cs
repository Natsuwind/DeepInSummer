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

        public ActionResult Index()
        {
            if (Session["admin"] == null || Session["admin"].ToString() != "admin")
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
            else
            {
                return View();
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
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return View("Index", "Home");
        }

        [HttpPost]
        public ActionResult ExecSql(string SqlScript)
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
            return View("Index");
        }
    }
}
