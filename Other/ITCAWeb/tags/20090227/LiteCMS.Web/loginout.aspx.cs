using System;


namespace LiteCMS.Web
{
    public partial class loginout : BasePage
    {
        protected override void Page_Show()
        {
            pagetitle = "注销登陆";
            currentcontext.Response.Cookies["cmsnt"].Expires = DateTime.Now.AddDays(-1d);
            ShowMsg("注销操作", "注销成功,跳转到首页.", "", "index.aspx");
        }
    }
}
