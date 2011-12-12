using System;
using System.Collections.Generic;
using iTCA.Yuwen.Core;
using iTCA.Yuwen.Entity;

namespace iTCA.Yuwen.Web
{
    public partial class index : BasePage
    {
        /// <summary>
        /// 协会新闻列表
        /// </summary>
        protected List<ArticleInfo> newslist;
        /// <summary>
        /// 协会公告列表
        /// </summary>
        protected List<ArticleInfo> annlist;

        protected override void Page_Show()
        {
            pagetitle = "欢迎来到 iTCA 重庆工学院计算机协会 - iTCA 重庆工学院计算机协会";
            annlist = Articles.GetArticleCollection(1, 4, 1);
            newslist = Articles.GetArticleCollection("2,3,4", 5, 1);
        }
    }
}
