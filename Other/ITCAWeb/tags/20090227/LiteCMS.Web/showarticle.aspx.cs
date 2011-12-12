using System;
using System.Collections.Generic;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class showarticle : BasePage
    {
        protected ArticleInfo articleinfo;
        protected List<CommentInfo> commentlist;
        protected string commentcounthtml;
        protected override void Page_Show()
        {
            int articleid = YRequest.GetQueryInt("id", 1);
            articleinfo = Articles.GetArticleInfo(articleid);
            if (articleinfo == null)
            {
                ShowError("查看文章", "错误,不存在的文章!", "", "");
            }
            pagetitle = string.Format("{0} - {1}", Natsuhime.Web.Utils.RemoveHtml(articleinfo.Title), articleinfo.Columnname);

            int commentpageid=YRequest.GetQueryInt("commentpageid", 1);
            commentlist = Comments.GetCommentCollection(articleid, 8, commentpageid);
            int commentpagecount = Comments.GetCommentCollectionPageCount(articleid, 8);
            commentcounthtml = config.Urlrewrite == 1 ? Natsuhime.Web.Utils.GetStaticPageNumbersHtml(commentpageid, commentpagecount, string.Format("showarticle-{0}", articleid), ".aspx", 8) : Utils.GetPageNumbersHtml(commentpageid, commentpagecount, string.Format("showarticle.aspx?id={0}", articleid), 8, "commentpageid", "");
        }
    }
}
