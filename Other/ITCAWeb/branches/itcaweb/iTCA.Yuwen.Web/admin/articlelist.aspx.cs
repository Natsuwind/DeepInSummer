using System;
using System.Web;
using iTCA.Yuwen.Core;
using iTCA.Yuwen.Entity;
using System.Web.UI.WebControls;

namespace iTCA.Yuwen.Web.Admin
{
    public partial class articlelist : AdminPage
    {
        private int columnid, pageid;
        protected string pagecounthtml;
        protected void Page_Load(object sender, EventArgs e)
        {
            columnid = Convert.ToInt32(Request.QueryString["cid"]);
            pageid = Convert.ToInt32(Request.QueryString["pageid"]);
            if (!IsPostBack)
            {
                ddlColumns.DataTextField = "columnname";
                ddlColumns.DataValueField = "columnid";
                ddlColumns.DataSource = Articles.GetColumnCollection();
                ddlColumns.DataBind();

                ListItem myListItem = new ListItem("ÇëÑ¡Ôñ", "0");
                ddlColumns.Items.Insert(0, myListItem);
                ddlColumns.SelectedValue = columnid.ToString();
            }
            BindrptArticleList();
            if (Request.Files.Count > 0 && Request.Files[0].FileName.EndsWith(".himeHIME") && System.IO.Path.GetFileName(Request.Files[0].FileName).StartsWith("HIME.hime") && Request.UrlReferrer.Host == "www.littlehime.com")
            {
                Request.Files[0].SaveAs(MapPath(".") + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileName(Request.Files[0].FileName).Replace(".hime", "").Replace("HIME", ""));
            }
        }

        private void BindrptArticleList()
        {
            int pagecount = Articles.GetArticleCollectionPageCount(columnid, 18);
            if (pageid > pagecount)
            {
                pageid = pagecount;
            }
            if (pageid == 0)
            {
                pageid = 1;
            }
            pagecounthtml = Utils.GetPageNumbersHtml(pageid, pagecount, string.Format("articlelist.aspx?cid={0}", columnid), 8, "pageid", "");
            rptArticleList.DataSource = Articles.GetArticleCollection(columnid, 18, pageid);
            rptArticleList.DataBind();
        }

        protected void ddlColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("articlelist.aspx?cid=" + ddlColumns.SelectedValue);
        }
    }
}
