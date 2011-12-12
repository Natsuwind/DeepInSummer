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
                lbMessage.Text = string.Format("ȷ��ɾ������: {0} ��?", articleinfo.Title);
            }
            else
            {
                lbMessage.Text = "�����ڴ�����,�뷵��!";
                btnYes.Visible = false;
                btnCancel.Visible = false;
            }
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            Articles.DeleteArticle(articleid);
            Articles.RemoveArtilceCache();
            ShowMsg("���¹���", "ɾ���ɹ�.", "", "frame.aspx?action=listarticle&id=" + articleinfo.Columnid, true);
            //Response.Redirect("frame.aspx?action=listarticle&id=" + articleinfo.Columnid);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frame.aspx?action=listarticle&id=" + articleinfo.Columnid);
        }
    }
}
