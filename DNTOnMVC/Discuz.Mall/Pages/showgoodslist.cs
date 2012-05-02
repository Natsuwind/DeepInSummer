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
    /// ��Ʒ�б�ҳ��
    /// </summary>
    public class showgoodslist : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// �����б�
        /// </summary>
        public GoodsinfoCollection goodslist = new GoodsinfoCollection();
        /// <summary>
        /// ��ǰ��������û��б�
        /// </summary>
        public List<OnlineUserInfo> onlineuserlist; 
        /// <summary>
        /// ����Ϣ�б�
        /// </summary>
        public List<PrivateMessageInfo> pmlist;

        public Goodscategoryinfo goodscategoryinfo;

        /// <summary>
        /// ����ͼ���б�
        /// </summary>
        public string onlineiconlist = "";
        /// <summary>
        /// �����б�
        /// </summary>
        public DataTable announcementlist = new DataTable();
        /// <summary>
        /// ҳ�����ֹ��
        /// </summary>
        public string[] pagewordad;
        /// <summary>
        /// �������
        /// </summary>
        public string doublead = "";
        /// <summary>
        /// �������
        /// </summary>
        public string floatad = "";
        /// <summary>
        /// Silverlight���
        /// </summary>
        public string mediaad = "";
        /// <summary>
        /// ��ǰ�����Ϣ
        /// </summary>
        public ForumInfo forum = new ForumInfo();
        /// <summary>
        /// �û��Ĺ�������Ϣ
        /// </summary>
        public AdminGroupInfo admingroupinfo = new AdminGroupInfo();
        /// <summary>
        /// ��ǰ����������û���
        /// </summary>
        public int forumtotalonline;
        /// <summary>
        /// ��ǰ���������ע���û���
        /// </summary>
        public int forumtotalonlineuser;
        /// <summary>
        /// ��ǰ����������ο���
        /// </summary>
        public int forumtotalonlineguest;
        /// <summary>
        /// ��ǰ������������û���
        /// </summary>
        public int forumtotalonlineinvisibleuser;
        /// <summary>
        /// ��ǰ���ID
        /// </summary>
        public int forumid;
        /// <summary>
        /// ��ǰ�������
        /// </summary>
        public string forumname;
        /// <summary>
        /// �Ӱ����
        /// </summary>
        public int subforumcount;
        /// <summary>
        /// ��̳������Ϣ
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// �Ƿ���ʾ���������ʾ 1Ϊ��ʾ, 0����ʾ
        /// </summary>
        public int showforumlogin;
        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int pageid;
        /// <summary>
        /// ��������
        /// </summary>
        public int goodscount = 0;
        /// <summary>
        /// ��ҳ����
        /// </summary>
        public int pagecount = 1;
        /// <summary>
        /// ��ҳҳ������
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// �����ת����ѡ��
        /// </summary>
        public string forumlistboxoptions;
        /// <summary>
        /// ������ʵİ��ѡ��
        /// </summary>
        public string visitedforumsoptions;
        /// <summary>
        /// �Ƿ�����Rss����
        /// </summary>
        public int forumallowrss;
        /// <summary>
        /// �Ƿ���ʾ�����б�
        /// </summary>
        public bool showforumonline;
        /// <summary>
        /// �Ƿ��ܷ�����������
        /// </summary>
        public int disablepostctrl;
        /// <summary>
        /// �Ƿ����� [img] ��ǩ
        /// </summary>
        public int allowimg;
        /// <summary>
        /// ÿҳ��ʾ��Ʒ��
        /// </summary>
        public int gpp;
        /// <summary>
        /// �Ƿ��ǹ�����
        /// </summary>
        public bool ismoder = false;
        /// <summary>
        /// �Ƿ�����������
        /// </summary>
        public bool canposttopic = false; //�Ƿ��з��������Ȩ��
        /// <summary>
        /// ��̳���������˵�HTML����
        /// </summary>
        public string navhomemenu = "";
        /// <summary>
        /// �Ƿ���ʾ����Ϣ��ʾ
        /// </summary>
        public bool showpmhint = false;
        /// <summary>
        /// �Ƿ���ʾ��Ҫ��¼����ʵĴ�����ʾ
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// ����ʽ
        /// </summary>
        public int order = 1; //�����ֶ�
        /// <summary>
        /// ʱ�䷶Χ
        /// </summary>
        public int cond = 0;
        /// <summary>
        /// ������
        /// </summary>
        public int direct = 1; //������[Ĭ�ϣ�����]
        /// <summary>
        /// ��ǰ�����µ��ӷ���json��ʽ��
        /// </summary>
        public string subcategoriesjson = "";
        /// <summary>
        /// ��Ʒ����Id
        /// </summary>
        public int categoryid = DNTRequest.GetInt("categoryid", 0); //��Ʒ����
        /// <summary>
        /// ��ȡ����ذ�����Ʒ������Ϣ
        /// </summary>
        public string goodscategoryfid = "";
        /// <summary>
        /// ���ڵ���Ϣ(��ʽ: "ʡ,��")
        /// </summary>
        public string locus = "";
        #endregion

        private string condition = ""; //��ѯ����
       

        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //δ���ý���ģʽ
            {
                AddErrLine("ϵͳδ��������ģʽ, ��ǰҳ����ʱ�޷�����!");
                return;
            }
            else
                goodscategoryfid = Discuz.Mall.GoodsCategories.GetGoodsCategoryWithFid();

            forumnav = "";
            forumallowrss = 0;
            if (categoryid <= 0)
            {
                AddErrLine("��Ч����Ʒ����ID");
                return;
            }

            if (config.Enablemall == 2) //�����߼�ģʽ
            {
                AddLinkRss("mallgoodslist.aspx?categoryid=" + categoryid, "��Ʒ�б�");
                AddErrLine("��ǰҳ���ڿ����̳�(�߼�)ģʽ���޷�����, ϵͳ�����ض�����Ʒ�б�ҳ��!");
                return;
            }

            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(categoryid);
            if (goodscategoryinfo != null && goodscategoryinfo.Categoryid > 0)
            {
                forumid = GoodsCategories.GetCategoriesFid(goodscategoryinfo.Categoryid);
            }
            else 
            {
                AddErrLine("��Ч����Ʒ����ID");
                return;
            }

            ///�õ�����б�
            ///ͷ��
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
                AddLinkRss("tools/rss.aspx", "������Ʒ");
                AddErrLine("��Ч����Ʒ����ID");
                return;
            }
            else
            {
                forum = Forums.GetForumInfo(forumid);
                // ����Ƿ���а��������
                if (useradminid > 0)
                    ismoder = Moderators.IsModer(useradminid, userid, forumid);

                #region �������������м���

                string orderStr = "goodsid";

                if (DNTRequest.GetString("search").Trim() != "") //����ָ����ѯ
                {
                    //���ڳ�����Ϣ
                    cond = DNTRequest.GetInt("locus_2", -1);                    
                    if (cond < 1)
                        condition = "";
                    else
                    {
                        locus = Locations.GetLocusByLID(cond);
                        condition = "AND [lid] = " + cond;
                    }

                    //������ֶ�
                    order = DNTRequest.GetInt("order", -1);
                    switch (order)
                    {
                        case 2:
                            orderStr = "expiration"; //������
                            break;
                        case 1:
                            orderStr = "price"; //��Ʒ�۸�
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
                        AddLinkRss("tools/rss.aspx", Utils.EncodeHtml(config.Forumtitle) + " ������Ʒ");

                    AddErrLine("�����ڵ���Ʒ����ID");
                    return;
                }


                //��������ⲿ����ʱ,��ֱ����ת
                if (forum.Redirect != null && forum.Redirect != string.Empty)
                {
                    System.Web.HttpContext.Current.Response.Redirect(forum.Redirect);
                    return;
                }

                if (forum.Istrade <= 0)
                {
                    AddErrLine("��ǰ��鲻������Ʒ����");
                    forumnav = "";
                    return;
                }

                if (forum.Fid < 1)
                {
                    if (config.Rssstatus == 1 && forum.Allowrss == 1)
                        AddLinkRss("tools/" + base.RssAspxRewrite(forum.Fid), Utils.EncodeHtml(forum.Name) + " ������Ʒ");

                    AddErrLine("�����ڵ���Ʒ����ID");
                    return;
                }
                if (config.Rssstatus == 1)
                    AddLinkRss("tools/" + base.RssAspxRewrite(forum.Fid), Utils.EncodeHtml(forum.Name) + " ������Ʒ");

                forumname = forum.Name;
                pagetitle = Utils.RemoveHtml(forum.Name);
                subforumcount = forum.Subforumcount;
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
                navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);

                //����ҳ��Meta�е�Description��, ���SEO�Ѻ���
                UpdateMetaInfo(config.Seokeywords, forum.Description, config.Seohead);

                // �Ƿ���ʾ���������ʾ 1Ϊ��ʾ, 0����ʾ
                showforumlogin = 1;
                // ������δ������
                if (forum.Password == "")
                    showforumlogin = 0;
                else
                {
                    // �����⵽��Ӧ��cookie��ȷ
                    if (Utils.MD5(forum.Password) == ForumUtils.GetCookie("forum" + forumid.ToString() + "password"))
                        showforumlogin = 0;
                    else
                    {
                        // ����û��ύ��������ȷ�򱣴�cookie
                        if (forum.Password == DNTRequest.GetString("forumpassword"))
                        {
                            ForumUtils.WriteCookie("forum" + forumid.ToString() + "password", Utils.MD5(forum.Password));
                            showforumlogin = 0;
                        }
                    }
                }

                if (!Forums.AllowViewByUserId(forum.Permuserlist, userid)) //�жϵ�ǰ�û��ڵ�ǰ������Ȩ��
                {
                    if (forum.Viewperm == null || forum.Viewperm == string.Empty) //�����Ȩ��Ϊ��ʱ�������û���Ȩ��
                    {
                        if (useradminid != 1 && (usergroupinfo.Allowvisit != 1 || usergroupinfo.Allowtrade != 1))
                        {
                            AddErrLine("����ǰ����� \"" + usergroupinfo.Grouptitle + "\" û���������Ʒ�����Ȩ��");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }
                    }
                    else //�����Ȩ�޲�Ϊ�գ����հ��Ȩ��
                    {
                        if (!Forums.AllowView(forum.Viewperm, usergroupid))
                        {
                            AddErrLine("��û���������Ʒ�����Ȩ��");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }
                    }
                }


                ////�ж��Ƿ��з������Ȩ��
                if (userid > -1 && Forums.AllowPostByUserID(forum.Permuserlist, userid))
                    canposttopic = true;

                if (forum.Postperm == null || forum.Postperm == string.Empty) //Ȩ������Ϊ��ʱ�������û���Ȩ���ж�
                {
                    // ��֤�û��Ƿ��з����׵�Ȩ��
                    if (usergroupinfo.Allowtrade == 1)
                    {
                        canposttopic = true;
                    }
                }
                else if (Forums.AllowPost(forum.Postperm, usergroupid))
                {
                    canposttopic = true;
                }

                //�������ǰ�û��ǹ���Ա������̳�趨�˽�ֹ����ʱ��Σ���ǰʱ����������е�һ��ʱ����ڣ��������û�����
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

                //�õ��ӷ���JSON��ʽ
                subcategoriesjson = GoodsCategories.GetSubCategoriesJson(categoryid);
                //�õ���ǰ�û������ҳ��
                pageid = DNTRequest.GetInt("page", 1);
                //��ȡ��������
                goodscount = Goods.GetGoodsCount(categoryid, condition);

                // �õ�gpp����
                if (gpp <= 0)
                    gpp = config.Gpp;

                if (gpp <= 0)
                    gpp = 16;
           
                //��������ҳ���п��ܵĴ���
                if (pageid < 1)
                    pageid = 1;

                if (forum.Layer > 0)
                {
                    //��ȡ��ҳ��
                    pagecount = goodscount % gpp == 0 ? goodscount / gpp : goodscount / gpp + 1;
                    if (pagecount == 0)
                        pagecount = 1;

                    if (pageid > pagecount)
                        pageid = pagecount;

                    goodslist = Goods.GetGoodsInfoList(categoryid, gpp, pageid, condition, orderStr, direct);

                    ForumUtils.WriteCookie("referer", string.Format("showgoodslist.aspx?categoryid={0}&page={1}&order={2}&direct={3}&locus2={4}&search={5}", categoryid.ToString(), pageid.ToString(), orderStr, direct, cond, DNTRequest.GetString("search")));

                    //�õ�ҳ������
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
                        else //������������ʱ
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
            //��ΪĿǰ��δ�ṩRSS����,������������Ϊ0
            forumallowrss = 0; 
        }
    }
}
