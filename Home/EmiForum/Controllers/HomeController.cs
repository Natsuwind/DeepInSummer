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
            return View(Posts.GetPostList());
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(PostInfo newPost)
        {
            if (!Request.IsAuthenticated && (newPost.Poster == null || newPost.Poster.Trim() == string.Empty))
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
                newPost.Poster = Request.IsAuthenticated ? User.Identity.Name : newPost.Poster;
                ShortUserInfo posterInfo = Users.GetUserInfoByUsername(newPost.Poster);
                newPost.PosterId = posterInfo != null ? posterInfo.Uid : 0;
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
