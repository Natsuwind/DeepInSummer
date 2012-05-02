using System;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Mall;
using Discuz.Plugin.Payment.Alipay;
using Discuz.Plugin.Payment;

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// 在线交易页面
    /// </summary>
    public class onlinetrade : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 商品信息
        /// </summary>
        public Goodsinfo goodsinfo;
        /// <summary>
        /// 所属版块名称
        /// </summary>
        public string forumname;
        /// <summary>
        /// 所属板块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav;
        /// <summary>
        /// 积分策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo;
        /// <summary>
        /// 商品交易日志Id
        /// </summary>
        public int goodstradelogid = DNTRequest.GetInt("goodstradelogid", 0);
        /// <summary>
        /// 所属版块信息
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad;
        /// <summary>
        /// 论坛弹出导航菜单HTML代码
        /// </summary>
        public string navhomemenu = "";
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead;
        /// <summary>
        /// 用户的管理组信息
        /// </summary>
        public AdminGroupInfo admininfo = null;
        /// <summary>
        /// 是否显示需要登录后访问的错误提示
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// 是否是管理者
        /// </summary>
        public int ismoder = 0;
        /// <summary>
        /// 商品交易日志信息
        /// </summary>
        public Goodstradeloginfo goodstradelog = new Goodstradeloginfo();
        /// <summary>
        /// 买家
        /// </summary>
        public bool isbuyer = false;
        /// <summary>
        /// 卖家
        /// </summary>
        public bool isseller = false;
        /// <summary>
        /// 是否进行支付
        /// </summary>
        public bool ispay = false;
        /// <summary>
        /// 是否已评价
        /// </summary>
        public bool israted = false;
        #endregion

        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //未启用交易服务
            {
                AddErrLine("系统未开启交易服务, 当前页面暂时无法访问!");
                return;
            }

            headerad = "";
            footerad = "";
            floatad = "";

            // 如果商品交易日志不正确
            if (goodstradelogid <= 0)
            {
                AddErrLine("无效的交易日志信息.");
                return;
            }

            goodstradelog = TradeLogs.GetGoodsTradeLogInfo(goodstradelogid);
            int oldstatus = goodstradelog.Status;

            if (config.Enablemall == 1) //开启普通模式
            {
                forumid = GoodsCategories.GetCategoriesFid(goodstradelog.Categoryid);
                forum = Forums.GetForumInfo(forumid);
                forumname = forum.Name;
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
            }
            else if (config.Enablemall == 2) //当为高级模式时
                forumid = 0;

            ///得到广告列表
            ///头部
            headerad = Advertisements.GetOneHeaderAd("", forumid);
            footerad = Advertisements.GetOneFooterAd("", forumid);
            doublead = Advertisements.GetDoubleAd("", forumid);
            floatad = Advertisements.GetFloatAd("", forumid);

            pagetitle = goodstradelog.Subject;
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);

            if (useradminid != 0)
            {
                if (config.Enablemall == 1) //开启普通模式
                    ismoder = Moderators.IsModer(useradminid, userid, forumid) ? 1 : 0;

                //得到管理组信息
                admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            }
            //验证不通过则返回
            if (!IsConditionsValid())
                return;

            if (goodstradelog.Status == 7 || goodstradelog.Status == 17)
                israted = GoodsRates.CanRate(goodstradelog.Id, userid) ? false : true; //如果当前用户已评价过则不允许再评价

            if (DNTRequest.GetString("pay") == "yes")
            {
                ispay = true;
                string alipayurl = GetAliPayUrl();
                SetUrl(alipayurl);
                SetMetaRefresh();
                AddMsgLine("正在提交编号为 " + goodstradelog.Tradeno + " 的订单<br />(<a href=\"" + alipayurl + "\">如果您的浏览器没有自动跳转, 请点击这里</a>)<br />");
                return;
            }


            //如果是提交则更新商品交易日志
            if (ispost)
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                //当为买家时
                if (isbuyer)
                { 
                    goodstradelog.Quality = goodsinfo.Quality;
                    goodstradelog.Categoryid = goodsinfo.Categoryid;
                    goodstradelog.Tax = 0;
                    goodstradelog.Locus = goodsinfo.Locus;
                    goodstradelog.Seller = goodsinfo.Seller;
                    goodstradelog.Sellerid = goodsinfo.Selleruid;
                    goodstradelog.Selleraccount = goodsinfo.Account;
                    goodstradelog.Buyerid = userid;
                    goodstradelog.Buyer = username;
                    goodstradelog.Buyercontact = DNTRequest.GetString("buyercontact");
                    goodstradelog.Buyercredit = 0;
                    goodstradelog.Buyermsg = DNTRequest.GetString("buyermsg");
                    goodstradelog.Status = 0;
                    goodstradelog.Lastupdate = DateTime.Now;
                    goodstradelog.Buyername = DNTRequest.GetString("buyername");
                    goodstradelog.Buyerzip = DNTRequest.GetString("buyerzip");
                    goodstradelog.Buyerphone = DNTRequest.GetString("buyerphone");
                    goodstradelog.Buyermobile = DNTRequest.GetString("buyermobile");
                    goodstradelog.Transport = goodsinfo.Transport;
                    goodstradelog.Baseprice = goodsinfo.Costprice;
                    goodstradelog.Discount = goodsinfo.Discount;
                    goodstradelog.Ratestatus = 0;
                    goodstradelog.Message = "";
                }
                else //当为卖家时
                    goodstradelog.Transportfee = DNTRequest.GetInt("fee", 0);

                if (TradeLogs.UpdateTradeLog(goodstradelog, oldstatus))
                {
                    SetUrl("onlinetrade.aspx?goodstradelogid=" + goodstradelogid);
                    SetMetaRefresh();
                    AddMsgLine("交易单已更新, 现在转入交易单页面<br />(<a href=\"" + "onlinetrade.aspx?goodstradelogid=" + goodstradelogid + "\">如果您的浏览器没有自动跳转, 请点击这里</a>)<br />");
                }
            }
        }


        /// <summary>
        /// 获取在线支付的URL字符串
        /// </summary>
        /// <returns></returns>
        private string GetAliPayUrl()
        {
            string current_url = DNTRequest.GetUrl();
            //forumurl = "http://124.207.144.194:8081/";
            if (forumurl.ToLower().StartsWith("http://"))
                current_url = forumurl + "tradenotify.aspx";
            else
            {
                if (current_url.IndexOf("/aspx/") > 0)
                    current_url = current_url.Substring(0, current_url.IndexOf("/aspx/") + 1) + "tradenotify.aspx";
                else
                    current_url = current_url.Substring(0, current_url.LastIndexOf("/") + 1) + "tradenotify.aspx";
            }

            IPayment _payment = AliPayment.GetService();
            if (goodstradelog.Transport > 0)
            {
                //普通(实物)交易
                NormalTrade normalTrade = new NormalTrade();
                normalTrade.Body = goodstradelog.Subject; //goodsinfo.Message;
                normalTrade.Out_Trade_No = goodstradelog.Tradeno;

                string transportpay = "";
                switch (goodstradelog.Transportpay)
                {
                    case 1: transportpay = "SELLER_PAY"; break;//卖家承担运费
                    case 2: transportpay = "BUYER_PAY"; break;//买家承担运费
                    case 3: transportpay = "BUYER_PAY_AFTER_RECEIVE"; break; //买家收到货后直接支付给物流公司，费用不用计到总价中
                }

                string transport = "";
                switch (goodstradelog.Transport)
                {
                    case 0: transport = "VIRTUAL"; break;//虚拟物品
                    case 1: transport = "POST"; break;//平邮
                    case 2: transport = "EMS"; break;//EMS
                    case 3: transport = "EXPRESS"; break; //其他快递公司
                }
                normalTrade.Logistics_Info = new LogisticsInfo[1] { new LogisticsInfo(transport, goodstradelog.Transportfee, transportpay) };
                normalTrade.Notify_Url = current_url;
                normalTrade.Payment_Type = goodsinfo.Itemtype;
                normalTrade.Price = goodstradelog.Price;
                normalTrade.Quantity = goodstradelog.Number;
                normalTrade.Seller_Email = goodstradelog.Selleraccount;
                normalTrade.Show_Url = current_url.Replace("tradenotify.aspx", base.ShowGoodsAspxRewrite(goodstradelog.Goodsid));
                normalTrade.Subject = goodstradelog.Subject;
                normalTrade.Buyer_Email = Users.GetShortUserInfo(goodstradelog.Buyerid).Email;
                
                return _payment.CreateNormalGoodsTradeUrl((ITrade)normalTrade);
            }
            else
            {
                //虚拟商品交易
                DigitalTrade digitalTrade = new DigitalTrade();
                digitalTrade.Body = goodstradelog.Subject;
                digitalTrade.Out_Trade_No = goodstradelog.Tradeno;

                digitalTrade.Notify_Url = current_url;
                digitalTrade.Payment_Type = goodsinfo.Itemtype;
                digitalTrade.Price = goodstradelog.Price;
                digitalTrade.Quantity = goodstradelog.Number;
                digitalTrade.Seller_Email = goodstradelog.Selleraccount;
                digitalTrade.Show_Url = current_url.Replace("tradenotify.aspx", base.ShowGoodsAspxRewrite(goodstradelog.Goodsid));
                digitalTrade.Subject = goodstradelog.Subject;
                digitalTrade.Buyer_Email = Users.GetShortUserInfo(goodstradelog.Buyerid).Email;

                return _payment.CreateDigitalGoodsTradeUrl((ITrade)digitalTrade);
            }
        }

        private bool IsConditionsValid()
        {
            if (goodstradelog.Offline == 1)
            {
                AddErrLine("当前交易为离线交易!");
                return false;
            }

            //当前用户为买家时
            if (goodstradelog.Buyerid == userid)
                isbuyer = true;

            //当前用户为卖家时
            if (goodstradelog.Sellerid == userid)
                isseller = true;

            //当前用户既不是买家也不是卖家
            if (!isbuyer && !isseller)
            {
                AddErrLine("当前用户身份既不是买家也不是卖家！");
                return false;
            }
            if (goodstradelog.Buyerid <= 0)
            {
                AddErrLine("商品买家信息错误！");
                return false;
            }
            if (goodstradelog.Sellerid <= 0)
            {
                AddErrLine("商品卖家信息错误！");
                return false;
            }

            int goodsid = goodstradelog.Goodsid;
            // 如果商品ID无效
            if (goodsid <= 0)
            {
                AddErrLine("无效的商品ID");
                return false;
            }

            goodsinfo = Goods.GetGoodsInfo(goodsid);
            if (goodsinfo.Displayorder == -1)
            {
                AddErrLine("此商品已被删除！");
                return false;
            }
            if (goodsinfo.Displayorder == -2)
            {
                AddErrLine("此商品未经审核！");
                return false;
            }
            if (goodsinfo.Expiration <= DateTime.Now)
            {
                AddErrLine("非常抱歉, 该商品不存在或已经到期!");
                return false;
            }

            return true;
        }
    }
}
