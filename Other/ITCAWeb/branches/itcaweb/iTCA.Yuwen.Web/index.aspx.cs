using System;
using System.Collections.Generic;
using iTCA.Yuwen.Core;
using iTCA.Yuwen.Entity;

namespace iTCA.Yuwen.Web
{
    public partial class index : BasePage
    {
        /// <summary>
        /// Э�������б�
        /// </summary>
        protected List<ArticleInfo> newslist;
        /// <summary>
        /// Э�ṫ���б�
        /// </summary>
        protected List<ArticleInfo> annlist;

        protected override void Page_Show()
        {
            pagetitle = "��ӭ���� iTCA ���칤ѧԺ�����Э�� - iTCA ���칤ѧԺ�����Э��";
            annlist = Articles.GetArticleCollection(1, 4, 1);
            newslist = Articles.GetArticleCollection("2,3,4", 5, 1);
        }
    }
}
