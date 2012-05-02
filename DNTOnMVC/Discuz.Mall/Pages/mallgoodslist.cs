using System;
using System.Data;
using System.Text;
using System.IO;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Data;
using Discuz.Config;
using Discuz.Tag;
using Discuz.Mall;
#if NET1
#else
using Discuz.Common.Generic;
#endif

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// �˳�ҳ��
    /// </summary>
    public class mallgoodslist : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// �����б�
        /// </summary>
        public GoodsinfoCollection goodslist = new GoodsinfoCollection();
        /// <summary>
        /// �����б�
        /// </summary>
        public DataTable announcementlist;
        /// <summary>
        /// ��������
        /// </summary>
        public int announcementcount;
        /// <summary>
        /// �������
        /// </summary>
        public string floatad = "";
        /// <summary>
        /// �������
        /// </summary>
        public string doublead = "";
        /// <summary>
        ///  Silverlight���
        /// </summary>
        public string mediaad = "";
        /// <summary>
        /// �������
        /// </summary>
        public string inforumad = "";
        /// <summary>
        /// ����Ʒ�б�
        /// </summary>
        public GoodsinfoCollection new_goodsinfocoll;
        /// <summary>
        /// ������Ʒ�б�
        /// </summary>
        public GoodsinfoCollection sec_hand_goodsinfocoll;
        /// <summary>
        /// һԪ��Ʒ�б�
        /// </summary>
        public GoodsinfoCollection one_yuan_goodsinfocoll;
        /// <summary>
        /// �����Ƽ���Ʒ�б�
        /// </summary>
        public GoodsinfoCollection recommend_goodsinfocoll;
        /// <summary>
        /// ��Ʒ������Ϣ(json��ʽ)
        /// </summary>
        public string goodscategory = "";
        /// <summary>
        /// ��Ʒ��������Ϣ
        /// </summary>
        public Goodscategoryinfo[] rootgoodscategoryarray;
        /// <summary>
        /// ��ѯ����
        /// </summary>
        public Goodscategoryinfo goodscategoryinfo;
        /// <summary>
        /// ��ǰ�����µ��ӷ���json��ʽ��
        /// </summary>
        public string subcategoriesjson = "";
        /// <summary>
        /// ��Ʒ����Id
        /// </summary>
        public int categoryid = 0; //��Ʒ����
        /// <summary>
        /// ����ʽ
        /// </summary>
        public int order = 1; //�����ֶ�
        /// <summary>
        /// ʱ�䷶Χ
        /// </summary>
        public int cond = 0;
        /// <summary>
        /// ������
        /// </summary>
        public int direct = 1; //������[Ĭ�ϣ�����]
        /// <summary>
        /// ÿҳ��ʾ������
        /// </summary>
        public int gpp;
        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int pageid;

        /// <summary>
        /// ��������
        /// </summary>
        public int goodscount = 0;

        /// <summary>
        /// ��ҳ����
        /// </summary>
        public int pagecount = 1;

        /// <summary>
        /// ��ҳҳ������
        /// </summary>
        public string pagenumbers = "";
        #endregion

        private string condition = ""; //��ѯ����

        protected override void ShowPage()
        {
            // �õ�����
            announcementlist = Announcements.GetSimplifiedAnnouncementList(nowdatetime, "2999-01-01 00:00:00");
            announcementcount = 0;
            if (announcementlist != null)
            {
                announcementcount = announcementlist.Rows.Count;
            }

            inforumad = "";

            floatad = Advertisements.GetFloatAd("indexad", 0);
            doublead = Advertisements.GetDoubleAd("indexad", 0);
            mediaad = Advertisements.GetMediaAd(templatepath, "indexad", 0);

            if (config.Enablemall <= 1) //������ͨģʽ
            {
                AddErrLine("��ǰҳ��ֻ���ڿ����̳�(�߼�)ģʽ�²ſɷ���");
                return;
            }

            categoryid = DNTRequest.GetInt("categoryid", 0);

            if (categoryid <= 0)
            {
                AddErrLine("��Ч����Ʒ����I1");
                return;
            }

            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(categoryid);

            if (goodscategoryinfo == null || goodscategoryinfo.Categoryid <= 0)
            {
                AddErrLine("��Ч����Ʒ����ID");
                return;
            }


            string orderStr = "goodsid";
            condition = "";


            //�õ���ǰ�û������ҳ��
            pageid = DNTRequest.GetInt("page", 1);

            //��ȡ��������
            goodscount = Goods.GetGoodsCount(categoryid, condition);


            // �õ�gpp����
            gpp = 16;//Utils.StrToInt(ForumUtils.GetCookie("tpp"), config.Tpp);

            if (gpp <= 0)
            {
                gpp = config.Tpp;
            }

            //��������ҳ���п��ܵĴ���
            if (pageid < 1)
            {
                pageid = 1;
            }

            //��ȡ��ҳ��
            pagecount = goodscount % gpp == 0 ? goodscount / gpp : goodscount / gpp + 1;
            if (pagecount == 0)
            {
                pagecount = 1;
            }

            if (pageid > pagecount)
            {
                pageid = pagecount;
            }

            goodslist = Goods.GetGoodsInfoList(goodscategoryinfo.Categoryid, gpp, pageid, condition, orderStr, direct);

            if (config.Aspxrewrite == 1)
            {
                pagenumbers = Utils.GetStaticPageNumbers(pageid, pagecount, "mallgoodslist-" + categoryid.ToString(), config.Extname, 8);
            }
            else
            {
                pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "mallgoodslist.aspx?categoryid=" + categoryid.ToString(), 8);
            }

            //�õ��ӷ���JSON��ʽ
            subcategoriesjson = GoodsCategories.GetSubCategoriesJson(goodscategoryinfo.Categoryid);

            new_goodsinfocoll = Goods.GetGoodsInfoList(3, 1, "", "goodsid", 1);

            sec_hand_goodsinfocoll = Goods.GetGoodsInfoList(9, 1, DatabaseProvider.GetInstance().GetGoodsQualityCondition((int)MallUtils.OperaCode.Equal, 2), "goodsid", 1);

            one_yuan_goodsinfocoll = Goods.GetGoodsInfoList(9, 1, DatabaseProvider.GetInstance().GetGoodsPriceCondition((int)MallUtils.OperaCode.Equal, 1), "goodsid", 1);

            recommend_goodsinfocoll = Goods.GetGoodsInfoList(10, 1, DatabaseProvider.GetInstance().GetGoodsRecommendCondition((int)MallUtils.OperaCode.Equal, 1), "goodsid", 1);

            goodscategory = GoodsCategories.GetRootGoodsCategoriesJson();

            rootgoodscategoryarray = GoodsCategories.GetShopRootCategory();
        }
    }
}
