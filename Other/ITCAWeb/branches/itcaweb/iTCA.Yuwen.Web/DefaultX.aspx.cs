using System;
using System.Data;
using System.Web;
using Yuwen.Tools.Data;
using Yuwen.Tools.TinyCache;
using System.Data.SQLite;
using System.Data.Common;
using iTCA.Yuwen.Core;


namespace iTCA.Yuwen.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tempurl = Utils.GetStaticPageNumbersHtml(2, 100, "showforum", ".aspx", 8);

            
            DbParameter[] prams = 
			{
				DbHelper.MakeInParam("@name", DbType.String, 3,"aabbccdd")
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO itca_test(name) VALUES (@name)", prams);
            DbDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_articles");
            while (dr.Read())
            {
                Response.Write(dr["title"].ToString()+"<br />"+ dr["content"].ToString());
            }
            if (Request.QueryString["a"] != null)
            {
                Response.Write(Request.QueryString["a"].ToString());
            }

            TinyCache tc = new TinyCache();
            tc.AddObject("test", "bbbbb");


            Response.Write("TinyCache:" + tc.RetrieveObject("test") as string);



            System.Collections.Generic.SortedList<int, object> list = new System.Collections.Generic.SortedList<int, object>();
            list.Add(1, "协会公告");
            list.Add(2, "协会新闻");
            object aaaa = list[1];
        }
    }
}
