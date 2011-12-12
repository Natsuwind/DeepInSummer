using System;
using System.Collections.Generic;
using iTCA.Yuwen.Core;
using iTCA.Yuwen.Entity;

namespace iTCA.Yuwen.Web
{
    public partial class news :BasePage
    {
        protected List<ArticleInfo> campusnewslist;
        protected List<ArticleInfo> canewslist;
        protected List<ArticleInfo> industrynewslist;
        protected override void Page_Show()
        {
            pagetitle = "�������� - iTCA ���칤ѧԺ�����Э��";
            campusnewslist = Articles.GetArticleCollection(3, 10, 1);
            canewslist = Articles.GetArticleCollection(2, 10, 1);
            industrynewslist = Articles.GetArticleCollection(4, 10, 1);
        }
    }
}
