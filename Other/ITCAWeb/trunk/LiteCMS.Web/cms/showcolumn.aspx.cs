using System;
using System.Collections.Generic;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class showcolumn : BasePage
    {
        protected List<ArticleInfo> articlelist;
        protected string pagecounthtml;
        protected override void Page_Show()
        {
            int columnid, pageid, pagecount;
            string type;
            columnid = YRequest.GetInt("cid", 0);
            pageid = YRequest.GetInt("pageid", 1);
            type = YRequest.GetQueryString("type");

            if (type == string.Empty)
            {//��׼��ʾ��ʽ(������Ŀ��ʾ)
                pagecount = Articles.GetArticleCollectionPageCount(columnid, 15);
                if (pageid > pagecount)
                {
                    pageid = pagecount;
                }
                pagecounthtml = config.Urlrewrite == 1 ? Natsuhime.Web.Utils.GetStaticPageNumbersHtml(pageid, pagecount, string.Format("showcolumn-{0}", columnid), ".aspx", 8) : Utils.GetPageNumbersHtml(pageid, pagecount, string.Format("showcolumn.aspx?cid={0}", columnid), 8, "pageid", "");
                articlelist = Articles.GetArticleCollection(columnid, 15, pageid);

                if (columnid > 0)
                {
                    pagetitle = Columns.GetColumnName(columnid);
                }
                else
                {
                    pagetitle = "���������б�";
                }
            }
            else if (type == "recommend")
            {//�Ƽ�������ʾ
                pagecount = Articles.GetRecommendArticleCollectionPageCount(15);
                if (pageid > pagecount)
                {
                    pageid = pagecount;
                }
                pagecounthtml = config.Urlrewrite == 1 ? Utils.GetStaticPageNumbersHtml(pageid, pagecount, "showcolumn-recommend", ".aspx", 8) : Utils.GetPageNumbersHtml(pageid, pagecount, "showcolumn.aspx?type=recommend", 8, "pageid", "");
                articlelist = Articles.GetRecommendArticles(15, pageid);
                pagetitle = "�Ƽ�����";
            }
            else if (type == "hot")
            {//����������ʾ
                pagecount = Articles.GetHotArticleCollectionPageCount(15);
                if (pageid > pagecount)
                {
                    pageid = pagecount;
                }
                pagecounthtml = config.Urlrewrite == 1 ? Utils.GetStaticPageNumbersHtml(pageid, pagecount, "showcolumn-hot", ".aspx", 8) : Utils.GetPageNumbersHtml(pageid, pagecount, "showcolumn.aspx?type=hot", 8, "pageid", "");
                articlelist = Articles.GetHotArticles(15, pageid);
                pagetitle = "��������";
            }
        }
    }
}
