using System;
using System.Data;
using System.Web;
using Natsuhime.TinyData;

namespace iTCA.Yuwen.GuestBook
{
    public class BasePage : System.Web.UI.Page
    {
        #region 页面变量
        /// <summary>
        /// 当前页面标题
        /// </summary>
        protected string pagetitle = "";
        /// <summary>
        /// 页面内容
        /// </summary>
        protected System.Text.StringBuilder templateBuilder = new System.Text.StringBuilder();
        /// <summary>
        /// 数据查询对象
        /// </summary>
        protected DBHelper dbhelper;
        /// <summary>
        /// 页面执行计时用
        /// </summary>
        protected System.Diagnostics.Stopwatch sw;
        /// <summary>
        /// 当前页面执行时间(毫秒)
        /// </summary>
        protected string processtime;
        /// <summary>
        /// 页面查询数
        /// </summary>
        protected int querycount;
        /// <summary>
        /// 页面查询Sql内容
        /// </summary>
        protected string querydetail;
        #endregion

        protected BasePage()
        {
            //dbhelper = new DBHelper();
            ////页面统计开始
            //sw = System.Diagnostics.Stopwatch.StartNew();
            //dbhelper.Querycount = 0;

            ////页面执行
            //Page_Show();

            ////页面统计结束
            //querycount = dbhelper.QueryCount;
            //processtime = sw.Elapsed.TotalSeconds.ToString("F6");
        }
        /// <summary>
        /// 页面处理虚方法
        /// </summary>
        protected virtual void Page_Show()
        {
            return;
        }
    }
}
