using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Natsuhime.Data;
using EmiForum.Models;

namespace EmiForum.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(Posts.GetPostList());
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(PostInfo newPost)
        {
            if (ModelState.IsValid)
            {
                newPost.Ip = Request.UserHostAddress;
                newPost.PostDate = DateTime.Now;
                newPost.Email = "test@qq.com";
                newPost.PosterId = 0;
                newPost.Website = "http://www.wysky.org/";
                Posts.CreatePost(newPost);
                return RedirectToAction("Index");
            }
            else
            {
                return View(newPost);
            }
        }
    }
}
