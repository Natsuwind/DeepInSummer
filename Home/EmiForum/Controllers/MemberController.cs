using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmiForum.Models.Entity;

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
            return View("Index");
        }

        [HttpPost]
        public ActionResult Login()
        {
            return View();
        }


    }
}
