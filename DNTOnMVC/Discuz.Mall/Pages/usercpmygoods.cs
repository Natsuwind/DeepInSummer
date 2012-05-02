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
    /// �û��ռ���ҳ��
    /// </summary>
    public class usercpmygoods : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// ѡ������
        /// </summary>
        public string item = DNTRequest.GetString("item");
        /// <summary>
        /// ��������
        /// </summary>
        public string filter = DNTRequest.GetString("filter");
        /// <summary>
        /// ��Ʒ�б�
        /// </summary>
        public GoodsinfoCollection goodslist;
        /// <summary>
        /// ��Ʒ������Ϣ��
        /// </summary>
        public DataTable goodstradeloglist;
        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// ��¼����
        /// </summary>
        public int reccount = 0;
        /// <summary>
        /// ��ҳ����
        /// </summary>
        public int pagecount = 1;
        /// <summary>
        /// ��ҳҳ������
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// ��ǰ�û���Ϣ
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// �Ƿ�����Ʒ�����б�
        /// </summary>
        public bool istradeloglist = false;
        /// <summary>
        /// �Ƿ���ʾ��������
        /// </summary>
        public bool isshowrate = false;
        /// <summary>
        /// ��Ʒid�ַ���(��ʽ:1,2,3)
        /// </summary>
        public string goodsidlist = DNTRequest.GetString("goodsid");

        public Goosdstradestatisticinfo tradestatisticinfo;
        #endregion           

        protected override void ShowPage()
        {
            if (userid == -1)
            {
                AddErrLine("����δ��¼");
                return;
            }
            if (config.Enablemall == 0) //δ���ý��׷���
            {
                AddErrLine("ϵͳδ�������׷���, ��ǰҳ����ʱ�޷�����!");
                return;
            }

            user = Users.GetUserInfo(userid);

            if (item == "")
                item = "tradestats";


            //����ʾ������־(���ǳ�������Ʒ)
            if ((item == "selltrade" && filter != "onsell" && filter != "allgoods") || item == "buytrade")
                istradeloglist = true;

            //��Ϊ����,���׳ɹ����˿�ʱ,����ʾ�����ֶ���Ϣ
            if (filter == "eccredit" || filter == "success" || filter == "refund")
                isshowrate = true;


            //��ȡ��ǰ�û�����Ʒ��
            if (filter == "allgoods" || filter == "onsell" || item == "tradestats") 
                reccount = (filter == "allgoods") ? Goods.GetGoodsCountBySellerUid(userid, true) : Goods.GetGoodsCountBySellerUid(userid, false);
            else
            {   //��ȡ��ǰ�û���Ϊ���ҵĽ�����
                if (item == "selltrade")
                    reccount = TradeLogs.GetGoodsTradeLogCount(userid, goodsidlist, 1, filter);
                else //��ȡ��ǰ�û���Ϊ��ҵĽ�����
                    reccount = TradeLogs.GetGoodsTradeLogCount(userid, goodsidlist, 2, filter);
            }

            if (item == "tradestats")
            {
                tradestatisticinfo = TradeLogs.GetTradeStatistic(user.Uid);
                return;
            }

            // �õ���ҳ��С����
            int pagesize = 10;
            //��������ҳ���п��ܵĴ���
            if (pageid < 1)
                pageid = 1;

            //��ȡ��ҳ��
            pagecount = reccount % pagesize == 0 ? reccount / pagesize : reccount / pagesize + 1;
            if (pagecount == 0)
                pagecount = 1;

            if (pageid > pagecount)
                pageid = pagecount;

              //��������ύ...
            if (!ispost)
            {
                if (item == "selltrade" && (filter == "allgoods" || filter == "onsell"))
                {
                    if (filter == "allgoods")
                        goodslist = Goods.GetGoodsListBySellerUID(userid, true, pagesize, pageid, "lastupdate", 1);
                    else
                        goodslist = Goods.GetGoodsListBySellerUID(userid, false, pagesize, pageid, "lastupdate", 1);

                    pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpmygoods.aspx?item=" + item + "&filter=" + filter , 8);
                }
                else
                {
                    if (item == "selltrade")
                        goodstradeloglist = TradeLogs.GetGoodsTradeLogList(userid, goodsidlist, 1, filter, pagesize, pageid);
                    else
                        goodstradeloglist = TradeLogs.GetGoodsTradeLogList(userid, goodsidlist, 2, filter, pagesize, pageid);

                    pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpmygoods.aspx?item=" + item + "&filter=" + filter, 8);
                }
            }
            else
            {
                string operation = DNTRequest.GetString("operation");

                if (operation == "")
                    operation = "deletegoods";

                if (operation == "deletegoods")
                {
                    if (goodsidlist == "")
                    {
                        AddErrLine("��δѡ���κ���Ʒ");
                        return;
                    }

                    if (Goods.IsSeller(goodsidlist, userid))
                    {
                        Goods.DeleteGoods(goodsidlist, false);

                        SetUrl("usercpmygoods.aspx?item=" + item + "&filter=" + filter);
                        SetMetaRefresh();
                        AddMsgLine("�����ɹ�. <br />(<a href=\"usercpmygoods.aspx?item=" + item + "&filter=" + filter + "\">������ﷵ��</a>)<br />");
                    }
                    else
                    {
                        AddErrLine("�㲻�ǵ�ǰ��Ʒ�����ң�����޷�ɾ������Ʒ");
                        return;
                    }
                }
            }
        }
    }
}
