using System;
using System.Web;
using LiteCMS.Core;
using LiteCMS.Entity;

namespace LiteCMS.Web.Admin
{
    public partial class deleteadmin : AdminPage
    {
        int articleid;
        ArticleInfo articleinfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            articleid = Convert.ToInt32(Request.QueryString["articleid"]);
            articleinfo = Articles.GetArticleInfo(articleid);
            if (articleinfo != null)
            {
                lbMessage.Text = string.Format("确定删除文章: {0} 吗?", articleinfo.Title);
            }
            else
            {
                lbMessage.Text = "不存在此文章,请返回!";
                btnYes.Visible = false;
                btnCancel.Visible = false;
            }
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            Articles.DeleteArticle(articleid);
            Articles.RemoveArtilceCache();
            ShowMsg("文章管理", "删除成功.", "", "frame.aspx?action=listarticle&id=" + articleinfo.Columnid, true);
            //Response.Redirect("frame.aspx?action=listarticle&id=" + articleinfo.Columnid);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frame.aspx?action=listarticle&id=" + articleinfo.Columnid);
        }
    }
}
