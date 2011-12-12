using System;
using System.Collections.Generic;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;
using Natsuhime;

namespace LiteCMS.Web
{
    public partial class index : BasePage
    {
        /// <summary>
        /// 协会公告列表
        /// </summary>
        protected List<ArticleInfo> mainarticlelist;
        /// <summary>
        /// 推荐文章页码
        /// </summary>
        protected int pagecount;
        protected string pagecounthtml;
        protected override void Page_Show()
        {
            int pageid = 1;
            TinyCache cache = new TinyCache();
            mainarticlelist = cache.RetrieveObject("articlelist_indexmain") as List<ArticleInfo>;
            if (mainarticlelist == null)
            {
                mainarticlelist = Articles.GetRecommendArticles(15, 1);
                cache.AddObject("articlelist_indexmain", mainarticlelist, config.Globalcachetimeout);
            }
            TinyCache cache_pagecount = new TinyCache();
            object o_pagecount = cache_pagecount.RetrieveObject("pagecount_articlelist_indexmain");
            if (o_pagecount != null)
            {
                pagecount = Convert.ToInt32(o_pagecount);
            }
            else
            {
                pagecount = Articles.GetRecommendArticleCollectionPageCount(15);
                cache.AddObject("pagecount_articlelist_indexmain", pagecount, config.Globalcachetimeout);
            }
            pagecounthtml = config.Urlrewrite == 1 ? Utils.GetStaticPageNumbersHtml(pageid, pagecount, "showcolumn-recommend", ".aspx", 8) : Utils.GetPageNumbersHtml(pageid, pagecount, "showcolumn.aspx?type=recommend", 8, "pageid", "");
        }
    }
}
