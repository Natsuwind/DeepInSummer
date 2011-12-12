using System;
using System.Collections.Generic;
using iTCA.Yuwen.Core;
using iTCA.Yuwen.Entity;

namespace iTCA.Yuwen.Web
{
    public partial class showarticle : BasePage
    {
        protected ArticleInfo articleinfo;
        protected override void Page_Show()
        {
            int articleid = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["id"]);
            if (articleid <= 0)
            {
                articleid = 1;
            }
            articleinfo = Articles.GetArticleInfo(articleid);
            if (articleinfo == null)
            {
                System.Web.HttpContext.Current.Response.Write("�����ڵ�����!");
                System.Web.HttpContext.Current.Response.End();
            }
            pagetitle = string.Format("{0} - {1} - iTCA ���칤ѧԺ�����Э��", Utils.RemoveHtml(articleinfo.Title), articleinfo.Columnname);
        }
    }
}
