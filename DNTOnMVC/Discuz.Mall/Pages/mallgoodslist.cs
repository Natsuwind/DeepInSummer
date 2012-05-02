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
    public class mallgoodslist : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 主题列表
        /// </summary>
        public GoodsinfoCollection goodslist = new GoodsinfoCollection();
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
        public string floatad = "";
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead = "";
        /// <summary>
        ///  Silverlight广告
        /// </summary>
        public string mediaad = "";
        /// <summary>
        /// 分类间广告
        /// </summary>
        public string inforumad = "";
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
        public Goodscategoryinfo goodscategoryinfo;
        /// <summary>
        /// 当前分类下的子分类json格式串
        /// </summary>
        public string subcategoriesjson = "";
        /// <summary>
        /// 商品分类Id
        /// </summary>
        public int categoryid = 0; //商品分类
        /// <summary>
        /// 排序方式
        /// </summary>
        public int order = 1; //排序字段
        /// <summary>
        /// 时间范围
        /// </summary>
        public int cond = 0;
        /// <summary>
        /// 排序方向
        /// </summary>
        public int direct = 1; //排序方向[默认：降序]
        /// <summary>
        /// 每页显示主题数
        /// </summary>
        public int gpp;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;

        /// <summary>
        /// 主题总数
        /// </summary>
        public int goodscount = 0;

        /// <summary>
        /// 分页总数
        /// </summary>
        public int pagecount = 1;

        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";
        #endregion

        private string condition = ""; //查询条件

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

            categoryid = DNTRequest.GetInt("categoryid", 0);

            if (categoryid <= 0)
            {
                AddErrLine("无效的商品分类I1");
                return;
            }

            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(categoryid);

            if (goodscategoryinfo == null || goodscategoryinfo.Categoryid <= 0)
            {
                AddErrLine("无效的商品分类ID");
                return;
            }


            string orderStr = "goodsid";
            condition = "";


            //得到当前用户请求的页数
            pageid = DNTRequest.GetInt("page", 1);

            //获取主题总数
            goodscount = Goods.GetGoodsCount(categoryid, condition);


            // 得到gpp设置
            gpp = 16;//Utils.StrToInt(ForumUtils.GetCookie("tpp"), config.Tpp);

            if (gpp <= 0)
            {
                gpp = config.Tpp;
            }

            //修正请求页数中可能的错误
            if (pageid < 1)
            {
                pageid = 1;
            }

            //获取总页数
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

            //得到子分类JSON格式
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
