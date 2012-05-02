using System;
using System.Data;
using System.Text;

using Discuz.Entity;
using Discuz.Plugin.Mall;
using Discuz.Mall.Data;

namespace Discuz.Mall
{
    /// <summary>
    /// 商城插件
    /// </summary>
    public class MallPlugin : MallPluginBase
    {
         /// <summary>
        /// 写入商品热门标签缓存文件
        /// </summary>
        /// <param name="count">数量</param>
        public override void WriteHotTagsListForGoodsJSONPCacheFile(int count)
        {
           GoodsTags.WriteHotTagsListForGoodsJSONPCacheFile(count);
        }

        /// <summary>
        /// 获取指定商品id和相关条件下的商品交易信息(json数据串)
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public override StringBuilder GetTradeLogJson(int goodsid, int pagesize, int pageindex, string orderby, int ascdesc)
        {
            return TradeLogs.GetTradeLogJson(goodsid, pagesize, pageindex, orderby, ascdesc);
        }

         /// <summary>
        /// 获取指定商品的交易日志JSON数据
        /// </summary>
        /// <param name="goodsid">指定商品</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public override StringBuilder GetLeaveWordJson(int leavewordid)
        {
            return GoodsLeaveWords.GetLeaveWordJson(leavewordid);
        }

        /// <summary>
        /// 获取指定商品的交易日志JSON数据
        /// </summary>
        /// <param name="goodsid">指定商品</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public override StringBuilder GetLeaveWordJson(int goodsid, int pagesize, int pageindex, string orderby, int ascdesc)
        {
            return GoodsLeaveWords.GetLeaveWordJson(goodsid, pagesize, pageindex, orderby, ascdesc);
        }


        /// <summary>
        /// 获取指定条件的商品评价数据(json格式)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="uidtype">用户id类型(1:卖家, 2:买家, 3:给他人)</param>
        /// <param name="ratetype">评价类型(1:好评, 2:中评, 3:差评)</param>
        /// <param name="filter">进行过滤的条件(oneweek:1周内, onemonth:1月内, sixmonth:半年内, sixmonthago:半年之前)</param>
        /// <returns></returns>
        public override string GetGoodsRatesJson(int uid, int uidtype, int ratetype, string filter)
        {
            return GoodsRates.GetGoodsRatesJson(uid, uidtype, ratetype, filter);
        }

        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="datetype">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        public override string GetHotGoodsJsonData(int days, int categroyid, int count)
        {
            return Goods.GetHotGoodsJsonData(days, categroyid, count);
        }


        /// <summary>
        /// 获取热门或新开的店铺信息
        /// </summary>
        /// <param name="shoptype">热门店铺(1:热门店铺, 2 :新开店铺)</param>
        /// <returns></returns>
        public override string GetShopInfoJson(int shoptype)
        {
            return Shops.GetShopInfoJson(shoptype);
        }

        /// <summary>
        /// 获取推荐商品字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="recommend">推荐信息</param>
        /// <returns>查询条件</returns>
        public override string GetGoodsRecommendCondition(int opcode, int recommend)
        {
            return DbProvider.GetInstance().GetGoodsRecommendCondition(opcode, recommend);
        }

        /// <summary>
        /// 获取商品类型(全新,二手)字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="quality">数量</param>
        /// <returns>查询条件</returns>
        public override string GetGoodsQualityCondition(int opcode, int quality)
        {
            return DbProvider.GetInstance().GetGoodsQualityCondition(opcode, quality);
        }

        /// <summary>
        /// 获取商品关闭字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="closed">关闭信息</param>
        /// <returns>查询条件</returns>
        public override string GetGoodsCloseCondition(int opcode, int closed)
        {
            return DbProvider.GetInstance().GetGoodsCloseCondition(opcode, closed);
        }

         /// <summary>
        /// 获取商品到期日期条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="day">天数</param>
        /// <returns>查询条件</returns>
        public override string GetGoodsExpirationCondition(int opcode, int day)
        {
            return DbProvider.GetInstance().GetGoodsExpirationCondition(opcode, day);
        }

         /// <summary>
        /// 获取商品开始日期条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="day">天数</param>
        /// <returns>查询条件</returns>
        public override string GetGoodsDateLineCondition(int opcode, int day)
        {
            return DbProvider.GetInstance().GetGoodsDateLineCondition(opcode, day);
        }

        /// <summary>
        /// 获取剩余商品数条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="amount">数量</param>
        /// <returns>查询条件</returns>
        public override string GetGoodsRemainCondition(int opcode, int amount)
        {
            return DbProvider.GetInstance().GetGoodsRemainCondition(opcode, amount);
        }

          /// <summary>
        /// 获取商品显示字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="displayorder">显示信息</param>
        /// <returns>查询条件</returns>
        public override string GetGoodsDisplayCondition(int opcode, int displayorder)
        {
            return DbProvider.GetInstance().GetGoodsDisplayCondition(opcode, displayorder);
        }

        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="datetype">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public override GoodsinfoCollection GetHotGoods(int days, int categoryid, int count, string condition)
        {
            return Goods.DTO.GetGoodsInfoList(DbProvider.GetInstance().GetHotGoods(days, categoryid, count, condition));
        }

          /// <summary>
        /// 获取指定分类和条件下的商品列表集合
        /// </summary>
        /// <param name="categoryid">商品分类</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式(0:升序, 1:降序)</param>
        /// <returns></returns>
        public override GoodsinfoCollection GetGoodsInfoList(int categoryid, int pagesize, int pageindex, string condition, string orderby, int ascdesc)
        {
            return Goods.GetGoodsInfoList(categoryid, pagesize, pageindex, condition, orderby, ascdesc);
        }

        /// <summary>
        /// 获取指定条件的商品信息
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public override GoodsinfoCollection GetGoodsInfoList(int pageSize, int pageIndex, string condition, string orderBy, int ascDesc)
        {
            return Goods.GetGoodsInfoList(pageSize, pageIndex, condition, orderBy, ascDesc);
        }


        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="datetype">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        public override string GetGoodsListJsonData(int categroyId, int order, int topNumber)
        {
            return Goods.GetGoodsListJsonData(categroyId, order, topNumber);
        }

        /// <summary>
        /// 获取绑定版块的商品分类
        /// </summary>
        /// <returns></returns>
        public override string GetGoodsCategoryWithFid()
        {
            return GoodsCategories.GetGoodsCategoryWithFid();
        }


        /// <summary>
        /// 返回商品所在地数据
        /// </summary>
        /// <returns></returns>
        public override DataTable GetLocationsTable()
        {
            return DbProvider.GetInstance().GetLocationsTable();
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="goodsid">商品Id</param>
        public override Goodsinfo GetGoodsInfo(int goodsId)
        {
            return Goods.GetGoodsInfo(goodsId);
        }

        /// <summary>
        /// 获取指定标签商品数量
        /// </summary>
        /// <param name="tagid">TAG id</param>
        /// <returns></returns>
        public override int GetGoodsCountWithSameTag(int tagId)
        {
            return DbProvider.GetInstance().GetGoodsCountWithSameTag(tagId);
        }

        /// <summary>
        /// 通过指定的论坛版块id获取相应的商品分类
        /// </summary>
        /// <param name="forumid">版块id</param>
        /// <returns></returns>
        public override int GetGoodsCategoryIdByFid(int forumId)
        {
            return GoodsCategories.GetGoodsCategoryIdByFid(forumId);
        }

        /// <summary>
        /// 获取指定商品标签id的商品信息集合
        /// </summary>
        /// <param name="tagid">tagid</param>
        /// <param name="pageid">页面id</param>
        /// <param name="pagesize">页面尺寸</param>
        /// <returns></returns>
        public override GoodsinfoCollection GetGoodsWithSameTag(int tagId, int pageId, int pageSize)
        {
            return Goods.GetGoodsWithSameTag(tagId, pageId, pageSize);
        }

        /// <summary>
        /// 获取指定附件id的相关附件信息
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <returns>附件信息</returns>
        public override Goodsattachmentinfo GetGoodsAttachmentsByAid(int aid)
        {
            return GoodsAttachments.GetGoodsAttachmentsByAid(aid);
        }

        /// <summary>
        /// 获取指定分类的fid(版块id)字段信息
        /// </summary>
        /// <param name="categoryid">指定的分类id</param>
        /// <returns>(fid)版块id</returns>
        public override int GetCategoriesFid(int categoryId)
        {
            return GoodsCategories.GetCategoriesFid(categoryId);
        }

        /// <summary>
        /// 清除商品分类绑定的版块
        /// </summary>
        /// <param name="fid"></param>
        public override void EmptyGoodsCategoryFid(int fid)
        {
            GoodsCategories.EmptyGoodsCategoryFid(fid);
        }

        /// <summary>
        /// 生成商品分类的json文件
        /// </summary>
        public override void StaticWriteJsonFile()
        {
            GoodsCategories.StaticWriteJsonFile();
        }
    }
}
