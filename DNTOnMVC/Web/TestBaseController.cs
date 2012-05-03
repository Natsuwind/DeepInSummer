using System;
using System.Web;
using System.Web.Mvc;

namespace Wysky.Donmvc.Web
{
    public class TestBaseController : Controller
    {
        public TestBaseController()
        {
            string abcd = "abcd";
        }
        protected void test()
        {
            string abc = "abc";
        }
        /// <summary>
        /// OnUnload事件处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            ViewBag.StrVar2 = "abc22222222";
            ViewBag.IntVar2 = 222222222;
            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// 控件初始化时计算执行时间
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.StrVar = "abc";
            ViewBag.IntVar = 1111111111;
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.StrVar3 = "abc333333";
            ViewBag.IntVar3 = 33333333333;
            base.OnActionExecuted(filterContext);
        }
    }
}