using System;
using System.Web;
using LiteCMS.Entity;
using LiteCMS.Core;

namespace LiteCMS.Web.Admin
{
    public class AdminPage : System.Web.UI.Page
    {
        protected string adminpath;
        protected AdminInfo admininfo;
        protected UserInfo userinfo;
        protected override void OnInit(EventArgs e)
        {
            /*
            if (Session["Admin"] == null)
            {
                Response.End();
            }
             */
            if (!CheckAdminLogin())
            {
                Response.Redirect("admincp.aspx");
                Response.End();
            }
            base.OnInit(e);
        }

        protected bool CheckAdminLogin()
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["cmsnt"];
            userinfo = null;
            if (cookie != null && cookie.Values["userid"] != null && cookie.Values["password"] != null)
            {
                int uid = Convert.ToInt32(cookie.Values["userid"]);
                string password = cookie.Values["password"].ToString().Trim();

                if (uid > 0 && password != string.Empty)
                {
                    userinfo = LiteCMS.Core.Users.GetUserInfo(uid, password);
                }
            }

            if (userinfo != null)
            {
                HttpCookie admincookie = Request.Cookies["cmsntadmin"];
                admininfo = null;
                if (admincookie != null && admincookie.Values["adminid"] != null && admincookie.Values["password"] != null)
                {
                    int adminid = Convert.ToInt32(admincookie.Values["adminid"]);
                    string password = admincookie.Values["password"].ToString().Trim();

                    if (adminid > 0 && password != string.Empty)
                    {
                        //admininfo todo
                        admininfo = Admins.GetAdminInfo(adminid, password);
                        if (admininfo != null && admininfo.Uid == userinfo.Uid)
                        {
                            admincookie.Expires = DateTime.Now.AddMinutes(20d);
                            Response.AppendCookie(admincookie);
                            adminpath = admincookie.Values["path"].ToString().Trim();
                            return true;
                        }
                    }
                }
            }
            adminpath = "";
            return false;
        }

        protected void ShowMsg(string header, string body, string footer, string redirecturl, bool isautoredirect)
        {
            if (redirecturl.Trim() == string.Empty)
            {
                redirecturl = "javascript:history.back(-1);";
            }
            Response.Clear();
            Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
            Response.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n");
            Response.Write("<head>\r\n");
            Response.Write("<link href=\"Main.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
            Response.Write("<meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\r\n");
            Response.Write("</head>\r\n");
            Response.Write("<body style=\"background-color:#FFF;\">\r\n");
            //Response.Write("<div class=\"div-header\" style=\"background-color:#B6C9E7;padding:5px 0 0 5px;\">系统提示</div>\r\n");
            Response.Write("<div style=\"border-style: dotted;border-width: 1px; margin: 100px auto 200px; width: 60%;vertical-align:middle;\">\r\n");
            Response.Write("<div class=\"div-header\" style=\"height:20px;background-color:#B6C9E7;padding:5px 0 0 5px;\">" + header + "</div>\r\n");
            Response.Write("<div style=\"padding: 10px;\">" + body + "</div>\r\n");
            Response.Write("<div style=\"padding-left: 10px;\">" + footer + "</div>\r\n");
            Response.Write("<div style=\"padding: 10px;\">");
            if (isautoredirect)
            {

                Response.Write("<span id=\"autoredirect\"></span>\r\n");

            }
            Response.Write("<a href=\"" + redirecturl + "\">如果浏览器没有自动转向, 请点击这里.</a></div>\r\n");
            Response.Write("</div>");
            if (isautoredirect)
            {

                Response.Write("    <script language='javascript' type='text/javascript'>\r\n");
                Response.Write("    var secs = 3; //倒计时的秒数\r\n");
                Response.Write("    var URL = '");
                Response.Write(redirecturl);
                Response.Write("';\r\n");
                Response.Write("    for(var i=secs;i>=0;i--)\r\n");
                Response.Write("    {\r\n");
                Response.Write("     window.setTimeout('doUpdate(' + i + ')', (secs-i) * 1000);\r\n");
                Response.Write("    }\r\n");
                Response.Write("    function doUpdate(num)\r\n");
                Response.Write("    {\r\n");
                Response.Write("     document.getElementById('autoredirect').innerHTML = '浏览器将在'+num+'秒后自动转向.' ;\r\n");
                Response.Write("     if(num == 0) { window.location=URL;  }\r\n");
                Response.Write("    }\r\n");
                Response.Write("    </" + "script> \r\n");
                Response.Write("</body>\r\n");
                Response.Write("</html>\r\n");        
            }
            Response.End();
            if (this.Context.ApplicationInstance != null)
            {
                this.Context.ApplicationInstance.CompleteRequest();
            }
            System.Threading.Thread.CurrentThread.Abort();
        }
    }
}
