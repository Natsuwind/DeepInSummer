using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Mall.Data;
using Discuz.Config;
using Discuz.Mall;

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// 买卖双方互评页面
    /// </summary>
    public class goodsrate : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 商品交易日志Id
        /// </summary>
        public int goodstradelogid = DNTRequest.GetInt("goodstradelogid", 0);
        /// <summary>
        /// 商品交易日志信息
        /// </summary>
        public Goodstradeloginfo goodstradelog = new Goodstradeloginfo();
        /// <summary>
        /// 是否显示需要登录后访问的错误提示
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad = "";
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead;
        /// <summary>
        /// 所属板块Id
        /// </summary>
        public int forumid;
        #endregion

        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //未启用交易服务
            {
                AddErrLine("系统未开启交易服务, 当前页面暂时无法访问!");
                return;
            }

            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }

            headerad = "";
            footerad = "";

            // 如果商品交易日志不正确
            if (goodstradelogid <= 0)
            {
                AddErrLine("无效的交易日志信息.");
                return;
            }

            goodstradelog = TradeLogs.GetGoodsTradeLogInfo(goodstradelogid);
            int oldstatus = goodstradelog.Status;

            if (config.Enablemall == 1) //开启普通模式
                forumid = GoodsCategories.GetCategoriesFid(goodstradelog.Categoryid);
            else
                forumid = 0;

            ///得到广告列表
            ///头部
            headerad = Advertisements.GetOneHeaderAd("", forumid);
            footerad = Advertisements.GetOneFooterAd("", forumid);
            doublead = Advertisements.GetDoubleAd("", forumid);
            floatad = Advertisements.GetFloatAd("", forumid);

            if (goodstradelog.Sellerid != userid && goodstradelog.Buyerid != userid)
            {
                AddErrLine("您的身份不是买卖双方, 因为不能评价");
                return;
            }
            if (goodstradelog.Status != 7 && goodstradelog.Status != 17)
            {
                AddErrLine("交易尚未结束, 因为不能评价");
                return;
            }
            if (!GoodsRates.CanRate(goodstradelog.Id, userid)) //如果当前用户已评价过则不允许再评价
            {
                AddErrLine("不能重复评价");
                return;
            }
           

            //如果是提交...
            if (ispost)
            {
                Goodsrateinfo goodsrateinfo = new Goodsrateinfo();
                goodsrateinfo.Ip = DNTRequest.GetIP();
                goodsrateinfo.Postdatetime = DateTime.Now;
                goodsrateinfo.Price = goodstradelog.Number * goodstradelog.Price + goodstradelog.Transportfee;
                goodsrateinfo.Ratetype = DNTRequest.GetInt("ratetype", 0);
                goodsrateinfo.Uid = userid;
                goodsrateinfo.Username = username;
                goodsrateinfo.Message = DNTRequest.GetString("message");
                goodsrateinfo.Goodstradelogid = goodstradelog.Id;
                goodsrateinfo.Goodstitle = goodstradelog.Subject;
                goodsrateinfo.Goodsid = goodstradelog.Goodsid;
                goodsrateinfo.Explain = "";

                if (goodstradelog.Buyerid == userid)  //买家
                {
                    goodsrateinfo.Uidtype = 2;
                    goodsrateinfo.Ratetouid = goodstradelog.Sellerid;
                    goodsrateinfo.Ratetousername = goodstradelog.Seller;
                    goodstradelog.Ratestatus = 2;
                }
                else //卖家
                {
                    goodsrateinfo.Uidtype = 1;
                    goodsrateinfo.Ratetouid = goodstradelog.Buyerid;
                    goodsrateinfo.Ratetousername = goodstradelog.Buyer;
                    goodstradelog.Ratestatus = 1;
                }
                if (GoodsRates.CreateGoodsRate(goodsrateinfo) > 0) //如果评价成功
                {
                    if(GoodsRates.RateClosed(goodsrateinfo.Goodstradelogid,goodstradelog.Sellerid,goodstradelog.Buyerid))
                    {
                        goodstradelog.Ratestatus = 3;
                        TradeLogs.UpdateTradeLog(goodstradelog, oldstatus); //更新交易的评价状态
                    }

                    GoodsUserCredits.SetUserCredit(goodsrateinfo, goodsrateinfo.Uidtype == 1 ? goodstradelog.Buyerid : goodstradelog.Sellerid);

                    SetUrl(base.ShowGoodsAspxRewrite(goodsrateinfo.Goodsid));
                    SetMetaRefresh();
                    AddMsgLine("您的评价已经成功<br />(<a href=\"" + base.ShowGoodsAspxRewrite(goodsrateinfo.Goodsid) + "\">点击这里返回商品页面</a>)<br />");
                }
            }
        }
    }
}
