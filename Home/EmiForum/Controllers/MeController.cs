using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmiForum.Controllers
{
    public class MeController : Controller
    {
        //
        // GET: /Me/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            return View();
        }
    }
}
