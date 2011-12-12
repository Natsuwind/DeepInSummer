using System;
using System.Data;
using System.Web;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Data;

namespace LiteCMS.Web.Admin
{
    public partial class dbcontrol : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, tbxSql.Text.Trim());
                lbMessage.Text = "²Ù×÷³É¹¦!";
            }
            catch (Exception ex)
            {
                lbMessage.Text = ex.Message;
            }
        }
    }
}
