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
    /// 退出页面
    /// </summary>
    public class mallindex : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist;
        /// <summary>
        /// 公告数量
        /// </summary>
        public int announcementcount;
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad;
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead;
        /// <summary>
        ///  Silverlight广告
        /// </summary>
        public string mediaad;
        /// <summary>
        /// 分类间广告
        /// </summary>
        public string inforumad;
        /// <summary>
        /// 新商品列表
        /// </summary>
        public GoodsinfoCollection new_goodsinfocoll;
        /// <summary>
        /// 二手商品列表
        /// </summary>
        public GoodsinfoCollection sec_hand_goodsinfocoll;
        /// <summary>
        /// 一元商品列表
        /// </summary>
        public GoodsinfoCollection one_yuan_goodsinfocoll;
        /// <summary>
        /// 热门推荐商品列表
        /// </summary>
        public GoodsinfoCollection recommend_goodsinfocoll;
        /// <summary>
        /// 商品分类信息(json格式)
        /// </summary>
        public string goodscategory = "";
        /// <summary>
        /// 商品根分类信息
        /// </summary>
        public Goodscategoryinfo[] rootgoodscategoryarray;
        /// <summary>
        /// 查询条件
        /// </summary>
        //public string condition = "";
        #endregion

        protected override void ShowPage()
        {
            // 得到公告
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

            if (config.Enablemall <= 1) //开启普通模式
            {
                AddErrLine("当前页面只有在开启商城(高级)模式下才可访问");
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
