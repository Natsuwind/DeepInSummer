using System;

namespace iTCA.Yuwen.Web.Admin
{
    public partial class index : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["name"] != null)
            {
                lbLoginName.Text = Session["name"].ToString();
            }
            else
            {
                lbLoginName.Text = "µÇÂ¼Ê§Ð§";
            }
        }
    }
}
