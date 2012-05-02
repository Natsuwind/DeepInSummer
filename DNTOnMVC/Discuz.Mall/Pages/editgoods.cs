using System;
using System.Data;
using System.Text;
using System.IO;
using System.Collections;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Mall.Data;
using Discuz.Config;
using Discuz.Mall;
using Discuz.Plugin.Album;


namespace Discuz.Mall.Pages
{
    /// <summary>
    /// 编辑商品
    /// </summary>
    public class editgoods : PageBase
    {

        #region 页面变量
        /// <summary>
        /// 商品信息
        /// </summary>
        public Goodsinfo goodsinfo = new Goodsinfo();
        /// <summary>
        /// 商品分类信息
        /// </summary>
        public Goodscategoryinfo goodscategoryinfo = new Goodscategoryinfo();
        /// <summary>
        /// 当前商品的TAG
        /// </summary>
        public string goodstags = "";
        /// <summary>
        /// 所属版块名称
        /// </summary>
        public string forumname;
        /// <summary>
        /// 所属板块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 发布商品内容
        /// </summary>
        public string message;
        /// <summary>
        /// 是否允许发布商品
        /// </summary>
        public bool allowpostgoods;
        /// <summary>
        /// 表情Javascript数组
        /// </summary>
        public string smilies;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav;
        /// <summary>
        /// 编辑器自定义按钮
        /// </summary>
        public string customeditbuttons;
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
        /// 是否允许 [img] 标签
        /// </summary>
        public int allowimg;
        /// <summary>
        /// 是否受发帖灌水限制
        /// </summary>
        public int disablepost;
        /// <summary>
        /// 允许的附件类型和大小数组
        /// </summary>
        public string attachextensions;
        /// <summary>
        /// 允许的附件类型
        /// </summary>
        public string attachextensionsnosize;
        /// <summary>
        /// 今天可上传附件大小
        /// </summary>
        public int attachsize;
        /// <summary>
        /// 积分策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo;
        /// <summary>
        /// 所属版块信息
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// 表情列表
        /// </summary>
        public DataTable smilietypes;
        /// <summary>
        /// 相册列表
        /// </summary>
        public DataTable albumlist;
        /// <summary>
        /// 是否允许上传附件
        /// </summary>
        public bool canpostattach;
        /// <summary>
        /// 是否允许同时发布到相册
        /// </summary>
        public bool caninsertalbum;
        /// <summary>
        /// 交易积分
        /// </summary>
        public int creditstrans;
        /// <summary>
        /// 是否允许Html标题
        /// </summary>
        public bool canhtmltitle = false;
        /// <summary>
        /// 第一页表情的JSON
        /// </summary>
        public string firstpagesmilies = string.Empty;
        /// <summary>
        /// 发布商品用户的个人空间Id
        /// </summary>
        public int spaceid = 0;
        /// <summary>
        /// 本版是否可用Tag
        /// </summary>
        public bool enabletag = false;
        /// <summary>
        /// 有效期,商品信息在网上发布的时间
        /// </summary>
        public string expiration = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
        /// <summary>
        /// 获取当前服务器时间距"1970-1-1"的毫秒数
        /// </summary>
        public string serverdatetime = DateTime.Now.Subtract(TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalMilliseconds.ToString();
        /// <summary>
        /// 当前帖的Html标题
        /// </summary>
        public string htmltitle = "";
        /// <summary>
        /// 是否显示下载链接
        /// </summary>		
        public bool allowviewattach = false;
        /// <summary>
        /// 附件列表
        /// </summary>
        public GoodsattachmentinfoCollection attachmentlist = new GoodsattachmentinfoCollection();
        /// <summary>
        /// 开启html功能
        /// </summary>
        public int htmlon = 0;

        public string action = DNTRequest.GetQueryString("action");

        public string special = DNTRequest.GetString("type").ToLower();

        public bool isfirstpost = false;

        public string editorid = "e";

        public char comma = ',';

        public string topictypeselectoptions = "";

        public bool adveditor = true;
        #endregion

        AlbumPluginBase apb = AlbumPluginProvider.GetInstance();

        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //未启用交易模式
            {
                AddErrLine("系统未开启交易模式, 当前页面暂时无法访问!");
                return;
            }

            #region 临时帐号发帖
            //int realuserid = -1;
            //string tempusername = DNTRequest.GetString("tempusername");
            //if (tempusername != "" && tempusername != username)
            //{
            //    string temppassword = DNTRequest.GetString("temppassword");
            //    int question = DNTRequest.GetInt("question", 0);
            //    string answer = DNTRequest.GetString("answer");
            //    realuserid = Users.CheckTempUserInfo(tempusername, temppassword, question, answer);
               
            //    if (realuserid == -1)
            //    {
            //        AddErrLine("临时帐号登录失败，无法继续发帖。");
            //        return;
            //    }
            //    else
            //    {
            //        userid = realuserid;
            //        username = tempusername;
            //        usergroupinfo = UserGroups.GetUserGroupInfo(Users.GetShortUserInfo(userid).Groupid);
            //        usergroupid = usergroupinfo.Groupid;
            //        useradminid = Users.GetShortUserInfo(userid).Adminid;
            //    }
            //}
            #endregion

            canhtmltitle = true;
            firstpagesmilies = Caches.GetSmiliesFirstPageCache();
   
            //内容设置为空;  
            message = "";

            int goodsid = DNTRequest.GetInt("goodsid", 0);
            // 如果商品交易日志不正确
            if (goodsid <= 0)
            {
                AddErrLine("错误的商品ID.");
                return;
            }

            goodsinfo = Goods.GetGoodsInfo(goodsid);
            if (goodsinfo == null || goodsinfo.Goodsid <= 0) 
            {
                AddErrLine("错误的商品ID.");
                return;
            }
                
            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(goodsinfo.Categoryid);
            if (goodscategoryinfo == null && goodscategoryinfo.Fid <= 0)
            {
                goodscategoryinfo = new Goodscategoryinfo();
                goodscategoryinfo.Categoryid = -1;
            }

            attachmentlist = GoodsAttachments.GetGoodsAttachmentsByGoodsid(goodsinfo.Goodsid);

            message = goodsinfo.Message;

            // 如果商品交易日志不正确
            if (goodsinfo.Selleruid != userid)
            {
                AddErrLine("您不是当前商品的卖家!");
                return;
            }
            allowpostgoods = true;

            if (config.Enablemall == 1) //开启普通模式
            {
                forumid = GoodsCategories.GetCategoriesFid(goodsinfo.Categoryid);
                allowpostgoods = false;
                forumnav = "";
                if (forumid == -1)
                {
                    if (userid == goodsinfo.Selleruid)
                    {
                        forum = new ForumInfo();
                        forum.Attachextensions = "";
                        forum.Password = "";
                        forum.Permuserlist = "";
                    }
                    else
                    {
                        AddErrLine("错误的商品分类ID");
                        return;
                    }
                }
                else
                {
                    forum = Forums.GetForumInfo(forumid);
                    if (forum == null || forum.Layer == 0)
                    {
                        AddErrLine("错误的商品分类ID");
                        return;
                    }
                    if (forum.Istrade <= 0)
                    {
                        AddErrLine("当前版块不允许编辑商品");
                        return;
                    }

                    forumname = forum.Name;
                    pagetitle = Utils.RemoveHtml(forum.Name);
                    forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
                    enabletag = (config.Enabletag & forum.Allowtag) == 1;
                }
            }
            else if (config.Enablemall == 2) //当为高级模式时
            {
                pagetitle = "编辑商品";
                forumnav = "";
                enabletag = true;
                forum = new ForumInfo();
                forum.Allowsmilies = 1;
                forum.Allowbbcode = 1;
            }

            //得到用户可以上传的文件类型
            StringBuilder sbAttachmentTypeSelect = new StringBuilder();
            if (!usergroupinfo.Attachextensions.Trim().Equals(""))
            {
                sbAttachmentTypeSelect.Append("[id] in (");
                sbAttachmentTypeSelect.Append(usergroupinfo.Attachextensions);
                sbAttachmentTypeSelect.Append(")");
            }

            if (config.Enablemall == 1) //开启普通模式
            {
                if (!forum.Attachextensions.Equals(""))
                {
                    if (sbAttachmentTypeSelect.Length > 0)
                    {
                        sbAttachmentTypeSelect.Append(" AND ");
                    }
                    sbAttachmentTypeSelect.Append("[id] in (");
                    sbAttachmentTypeSelect.Append(forum.Attachextensions);
                    sbAttachmentTypeSelect.Append(")");
                }
            }
            attachextensions = Attachments.GetAttachmentTypeArray(sbAttachmentTypeSelect.ToString());
            attachextensionsnosize = Attachments.GetAttachmentTypeString(sbAttachmentTypeSelect.ToString());

            //得到今天允许用户上传的附件总大小(字节)
            int MaxTodaySize = 0;
            if (userid > 0)
            {
                MaxTodaySize = Attachments.GetUploadFileSizeByuserid(userid);		//今天已上传大小
            }
            attachsize = usergroupinfo.Maxsizeperday - MaxTodaySize;//今天可上传得大小
            parseurloff = 0;
            bbcodeoff = 1;

            if (config.Enablemall == 1) //开启普通模式
            {
                smileyoff = 1 - forum.Allowsmilies;
                if (forum.Allowbbcode == 1 && usergroupinfo.Allowcusbbcode == 1)
                    bbcodeoff = 0;

                allowimg = forum.Allowimgcode;

                if (forum.Password != "" && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid.ToString() + "password"))
                {
                    AddErrLine("本版块被管理员设置了密码");
                    SetBackLink(base.ShowForumAspxRewrite(forumid, 0));
                    return;
                }

                if (!Forums.AllowViewByUserId(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
                {
                    if (forum.Viewperm == null || forum.Viewperm == string.Empty)//当板块权限为空时，按照用户组权限
                    {
                        if (useradminid != 1 && (usergroupinfo.Allowvisit != 1 || usergroupinfo.Allowtrade != 1))
                        {
                            AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该版块的权限");
                            return;
                        }
                    }
                    else//当板块权限不为空，按照板块权限
                    {
                        if (!Forums.AllowView(forum.Viewperm, usergroupid))
                        {
                            AddErrLine("您没有浏览该商品分类的权限");
                            return;
                        }
                    }
                }

                //当前用户是卖家时
                if (goodsinfo.Selleruid == userid)
                {
                    //当前用户是否有允许下载附件权限
                    if (Forums.AllowGetAttachByUserID(forum.Permuserlist, userid))
                        allowviewattach = true;
                    else
                    {
                        if (forum.Getattachperm == null || forum.Getattachperm == string.Empty)//权限设置为空时，根据用户组权限判断
                        {
                            // 验证用户是否有有允许下载附件权限
                            if (usergroupinfo.Allowgetattach == 1)
                                allowviewattach = true;
                        }
                        else if (Forums.AllowGetAttach(forum.Getattachperm, usergroupid))
                            allowviewattach = true;
                    }

                    //是否有上传附件的权限
                    if (Forums.AllowPostAttachByUserID(forum.Permuserlist, userid))
                        canpostattach = true;
                    else
                    {
                        if (forum.Postattachperm == "")
                        {
                            if (usergroupinfo.Allowpostattach == 1)
                                canpostattach = true;
                        }
                        else
                        {
                            if (Forums.AllowPostAttach(forum.Postattachperm, usergroupid))
                            {
                                canpostattach = true;
                            }
                        }
                    }
                }
            }
            else if (config.Enablemall == 2) //当为高级模式时
            {
                canpostattach = true;
                allowimg = 1;
                smileyoff = 0;
            }

            ShortUserInfo user = Users.GetShortUserInfo(userid);
            if (canpostattach && user != null && apb != null && config.Enablealbum == 1 &&
                (UserGroups.GetUserGroupInfo(user.Groupid).Maxspacephotosize - apb.GetPhotoSizeByUserid(userid) > 0))
            {
                caninsertalbum = true;
                albumlist = apb.GetSpaceAlbumByUserId(userid);
            }
            else
                caninsertalbum = false;

            if (Topics.GetMagicValue(goodsinfo.Magic, MagicType.HtmlTitle) == 1)
            {
                htmltitle = Goods.GetHtmlTitle(goodsinfo.Goodsid).Replace("\"", "\\\"").Replace("'", "\\'");
            }

            
            if (enabletag && Topics.GetMagicValue(goodsinfo.Magic, MagicType.TopicTag) == 1)
            {
                foreach (TagInfo tag in GoodsTags.GetTagsListByGoods(goodsinfo.Goodsid))
                {
                    if (tag.Orderid > -1)
                        goodstags += string.Format(" {0}", tag.Tagname);
                }
                goodstags = goodstags.Trim();
            }


            // 如果是受灌水限制用户, 则判断是否是灌水
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            disablepost = (admininfo != null) ? admininfo.Disablepostctrl : 0;
            creditstrans = Scoresets.GetCreditsTrans();
            userextcreditsinfo = Scoresets.GetScoreSet(creditstrans);
            if (userid > 0)
                spaceid = Users.GetShortUserInfo(userid).Spaceid;

            //如果不是提交...
            if (!ispost)
            {
                AddLinkCss(BaseConfigs.GetForumPath + "templates/" + templatepath + "/editor.css", "css");
                smilies = Caches.GetSmiliesCache();
                smilietypes = Caches.GetSmilieTypesCache();
                customeditbuttons = Caches.GetCustomEditButtonList();
            }
            else
            {
                SetBackLink(string.Format("postgoods.aspx?forumid={0}&restore=1", forumid));

                string postmessage = DNTRequest.GetString("message");

                ForumUtils.WriteCookie("postmessage", postmessage);

                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                if (DNTRequest.GetString("title").Trim().Equals(""))
                    AddErrLine("商品标题不能为空");
                else if (DNTRequest.GetString("title").IndexOf("　") != -1)
                    AddErrLine("商品标题不能包含全角空格符");
                else if (DNTRequest.GetString("title").Length > 60)
                    AddErrLine("商品标题最大长度为60个字符,当前为 " + DNTRequest.GetString("title").Length.ToString() + " 个字符");

                if (postmessage.Equals("") || postmessage.Replace("　", "").Equals(""))
                    AddErrLine("商品内容不能为空");

                if (admininfo != null && admininfo.Disablepostctrl != 1)
                {
                    if (postmessage.Length < config.Minpostsize)
                        AddErrLine("您发表的内容过少, 系统设置要求商品内容不得少于 " + config.Minpostsize + " 字多于 " + config.Maxpostsize + " 字");
                    else if (postmessage.Length > config.Maxpostsize)
                        AddErrLine("您发表的内容过多, 系统设置要求商品内容不得少于 " + config.Minpostsize + " 字多于 " + config.Maxpostsize + " 字");
                }

                if (IsErr())
                    return;

                // 如果用户上传了附件,则检测用户是否有上传附件的权限
                if (ForumUtils.IsPostFile())
                {
                    if (Attachments.GetAttachmentTypeArray(sbAttachmentTypeSelect.ToString()).Trim() == "")
                        AddErrLine("系统不允许上传附件");

                    if (config.Enablemall == 1) //开启普通模式
                    {
                        if (!Forums.AllowPostAttachByUserID(forum.Permuserlist, userid))
                        {
                            if (!Forums.AllowPostAttach(forum.Postattachperm, usergroupid))
                                AddErrLine("您没有在该版块上传附件的权限");
                            else if (usergroupinfo.Allowpostattach != 1)
                                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有上传附件的权限", usergroupinfo.Grouptitle));
                        }
                    }
                }

                if (IsErr())
                    return;

                int iconid = DNTRequest.GetInt("iconid", 0);
                if (iconid > 15 || iconid < 0)
                    iconid = 0;

                string curdatetime = Utils.GetDateTime();

                goodsinfo.Shopid = 0;
                //商品分类原值
                int oldgoodscategoryid = goodsinfo.Categoryid;
                //商品父分类原值
                string oldparentcategorylist = goodsinfo.Parentcategorylist;

                //当商品分类发生变化时
                if (DNTRequest.GetInt("goodscategoryid", 0) != 0 && goodsinfo.Categoryid != DNTRequest.GetInt("goodscategoryid", 0))
                {
                    goodsinfo.Categoryid = DNTRequest.GetInt("goodscategoryid", 0);
                    goodsinfo.Parentcategorylist = GoodsCategories.GetParentCategoryList(goodsinfo.Categoryid);
                }
                
                goodsinfo.Recommend = DNTRequest.GetString("recommend") == "on" ? 1 : 0;
                goodsinfo.Discount = DNTRequest.GetInt("discount", 0);
                goodsinfo.Selleruid = userid;
                goodsinfo.Seller = username;
                goodsinfo.Account = DNTRequest.GetString("account");
                goodsinfo.Price = Convert.ToDecimal(DNTRequest.GetFormFloat("price", 1).ToString());
                goodsinfo.Amount = DNTRequest.GetInt("amount", 0);
                goodsinfo.Quality = DNTRequest.GetInt("quality", 0);
                if(DNTRequest.GetInt("locus_2", 0) != 0)
                {
                    goodsinfo.Lid = DNTRequest.GetInt("locus_2", 0);
                    goodsinfo.Locus = Locations.GetLocusByLID(goodsinfo.Lid);
                }
                goodsinfo.Transport = DNTRequest.GetInt("transport", 0);
                if (goodsinfo.Transport != 0)
                {
                    goodsinfo.Ordinaryfee = Convert.ToDecimal(DNTRequest.GetFormFloat("postage_mail", 0).ToString());
                    goodsinfo.Expressfee = Convert.ToDecimal(DNTRequest.GetFormFloat("postage_express", 0).ToString());
                    goodsinfo.Emsfee = Convert.ToDecimal(DNTRequest.GetFormFloat("postage_ems", 0).ToString());
                }

                goodsinfo.Itemtype = DNTRequest.GetInt("itemtype", 0);

                DateTime dateline;
                switch (DNTRequest.GetInt("_now", 0))
                {
                    case 1: dateline = Convert.ToDateTime(string.Format("{0} {1}:{2}:00", DNTRequest.GetString("_date"), DNTRequest.GetInt("_hour", 0), DNTRequest.GetInt("_minute", 0))); break; //设定
                    case 2: dateline = Convert.ToDateTime("1900-01-01 00:00:00"); break; //返回100年之后的日期作为"暂不设置"
                    default: dateline = DateTime.Now; break; //立即
                }

                goodsinfo.Dateline = dateline;
                goodsinfo.Expiration = Convert.ToDateTime(DNTRequest.GetString("expiration"));
                goodsinfo.Lastbuyer = "";
                goodsinfo.Lastupdate = DateTime.Now;
                goodsinfo.Totalitems = 0;
                goodsinfo.Tradesum = 0;
                goodsinfo.Closed = 0;
                goodsinfo.Aid = 0;

                int displayorder = goodsinfo.Displayorder;
                goodsinfo.Displayorder = DNTRequest.GetString("displayorder") == "on" ? 0 : -3;

                if (config.Enablemall == 1) //当为版块交易帖是时
                {
                    if (forum.Modnewposts == 1 && useradminid != 1)
                    {
                        if (useradminid > 1)
                        {
                            if (disablepost != 1)
                                goodsinfo.Displayorder = -2;
                        }
                        else
                            goodsinfo.Displayorder = -2;
                    }
                }

                goodsinfo.Costprice = Convert.ToDecimal(DNTRequest.GetFormFloat("costprice", 1).ToString());
                goodsinfo.Invoice = DNTRequest.GetInt("invoice", 0);
                goodsinfo.Repair = DNTRequest.GetInt("repair", 0);
                if (useradminid == 1)
                    goodsinfo.Message = Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
                else
                    goodsinfo.Message = Utils.HtmlEncode(postmessage);
                
                goodsinfo.Otherlink = "";
                int readperm = DNTRequest.GetInt("readperm", 0);
                goodsinfo.Readperm = readperm > 255 ? 255 : readperm;
                goodsinfo.Tradetype = DNTRequest.GetInt("tradetype", 0);

                if (goodsinfo.Tradetype == 1 && Utils.StrIsNullOrEmpty(goodsinfo.Account)) //当为支付宝在线支付方式下,如果"支付宝账户"为空时
                {
                    AddErrLine("请输入支付宝帐号信息。");
                    return;
                }
                
                goodsinfo.Smileyoff = smileyoff;
                if (smileyoff == 0 && forum.Allowsmilies == 1)
                    goodsinfo.Smileyoff = Utils.StrToInt(DNTRequest.GetString("smileyoff"), 0);

                goodsinfo.Bbcodeoff = 1;
                if (usergroupinfo.Allowcusbbcode == 1 && forum.Allowbbcode == 1)
                    goodsinfo.Bbcodeoff = Utils.StrToInt(DNTRequest.GetString("bbcodeoff"), 0);

                goodsinfo.Parseurloff = Utils.StrToInt(DNTRequest.GetString("parseurloff"), 0);

                if (useradminid == 1)
                    goodsinfo.Title = Utils.HtmlEncode(DNTRequest.GetString("title"));
                else
                    goodsinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("title")));
               
                string htmltitle = DNTRequest.GetString("htmltitle").Trim();
                if (htmltitle != string.Empty && Utils.HtmlDecode(htmltitle).Trim() != goodsinfo.Title)
                {
                    goodsinfo.Magic = 11000;
                    //按照  附加位/htmltitle(1位)/magic(3位)/以后扩展（未知位数） 的方式来存储
                    //例： 11001
                }

                //标签(Tag)操作                
                string tags = DNTRequest.GetString("tags").Trim();
                string[] tagsArray = null;
                if (enabletag && tags != string.Empty)
                {
                    tagsArray = Utils.SplitString(tags, " ", true, 2, 10);
                    if (tagsArray.Length > 0)
                    {
                        if (goodsinfo.Magic == 0)
                            goodsinfo.Magic = 10000;

                        goodsinfo.Magic = Utils.StrToInt(goodsinfo.Magic.ToString() + "1", 0);
                    }
                }

                Goods.UpdateGoods(goodsinfo, oldgoodscategoryid, oldparentcategorylist);

                if (displayorder != goodsinfo.Displayorder) //当发生变化时
                {
                    if (displayorder < 0 && goodsinfo.Displayorder > 0) //该商品转为上架
                        DbProvider.GetInstance().UpdateCategoryGoodsCounts(goodsinfo.Categoryid, goodsinfo.Parentcategorylist, 1);
                    else if (displayorder >= 0 && goodsinfo.Displayorder < 0) //该商品转为下架(或进入回收站/待审核状态)
                        DbProvider.GetInstance().UpdateCategoryGoodsCounts(goodsinfo.Categoryid, goodsinfo.Parentcategorylist, -1);
                }

                //保存htmltitle
                if (canhtmltitle && htmltitle != string.Empty && htmltitle != goodsinfo.Title)
                    Goods.WriteHtmlSubjectFile(htmltitle, goodsinfo.Goodsid);

                if (enabletag && tagsArray != null && tagsArray.Length > 0)
                {
                    DbProvider.GetInstance().CreateGoodsTags(string.Join(" ", tagsArray), goodsinfo.Goodsid, userid, curdatetime);
                    GoodsTags.WriteGoodsTagsCacheFile(goodsinfo.Goodsid);
                }

                StringBuilder sb = new StringBuilder();
                sb.Remove(0, sb.Length);

                //编辑帖子时如果进行了批量删除附件
                string delAttId = DNTRequest.GetFormString("deleteaid");
                if (delAttId != string.Empty)
                {
                    if (Utils.IsNumericList(delAttId))//如果要删除的附件ID列表为数字数组
                        GoodsAttachments.DeleteGoodsAttachment(delAttId);
                }
                //编辑帖子时如果进行了更新附件操作
                string updatedAttId = DNTRequest.GetFormString("attachupdatedid");//被更新的附件Id列表
                string updateAttId = DNTRequest.GetFormString("attachupdateid");//所有已上传的附件Id列表
                string[] descriptionArray = DNTRequest.GetFormString("attachupdatedesc").Split(',');//所有已上传的附件的描述
                string[] readpermArray = DNTRequest.GetFormString("attachupdatereadperm").Split(',');//所有已上传得附件的阅读权限

                ArrayList updateAttArrayList = new ArrayList();
                if (updateAttId != string.Empty)
                {
                    foreach (string s in updateAttId.Split(','))
                    {
                        if (!Utils.InArray(s, delAttId, ","))//已上传的附件Id不在被删除的附件Id列表中时
                            updateAttArrayList.Add(s);
                    }
                }

                string[] updateAttArray = (string[])updateAttArrayList.ToArray(typeof(string));

                if (updateAttId != string.Empty)//原来有附件
                {
                    int watermarkstate = config.Watermarkstatus;

                    if (forum.Disablewatermark == 1)
                        watermarkstate = 0;

                    string[] updatedAttArray = updatedAttId.Split(',');

                    string filekey = "attachupdated";

                    //保存新的文件
                    Goodsattachmentinfo[] attArray = Discuz.Mall.MallUtils.SaveRequestFiles(
                                                    goodsinfo.Categoryid, config.Maxattachments + updateAttArray.Length,
                                                    usergroupinfo.Maxsizeperday, usergroupinfo.Maxattachsize, MaxTodaySize,
                                                    attachextensions, watermarkstate, config, filekey);

                    if (Utils.IsNumericArray(updateAttArray))
                    {
                        for (int i = 0; i < updateAttArray.Length; i++) //遍历原来所有附件
                        {
                            string attachmentId = updateAttArray[i];
                            if (Utils.InArray(attachmentId, updatedAttArray)) //附件文件被更新
                            {
                                if (Utils.InArray(attachmentId, delAttId, ","))//附件进行了删除操作, 则不操作此附件,即使其也被更新
                                {
                                    continue;
                                }
                                //更新附件
                                int attachmentUpdatedIndex = GetAttachmentUpdatedIndex(attachmentId, updatedAttArray);//获取此次上传的被更新附件在数组中的索引
                                if (attachmentUpdatedIndex > -1)//附件索引存在
                                {
                                    if (attArray[attachmentUpdatedIndex].Sys_noupload.Equals(string.Empty)) //由此属性为空可以判断上传成功
                                    {
                                        //获取将被更新的附件信息
                                        Goodsattachmentinfo attachmentInfo =
                                            GoodsAttachments.GetGoodsAttachmentsByAid(Utils.StrToInt(updatedAttArray[attachmentUpdatedIndex], 0));
                                        if (attachmentInfo != null)
                                        {
                                            if (attachmentInfo.Filename.Trim().ToLower().IndexOf("http") < 0)
                                            {
                                                //删除原来的文件
                                                File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" +
                                                                     attachmentInfo.Filename));
                                            }

                                            //记住Aid以便稍后更新
                                            attArray[attachmentUpdatedIndex].Aid = attachmentInfo.Aid;
                                            attArray[attachmentUpdatedIndex].Description = descriptionArray[i];
                                            int att_readperm = Utils.StrToInt(readpermArray[i], 0);
                                            att_readperm = att_readperm > 255 ? 255 : att_readperm;
                                            attArray[attachmentUpdatedIndex].Readperm = att_readperm;
                                            attArray[attachmentUpdatedIndex].Categoryid = attachmentInfo.Categoryid;
                                            attArray[attachmentUpdatedIndex].Goodscount = attachmentInfo.Goodscount;
                                            attArray[attachmentUpdatedIndex].Goodsid = attachmentInfo.Goodsid;

                                            GoodsAttachments.SaveGoodsAttachment(attArray[attachmentUpdatedIndex]);
                                        }
                                    }
                                    else //上传失败的附件，稍后提示
                                    {
                                        sb.Append("<tr><td align=\"left\">");
                                        sb.Append(attArray[attachmentUpdatedIndex].Attachment);
                                        sb.Append("</td>");
                                        sb.Append("<td align=\"left\">");
                                        sb.Append(attArray[attachmentUpdatedIndex].Sys_noupload);
                                        sb.Append("</td></tr>");
                                    }
                                }
                            }
                            else //仅修改了阅读权限和描述等
                            {
                                if (Utils.InArray(updateAttArray[i], delAttId, ","))
                                {
                                    continue;
                                }
                                if ((attachmentlist[i].Readperm.ToString() != readpermArray[i]) ||
                                    (attachmentlist[i].Description.Trim() != descriptionArray[i]))
                                {
                                    int att_readperm = Utils.StrToInt(readpermArray[i], 0);
                                    att_readperm = att_readperm > 255 ? 255 : att_readperm;
                                    GoodsAttachments.SaveGoodsAttachment(Utils.StrToInt(updateAttArray[i], 0), att_readperm,
                                                                 descriptionArray[i]);
                                }
                            }
                        }
                    }
                }

                int watermarkstatus = config.Watermarkstatus;
                if (forum.Disablewatermark == 1)
                {
                    watermarkstatus = 0;
                }
                Goodsattachmentinfo[] attachmentinfo = Discuz.Mall.MallUtils.SaveRequestFiles(forumid, config.Maxattachments, usergroupinfo.Maxsizeperday, usergroupinfo.Maxattachsize, MaxTodaySize, attachextensions, watermarkstatus, config, "postfile");
                if (attachmentinfo != null)
                {
                    if (attachmentinfo.Length > config.Maxattachments)
                    {
                        AddErrLine("系统设置为每个商品附件不得多于" + config.Maxattachments + "个");
                        return;
                    }
                    int errorAttachment = GoodsAttachments.BindAttachment(attachmentinfo, goodsinfo.Goodsid, sb, goodsinfo.Categoryid, userid);
                    int[] aid = GoodsAttachments.CreateAttachments(attachmentinfo);
                    string tempMessage = GoodsAttachments.FilterLocalTags(aid, attachmentinfo, goodsinfo.Message);
                    if (attachmentinfo.Length == (System.Web.HttpContext.Current.Request.Files.Count-2))
                    {
                        goodsinfo.Goodspic = attachmentinfo[0].Filename;
                        goodsinfo.Aid = aid[0];
                    }
                    if (!tempMessage.Equals(goodsinfo.Message))
                        goodsinfo.Message = tempMessage;

                    Goods.UpdateGoods(goodsinfo);

                    UserCredits.UpdateUserExtCreditsByUploadAttachment(userid, aid.Length - errorAttachment);
                }

                //加入相册
                if (config.Enablealbum == 1 && apb != null)
                    sb.Append(apb.CreateAttachment(attachmentinfo, usergroupid, userid, username));

                if (config.Enablemall == 1) //开启普通模式
                {
                    OnlineUsers.UpdateAction(olid, UserAction.PostTopic.ActionID, forumid, forumname, -1, "");                  
                }

                if (sb.Length > 0)
                {
                    SetUrl(base.ShowGoodsAspxRewrite(goodsinfo.Goodsid));
                    SetMetaRefresh(5);
                    SetShowBackLink(true);
                    sb.Insert(0, "<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\"><tr><td colspan=2 align=\"left\"><span class=\"bold\"><nobr>发布商品成功,但以下附件上传失败:</nobr></span><br /></td></tr>");
                    sb.Append("</table>");
                    AddMsgLine(sb.ToString());
                }
                else
                {
                    SetShowBackLink(false);
                    if (config.Enablemall == 1 && forum.Modnewposts == 1 && useradminid != 1)
                    {
                        if (useradminid != 1)
                        {
                            if (disablepost == 1)
                            {
                                if (goodsinfo.Displayorder == -3)
                                {
                                    SetUrl(base.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1));
                                    SetMetaRefresh(5);
                                    AddMsgLine("编辑商品成功, 但未上架. 您可到用户中心进行上架操作!");
                                }
                                else
                                {
                                    SetUrl(base.ShowGoodsAspxRewrite(goodsinfo.Goodsid));
                                    SetMetaRefresh();
                                    AddMsgLine("编辑商品成功, 返回该商品<br />(<a href=\"" + base.ShowGoodsAspxRewrite(goodsinfo.Goodsid) + "\">点击这里返回 " + forumname + "</a>)<br />");
                                }
                            }
                            else
                            {
                                SetUrl(base.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1));
                                SetMetaRefresh();
                                AddMsgLine("编辑商品成功, 但需要经过审核才可以显示. 返回商品列表");
                            }
                        }
                        else
                        {
                            SetUrl(base.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1));
                            SetMetaRefresh();
                            AddMsgLine("发布商品成功, 返回商品列表");
                        }
                    }
                    else
                    {
                        if (goodsinfo.Displayorder == -3)
                        {
                            SetUrl(base.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1));
                            SetMetaRefresh(5);
                            AddMsgLine("编辑商品成功, 但未上架. 您可到用户中心进行上架操作!");
                        }
                        else
                        {
                            SetUrl(base.ShowGoodsAspxRewrite(goodsinfo.Goodsid));
                            SetMetaRefresh();
                            AddMsgLine("编辑商品成功, 返回该商品<br />(<a href=\"" + base.ShowGoodsAspxRewrite(goodsinfo.Goodsid) + "\">点击这里返回</a>)<br />");
                        }
                    }
                }
                ForumUtils.WriteCookie("postmessage", "");
            }

            topicattachscorefield = 0;
        }

        private int GetAttachmentUpdatedIndex(string attachmentId, string[] updatedAttArray)
        {
            for (int i = 0; i < updatedAttArray.Length; i++)
            {
                if (updatedAttArray[i] == attachmentId)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}