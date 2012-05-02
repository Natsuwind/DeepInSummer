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
    /// ��Ʒ��ʾҳ��
    /// </summary>
    public class showgoods : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// ��Ʒ��Ϣ
        /// </summary>
        public Goodsinfo goodsinfo;
        /// <summary>
        /// �Ƽ���Ʒ�б�
        /// </summary>
        public GoodsinfoCollection recommendgoodslist;
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public Goodscategoryinfo goodscategoryinfo = new Goodscategoryinfo();

#if NET1
		
        public ShowtopicPageAttachmentInfoCollection attachmentlist;
        public PrivateMessageInfoCollection pmlist = new PrivateMessageInfoCollection();
#else
        /// <summary>
        /// �����б�
        /// </summary>
        public List<ShowtopicPageAttachmentInfo> attachmentlist;
        /// <summary>
        /// ����Ϣ�б�
        /// </summary>
        public List<PrivateMessageInfo> pmlist; //= new Discuz.Common.Generic.List<PrivateMessageInfo>();
#endif
        /// <summary>
        /// �������
        /// </summary>
        public string doublead = "";
        /// <summary>
        /// �������
        /// </summary>
        public string floatad = "";
        /// <summary>
        /// ���ٷ������
        /// </summary>
        public string quickeditorad = string.Empty;
        /// <summary>
        /// �������Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// �����������
        /// </summary>
        public string forumname;
        /// <summary>
        /// ��̳������Ϣ
        /// </summary>
        public string forumnav;
        /// <summary>
        /// ��ƷId
        /// </summary>
        public int goodsid = DNTRequest.GetInt("goodsid", -1);
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable smilietypes = new DataTable();
        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int pageid;
        /// <summary>
        /// ���׼�¼��
        /// </summary>
        public int tradecount;
        /// <summary>
        /// ��ҳҳ��
        /// </summary>
        public int pagecount;
        /// <summary>
        /// ��ҳҳ������
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// ��̳��ת����ѡ��
        /// </summary>
        public string forumlistboxoptions;
        /// <summary>
        /// �Ƿ��ǹ�����
        /// </summary>
        public int ismoder = 0;
        /// <summary>
        /// �Ƿ���ʾ���׼�¼
        /// </summary>
        public int showtradelog;
        /// <summary>
        /// �Ƿ����URL
        /// </summary>
        public int parseurloff;
        /// <summary>
        /// �Ƿ��������
        /// </summary>
        public int smileyoff;
        /// <summary>
        /// �Ƿ���� Discuz!NT ����
        /// </summary>
        public int bbcodeoff;
        /// <summary>
        /// �Ƿ�ʹ��ǩ��
        /// </summary>
        public int usesig;
        /// <summary>
        /// �Ƿ����� [img]��ǩ
        /// </summary>
        public int allowimg;
        /// <summary>
        /// �û��Ĺ�������Ϣ
        /// </summary>
        public AdminGroupInfo admininfo = null;
        /// <summary>
        /// ��ǰ�����Ϣ
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// �Ƿ������Ե�Ȩ��
        /// </summary>
        public bool canleaveword = false;
        /// <summary>
        /// ��̳���������˵�HTML����
        /// </summary>
        public string navhomemenu = "";
        /// <summary>
        /// �Ƿ���ʾ����Ϣ�б�
        /// </summary>
        public bool showpmhint = false;
        /// <summary>
        /// ÿҳ��־��
        /// </summary>
        public int pptradelog;
        /// <summary>
        /// �Ƿ���ʾ��Ҫ��¼����ʵĴ�����ʾ
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// ��һҳ�����JSON
        /// </summary>
        public string firstpagesmilies = Caches.GetSmiliesFirstPageCache();
        /// <summary>
        /// �����Ƿ�������Tag
        /// </summary>
        public bool enabletag = false;
        /// <summary>
        /// ������ʵİ��ѡ��
        /// </summary>
        public string visitedforumsoptions;
        /// <summary>
        /// �Ƿ��ܷ�����ˮ����
        /// </summary>
        public int disablepostctrl;
        /// <summary>
        /// ������
        /// </summary>
        public int leavewordcount = 0;
        /// <summary>
        /// ���
        /// </summary>
        public bool isbuyer = false;
        /// <summary>
        /// ����
        /// </summary>
        public bool isseller = false;
        /// <summary>
        /// �����б�ĵ�ǰ��ҳ
        /// </summary>
        public int leaveword_page_currentpage = 1;
        /// <summary>
        /// �Ƿ�ɾ����������Ʒ/���ԣ�
        /// </summary>
        public bool isdeleteop = false;
        /// <summary>
        /// ������Ʒ����
        /// </summary>
        public string message;
        /// <summary>
        /// Ҫ��ʾ�����ü�¼
        /// </summary>
        public StringBuilder sb_usercredit = new StringBuilder();
        /// <summary>
        /// ��ȡ���Ź����б�GetCreditRulesJsonData
        /// </summary>
        public string creditrulesjsondata = "";
        /// <summary>
        /// �û�ע������
        /// </summary>
        public string joindate = "";
        /// <summary>
        /// ��ȡ����ذ�����Ʒ������Ϣ
        /// </summary>
        public string goodscategoryfid = "";
        #endregion

        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //δ���ý��׷���
            {
                AddErrLine("ϵͳδ�������׷���, ��ǰҳ����ʱ�޷�����!");
                return;
            }
            else
                goodscategoryfid = Discuz.Mall.GoodsCategories.GetGoodsCategoryWithFid();

            headerad = "";
            footerad = "";
            floatad = "";

            disablepostctrl = 0;

            // �����ƷID��Ч
            if (goodsid == -1)
            {
                AddErrLine("��Ч����ƷID");
                return ;
            }

            goodsinfo = Goods.GetGoodsInfo(goodsid);
            if (goodsinfo == null || goodsinfo.Closed > 1)
            {
                AddErrLine("�����ڵ���ƷID");
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

            if (config.Enablemall == 1) //������ͨģʽ
            {
                forumid = GoodsCategories.GetCategoriesFid(goodsinfo.Categoryid);
                forum = Forums.GetForumInfo(forumid);
                if (forum == null)
                {
                    AddErrLine("��ǰ��Ʒ��������δ����Ӧ���");
                    return;
                }

                forumname = forum.Name;
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

                ///�õ�����б�
                ///ͷ��
                headerad = Advertisements.GetOneHeaderAd("", forumid);
                footerad = Advertisements.GetOneFooterAd("", forumid);
                doublead = Advertisements.GetDoubleAd("", forumid);
                floatad = Advertisements.GetFloatAd("", forumid);

                // ����Ƿ���а��������
                if (useradminid != 0)
                {
                    ismoder = Moderators.IsModer(useradminid, userid, forumid) ? 1 : 0;
                    //�õ���������Ϣ
                    admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
                    if (admininfo != null)
                        disablepostctrl = admininfo.Disablepostctrl;
                }
            }
            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(goodsinfo.Categoryid);
            pagetitle = goodsinfo.Title;
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);
        
            //��֤��ͨ���򷵻�
            if (!IsConditionsValid())
                return;        

            //�༭��״̬
            StringBuilder sb = new StringBuilder("var Allowhtml=1;\r\n");
            
            parseurloff = 0;
            bbcodeoff = 1;
            if (config.Enablemall == 1) //������ͨģʽ
            {
                smileyoff = 1 - forum.Allowsmilies;
                
                if (forum.Allowbbcode == 1 && usergroupinfo.Allowcusbbcode == 1)
                    bbcodeoff = 0;
            
                allowimg = forum.Allowimgcode;
            }
            else if (config.Enablemall == 2) //��Ϊ�߼�ģʽʱ
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


            // �õ�pptradelog����
            pptradelog = Utils.StrToInt(ForumUtils.GetCookie("ppp"), config.Ppp);
            if (pptradelog <= 0)
                pptradelog = config.Ppp;

            //���ٷ������
            if (config.Enablemall == 1) //������ͨģʽ
                quickeditorad = Advertisements.GetQuickEditorAD("", forumid);

            //����ҳ��Meta�е�Description��, ���SEO�Ѻ���
            string metadescritpion = Utils.RemoveHtml(goodsinfo.Message);
            metadescritpion = metadescritpion.Length > 100 ? metadescritpion.Substring(0, 100) : metadescritpion;
            UpdateMetaInfo(config.Seokeywords, metadescritpion, config.Seohead);

            GoodspramsInfo goodspramsInfo = new GoodspramsInfo();
            goodspramsInfo.Goodsid = goodsinfo.Goodsid;

            if (config.Enablemall == 1) //������ͨģʽ
            {
                goodspramsInfo.Fid = forum.Fid;
                goodspramsInfo.Jammer = forum.Jammer;
                goodspramsInfo.Getattachperm = forum.Getattachperm;
                goodspramsInfo.Showimages = forum.Allowimgcode;
            }
            else if (config.Enablemall == 2) //��Ϊ�߼�ģʽʱ
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

            if (config.Enablemall == 1) //������ͨģʽ
                ForumUtils.UpdateVisitedForumsOptions(forumid);

            visitedforumsoptions = ForumUtils.GetVisitedForumsOptions(config.Visitedforums);

            //ɾ������
            if (DNTRequest.GetInt("deleteleaveword", 0) == 1)
            {
                isdeleteop = true;
                int leavewordid = DNTRequest.GetInt("leavewordid", 0);

                if (leavewordid <= 0)
                {
                    AddErrLine("��Ҫɾ���������ѱ�ɾ��, ����ת����Ʒҳ��");
                    return;
                }
                if (GoodsLeaveWords.DeleteLeaveWordById(leavewordid, userid, goodsinfo.Selleruid, useradminid))
                {
                    SetUrl(base.ShowGoodsAspxRewrite(goodsinfo.Goodsid));
                    SetMetaRefresh();
                    AddMsgLine("�������ѱ�ɾ��, ����ת����Ʒҳ��<br />(<a href=\"" + base.ShowGoodsAspxRewrite(goodsinfo.Goodsid) + "\">������������û���Զ���ת, ��������</a>)<br />");
                    return;
                }
                else
                {
                    AddErrLine("�����û������Чɾ��������, ����ת����Ʒҳ��");
                    return;
                }
            }

            //ɾ����Ʒ
            if (DNTRequest.GetInt("deletegoods", 0) == 1)
            {
                isdeleteop = true;
                //�Ƿ�Ϊ���һ����
                if (Goods.IsSeller(goodsinfo.Goodsid.ToString(), userid) || ismoder == 1)
                {
                    Goods.DeleteGoods(goodsinfo.Goodsid.ToString(), false);

                    SetUrl(this.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1));
                    SetMetaRefresh();
                    AddMsgLine("�����ɹ�. <br />(<a href=\"" + this.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1) + "\">������ﷵ��</a>)<br />");
                    return;
                }
                else
                {
                    AddErrLine("�㲻�ǵ�ǰ��Ʒ�����һ����������޷�ɾ������Ʒ");
                    return;
                }
            }                     
       

            //������ύ
            if (ispost)
            {
                //��������ύ...
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("����������·����ȷ���޷��ύ���������װ��ĳ��Ĭ��������·��Ϣ�ĸ��˷���ǽ���(�� Norton Internet Security)���������䲻Ҫ��ֹ��·��Ϣ�����ԡ�");
                    return;
                }

                if (DNTRequest.GetString("postleaveword") == "add")
                {
                    //����֤������ȷ��,������Ӧ����
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
                        AddMsgLine("���������ѷ���, ����ת����Ʒҳ��<br />(<a href=\"" + base.ShowGoodsAspxRewrite(goodsinfo.Goodsid) + "\">������������û���Զ���ת, ��������</a>)<br />");
                    }
                }
                else
                {
                    //����֤������ȷ��,������Ӧ����
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
                            AddMsgLine("���Ը��³ɹ�, ����ת����Ʒҳ��<br />(<a href=\"" + base.ShowGoodsAspxRewrite(goodsinfo.Goodsid) + "\">������������û���Զ���ת, ��������</a>)<br />");
                        }
                    }
                    else
                    {
                        AddErrLine("��ǰ���Բ����ڻ��ѱ�ɾ��");
                        return;
                    }
                }
            }
            else
            {
                goodsinfo.Viewcount += 1; //�������1
                Goods.UpdateGoods(goodsinfo);
            }
        }

        private bool IsConditionsValid()
        {
            if (goodsinfo.Expiration < DateTime.Now)
            {
                AddErrLine("��ǰ��Ʒ�ѹ���!");
                return false;
            }          
            if (goodsinfo.Displayorder == -1)
            {
                AddErrLine("����Ʒ�ѱ�ɾ����");
                return false;
            }
            if (goodsinfo.Displayorder == -2)
            {
                AddErrLine("����Ʒδ����ˣ�");
                return false;
            }
            if (goodsinfo.Displayorder == -3)
            {
                AddErrLine("��ǰ��Ʒ��δ�ϼ�!");
                return false;
            }
            //��ǰ�û�Ϊ����ʱ
            if (goodsinfo.Selleruid == userid)
                isseller = true;
            else 
                isbuyer = true;

            if (!isseller && config.Enablemall == 1) //������ͨģʽ
            {
                if (forum.Password != "" && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid + "password"))
                {
                    AddErrLine("����鱻����Ա����������");
                    System.Web.HttpContext.Current.Response.Redirect(base.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1), true);
                    return false;
                }

                if (!Forums.AllowViewByUserId(forum.Permuserlist, userid)) //�жϵ�ǰ�û��ڵ�ǰ������Ȩ��
                {
                    if (forum.Viewperm == null || forum.Viewperm == string.Empty) //�����Ȩ��Ϊ��ʱ�������û���Ȩ��
                    {
                        if (useradminid != 1 && (usergroupinfo.Allowvisit != 1 || usergroupinfo.Allowtrade != 1))
                        {
                            AddErrLine("����ǰ����� \"" + usergroupinfo.Grouptitle + "\" û���������Ʒ��Ȩ��");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return false;
                        }
                    }
                    else //�����Ȩ�޲�Ϊ�գ����հ��Ȩ��
                    {
                        if (!Forums.AllowView(forum.Viewperm, usergroupid))
                        {
                            AddErrLine("��û���������Ʒ��Ȩ��");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return false;
                        }
                    }
                }

                //�Ƿ���ʾ�ظ�����
                if (Forums.AllowReplyByUserID(forum.Permuserlist, userid))
                {
                    canleaveword = true;
                }
                else
                {
                    if (forum.Replyperm == null || forum.Replyperm == string.Empty) //Ȩ������Ϊ��ʱ�������û���Ȩ���ж�
                    {
                        // ��֤�û��Ƿ��з��������Ȩ��
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
