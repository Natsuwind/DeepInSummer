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
    /// ���½���ҳ��
    /// </summary>
    public class offlinetrade : PageBase
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
        /// ��Ʒ������־Id
        /// </summary>
        public int goodstradelogid = DNTRequest.GetInt("goodstradelogid", 0);
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
        /// ���
        /// </summary>
        public bool isbuyer = false;
        /// <summary>
        /// ����
        /// </summary>
        public bool isseller = false;
        /// <summary>
        /// �Ƿ����������
        /// </summary>
        private int buyerleaveword = 1;
        /// <summary>
        /// �Ƿ����֧��
        /// </summary>
        public bool ispay = false;
        /// <summary>
        /// �Ƿ�������
        /// </summary>
        public bool israted = false;
        /// <summary>
        /// �����б�
        /// </summary>
        public GoodsleavewordinfoCollection goodsleavewordlist = new GoodsleavewordinfoCollection();
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

            // �����Ʒ������־����ȷ
            if (goodstradelogid <= 0)
            {
                AddErrLine("��Ч�Ľ�����־��Ϣ.");
                return;
            }

            goodstradelog = TradeLogs.GetGoodsTradeLogInfo(goodstradelogid);
            int oldstatus = goodstradelog.Status;

            if (config.Enablemall == 1) //������ͨģʽ
            {
                forumid = GoodsCategories.GetCategoriesFid(goodstradelog.Categoryid);
                forum = Forums.GetForumInfo(forumid);
                forumname = forum.Name;
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
            }
            else if (config.Enablemall == 2) //��Ϊ�߼�ģʽʱ
                forumid = 0;

            ///�õ�����б�
            ///ͷ��
            headerad = Advertisements.GetOneHeaderAd("", forumid);
            footerad = Advertisements.GetOneFooterAd("", forumid);
            doublead = Advertisements.GetDoubleAd("", forumid);
            floatad = Advertisements.GetFloatAd("", forumid);

            pagetitle = goodstradelog.Subject;
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);

            if (useradminid != 0)
            {
                if (config.Enablemall == 1) //������ͨģʽ
                    ismoder = Moderators.IsModer(useradminid, userid, forumid) ? 1 : 0;

                //�õ���������Ϣ
                admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            }
            //��֤��ͨ���򷵻�
            if (!IsConditionsValid())
                return;

            goodsleavewordlist = GoodsLeaveWords.GetLeaveWordList(goodstradelog.Id);

            if (goodstradelog.Status == 7 || goodstradelog.Status == 17)
                israted = GoodsRates.CanRate(goodstradelog.Id, userid) ? false : true; //�����ǰ�û������۹�������������
           
            //������ύ�������Ʒ������־
            if (ispost && goodstradelog.Status >= 0)
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("����������·����ȷ���޷��ύ���������װ��ĳ��Ĭ��������·��Ϣ�ĸ��˷���ǽ���(�� Norton Internet Security)���������䲻Ҫ��ֹ��·��Ϣ�����ԡ�");
                    return;
                }

                //��Ҫ��֤����ʱ
                if (DNTRequest.GetInt("status", -1) > 0 && IsVerifyPassWord(goodstradelog.Status))
                {
                    if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("password")))
                    {
                        AddErrLine("���벻��Ϊ��, �뷵����д.");
                        return;
                    }

                    int uid = -1;
                    if (config.Passwordmode == 1)
                        uid = Users.CheckDvBbsPassword(base.username, DNTRequest.GetString("password"));
                    else
                        uid = Users.CheckPassword(username, DNTRequest.GetString("password"), true);

                    if (uid < 0)
                    {
                        AddErrLine("����������벻��ȷ, �����޸Ķ���״̬, �뷵���޸�.");
                        return;
                    }

                    //����֤������ȷ��,������Ӧ����
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
                    //��Ϊ���ʱ
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

                    if (isseller) //��Ϊ����ʱ
                        goodstradelog.Transportfee = DNTRequest.GetInt("fee", 0);
                }
              
                if (TradeLogs.UpdateTradeLog(goodstradelog, oldstatus, true))
                {
                    SetUrl("offlinetrade.aspx?goodstradelogid=" + goodstradelogid);
                    SetMetaRefresh();
                    AddMsgLine("���׵��Ѹ���, ����ת�뽻�׵�ҳ��<br />(<a href=\"" + "offlinetrade.aspx?goodstradelogid=" + goodstradelogid + "\">������������û���Զ���ת, ��������</a>)<br />");
                }
            }
        }

        private bool IsConditionsValid()
        {
            if (goodstradelog.Offline == 0)
            {
                AddErrLine("��ǰ����Ϊ���߽���!");
                return false;
            }

            //��ǰ�û�Ϊ���ʱ
            if (goodstradelog.Buyerid == userid)
            {
                isbuyer = true;
            }
            //��ǰ�û�Ϊ����ʱ
            if (goodstradelog.Sellerid == userid)
            {
                isseller = true;
            }

            //��ǰ�û��Ȳ������Ҳ��������
            if (!isbuyer && !isseller)
            {
                AddErrLine("��ǰ�û���ݼȲ������Ҳ��������!");
                return false;
            }

            if (goodstradelog.Buyerid <= 0)
            {
                AddErrLine("��Ʒ�����Ϣ����!");
                return false;
            }
            if (goodstradelog.Sellerid <= 0)
            {
                AddErrLine("��Ʒ������Ϣ����!");
                return false;
            }

            int goodsid = goodstradelog.Goodsid;
            // �����ƷID��Ч
            if (goodsid <= 0)
            {
                AddErrLine("��Ч����ƷID");
                return false;
            }

            goodsinfo = Goods.GetGoodsInfo(goodsid);
            if (goodsinfo.Displayorder == -1)
            {
                AddErrLine("����Ʒ�ѱ�ɾ��!");
                return false;
            }
            if (goodsinfo.Displayorder == -2)
            {
                AddErrLine("����Ʒδ�����!");
                return false;
            }
            if (goodsinfo.Expiration <= DateTime.Now)
            {
                AddErrLine("�ǳ���Ǹ, ����Ʒ�����ڻ��Ѿ�����!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// �Ƿ���֤��ǰ�û����룬�Ա��޸Ľ���״̬
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool IsVerifyPassWord(int status)
        {
            //�Ƿ�������
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
