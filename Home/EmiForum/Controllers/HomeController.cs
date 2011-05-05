using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Natsuhime.Data;
using EmiForum.Models;
using EmiForum.Models.Entity;

namespace EmiForum.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ShortUserInfo userInfo = Users.GetLoginStatus();
            if (userInfo != null)
            {
                ViewBag.IsLogined = true;
                ViewBag.LoginUserInfo = userInfo;
            }
            else
            {
                ViewBag.IsLogined = false;
            }
            return View(Posts.GetPostList());
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(PostInfo newPost)
        {
            ShortUserInfo shortUserInfo = Users.GetLoginStatus();
            if ((shortUserInfo == null || shortUserInfo.Username == null) && (newPost.Poster == null || newPost.Poster.Trim() == string.Empty))
            {
                ModelState.AddModelError("", "请填写昵称或登录帐号，然后再发表留言！");
                return View(newPost);
            }

            if (newPost.Content == null || newPost.Content.Trim() == string.Empty)
            {
                ModelState.AddModelError("", "请填写留言内容！");
                return View(newPost);
            }
            if (ModelState.IsValid)
            {
                newPost.Content = HttpUtility.HtmlEncode(newPost.Content).Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
                newPost.PosterId = (shortUserInfo != null && shortUserInfo.Uid > 0) ? shortUserInfo.Uid : 0;
                newPost.Poster = (shortUserInfo != null && shortUserInfo.Username != null && shortUserInfo.Username.Trim() != string.Empty) ? shortUserInfo.Username : newPost.Poster;
                newPost.Ip = Request.UserHostAddress;
                newPost.PostDate = DateTime.Now;
                newPost.Email = "";
                newPost.Website = "";

                Posts.CreatePost(newPost);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "验证失败，请填写必填项！");
                return View(newPost);
            }
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
