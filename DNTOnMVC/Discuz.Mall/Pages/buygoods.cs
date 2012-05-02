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
    /// ������Ʒҳ��
    /// </summary>
    public class buygoods : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// ��Ʒ��Ϣ
        /// </summary>
        public Goodsinfo goodsinfo;
        /// <summary>
        /// �����������
        /// </summary>
        public string forumname;
        /// <summary>
        /// �������Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// ��̳������Ϣ
        /// </summary>
        public string forumnav;
        /// <summary>
        /// ���ֲ�����Ϣ
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo;
        /// <summary>
        /// ��ƷId
        /// </summary>
        public int goodsid = DNTRequest.GetInt("goodsid", -1);
        /// <summary>
        /// ���������Ϣ
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// �������
        /// </summary>
        public string floatad = "";
        /// <summary>
        /// ��̳���������˵�HTML����
        /// </summary>
        public string navhomemenu = "";
        /// <summary>
        /// �������
        /// </summary>
        public string doublead;
        /// <summary>
        /// �û��Ĺ�������Ϣ
        /// </summary>
        public AdminGroupInfo admininfo = null;
        /// <summary>
        /// �Ƿ���ʾ��Ҫ��¼����ʵĴ�����ʾ
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// �Ƿ��ǹ�����
        /// </summary>
        public int ismoder = 0;
        /// <summary>
        /// ��Ʒ������־��Ϣ
        /// </summary>
        public Goodstradeloginfo goodstradelog = new Goodstradeloginfo();
        /// <summary>
        /// ��ǰ��Ʒ����
        /// </summary>
        public Goodscategoryinfo goodscategoryinfo;
        #endregion

        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //δ���ý��׷���
            {
                AddErrLine("ϵͳδ�������׷���, ��ǰҳ����ʱ�޷�����!");
                return;
            }

            headerad = "";
            footerad = "";

            // �������ID������
            if (goodsid == -1)
            {
                AddErrLine("��Ч����ƷID");
                return;
            }

            if (userid <= 0)
            {
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "login.aspx?reurl=buygoods.aspx?goodsid=" + goodsid);
            }

            goodsinfo = Goods.GetGoodsInfo(goodsid);

            //��֤��ͨ���򷵻�
            if (!IsConditionsValid())
                return;

            goodscategoryinfo = GoodsCategories.GetGoodsCategoryInfoById(goodsinfo.Categoryid);

            if (config.Enablemall == 1) //������ͨģʽ
            {
                forumid = goodscategoryinfo.Fid;
                forum = Forums.GetForumInfo(forumid);

                if (forum.Password != "" &&
                    Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid + "password"))
                {
                    AddErrLine("����鱻����Ա����������");
                    System.Web.HttpContext.Current.Response.Redirect(base.ShowGoodsListAspxRewrite(goodsinfo.Categoryid, 1), true);
                    return;
                }                

                if (!Forums.AllowViewByUserId(forum.Permuserlist, userid)) //�жϵ�ǰ�û��ڵ�ǰ������Ȩ��
                {
                    if (forum.Viewperm == null || forum.Viewperm == string.Empty)//�����Ȩ��Ϊ��ʱ�������û���Ȩ��
                    {
                        if (usergroupinfo.Allowvisit != 1)
                        {
                            AddErrLine("����ǰ����� \"" + usergroupinfo.Grouptitle + "\" û������ð���Ȩ��");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }

                        if (useradminid != 1 && (usergroupinfo.Allowvisit != 1 || usergroupinfo.Allowtrade != 1))
                        {
                            AddErrLine("����ǰ����� \"" + usergroupinfo.Grouptitle + "\" û�н��н�����Ʒ��Ȩ��");
                            return;
                        }
                    }
                    else//�����Ȩ�޲�Ϊ�գ����հ��Ȩ��
                    {
                        if (!Forums.AllowView(forum.Viewperm, usergroupid))
                        {
                            AddErrLine("��û������ð���Ȩ��");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }                       
                    }
                }

                if (!Forums.AllowPostByUserID(forum.Permuserlist, userid)) //�жϵ�ǰ�û��ڵ�ǰ��鷢����ƷȨ��
                {
                    if (forum.Postperm == null || forum.Postperm == string.Empty)//Ȩ������Ϊ��ʱ�������û���Ȩ���ж�
                    {
                        // ��֤�û��Ƿ��з�����Ʒ��Ȩ��
                        if (usergroupinfo.Allowtrade != 1)
                        {
                            AddErrLine("����ǰ����� \"" + usergroupinfo.Grouptitle + "\" û�н��н�����Ʒ��Ȩ��");
                            return;
                        }
                    }
                    else//Ȩ�����ò�Ϊ��ʱ,���ݰ��Ȩ���ж�
                    {
                        if (!Forums.AllowPost(forum.Postperm, usergroupid))
                        {
                            AddErrLine("��û�н��н�����Ʒ��Ȩ��");
                            return;
                        }
                    }
                }

                forumname = forum.Name;
                pagetitle = goodsinfo.Title;
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
            }
            else if (config.Enablemall == 2) //��Ϊ�߼�ģʽʱ
            {
                forumid = 0;
            }

            ///�õ�����б�
            ///ͷ��
            headerad = Advertisements.GetOneHeaderAd("", forumid);
            footerad = Advertisements.GetOneFooterAd("", forumid);
            doublead = Advertisements.GetDoubleAd("", forumid);
            floatad = Advertisements.GetFloatAd("", forumid);

            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);

            if (useradminid != 0)
            {
                if (config.Enablemall == 1) //������ͨģʽ
                {
                    ismoder = Moderators.IsModer(useradminid, userid, forumid) ? 1 : 0;
                }
                //�õ���������Ϣ
                admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            }          


            //������ύ...
            if (ispost)
            {
                //������Ʒ������־
                goodstradelog.Number = DNTRequest.GetInt("number", 0);
                // ��Ʒ������ȷ
                if (goodstradelog.Number <= 0)
                {
                    AddErrLine("��������ȷ����Ʒ��, �뷵���޸�.");
                    return;
                }
                if (goodsinfo.Amount < goodstradelog.Number)
                {
                    AddErrLine("��Ʒʣ���������� (ʣ������Ϊ " + goodsinfo.Amount + ", ����������Ϊ " + goodstradelog.Number + ").");
                    return;
                }

                goodstradelog.Sellerid = goodsinfo.Selleruid;
                goodstradelog.Buyerid = userid;
                if (goodstradelog.Buyerid == goodstradelog.Sellerid)
                {
                    AddErrLine("����˫��������ͬһ�û�.");
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
                    AddMsgLine("���׵��Ѵ���, ���ڽ�ת�뽻�׵�ҳ��<br />(<a href=\"" + jumpurl + "\">������������û���Զ���ת, ��������</a>)<br />");
                }
                else
                {
                    SetUrl("buygoods.aspx?goodsid=" + goodsid);
                    SetMetaRefresh();
                    AddMsgLine("���׵���������, ��������д���׵�<br />(<a href=\"" + "buygoods.aspx?goodsid=" + goodsid + "\">������������û���Զ���ת, ��������</a>)<br />");
                }
            }
        }

        private bool IsConditionsValid()
        {
            if (goodsinfo == null || goodsinfo.Closed > 1 || goodsinfo.Amount <= 0)
            {
                if (goodsinfo.Amount <= 0)
                {
                    AddErrLine("��Ʒ��治��");
                }
                else
                {
                    AddErrLine("�����ڵ���ƷID");
                }
                headerad = Advertisements.GetOneHeaderAd("", 0);
                footerad = Advertisements.GetOneFooterAd("", 0);
                floatad = Advertisements.GetFloatAd("", 0);
                return false;
            }
            if (goodsinfo.Expiration <= DateTime.Now)
            {
                AddErrLine("�ǳ���Ǹ, �ñ��������ڻ��Ѿ������ˣ�");
                return false;
            }
            if (goodsinfo.Closed == 1)
            {
                AddErrLine("����Ʒ�ѹرգ�");
                return false;
            }
            if (goodsinfo.Selleruid <= 0)
            {
                AddErrLine("��Ʒ������Ϣ����");
                return false;
            }
            if (userid == goodsinfo.Selleruid)
            {
                AddErrLine("����˫������Ϊͬһ�û���");
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
            return true;
        }
    }
}
