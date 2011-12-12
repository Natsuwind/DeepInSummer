using System;
using iTCA.Yuwen.Core;
using System.Collections.Generic;
using iTCA.Yuwen.Entity;

namespace iTCA.Yuwen.Web
{
    public partial class ann : BasePage
    {
        protected List<ArticleInfo> annlist;
        protected override void Page_Show()
        {
            pagetitle = "协会公告 - iTCA 重庆工学院计算机协会";
            annlist = Articles.GetArticleCollection(1, 26, 1);
        }
    }
}
