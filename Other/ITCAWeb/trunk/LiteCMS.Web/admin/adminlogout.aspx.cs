using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace LiteCMS.Web.Admin
{
    public partial class adminlogout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie admincookie = Request.Cookies["cmsntadmin"];
            if (admincookie != null)
            {
                admincookie.Expires = DateTime.Now.AddYears(-1);
                Response.Write("成功退出!");
                Response.End();
            }
        }
    }
}
