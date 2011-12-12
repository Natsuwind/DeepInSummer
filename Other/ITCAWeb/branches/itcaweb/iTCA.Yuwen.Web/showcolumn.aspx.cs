using System;
using System.Collections.Generic;
using iTCA.Yuwen.Core;
using iTCA.Yuwen.Entity;


namespace iTCA.Yuwen.Web
{
    public partial class showcolumn : BasePage
    {
        protected List<ArticleInfo> newslist;
        protected string pagecounthtml;
        protected override void Page_Show()
        {
            int columnid, pageid, pagecount;
            columnid = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["cid"]);
            pageid = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["pageid"]);

            pagecount = Articles.GetArticleCollectionPageCount(columnid, 26);
            if (pageid > pagecount)
            {
                pageid = pagecount;
            }
            if (pageid == 0)
            {
                pageid = 1;
            }
            pagecounthtml = Utils.GetStaticPageNumbersHtml(pageid, pagecount, string.Format("showcolumn-{0}", columnid), ".aspx", 8);
            newslist = Articles.GetArticleCollection(columnid, 26, pageid);

            pagetitle = string.Format("{0} - iTCA 重庆工学院计算机协会", Articles.GetColumnName(columnid));
        }
    }
}
