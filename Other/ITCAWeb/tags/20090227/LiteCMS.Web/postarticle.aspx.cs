using System;
using System.Collections.Generic;

using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class postarticle : BasePage
    {
        protected List<ColumnInfo> columnlist;
        protected override void Page_Show()
        {
            pagetitle = "投递文章";
            UserInfo userinfo = GetUserInfo();
            if (userinfo == null)
            {
                ShowError("投递文章", "请登录后再投递文章,谢谢~", "", "login.aspx");
            }
            columnlist = Columns.GetColumnCollection();
            if (YRequest.IsPost())
            {
                int columnid = YRequest.GetInt("columnid", 0);
                string title = Utils.RemoveHtml(YRequest.GetString("title"));
                string summary = Utils.RemoveHtml(YRequest.GetString("summary"));
                string content = Utils.RemoveUnsafeHtml(YRequest.GetString("content"));

                ArticleInfo articleinfo = new ArticleInfo();
                articleinfo.Columnid = columnid;
                articleinfo.Title = title;
                //articleinfo.Highlight = ddlHightlight.SelectedValue;
                articleinfo.Summary = summary.Length > 160 ? summary.Substring(0, 159) : summary;
                articleinfo.Content = content;
                articleinfo.Postdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                articleinfo.Uid = userinfo.Uid;
                articleinfo.Username = userinfo.Username;
                Articles.CreateArticle(articleinfo);
                Articles.RemoveArtilceCache();
                ShowMsg("投递文章", "发布成功,跳转到栏目列表.", "", string.Format("showcolumn-{0}-1.aspx", articleinfo.Columnid));
            }
        }
    }
}
