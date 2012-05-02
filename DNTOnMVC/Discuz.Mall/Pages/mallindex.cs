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
    public class mallindex : PageBase
    {
        #region ҳ�����
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
        public string floatad;
        /// <summary>
        /// �������
        /// </summary>
        public string doublead;
        /// <summary>
        ///  Silverlight���
        /// </summary>
        public string mediaad;
        /// <summary>
        /// �������
        /// </summary>
        public string inforumad;
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
        //public string condition = "";
        #endregion

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

            new_goodsinfocoll = Goods.GetGoodsInfoList(3, 1, "", "goodsid", 1);

            sec_hand_goodsinfocoll = Goods.GetGoodsInfoList(9, 1, DatabaseProvider.GetInstance().GetGoodsQualityCondition((int)MallUtils.OperaCode.Equal, 2), "goodsid", 1);

            one_yuan_goodsinfocoll = Goods.GetGoodsInfoList(9, 1, DatabaseProvider.GetInstance().GetGoodsPriceCondition((int)MallUtils.OperaCode.Equal, 1), "goodsid", 1);

            recommend_goodsinfocoll = Goods.GetGoodsInfoList(10, 1, DatabaseProvider.GetInstance().GetGoodsRecommendCondition((int)MallUtils.OperaCode.Equal, 1), "goodsid", 1);

            goodscategory = GoodsCategories.GetRootGoodsCategoriesJson();

            rootgoodscategoryarray = GoodsCategories.GetShopRootCategory();
        }   
    }
}
