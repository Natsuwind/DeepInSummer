using System;
using System.Web;
using System.Data;
using System.Data.Common;

using Natsuhime.Data;
using Natsuhime.Web;

namespace LiteCMS.Web.Utility
{
    public partial class dbtool : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT articleid,content FROM wy_articles").Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                string content = Utils.RemoveHtml(dr["content"].ToString());
                string summary = content.Length > 160 ? content.Substring(0, 159) : content;
                DbParameter[] param =
                {
                    DbHelper.MakeInParam("@summary", DbType.String, 100, summary),
                    DbHelper.MakeInParam("@articleid", DbType.Int32, 4, dr["articleid"])
                };
                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_articles SET summary=@summary WHERE articleid=@articleid", param);

                Response.Write("id:" + dr["articleid"].ToString()+"<br />");
            }

            Response.Write("OK");
        }
    }
}
