using System;
using System.Web;
using LiteCMS.Core;
using LiteCMS.Entity;
using System.IO;

namespace LiteCMS.Web.Admin
{
    public partial class postarticle : AdminPage
    {
        protected int articleid;
        protected void Page_Load(object sender, EventArgs e)
        {
            articleid = Convert.ToInt32(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                BindddlColumns();
                if (articleid > 0)
                {
                    BindArticleInfo();
                }
            }
        }

        private void BindArticleInfo()
        {
            ArticleInfo articleinfo = Articles.GetArticleInfo(articleid);
            if (articleinfo != null)
            {
                ddlColumns.SelectedValue = articleinfo.Columnid.ToString();
                ddlHightlight.SelectedValue = articleinfo.Highlight;
                tbxTitle.Text = Natsuhime.Web.Utils.RemoveHtml(articleinfo.Title.TrimEnd());
                tbxSummary.Text = articleinfo.Summary.TrimEnd();
                tbxContent.Text = articleinfo.Content.TrimEnd();
                ckbxRecommand.Checked = Convert.ToBoolean(articleinfo.Recommend);
                btnSubmit.Text = "编辑";
            }
        }

        private void BindddlColumns()
        {
            ddlColumns.DataTextField = "columnname";
            ddlColumns.DataValueField = "columnid";
            ddlColumns.DataSource = Columns.GetColumnCollection();
            ddlColumns.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ArticleInfo articleinfo = new ArticleInfo();
            articleinfo.Columnid = Convert.ToInt32(ddlColumns.SelectedValue);
            articleinfo.Title = tbxTitle.Text.Trim();
            articleinfo.Highlight = ddlHightlight.SelectedValue;
            articleinfo.Summary = tbxSummary.Text.TrimEnd();
            articleinfo.Content = tbxContent.Text.TrimEnd();
            articleinfo.Postdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            articleinfo.Recommend = Convert.ToInt32(ckbxRecommand.Checked);
            articleinfo.Uid = userinfo.Uid;
            articleinfo.Username = userinfo.Username;
            if (articleid > 0)
            {
                articleinfo.Articleid = articleid;
                Articles.EditArticle(articleinfo);
            }
            else
            {
                Articles.CreateArticle(articleinfo);
            }
            Articles.RemoveArtilceCache();

            ShowMsg("文章管理", "文章提交成功.", "", "articlelist.aspx?cid=" + articleinfo.Columnid, true);
            //Response.Redirect("articlelist.aspx?cid=" + articleinfo.Columnid);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            Random random = new Random();
            string savepath = Path.Combine("upload", DateTime.Now.ToString("yyMM"));
            string filename = fuUploader.FileName;
            string fileextname = Path.GetExtension(filename).ToLower();
            string savefilename = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyMMddhhmm"), Guid.NewGuid().ToString(), fileextname); ;

            bool canUpload = false;
            string[] allowedextensions = { ".gif", ".png", ".jpeg", ".jpg", ".zip", ".rar" };
            foreach (string allowextname in allowedextensions)
            {
                if (fileextname == allowextname)
                {
                    canUpload = true;
                    break;
                }
            }

            if (canUpload == true)
            {
                if (!Directory.Exists(MapPath("~/" + savepath)))
                {
                    Directory.CreateDirectory(MapPath("~/" + savepath));
                }
                fuUploader.SaveAs(MapPath("~/" + savepath) + savefilename);
                lbMessage.Text = "上传成功!";
                tbxContent.Text += string.Format("<br /><img src=/{0}{1}>", savepath, savefilename);
            }
            else
            {
                lbMessage.Text = "不允许的格式!";
            }
        }
    }
}
