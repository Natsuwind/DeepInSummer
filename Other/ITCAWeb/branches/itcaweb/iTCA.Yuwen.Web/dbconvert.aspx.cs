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
using Yuwen.Tools.Data;
using Yuwen.Tools.TinyCache;
using System.Data.Common;

namespace iTCA.Yuwen.Web
{
    public partial class dbconvert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbHelper.ResetDbProvider();
            DbHelper.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/db.mdb");
            DbHelper.Dbtype = "Access";
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM  i_news").Tables[0];
            gvPreData.DataSource = dt;
            gvPreData.DataBind();

            TinyCache cache = new TinyCache();
            cache.AddObject("dt", dt);
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            DbHelper.ResetDbProvider();
            DbHelper.ConnectionString = @"Data Source=D:\Documents\DotNet\CvsPlace\itca\iTCA.Yuwen.Web\config\db.config";
            DbHelper.Dbtype = "Sqlite";
            TinyCache cache = new TinyCache();
            DataTable dt = cache.RetrieveObject("dt") as DataTable;

            foreach (DataRow dr in dt.Rows)
            {
                int newid = 23;
                int newaid = newid + Convert.ToInt32(dr["n_id"]);
                int cid;
                switch (dr["n_class"].ToString().Trim())
                {
                    case "协会新闻": cid = 2;
                        break;
                    case "校园新闻": cid = 3;
                        break;
                    case "业内新闻": cid = 4;
                        break;
                    default: cid = 100;
                        break;
                }

                DbParameter[] prams = 
		        {
			        DbHelper.MakeInParam("@articleid", DbType.Int32, 4,newaid),
			        DbHelper.MakeInParam("@title", DbType.String, 100,dr["n_title"]),
			        DbHelper.MakeInParam("@columnid", DbType.Int32, 4,cid),
			        DbHelper.MakeInParam("@content", DbType.String, 100,dr["n_content"]),
			        DbHelper.MakeInParam("@postdate", DbType.DateTime, 8,dr["n_date"]),
			        DbHelper.MakeInParam("@uid", DbType.Int32, 4,1),
			        DbHelper.MakeInParam("@username", DbType.String, 20,dr["n_user"])
		        };
                DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO wy_articles(articleid,columnid,title,uid,username,postdate,content) VALUES(@articleid,@columnid,@title,@uid,@username,@postdate,@content)", prams);
            }

            Response.Write("OK~");
        }
    }
}
