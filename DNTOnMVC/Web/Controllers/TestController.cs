using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wysky.Donmvc.Web.Controllers
{
    public class TestController : TestBaseController
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }

    }
}
