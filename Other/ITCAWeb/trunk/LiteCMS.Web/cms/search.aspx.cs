using System;
using System.Collections.Generic;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class search : BasePage
    {
        protected List<ArticleInfo> searchresultlist;
        protected string pagecounthtml;
        protected override void Page_Show()
        {
            UserInfo userinfo = GetUserInfo();
            if (userinfo == null)
            {
                ShowError("站内搜索", "请登录后再搜索文章,谢谢~", "", "login.aspx");
            }
            string searchkey = YRequest.GetQueryString("searchkey");
            if (searchkey != string.Empty && Natsuhime.Common.Utils.IsSafeSqlString(searchkey))
            {
                int pageid = YRequest.GetQueryInt("pageid", 1);
                int pagecount;
                pagecount = Articles.GetSearchArticleCollectionPageCount(searchkey, 12);
                searchresultlist = Articles.GetSearchArticles(searchkey, 12, pageid);
                pagecounthtml = config.Urlrewrite == 1 ? Natsuhime.Web.Utils.GetStaticPageNumbersHtml(pageid, pagecount, string.Format("search-{0}", searchkey), ".aspx", 8) : Utils.GetPageNumbersHtml(pageid, pagecount, string.Format("search.aspx?searchkey={0}", searchkey), 8, "pageid", "");
            }
            else
            {
                ShowError("站内搜索", "参数异常!", "", "");
            }
        }
    }
}
