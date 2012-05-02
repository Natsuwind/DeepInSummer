using System;
using System.Data;
using System.Text;
using System.Web;

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
    /// 购买商品页面
    /// </summary>
    public class buygoods : PageBase
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
        /// 商品Id
        /// </summary>
        public int goodsid = DNTRequest.GetInt("goodsid", -1);
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
        /// 当前商品分类
        /// </summary>
        public Goodscategoryinfo goodscategoryinfo;
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

            // 如果主题ID非数字
            if (goodsid == -1)
            {
                AddErrLine("无效的商品ID");
                return;
            }

            if (userid <= 0)
            {
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "login.aspx?reurl=buygoods.aspx?goodsid=" + goodsid);
            }

            goodsinfo = Goods.GetGoodsInfo(goodsid);

            //验证不通过则返回
            if (!IsConditionsValid())
                return;

            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(goodsinfo.Categoryid);

            if (config.Enablemall == 1) //开启普通模式
            {
                forumid = goodscategoryinfo.Fid;
                forum = Forums.GetForumInfo(forumid);

                if (forum.Password != "" &&
                    Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid + "password"))
                {
                    AddErrLine("本版块被管理员设置了密码");
                    System.Web.HttpContext.Current.Response.Redirect(base.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1), true);
                    return;
                }                

                if (!Forums.AllowViewByUserId(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
                {
                    if (forum.Viewperm == null || forum.Viewperm == string.Empty)//当板块权限为空时，按照用户组权限
                    {
                        if (usergroupinfo.Allowvisit != 1)
                        {
                            AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该版块的权限");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }

                        if (useradminid != 1 && (usergroupinfo.Allowvisit != 1 || usergroupinfo.Allowtrade != 1))
                        {
                            AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有进行交易商品的权限");
                            return;
                        }
                    }
                    else//当板块权限不为空，按照板块权限
                    {
                        if (!Forums.AllowView(forum.Viewperm, usergroupid))
                        {
                            AddErrLine("您没有浏览该版块的权限");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }                       
                    }
                }

                if (!Forums.AllowPostByUserID(forum.Permuserlist, userid)) //判断当前用户在当前版块发布商品权限
                {
                    if (forum.Postperm == null || forum.Postperm == string.Empty)//权限设置为空时，根据用户组权限判断
                    {
                        // 验证用户是否有发布商品的权限
                        if (usergroupinfo.Allowtrade != 1)
                        {
                            AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有进行交易商品的权限");
                            return;
                        }
                    }
                    else//权限设置不为空时,根据板块权限判断
                    {
                        if (!Forums.AllowPost(forum.Postperm, usergroupid))
                        {
                            AddErrLine("您没有进行交易商品的权限");
                            return;
                        }
                    }
                }

                forumname = forum.Name;
                pagetitle = goodsinfo.Title;
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
            }
            else if (config.Enablemall == 2) //当为高级模式时
            {
                forumid = 0;
            }

            ///得到广告列表
            ///头部
            headerad = Advertisements.GetOneHeaderAd("", forumid);
            footerad = Advertisements.GetOneFooterAd("", forumid);
            doublead = Advertisements.GetDoubleAd("", forumid);
            floatad = Advertisements.GetFloatAd("", forumid);

            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);

            if (useradminid != 0)
            {
                if (config.Enablemall == 1) //开启普通模式
                {
                    ismoder = Moderators.IsModer(useradminid, userid, forumid) ? 1 : 0;
                }
                //得到管理组信息
                admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            }          


            //如果是提交...
            if (ispost)
            {
                //创建商品交易日志
                goodstradelog.Number = DNTRequest.GetInt("number", 0);
                // 商品数不正确
                if (goodstradelog.Number <= 0)
                {
                    AddErrLine("请输入正确的商品数, 请返回修改.");
                    return;
                }
                if (goodsinfo.Amount < goodstradelog.Number)
                {
                    AddErrLine("商品剩余数量不足 (剩余数量为 " + goodsinfo.Amount + ", 而购买数量为 " + goodstradelog.Number + ").");
                    return;
                }

                goodstradelog.Sellerid = goodsinfo.Selleruid;
                goodstradelog.Buyerid = userid;
                if (goodstradelog.Buyerid == goodstradelog.Sellerid)
                {
                    AddErrLine("买卖双方不能是同一用户.");
                    return;
                }
                goodstradelog.Goodsid = goodsinfo.Goodsid;
                goodstradelog.Offline = DNTRequest.GetInt("offline", 0);
                goodstradelog.Orderid = TradeLogs.GetOrderID();
                goodstradelog.Subject = goodsinfo.Title;
                goodstradelog.Price = goodsinfo.Price;
                goodstradelog.Quality = goodsinfo.Quality;
                goodstradelog.Categoryid = goodsinfo.Categoryid;
                goodstradelog.Tax = 0;
                goodstradelog.Locus = goodsinfo.Locus;
                goodstradelog.Seller = goodsinfo.Seller;
                goodstradelog.Selleraccount = goodsinfo.Account;
                goodstradelog.Buyer = username;
                goodstradelog.Buyercontact = DNTRequest.GetString("buyercontact");
                goodstradelog.Buyercredit = 0;
                goodstradelog.Buyermsg = DNTRequest.GetString("buyermsg");
                goodstradelog.Status = (int) TradeStatusEnum.UnStart;
                goodstradelog.Lastupdate = DateTime.Now;
                goodstradelog.Buyername = DNTRequest.GetString("buyername");
                goodstradelog.Buyerzip = DNTRequest.GetString("buyerzip");
                goodstradelog.Buyerphone = DNTRequest.GetString("buyerphone");
                goodstradelog.Buyermobile = DNTRequest.GetString("buyermobile");
                goodstradelog.Transport = DNTRequest.GetInt("transport", 0);
                goodstradelog.Transportpay = goodsinfo.Transport;
                goodstradelog.Transportfee = Convert.ToDecimal(DNTRequest.GetFormFloat("fee", 0).ToString());
                goodstradelog.Tradesum = goodstradelog.Number * goodstradelog.Price + (goodstradelog.Transportpay == 2 ? goodstradelog.Transportfee : 0);
                goodstradelog.Baseprice = goodsinfo.Costprice;
                goodstradelog.Discount = goodsinfo.Discount;
                goodstradelog.Ratestatus = 0;
                goodstradelog.Message = "";

                int tradelogid = TradeLogs.CreateTradeLog(goodstradelog);

                if (tradelogid > 0)
                {
                    string jumpurl = "";
                    if (goodstradelog.Offline == 0)
                        jumpurl = "onlinetrade.aspx?goodstradelogid=" + tradelogid;
                    else
                        jumpurl = "offlinetrade.aspx?goodstradelogid=" + tradelogid;

                    SetUrl(jumpurl);
                    SetMetaRefresh();
                    AddMsgLine("交易单已创建, 现在将转入交易单页面<br />(<a href=\"" + jumpurl + "\">如果您的浏览器没有自动跳转, 请点击这里</a>)<br />");
                }
                else
                {
                    SetUrl("buygoods.aspx?goodsid=" + goodsid);
                    SetMetaRefresh();
                    AddMsgLine("交易单创建错误, 请重新添写交易单<br />(<a href=\"" + "buygoods.aspx?goodsid=" + goodsid + "\">如果您的浏览器没有自动跳转, 请点击这里</a>)<br />");
                }
            }
        }

        private bool IsConditionsValid()
        {
            if (goodsinfo == null || goodsinfo.Closed > 1 || goodsinfo.Amount <= 0)
            {
                if (goodsinfo.Amount <= 0)
                {
                    AddErrLine("商品库存不足");
                }
                else
                {
                    AddErrLine("不存在的商品ID");
                }
                headerad = Advertisements.GetOneHeaderAd("", 0);
                footerad = Advertisements.GetOneFooterAd("", 0);
                floatad = Advertisements.GetFloatAd("", 0);
                return false;
            }
            if (goodsinfo.Expiration <= DateTime.Now)
            {
                AddErrLine("非常抱歉, 该宝贝不存在或已经结束了！");
                return false;
            }
            if (goodsinfo.Closed == 1)
            {
                AddErrLine("此商品已关闭！");
                return false;
            }
            if (goodsinfo.Selleruid <= 0)
            {
                AddErrLine("商品卖家信息错误！");
                return false;
            }
            if (userid == goodsinfo.Selleruid)
            {
                AddErrLine("买卖双方不能为同一用户！");
                return false;
            }
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
            return true;
        }
    }
}
