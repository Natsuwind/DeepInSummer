using System;
using System.Data;
using System.Text;
using System.IO;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Mall.Data;
using Discuz.Config;
using Discuz.Mall;
using Discuz.Common.Generic;

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// 用户收件箱页面
    /// </summary>
    public class usercpmygoods : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 选项名称
        /// </summary>
        public string item = DNTRequest.GetString("item");
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string filter = DNTRequest.GetString("filter");
        /// <summary>
        /// 商品列表
        /// </summary>
        public GoodsinfoCollection goodslist;
        /// <summary>
        /// 商品交易信息表
        /// </summary>
        public DataTable goodstradeloglist;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 记录总数
        /// </summary>
        public int reccount = 0;
        /// <summary>
        /// 分页总数
        /// </summary>
        public int pagecount = 1;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 是否是商品交易列表
        /// </summary>
        public bool istradeloglist = false;
        /// <summary>
        /// 是否显示信用评价
        /// </summary>
        public bool isshowrate = false;
        /// <summary>
        /// 商品id字符串(格式:1,2,3)
        /// </summary>
        public string goodsidlist = DNTRequest.GetString("goodsid");

        public Goosdstradestatisticinfo tradestatisticinfo;
        #endregion           

        protected override void ShowPage()
        {
            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            if (config.Enablemall == 0) //未启用交易服务
            {
                AddErrLine("系统未开启交易服务, 当前页面暂时无法访问!");
                return;
            }

            user = Users.GetUserInfo(userid);

            if (item == "")
                item = "tradestats";


            //当显示交易日志(不是出售中商品)
            if ((item == "selltrade" && filter != "onsell" && filter != "allgoods") || item == "buytrade")
                istradeloglist = true;

            //当为评价,交易成功或退款时,则显示评价字段信息
            if (filter == "eccredit" || filter == "success" || filter == "refund")
                isshowrate = true;


            //获取当前用户的商品数
            if (filter == "allgoods" || filter == "onsell" || item == "tradestats") 
                reccount = (filter == "allgoods") ? Goods.GetGoodsCountBySellerUid(userid, true) : Goods.GetGoodsCountBySellerUid(userid, false);
            else
            {   //获取当前用户做为卖家的交易数
                if (item == "selltrade")
                    reccount = TradeLogs.GetGoodsTradeLogCount(userid, goodsidlist, 1, filter);
                else //获取当前用户做为买家的交易数
                    reccount = TradeLogs.GetGoodsTradeLogCount(userid, goodsidlist, 2, filter);
            }

            if (item == "tradestats")
            {
                tradestatisticinfo = TradeLogs.GetTradeStatistic(user.Uid);
                return;
            }

            // 得到分页大小设置
            int pagesize = 10;
            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;

            //获取总页数
            pagecount = reccount % pagesize == 0 ? reccount / pagesize : reccount / pagesize + 1;
            if (pagecount == 0)
                pagecount = 1;

            if (pageid > pagecount)
                pageid = pagecount;

              //如果不是提交...
            if (!ispost)
            {
                if (item == "selltrade" && (filter == "allgoods" || filter == "onsell"))
                {
                    if (filter == "allgoods")
                        goodslist = Goods.GetGoodsListBySellerUID(userid, true, pagesize, pageid, "lastupdate", 1);
                    else
                        goodslist = Goods.GetGoodsListBySellerUID(userid, false, pagesize, pageid, "lastupdate", 1);

                    pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpmygoods.aspx?item=" + item + "&filter=" + filter , 8);
                }
                else
                {
                    if (item == "selltrade")
                        goodstradeloglist = TradeLogs.GetGoodsTradeLogList(userid, goodsidlist, 1, filter, pagesize, pageid);
                    else
                        goodstradeloglist = TradeLogs.GetGoodsTradeLogList(userid, goodsidlist, 2, filter, pagesize, pageid);

                    pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpmygoods.aspx?item=" + item + "&filter=" + filter, 8);
                }
            }
            else
            {
                string operation = DNTRequest.GetString("operation");

                if (operation == "")
                    operation = "deletegoods";

                if (operation == "deletegoods")
                {
                    if (goodsidlist == "")
                    {
                        AddErrLine("你未选中任何商品");
                        return;
                    }

                    if (Goods.IsSeller(goodsidlist, userid))
                    {
                        Goods.DeleteGoods(goodsidlist, false);

                        SetUrl("usercpmygoods.aspx?item=" + item + "&filter=" + filter);
                        SetMetaRefresh();
                        AddMsgLine("操作成功. <br />(<a href=\"usercpmygoods.aspx?item=" + item + "&filter=" + filter + "\">点击这里返回</a>)<br />");
                    }
                    else
                    {
                        AddErrLine("你不是当前商品的卖家，因此无法删除该商品");
                        return;
                    }
                }
            }
        }
    }
}
