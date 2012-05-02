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
using Discuz.Common.Generic;

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// 商品列表页面
    /// </summary>
    public class showgoodslist : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 主题列表
        /// </summary>
        public GoodsinfoCollection goodslist = new GoodsinfoCollection();
        /// <summary>
        /// 当前版块在线用户列表
        /// </summary>
        public List<OnlineUserInfo> onlineuserlist; 
        /// <summary>
        /// 短消息列表
        /// </summary>
        public List<PrivateMessageInfo> pmlist;

        public Goodscategoryinfo goodscategoryinfo;

        /// <summary>
        /// 在线图例列表
        /// </summary>
        public string onlineiconlist = "";
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist = new DataTable();
        /// <summary>
        /// 页内文字广告
        /// </summary>
        public string[] pagewordad;
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead = "";
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad = "";
        /// <summary>
        /// Silverlight广告
        /// </summary>
        public string mediaad = "";
        /// <summary>
        /// 当前版块信息
        /// </summary>
        public ForumInfo forum = new ForumInfo();
        /// <summary>
        /// 用户的管理组信息
        /// </summary>
        public AdminGroupInfo admingroupinfo = new AdminGroupInfo();
        /// <summary>
        /// 当前版块总在线用户数
        /// </summary>
        public int forumtotalonline;
        /// <summary>
        /// 当前版块总在线注册用户数
        /// </summary>
        public int forumtotalonlineuser;
        /// <summary>
        /// 当前版块总在线游客数
        /// </summary>
        public int forumtotalonlineguest;
        /// <summary>
        /// 当前版块在线隐身用户数
        /// </summary>
        public int forumtotalonlineinvisibleuser;
        /// <summary>
        /// 当前版块ID
        /// </summary>
        public int forumid;
        /// <summary>
        /// 当前版块名称
        /// </summary>
        public string forumname;
        /// <summary>
        /// 子版块数
        /// </summary>
        public int subforumcount;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// 是否显示版块密码提示 1为显示, 0不显示
        /// </summary>
        public int showforumlogin;
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
        /// <summary>
        /// 版块跳转链接选项
        /// </summary>
        public string forumlistboxoptions;
        /// <summary>
        /// 最近访问的版块选项
        /// </summary>
        public string visitedforumsoptions;
        /// <summary>
        /// 是否允许Rss订阅
        /// </summary>
        public int forumallowrss;
        /// <summary>
        /// 是否显示在线列表
        /// </summary>
        public bool showforumonline;
        /// <summary>
        /// 是否受发帖控制限制
        /// </summary>
        public int disablepostctrl;
        /// <summary>
        /// 是否允许 [img] 标签
        /// </summary>
        public int allowimg;
        /// <summary>
        /// 每页显示商品数
        /// </summary>
        public int gpp;
        /// <summary>
        /// 是否是管理者
        /// </summary>
        public bool ismoder = false;
        /// <summary>
        /// 是否允许发表主题
        /// </summary>
        public bool canposttopic = false; //是否有发表主题的权限
        /// <summary>
        /// 论坛弹出导航菜单HTML代码
        /// </summary>
        public string navhomemenu = "";
        /// <summary>
        /// 是否显示短消息提示
        /// </summary>
        public bool showpmhint = false;
        /// <summary>
        /// 是否显示需要登录后访问的错误提示
        /// </summary>
        public bool needlogin = false;
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
        /// 当前分类下的子分类json格式串
        /// </summary>
        public string subcategoriesjson = "";
        /// <summary>
        /// 商品分类Id
        /// </summary>
        public int categoryid = DNTRequest.GetInt("categoryid", 0); //商品分类
        /// <summary>
        /// 获取绑定相关版块的商品分类信息
        /// </summary>
        public string goodscategoryfid = "";
        /// <summary>
        /// 所在地信息(格式: "省,市")
        /// </summary>
        public string locus = "";
        #endregion

        private string condition = ""; //查询条件
       

        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //未启用交易模式
            {
                AddErrLine("系统未开启交易模式, 当前页面暂时无法访问!");
                return;
            }
            else
                goodscategoryfid = Discuz.Mall.GoodsCategories.GetGoodsCategoryWithFid();

            forumnav = "";
            forumallowrss = 0;
            if (categoryid <= 0)
            {
                AddErrLine("无效的商品分类ID");
                return;
            }

            if (config.Enablemall == 2) //开启高级模式
            {
                AddLinkRss("mallgoodslist.aspx?categoryid=" + categoryid, "商品列表");
                AddErrLine("当前页面在开启商城(高级)模式下无法访问, 系统将会重定向到商品列表页面!");
                return;
            }

            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(categoryid);
            if (goodscategoryinfo != null && goodscategoryinfo.Categoryid > 0)
            {
                forumid = GoodsCategories.GetCategoriesFid(goodscategoryinfo.Categoryid);
            }
            else 
            {
                AddErrLine("无效的商品分类ID");
                return;
            }

            ///得到广告列表
            ///头部
            headerad = Advertisements.GetOneHeaderAd("", forumid);
            footerad = Advertisements.GetOneFooterAd("", forumid);
            pagewordad = Advertisements.GetPageWordAd("", forumid);
            doublead = Advertisements.GetDoubleAd("", forumid);
            floatad = Advertisements.GetFloatAd("", forumid);
            mediaad = Advertisements.GetMediaAd(templatepath, "", forumid);

            disablepostctrl = 0;
            if (userid > 0 && useradminid > 0)
                admingroupinfo = AdminGroups.GetAdminGroupInfo(usergroupid);

            if (admingroupinfo != null)
                this.disablepostctrl = admingroupinfo.Disablepostctrl;

            if (forumid == -1)
            {
                AddLinkRss("tools/rss.aspx", "最新商品");
                AddErrLine("无效的商品分类ID");
                return;
            }
            else
            {
                forum = Forums.GetForumInfo(forumid);
                // 检查是否具有版主的身份
                if (useradminid > 0)
                    ismoder = Moderators.IsModer(useradminid, userid, forumid);

                #region 对搜索条件进行检索

                string orderStr = "goodsid";

                if (DNTRequest.GetString("search").Trim() != "") //进行指定查询
                {
                    //所在城市信息
                    cond = DNTRequest.GetInt("locus_2", -1);                    
                    if (cond < 1)
                        condition = "";
                    else
                    {
                        locus = Locations.GetLocusByLID(cond);
                        condition = "AND [lid] = " + cond;
                    }

                    //排序的字段
                    order = DNTRequest.GetInt("order", -1);
                    switch (order)
                    {
                        case 2:
                            orderStr = "expiration"; //到期日
                            break;
                        case 1:
                            orderStr = "price"; //商品价格
                            break;
                        default:
                            orderStr = "goodsid";
                            break;
                    }

                    if (DNTRequest.GetInt("direct", -1) == 0)
                        direct = 0;
                }

                #endregion

                if (forum == null)
                {
                    if (config.Rssstatus == 1)
                        AddLinkRss("tools/rss.aspx", Utils.EncodeHtml(config.Forumtitle) + " 最新商品");

                    AddErrLine("不存在的商品分类ID");
                    return;
                }


                //当版块有外部链接时,则直接跳转
                if (forum.Redirect != null && forum.Redirect != string.Empty)
                {
                    System.Web.HttpContext.Current.Response.Redirect(forum.Redirect);
                    return;
                }

                if (forum.Istrade <= 0)
                {
                    AddErrLine("当前版块不允许商品交易");
                    forumnav = "";
                    return;
                }

                if (forum.Fid < 1)
                {
                    if (config.Rssstatus == 1 && forum.Allowrss == 1)
                        AddLinkRss("tools/" + base.RssAspxRewrite(forum.Fid), Utils.EncodeHtml(forum.Name) + " 最新商品");

                    AddErrLine("不存在的商品分类ID");
                    return;
                }
                if (config.Rssstatus == 1)
                    AddLinkRss("tools/" + base.RssAspxRewrite(forum.Fid), Utils.EncodeHtml(forum.Name) + " 最新商品");

                forumname = forum.Name;
                pagetitle = Utils.RemoveHtml(forum.Name);
                subforumcount = forum.Subforumcount;
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
                navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);

                //更新页面Meta中的Description项, 提高SEO友好性
                UpdateMetaInfo(config.Seokeywords, forum.Description, config.Seohead);

                // 是否显示版块密码提示 1为显示, 0不显示
                showforumlogin = 1;
                // 如果版块未设密码
                if (forum.Password == "")
                    showforumlogin = 0;
                else
                {
                    // 如果检测到相应的cookie正确
                    if (Utils.MD5(forum.Password) == ForumUtils.GetCookie("forum" + forumid.ToString() + "password"))
                        showforumlogin = 0;
                    else
                    {
                        // 如果用户提交的密码正确则保存cookie
                        if (forum.Password == DNTRequest.GetString("forumpassword"))
                        {
                            ForumUtils.WriteCookie("forum" + forumid.ToString() + "password", Utils.MD5(forum.Password));
                            showforumlogin = 0;
                        }
                    }
                }

                if (!Forums.AllowViewByUserId(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
                {
                    if (forum.Viewperm == null || forum.Viewperm == string.Empty) //当板块权限为空时，按照用户组权限
                    {
                        if (useradminid != 1 && (usergroupinfo.Allowvisit != 1 || usergroupinfo.Allowtrade != 1))
                        {
                            AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该商品分类的权限");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }
                    }
                    else //当板块权限不为空，按照板块权限
                    {
                        if (!Forums.AllowView(forum.Viewperm, usergroupid))
                        {
                            AddErrLine("您没有浏览该商品分类的权限");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }
                    }
                }


                ////判断是否有发主题的权限
                if (userid > -1 && Forums.AllowPostByUserID(forum.Permuserlist, userid))
                    canposttopic = true;

                if (forum.Postperm == null || forum.Postperm == string.Empty) //权限设置为空时，根据用户组权限判断
                {
                    // 验证用户是否有发表交易的权限
                    if (usergroupinfo.Allowtrade == 1)
                    {
                        canposttopic = true;
                    }
                }
                else if (Forums.AllowPost(forum.Postperm, usergroupid))
                {
                    canposttopic = true;
                }

                //　如果当前用户非管理员并且论坛设定了禁止发帖时间段，当前时间如果在其中的一个时间段内，不允许用户发帖
                if (useradminid != 1 && usergroupinfo.Disableperiodctrl != 1)
                {
                    string visittime = "";
                    if (Scoresets.BetweenTime(config.Postbanperiods, out visittime))
                        canposttopic = false;
                }

                if (newpmcount > 0)
                {
                    pmlist = PrivateMessages.GetPrivateMessageListForIndex(userid, 5, 1, 1);
                    showpmhint = Convert.ToInt32(Users.GetShortUserInfo(userid).Newsletter) > 4;
                }

                //得到子分类JSON格式
                subcategoriesjson = GoodsCategories.GetSubCategoriesJson(categoryid);
                //得到当前用户请求的页数
                pageid = DNTRequest.GetInt("page", 1);
                //获取主题总数
                goodscount = Goods.GetGoodsCount(categoryid, condition);

                // 得到gpp设置
                if (gpp <= 0)
                    gpp = config.Gpp;

                if (gpp <= 0)
                    gpp = 16;
           
                //修正请求页数中可能的错误
                if (pageid < 1)
                    pageid = 1;

                if (forum.Layer > 0)
                {
                    //获取总页数
                    pagecount = goodscount % gpp == 0 ? goodscount / gpp : goodscount / gpp + 1;
                    if (pagecount == 0)
                        pagecount = 1;

                    if (pageid > pagecount)
                        pageid = pagecount;

                    goodslist = Goods.GetGoodsInfoList(categoryid, gpp, pageid, condition, orderStr, direct);

                    ForumUtils.WriteCookie("referer", string.Format("showgoodslist.aspx?categoryid={0}&page={1}&order={2}&direct={3}&locus2={4}&search={5}", categoryid.ToString(), pageid.ToString(), orderStr, direct, cond, DNTRequest.GetString("search")));

                    //得到页码链接
                    if (DNTRequest.GetString("search") == "")
                    {
                        if (categoryid == 0)
                        {
                            if (config.Aspxrewrite == 1)
                            {
                                pagenumbers = Utils.GetStaticPageNumbers(pageid, pagecount, "showgoodslist-" + categoryid.ToString(), config.Extname, 8);
                            }
                            else
                            {
                                pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "showgoodslist.aspx?categoryid=" + categoryid.ToString(), 8);
                            }

                        }
                        else //当有类型条件时
                        {
                            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "showgoodslist.aspx?categoryid=" + categoryid, 8);
                        }
                    }
                    else
                    {
                        pagenumbers = Utils.GetPageNumbers(pageid, pagecount,
                                         "showgoodslist.aspx?search=" + DNTRequest.GetString("search") + "&order=" + 2 + "&direct=" + direct + "&categoryid=" + categoryid + "&locus_2=" + cond , 8);
                    }
                }
            }


            forumlistboxoptions = Caches.GetForumListBoxOptionsCache();

            OnlineUsers.UpdateAction(olid, UserAction.ShowForum.ActionID, forumid, forumname, -1, "");


            showforumonline = false;
            onlineiconlist = Caches.GetOnlineGroupIconList();
            if (forumtotalonline < config.Maxonlinelist || DNTRequest.GetString("showonline") == "yes")
            {
                showforumonline = true;
                onlineuserlist = OnlineUsers.GetForumOnlineUserCollection(forumid, out forumtotalonline, out forumtotalonlineguest,
                                                             out forumtotalonlineuser, out forumtotalonlineinvisibleuser);
            }

            if (DNTRequest.GetString("showonline") == "no")
                showforumonline = false;

            ForumUtils.UpdateVisitedForumsOptions(forumid);
            visitedforumsoptions = ForumUtils.GetVisitedForumsOptions(config.Visitedforums);
            //因为目前还未提供RSS功能,所以下面两项为0
            forumallowrss = 0; 
        }
    }
}
