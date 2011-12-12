using System;
using System.Web;
using LiteCMS.Core;
using LiteCMS.Entity;
using System.Web.UI.WebControls;

namespace LiteCMS.Web.Admin
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
                ddlColumns.DataSource = Columns.GetColumnCollection();
                ddlColumns.DataBind();

                ListItem myListItem = new ListItem("ÇëÑ¡Ôñ", "0");
                ddlColumns.Items.Insert(0, myListItem);
                ddlColumns.SelectedValue = columnid.ToString();
            }
            BindrptArticleList();
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
            pagecounthtml = Natsuhime.Web.Utils.GetPageNumbersHtml(pageid, pagecount, string.Format("articlelist.aspx?cid={0}", columnid), 8, "pageid", "");
            rptArticleList.DataSource = Articles.GetArticleCollection(columnid, 18, pageid);
            rptArticleList.DataBind();
        }

        protected void ddlColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("articlelist.aspx?cid=" + ddlColumns.SelectedValue);
        }
    }
}
