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
            pagetitle = "Э�ṫ�� - iTCA ���칤ѧԺ�����Э��";
            annlist = Articles.GetArticleCollection(1, 26, 1);
        }
    }
}
