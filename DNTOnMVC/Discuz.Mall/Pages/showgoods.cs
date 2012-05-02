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
    /// 商品显示页面
    /// </summary>
    public class showgoods : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 商品信息
        /// </summary>
        public Goodsinfo goodsinfo;
        /// <summary>
        /// 推荐商品列表
        /// </summary>
        public GoodsinfoCollection recommendgoodslist;
        /// <summary>
        /// 商品分类
        /// </summary>
        public Goodscategoryinfo goodscategoryinfo = new Goodscategoryinfo();

#if NET1
		
        public ShowtopicPageAttachmentInfoCollection attachmentlist;
        public PrivateMessageInfoCollection pmlist = new PrivateMessageInfoCollection();
#else
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<ShowtopicPageAttachmentInfo> attachmentlist;
        /// <summary>
        /// 短消息列表
        /// </summary>
        public List<PrivateMessageInfo> pmlist; //= new Discuz.Common.Generic.List<PrivateMessageInfo>();
#endif
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead = "";
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad = "";
        /// <summary>
        /// 快速发帖广告
        /// </summary>
        public string quickeditorad = string.Empty;
        /// <summary>
        /// 所属版块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 所属版块名称
        /// </summary>
        public string forumname;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav;
        /// <summary>
        /// 商品Id
        /// </summary>
        public int goodsid = DNTRequest.GetInt("goodsid", -1);
        /// <summary>
        /// 表情分类列表
        /// </summary>
        public DataTable smilietypes = new DataTable();
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;
        /// <summary>
        /// 交易记录数
        /// </summary>
        public int tradecount;
        /// <summary>
        /// 分页页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// 论坛跳转链接选项
        /// </summary>
        public string forumlistboxoptions;
        /// <summary>
        /// 是否是管理者
        /// </summary>
        public int ismoder = 0;
        /// <summary>
        /// 是否显示交易记录
        /// </summary>
        public int showtradelog;
        /// <summary>
        /// 是否解析URL
        /// </summary>
        public int parseurloff;
        /// <summary>
        /// 是否解析表情
        /// </summary>
        public int smileyoff;
        /// <summary>
        /// 是否解析 Discuz!NT 代码
        /// </summary>
        public int bbcodeoff;
        /// <summary>
        /// 是否使用签名
        /// </summary>
        public int usesig;
        /// <summary>
        /// 是否允许 [img]标签
        /// </summary>
        public int allowimg;
        /// <summary>
        /// 用户的管理组信息
        /// </summary>
        public AdminGroupInfo admininfo = null;
        /// <summary>
        /// 当前版块信息
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// 是否有留言的权限
        /// </summary>
        public bool canleaveword = false;
        /// <summary>
        /// 论坛弹出导航菜单HTML代码
        /// </summary>
        public string navhomemenu = "";
        /// <summary>
        /// 是否显示短消息列表
        /// </summary>
        public bool showpmhint = false;
        /// <summary>
        /// 每页日志数
        /// </summary>
        public int pptradelog;
        /// <summary>
        /// 是否显示需要登录后访问的错误提示
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// 第一页表情的JSON
        /// </summary>
        public string firstpagesmilies = Caches.GetSmiliesFirstPageCache();
        /// <summary>
        /// 本版是否启用了Tag
        /// </summary>
        public bool enabletag = false;
        /// <summary>
        /// 最近访问的版块选项
        /// </summary>
        public string visitedforumsoptions;
        /// <summary>
        /// 是否受发帖灌水限制
        /// </summary>
        public int disablepostctrl;
        /// <summary>
        /// 留言数
        /// </summary>
        public int leavewordcount = 0;
        /// <summary>
        /// 买家
        /// </summary>
        public bool isbuyer = false;
        /// <summary>
        /// 卖家
        /// </summary>
        public bool isseller = false;
        /// <summary>
        /// 留言列表的当前分页
        /// </summary>
        public int leaveword_page_currentpage = 1;
        /// <summary>
        /// 是否删除操作（商品/留言）
        /// </summary>
        public bool isdeleteop = false;
        /// <summary>
        /// 发布商品内容
        /// </summary>
        public string message;
        /// <summary>
        /// 要显示的信用记录
        /// </summary>
        public StringBuilder sb_usercredit = new StringBuilder();
        /// <summary>
        /// 获取诚信规则列表GetCreditRulesJsonData
        /// </summary>
        public string creditrulesjsondata = "";
        /// <summary>
        /// 用户注册日期
        /// </summary>
        public string joindate = "";
        /// <summary>
        /// 获取绑定相关版块的商品分类信息
        /// </summary>
        public string goodscategoryfid = "";
        #endregion

        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //未启用交易服务
            {
                AddErrLine("系统未开启交易服务, 当前页面暂时无法访问!");
                return;
            }
            else
                goodscategoryfid = Discuz.Mall.GoodsCategories.GetGoodsCategoryWithFid();

            headerad = "";
            footerad = "";
            floatad = "";

            disablepostctrl = 0;

            // 如果商品ID无效
            if (goodsid == -1)
            {
                AddErrLine("无效的商品ID");
                return ;
            }

            goodsinfo = Goods.GetGoodsInfo(goodsid);
            if (goodsinfo == null || goodsinfo.Closed > 1)
            {
                AddErrLine("不存在的商品ID");
                headerad = Advertisements.GetOneHeaderAd("", 0);
                footerad = Advertisements.GetOneFooterAd("", 0);
                floatad = Advertisements.GetFloatAd("", 0);
                return;
            }

            UserInfo userinfo = Users.GetUserInfo(goodsinfo.Selleruid);
            if(userinfo != null)
                joindate = Convert.ToDateTime(userinfo.Joindate).ToString("yyyy-MM-dd");

            sb_usercredit = GoodsUserCredits.GetUserCreditJsonData(goodsinfo.Selleruid);
            creditrulesjsondata = GoodsUserCredits.GetCreditRulesJsonData().ToString();

            if (config.Enablemall == 1) //开启普通模式
            {
                forumid = GoodsCategories.GetCategoriesFid(goodsinfo.Categoryid);
                forum = Forums.GetForumInfo(forumid);
                if (forum == null)
                {
                    AddErrLine("当前商品所属分类未绑定相应版块");
                    return;
                }

                forumname = forum.Name;
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

                ///得到广告列表
                ///头部
                headerad = Advertisements.GetOneHeaderAd("", forumid);
                footerad = Advertisements.GetOneFooterAd("", forumid);
                doublead = Advertisements.GetDoubleAd("", forumid);
                floatad = Advertisements.GetFloatAd("", forumid);

                // 检查是否具有版主的身份
                if (useradminid != 0)
                {
                    ismoder = Moderators.IsModer(useradminid, userid, forumid) ? 1 : 0;
                    //得到管理组信息
                    admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
                    if (admininfo != null)
                        disablepostctrl = admininfo.Disablepostctrl;
                }
            }
            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(goodsinfo.Categoryid);
            pagetitle = goodsinfo.Title;
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);
        
            //验证不通过则返回
            if (!IsConditionsValid())
                return;        

            //编辑器状态
            StringBuilder sb = new StringBuilder("var Allowhtml=1;\r\n");
            
            parseurloff = 0;
            bbcodeoff = 1;
            if (config.Enablemall == 1) //开启普通模式
            {
                smileyoff = 1 - forum.Allowsmilies;
                
                if (forum.Allowbbcode == 1 && usergroupinfo.Allowcusbbcode == 1)
                    bbcodeoff = 0;
            
                allowimg = forum.Allowimgcode;
            }
            else if (config.Enablemall == 2) //当为高级模式时
            {
                if (usergroupinfo.Allowcusbbcode == 1)
                    bbcodeoff = 0;

                allowimg = 1;
            }

            sb.Append("var Allowsmilies=" + (1 - smileyoff) + ";\r\n");
            sb.Append("var Allowbbcode=" + (1 - bbcodeoff) + ";\r\n");
            usesig = ForumUtils.GetCookie("sigstatus") == "0" ? 0 : 1;
            sb.Append("var Allowimgcode=" + allowimg + ";\r\n");

            AddScript(sb.ToString());

            if (config.Enablemall == 2)
            {
                recommendgoodslist = Goods.GetGoodsRecommendList(goodsinfo.Selleruid, 6, 1,
                    DbProvider.GetInstance().GetGoodsIdCondition((int)MallUtils.OperaCode.NoEuqal, goodsinfo.Goodsid));
            }
       
            smilietypes = Caches.GetSmilieTypesCache();
           
            if (newpmcount > 0)
            {
                pmlist = PrivateMessages.GetPrivateMessageListForIndex(userid, 5, 1, 1);
                showpmhint = Convert.ToInt32(Users.GetShortUserInfo(userid).Newsletter) > 4;
            }


            // 得到pptradelog设置
            pptradelog = Utils.StrToInt(ForumUtils.GetCookie("ppp"), config.Ppp);
            if (pptradelog <= 0)
                pptradelog = config.Ppp;

            //快速发帖广告
            if (config.Enablemall == 1) //开启普通模式
                quickeditorad = Advertisements.GetQuickEditorAD("", forumid);

            //更新页面Meta中的Description项, 提高SEO友好性
            string metadescritpion = Utils.RemoveHtml(goodsinfo.Message);
            metadescritpion = metadescritpion.Length > 100 ? metadescritpion.Substring(0, 100) : metadescritpion;
            UpdateMetaInfo(config.Seokeywords, metadescritpion, config.Seohead);

            GoodspramsInfo goodspramsInfo = new GoodspramsInfo();
            goodspramsInfo.Goodsid = goodsinfo.Goodsid;

            if (config.Enablemall == 1) //开启普通模式
            {
                goodspramsInfo.Fid = forum.Fid;
                goodspramsInfo.Jammer = forum.Jammer;
                goodspramsInfo.Getattachperm = forum.Getattachperm;
                goodspramsInfo.Showimages = forum.Allowimgcode;
            }
            else if (config.Enablemall == 2) //当为高级模式时
            {
                goodspramsInfo.Jammer = 0;
                goodspramsInfo.Getattachperm = "";
                goodspramsInfo.Showimages = 1;
            }
            goodspramsInfo.Pageindex = pageid;
            goodspramsInfo.Usergroupid = usergroupid;
            goodspramsInfo.Attachimgpost = config.Attachimgpost;
            goodspramsInfo.Showattachmentpath = config.Showattachmentpath;
            goodspramsInfo.Hide = 0;
            goodspramsInfo.Price = 0;
            goodspramsInfo.Usergroupreadaccess = usergroupinfo.Readaccess;

            if (ismoder == 1)
                goodspramsInfo.Usergroupreadaccess = int.MaxValue;

            goodspramsInfo.CurrentUserid = userid;            
            goodspramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            goodspramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            goodspramsInfo.Smiliesmax = config.Smiliesmax;
            goodspramsInfo.Bbcodemode = config.Bbcodemode;
            goodspramsInfo.CurrentUserGroup = usergroupinfo;
            goodspramsInfo.Sdetail = goodsinfo.Message;
            goodspramsInfo.Smileyoff = goodsinfo.Smileyoff;
            goodspramsInfo.Bbcodeoff = goodsinfo.Bbcodeoff;
            goodspramsInfo.Parseurloff = goodsinfo.Parseurloff;
            goodspramsInfo.Allowhtml = 1;
            goodspramsInfo.Sdetail = goodsinfo.Message;

            message = Goods.MessgeTranfer(goodspramsInfo, GoodsAttachments.GetGoodsAttachmentsByGoodsid(goodsinfo.Goodsid));
            
            forumlistboxoptions = Caches.GetForumListBoxOptionsCache();
            tradecount = TradeLogs.GetGoodsTradeLogCount(goodsid);
            leavewordcount = GoodsLeaveWords.GetGoodsLeaveWordCount(goodsid);
            pptradelog = 16;

            ForumUtils.WriteCookie("referer", string.Format(base.ShowGoodsAspxRewrite(goodsinfo.Goodsid)));

            if (config.Enablemall == 1) //开启普通模式
                ForumUtils.UpdateVisitedForumsOptions(forumid);

            visitedforumsoptions = ForumUtils.GetVisitedForumsOptions(config.Visitedforums);

            //删除留言
            if (DNTRequest.GetInt("deleteleaveword", 0) == 1)
            {
                isdeleteop = true;
                int leavewordid = DNTRequest.GetInt("leavewordid", 0);

                if (leavewordid <= 0)
                {
                    AddErrLine("您要删除的留言已被删除, 现在转入商品页面");
                    return;
                }
                if (GoodsLeaveWords.DeleteLeaveWordById(leavewordid, userid, goodsinfo.Selleruid, useradminid))
                {
                    SetUrl(base.ShowGoodsAspxRewrite(goodsinfo.Goodsid));
                    SetMetaRefresh();
                    AddMsgLine("该留言已被删除, 现在转入商品页面<br />(<a href=\"" + base.ShowGoodsAspxRewrite(goodsinfo.Goodsid) + "\">如果您的浏览器没有自动跳转, 请点击这里</a>)<br />");
                    return;
                }
                else
                {
                    AddErrLine("您的用户身份无效删除该留言, 现在转入商品页面");
                    return;
                }
            }

            //删除商品
            if (DNTRequest.GetInt("deletegoods", 0) == 1)
            {
                isdeleteop = true;
                //是否为卖家或版主
                if (Goods.IsSeller(goodsinfo.Goodsid.ToString(), userid) || ismoder == 1)
                {
                    Goods.DeleteGoods(goodsinfo.Goodsid.ToString(), false);

                    SetUrl(this.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1));
                    SetMetaRefresh();
                    AddMsgLine("操作成功. <br />(<a href=\"" + this.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1) + "\">点击这里返回</a>)<br />");
                    return;
                }
                else
                {
                    AddErrLine("你不是当前商品的卖家或版主，因此无法删除该商品");
                    return;
                }
            }                     
       

            //如果是提交
            if (ispost)
            {
                //如果不是提交...
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                if (DNTRequest.GetString("postleaveword") == "add")
                {
                    //当验证密码正确后,则发送相应留言
                    Goodsleavewordinfo goodsleavewordinfo = new Goodsleavewordinfo();
                    goodsleavewordinfo.Ip = DNTRequest.GetIP();
                    goodsleavewordinfo.Goodsid = goodsinfo.Goodsid;
                    goodsleavewordinfo.Tradelogid = 0;
                    goodsleavewordinfo.Uid = userid;
                    goodsleavewordinfo.Username = username;
                    goodsleavewordinfo.Message = DNTRequest.GetString("message");
                    goodsleavewordinfo.Isbuyer = goodsinfo.Selleruid != userid ? 1 : 0;
                    if (GoodsLeaveWords.CreateLeaveWord(goodsleavewordinfo, goodsinfo.Selleruid, DNTRequest.GetString("sendnotice") == "on" ? true : false) > 0)
                    {
                        SetUrl(base.ShowGoodsAspxRewrite(goodsinfo.Goodsid));
                        SetMetaRefresh();
                        AddMsgLine("您的留言已发布, 现在转入商品页面<br />(<a href=\"" + base.ShowGoodsAspxRewrite(goodsinfo.Goodsid) + "\">如果您的浏览器没有自动跳转, 请点击这里</a>)<br />");
                    }
                }
                else
                {
                    //当验证密码正确后,则发送相应留言
                    Goodsleavewordinfo goodsleavewordinfo = GoodsLeaveWords.GetGoodsLeaveWordById(DNTRequest.GetInt("leavewordid", 0));
                    if (goodsleavewordinfo != null && goodsleavewordinfo.Id > 0)
                    {
                        goodsleavewordinfo.Ip = DNTRequest.GetIP();
                        goodsleavewordinfo.Uid = userid;
                        goodsleavewordinfo.Username = username;
                        goodsleavewordinfo.Message = DNTRequest.GetString("message");
                        goodsleavewordinfo.Postdatetime = DateTime.Now;
                        if (GoodsLeaveWords.UpdateLeaveWord(goodsleavewordinfo))
                        {
                            SetUrl(base.ShowGoodsAspxRewrite(goodsinfo.Goodsid));
                            SetMetaRefresh();
                            AddMsgLine("留言更新成功, 现在转入商品页面<br />(<a href=\"" + base.ShowGoodsAspxRewrite(goodsinfo.Goodsid) + "\">如果您的浏览器没有自动跳转, 请点击这里</a>)<br />");
                        }
                    }
                    else
                    {
                        AddErrLine("当前留言不存在或已被删除");
                        return;
                    }
                }
            }
            else
            {
                goodsinfo.Viewcount += 1; //浏览量加1
                Goods.UpdateGoods(goodsinfo);
            }
        }

        private bool IsConditionsValid()
        {
            if (goodsinfo.Expiration < DateTime.Now)
            {
                AddErrLine("当前商品已过期!");
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
            if (goodsinfo.Displayorder == -3)
            {
                AddErrLine("当前商品还未上架!");
                return false;
            }
            //当前用户为卖家时
            if (goodsinfo.Selleruid == userid)
                isseller = true;
            else 
                isbuyer = true;

            if (!isseller && config.Enablemall == 1) //开启普通模式
            {
                if (forum.Password != "" && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid + "password"))
                {
                    AddErrLine("本版块被管理员设置了密码");
                    System.Web.HttpContext.Current.Response.Redirect(base.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1), true);
                    return false;
                }

                if (!Forums.AllowViewByUserId(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
                {
                    if (forum.Viewperm == null || forum.Viewperm == string.Empty) //当板块权限为空时，按照用户组权限
                    {
                        if (useradminid != 1 && (usergroupinfo.Allowvisit != 1 || usergroupinfo.Allowtrade != 1))
                        {
                            AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该商品的权限");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return false;
                        }
                    }
                    else //当板块权限不为空，按照板块权限
                    {
                        if (!Forums.AllowView(forum.Viewperm, usergroupid))
                        {
                            AddErrLine("您没有浏览该商品的权限");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return false;
                        }
                    }
                }

                //是否显示回复链接
                if (Forums.AllowReplyByUserID(forum.Permuserlist, userid))
                {
                    canleaveword = true;
                }
                else
                {
                    if (forum.Replyperm == null || forum.Replyperm == string.Empty) //权限设置为空时，根据用户组权限判断
                    {
                        // 验证用户是否有发表主题的权限
                        if (usergroupinfo.Allowtrade == 1)
                        {
                            canleaveword = true;
                        }
                    }
                    else if (Forums.AllowReply(forum.Replyperm, usergroupid))
                    {
                        canleaveword = true;
                    }
                }
            }

            if ((goodsinfo.Closed == 0 && canleaveword) || ismoder == 1)
                canleaveword = true;
            else
                canleaveword = false;

            return true;
        }
    }
}
