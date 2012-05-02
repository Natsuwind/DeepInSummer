using System;
using System.Web;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Mall.Data;
using Discuz.Mall;

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// 帖子管理页面
    /// </summary>
    public class goodsadmin : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 操作标题
        /// </summary>
        public string operationtitle = "";
        /// <summary>
        /// 操作类型
        /// </summary>
        public string operation = DNTRequest.GetQueryString("operation").ToLower();
        /// <summary>
        /// 操作类型参数
        /// </summary>
        public string action = DNTRequest.GetQueryString("action");
        ///// <summary>
        ///// 商品列表
        ///// </summary>
        public string goodslist = DNTRequest.GetString("goodsid");
        /// <summary>
        /// 版块名称
        /// </summary>
        public string forumname = "";
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// 帖子标题
        /// </summary>
        public string title = "";
        /// <summary>
        /// 帖子作者用户名
        /// </summary>
        public string poster = "";
        /// <summary>
        /// 版块Id
        /// </summary>
        public int forumid = 0;
        /// <summary>
        /// 主题置顶状态
        /// </summary>
        public int displayorder = 0;
        /// <summary>
        /// 高亮颜色
        /// </summary>
        public string highlight_color = "";
        /// <summary>
        /// 是否加粗
        /// </summary>
        public string highlight_style_b = "";
        /// <summary>
        /// 是否斜体
        /// </summary>
        public string highlight_style_i = "";
        /// <summary>
        /// 是否带下划线
        /// </summary>
        public string highlight_style_u = "";
        /// <summary>
        /// 关闭主题, 0=打开,1=关闭 
        /// </summary>
        public int close = 0;
        /// <summary>
        /// 后续操作
        /// </summary>
        public int donext = 0;
        /// <summary>
        /// 商品分类Id
        /// </summary>
        public int categoryid = DNTRequest.GetInt("categoryid", 0); //商品分类
        /// <summary>
        /// 商品分类信息
        /// </summary>
        public Goodscategoryinfo goodscategoryinfo;
        #endregion

        /// <summary>
        /// 是否允许管理商品, 初始false为不允许
        /// </summary>
        protected bool ismoder = false;
        protected int RateIsReady = 0;
        private ForumInfo forum;
        private int highlight = 0;
       
        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //未启用交易服务
            {
                AddErrLine("系统未开启交易服务, 当前页面暂时无法访问!");
                return;
            }

            if (userid == -1)
            {
                AddErrLine("请先登录");
                return;
            }
            if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost()) || action == "")
            {
                AddErrLine("非法提交");
                return;
            }

            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(categoryid);
            forumid = goodscategoryinfo.Fid;
            // 检查是否具有版主的身份
            ismoder = Moderators.IsModer(useradminid, userid, forumid);
            // 如果拥有管理组身份
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
         
            operationtitle = "操作提示";
            SetUrl(base.ShowGoodsListAspxRewrite(categoryid, 0));

            if (action == "")
            {
                AddErrLine("操作类型参数为空");
                return;
            }
            if (forumid == -1)
            {
                AddErrLine("无效的商品分类ID");
                return;
            }
            if (DNTRequest.GetFormString("goodsid") != "" && !Goods.InSameCategory(goodslist, categoryid))
            {
                AddErrLine("无法对非本分类商品进行管理操作");
                return;
            }

            forum = Forums.GetForumInfo(forumid);
            forumname = forum.Name;

            if (!Forums.AllowViewByUserId(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
            {
                if (forum.Viewperm == null || forum.Viewperm == string.Empty) //当板块权限为空时，按照用户组权限
                {
                    if (useradminid != 1 && (usergroupinfo.Allowvisit != 1 || usergroupinfo.Allowtrade != 1))
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该商品的权限");
                        return;
                    }
                }
                else //当板块权限不为空，按照板块权限
                {
                    if (!Forums.AllowView(forum.Viewperm, usergroupid))
                    {
                        AddErrLine("您没有浏览该商品的权限");
                        return; 
                    }
                }
            }

            pagetitle = Utils.RemoveHtml(forumname);
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

            if (goodslist.CompareTo("") == 0)
            {
                AddErrLine("您没有选择商品或相应的管理操作,请返回修改");
                return;
            }

            if (operation.CompareTo("") != 0)
            {
                // DoOperations执行管理操作
                if (!DoOperations(forum, admininfo, config.Reasonpm))
                    return;
            }

            if (action.CompareTo("moderate") != 0)
            {
                if ("delete,highlight,close".IndexOf(operation) == -1)
                {
                    AddErrLine("你无权操作此功能");
                    return;
                }
                operation = action;
            }
            else
            {
                if (operation.CompareTo("") == 0)
                    operation = DNTRequest.GetString("operat");

                if (operation.CompareTo("") == 0)
                {
                    AddErrLine("您没有选择商品或相应的管理操作,请返回修改");
                    return;
                }
            }

            if (!BindTitle())
                return;
        }

        private bool BindTitle()
        {
            switch (operation)
            {
                case "delete":
                    operationtitle = "删除商品";
                    break;
                case "highlight": //设置高亮
                    operationtitle = "高亮显示";
                    donext = 1;
                    break;
                case "close":
                    operationtitle = "关闭商品";
                    donext = 1;
                    break;
                case "movecategory":
                    operationtitle = "移动商品";
                    donext = 1;
                    break;
                default:
                    operationtitle = "无效引用页";
                    break;
            }
            return true;
        }


        private bool DoOperations(ForumInfo forum, AdminGroupInfo admininfo, int reasonpm)
        {
            string operationName = "";
            string next = DNTRequest.GetFormString("next");
            string referer = DNTRequest.GetFormString("referer");

            DataTable dt = null;

            #region DoOperation

            string reason = DNTRequest.GetString("reason");
            if (operation != "identify")
            {
                if (reason.Equals(""))
                {
                    AddErrLine("操作原因不能为空");
                    return false;
                }
                else
                {
                    if (reason.Length > 200)
                    {
                        AddErrLine("操作原因不能多于200个字符");
                        return false;
                    }
                }
            }

            if ("delete,highlight,close,movecategory".IndexOf(operation) == -1)
            {
                AddErrLine("未知的操作参数");
                return false;
            }
            //执行提交操作
            if (next.Trim() != "")
                referer = string.Format("goodsadmin.aspx?action={0}&categoryid={1}&goodsid={2}", next, categoryid, goodslist);
            else
                referer = string.Format(base.ShowGoodsListAspxRewrite(categoryid, 1));

            #region switch operation

            switch (operation)
            {
                case "delete":
                    operationName = "删除商品";
                    if (!DoDeleteOperation(forum))
                        return false;
                    break;
                case "highlight": //设置高亮
                    operationName = "设置高亮";
                    if (!DoHighlightOperation())
                        return false;
                    break;
                case "close":
                    operationName = "关闭商品/取消";
                    if (!DoCloseOperation())
                        return false;
                    break;
                case "movecategory":
                    operationName = "移动商品";
                    if (!DoMoveOperation())
                        return false;
                    break;
                default:
                    operationName = "未知操作";
                    break;
            }

            #endregion

            if (next.CompareTo("") == 0)
                AddMsgLine("管理操作成功,现在将转入商品列表");
            else
                AddMsgLine("管理操作成功,现在将转入后续操作");

            dt = Goods.GetGoodsList(goodslist);
            if (config.Modworkstatus == 1)
            {
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdminModeratorLogs.InsertLog(this.userid.ToString(), username, usergroupid.ToString(),
                                                     this.usergroupinfo.Grouptitle, Utils.GetRealIP(),
                                                     Utils.GetDateTime(), this.forumid.ToString(), this.forumname,
                                                     dr["goodsid"].ToString(), dr["title"].ToString(), operationName,
                                                     reason);

                        if (reasonpm == 1)
                        {
                            int posterid = Utils.StrToInt(dr["selleruid"], -1);
                            if (posterid != -1) //是游客，管理操作就不发短消息了
                            {
                                if (PrivateMessages.GetPrivateMessageCount(posterid, -1) <
                                    UserGroups.GetUserGroupInfo(Users.GetShortUserInfo(posterid).Groupid).Maxpmnum)
                                {
                                    PrivateMessageInfo __privatemessageinfo = new PrivateMessageInfo();

                                    string curdatetime = Utils.GetDateTime();
                                    // 收件箱
                                    __privatemessageinfo.Message = 
                                                 Utils.HtmlEncode(
                                                         string.Format(
                                                                    "这是由论坛系统自动发送的通知短消息。\r\n以下您所发表的商品被 {0} {1} 执行 {2} 操作。\r\n\r\n商品: {3} \r\n操作理由: {4}\r\n\r\n如果您对本管理操作有异议，请与我取得联系。",
                                                                    Utils.RemoveHtml(this.usergroupinfo.Grouptitle), username,
                                                                    operationName, dr["title"].ToString().Trim(),
                                                                    reason));
                                    __privatemessageinfo.Subject = Utils.HtmlEncode("您发表的商品被执行管理操作");
                                    __privatemessageinfo.Msgto = dr["seller"].ToString();
                                    __privatemessageinfo.Msgtoid = posterid;
                                    __privatemessageinfo.Msgfrom = username;
                                    __privatemessageinfo.Msgfromid = userid;
                                    __privatemessageinfo.New = 1;
                                    __privatemessageinfo.Postdatetime = curdatetime;
                                    __privatemessageinfo.Folder = 0;
                                    PrivateMessages.CreatePrivateMessage(__privatemessageinfo, 0);
                                }
                            }
                        }
                    }
                    dt.Dispose();
                }
            }


            //执行完某一操作后转到后续操作
            SetUrl(referer);
            if (next != string.Empty)
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + referer, false);
            else
                AddScript("window.setTimeout('redirectURL()', 2000);function redirectURL() {window.location='" + referer + "';}");

            SetShowBackLink(false);

            #endregion

            return true;
        }
        
        #region Operations
        
        private bool DoCloseOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有关闭商品的权限", usergroupinfo.Grouptitle));
                return false;
            }
            close = 1;
          
            int reval = Goods.SetClose(goodslist, short.Parse(close.ToString()));
            if (reval < 1)
            {
                AddErrLine("要(关闭)的商品未找到");
                return false;
            }
            return true;
        }

        #region 注释代码段
        private bool DoHighlightOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有设置高亮的权限", usergroupinfo.Grouptitle));
                return false;
            }
            highlight_color = DNTRequest.GetFormString("highlight_color");
            highlight_style_b = DNTRequest.GetFormString("highlight_style_b");
            highlight_style_i = DNTRequest.GetFormString("highlight_style_i");
            highlight_style_u = DNTRequest.GetFormString("highlight_style_u");

            string highlightStyle = "";

            //加粗
            if (!highlight_style_b.Equals(""))
                highlightStyle = highlightStyle + "font-weight:bold;";

            //加斜
            if (!highlight_style_i.Equals(""))
                highlightStyle = highlightStyle + "font-style:italic;";

            //加下划线
            if (!highlight_style_u.Equals(""))
                highlightStyle = highlightStyle + "text-decoration:underline;";

            //设置颜色
            if (!highlight_color.Equals(""))
                highlightStyle = highlightStyle + "color:" + highlight_color + ";";

            if (highlight == -1)
            {
                AddErrLine("您没有选择字体样式及颜色");
                return false;
            }

            Goods.SetHighlight(goodslist, highlightStyle);
            return true;
        }

        
     
        #endregion

        private bool DoDeleteOperation(ForumInfo forum)
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有删除的权限", usergroupinfo.Grouptitle));
                return false;
            }
            Goods.DeleteGoods(goodslist, byte.Parse(forum.Recyclebin.ToString()), DNTRequest.GetInt("reserveattach", 0) == 1);
            return true;
        }

        private bool DoMoveOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有移动商品的权限", usergroupinfo.Grouptitle));
                return false;
            }

            if (Utils.StrIsNullOrEmpty(goodslist) || !Utils.IsNumericArray(goodslist.Split(',')))
            {
                AddErrLine(string.Format("您提交的商品信息有错误", usergroupinfo.Grouptitle));
                return false;
            }

            int goodscategoryid = DNTRequest.GetInt("goodscategoryid", 0);
            if (goodscategoryid <= 0)
            {
                AddErrLine(string.Format("您选择的商品分类无效", usergroupinfo.Grouptitle));
                return false;               
            }
            //当商品分类未发生变化时
            if (goodscategoryid == DNTRequest.GetInt("categoryid", 0))
            {
                AddErrLine(string.Format("您未选择新的商品分类", usergroupinfo.Grouptitle));
                return false;            
            }

            Goodsinfo[]  goodsinfoArray = Goods.DTO.GetGoodsInfoArray(Goods.GetGoodsList(goodslist));
            int oldgoodscategoryid = 0;
            string oldparentcategorylist = "";
            foreach (Goodsinfo goodsinfo in goodsinfoArray)
            {
                oldgoodscategoryid = goodsinfo.Categoryid;//设置参数值
                oldparentcategorylist = goodsinfo.Parentcategorylist;//设置参数值
                goodsinfo.Categoryid = goodscategoryid;//绑定新的商品分类
                goodsinfo.Parentcategorylist = GoodsCategories.GetParentCategoryList(goodsinfo.Categoryid);//绑定新的商品分类
                Goods.UpdateGoods(goodsinfo, oldgoodscategoryid, oldparentcategorylist);
            }
          
            return true;
        }

        

        private bool DoIndentifyOperation()
        {
            int identify = DNTRequest.GetInt("selectidentify", 0);

            if (identify > 0 || identify == -1)
            {
                TopicAdmins.IdentifyTopic(goodslist, identify);
                return true;
            }
            else
            {
                AddErrLine("错误的参数");
                return false;
            }
        }

        #endregion

    }
}
