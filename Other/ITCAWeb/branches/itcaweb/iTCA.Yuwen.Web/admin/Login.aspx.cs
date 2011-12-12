using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace iTCA.Yuwen.Web.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["sid"] == null || Request.QueryString["sid"].ToString() != "hime")
            {
                Response.End();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text == "my" && TextBox2.Text == "hime" && TextBox3.Text == "liya")
            {
                Session["admin"] = "adminywen";
            }
        }
    }
}
