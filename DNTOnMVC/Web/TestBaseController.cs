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
            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// 控件初始化时计算执行时间
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}