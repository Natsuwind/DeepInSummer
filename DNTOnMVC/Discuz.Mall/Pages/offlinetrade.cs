using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Mall.Data;
using Discuz.Mall;

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// 线下交易页面
    /// </summary>
    public class offlinetrade : PageBase
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
        public string floatad = "";
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
        /// 是否是买家留言
        /// </summary>
        private int buyerleaveword = 1;
        /// <summary>
        /// 是否进行支付
        /// </summary>
        public bool ispay = false;
        /// <summary>
        /// 是否已评价
        /// </summary>
        public bool israted = false;
        /// <summary>
        /// 留言列表
        /// </summary>
        public GoodsleavewordinfoCollection goodsleavewordlist = new GoodsleavewordinfoCollection();
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

            goodsleavewordlist = GoodsLeaveWords.GetLeaveWordList(goodstradelog.Id);

            if (goodstradelog.Status == 7 || goodstradelog.Status == 17)
                israted = GoodsRates.CanRate(goodstradelog.Id, userid) ? false : true; //如果当前用户已评价过则不允许再评价
           
            //如果是提交则更新商品交易日志
            if (ispost && goodstradelog.Status >= 0)
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                //当要验证密码时
                if (DNTRequest.GetInt("status", -1) > 0 && IsVerifyPassWord(goodstradelog.Status))
                {
                    if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("password")))
                    {
                        AddErrLine("密码不能为空, 请返回填写.");
                        return;
                    }

                    int uid = -1;
                    if (config.Passwordmode == 1)
                        uid = Users.CheckDvBbsPassword(base.username, DNTRequest.GetString("password"));
                    else
                        uid = Users.CheckPassword(username, DNTRequest.GetString("password"), true);

                    if (uid < 0)
                    {
                        AddErrLine("您输入的密码不正确, 不能修改订单状态, 请返回修改.");
                        return;
                    }

                    //当验证密码正确后,则发送相应留言
                    Goodsleavewordinfo goodsleavewordinfo = new Goodsleavewordinfo();
                    goodsleavewordinfo.Ip = DNTRequest.GetIP();
                    goodsleavewordinfo.Goodsid = goodstradelog.Goodsid;
                    goodsleavewordinfo.Tradelogid = goodstradelog.Id;
                    goodsleavewordinfo.Uid = userid;
                    goodsleavewordinfo.Username = username;
                    goodsleavewordinfo.Message = DNTRequest.GetString("message");
                    goodsleavewordinfo.Isbuyer = buyerleaveword;
                    GoodsLeaveWords.CreateLeaveWord(goodsleavewordinfo, goodsinfo.Selleruid);
                }

                goodstradelog.Status = DNTRequest.GetInt("status", -1);
               
                if (goodstradelog.Status == 0)
                {
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

                    if (isseller) //当为卖家时
                        goodstradelog.Transportfee = DNTRequest.GetInt("fee", 0);
                }
              
                if (TradeLogs.UpdateTradeLog(goodstradelog, oldstatus, true))
                {
                    SetUrl("offlinetrade.aspx?goodstradelogid=" + goodstradelogid);
                    SetMetaRefresh();
                    AddMsgLine("交易单已更新, 现在转入交易单页面<br />(<a href=\"" + "offlinetrade.aspx?goodstradelogid=" + goodstradelogid + "\">如果您的浏览器没有自动跳转, 请点击这里</a>)<br />");
                }
            }
        }

        private bool IsConditionsValid()
        {
            if (goodstradelog.Offline == 0)
            {
                AddErrLine("当前交易为在线交易!");
                return false;
            }

            //当前用户为买家时
            if (goodstradelog.Buyerid == userid)
            {
                isbuyer = true;
            }
            //当前用户为卖家时
            if (goodstradelog.Sellerid == userid)
            {
                isseller = true;
            }

            //当前用户既不是买家也不是卖家
            if (!isbuyer && !isseller)
            {
                AddErrLine("当前用户身份既不是买家也不是卖家!");
                return false;
            }

            if (goodstradelog.Buyerid <= 0)
            {
                AddErrLine("商品买家信息错误!");
                return false;
            }
            if (goodstradelog.Sellerid <= 0)
            {
                AddErrLine("商品卖家信息错误!");
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
                AddErrLine("此商品已被删除!");
                return false;
            }
            if (goodsinfo.Displayorder == -2)
            {
                AddErrLine("此商品未经审核!");
                return false;
            }
            if (goodsinfo.Expiration <= DateTime.Now)
            {
                AddErrLine("非常抱歉, 该商品不存在或已经到期!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否验证当前用户密码，以便修改交易状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool IsVerifyPassWord(int status)
        {
            //是否检查密码
            bool isverify = false;
            switch ((TradeStatusEnum)status)
            {
                case TradeStatusEnum.UnStart:
                    {
                        buyerleaveword = 0;
                        isverify = true; break;
                    }
                case TradeStatusEnum.WAIT_BUYER_PAY:
                    {
                        buyerleaveword = 0; 
                        isverify = true; break;
                    }
                case TradeStatusEnum.WAIT_SELLER_SEND_GOODS:
                    {
                        buyerleaveword = 1; 
                        isverify = true; break;
                    }
                case TradeStatusEnum.WAIT_BUYER_CONFIRM_GOODS:
                    {
                        buyerleaveword = 0; 
                        isverify = true; break;
                    }
                case TradeStatusEnum.TRADE_CLOSED:
                    {
                        isverify = true; break;
                    }
                case TradeStatusEnum.WAIT_SELLER_AGREE:
                    {
                        buyerleaveword = 1; 
                        isverify = true; break;
                    }
                case TradeStatusEnum.SELLER_REFUSE_BUYER:
                    {
                        buyerleaveword = 0; 
                        isverify = true; break;
                    }
                case TradeStatusEnum.WAIT_BUYER_RETURN_GOODS:
                    {
                        buyerleaveword = 0; 
                        isverify = true; break;
                    }
                case TradeStatusEnum.WAIT_SELLER_CONFIRM_GOODS:
                    {
                        buyerleaveword = 1; 
                        isverify = true; break;
                    }
                case TradeStatusEnum.TRADE_FINISHED:
                    {
                        buyerleaveword = 0; 
                        isverify = true; break;
                    }
            }
            return isverify;
        }
    }
}
