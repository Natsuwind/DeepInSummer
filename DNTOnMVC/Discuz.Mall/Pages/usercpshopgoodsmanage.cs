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
    /// �û�������Ʒ����
    /// </summary>
    public class usercpshopgoodsmanage : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// ��ǰ�����µ���Ʒ��������
        /// </summary>
        public string shopcategorydata = "";
        /// <summary>
        /// ��Ʒ�б�
        /// </summary>
        public GoodsinfoCollection goodslist;
        /// <summary>
        /// ѡ������
        /// </summary>
        public string item = DNTRequest.GetString("item");
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
        /// ��ǰҳ��
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// ��Ʒid�ַ���(��ʽ:1,2,3)
        /// </summary>
        public string goodsidlist = DNTRequest.GetString("goodsid");
        /// <summary>
        /// ��¼����
        /// </summary>
        public int reccount = 0;
        /// <summary>
        /// ��ǰ���̵���Ϣ
        /// </summary>
        public Shopinfo shopinfo;
        /// <summary>
        /// ������Ʒ�������ݱ�
        /// </summary>
        public DataTable shopcategorydt = new DataTable();
        /// <summary>
        /// ������Ʒ�������ݱ��¼��
        /// </summary>
        public int shopcategorydt_count = 0;
        /// <summary>
        /// ��ǰ�����µ���Ʒ����ѡ��
        /// </summary>
        public string categoryoptions;
        /// <summary>
        /// ������Ʒ����id
        /// </summary>
        public int shopgoodscategoryid = DNTRequest.GetInt("shopgoodscategoryid", 0);
        /// <summary>
        /// �Ƽ���Ʒ�б�
        /// </summary>
        public GoodsinfoCollection recommendgoodslist;
        #endregion

        protected override void ShowPage()
        {
            if (userid == -1)
            {
                AddErrLine("����δ��¼");
                return;
            }

            if (config.Enablemall < 2)
            {
                AddErrLine("��ǰ����ֻ���ڿ����̳�(�߼�)ģʽʱ�ſ���ʹ��!");
                return;
            }

            user = Users.GetUserInfo(userid);

            if (item == "")
                item = "shopcategory";

            if(item == "recommend")
                recommendgoodslist = Goods.GetGoodsRecommendManageList(userid, 6, 1, "");
            
            reccount = (shopgoodscategoryid <= 0) ? Goods.GetGoodsCountBySellerUid(userid, true) : Goods.GetGoodsCountByShopCategory(shopgoodscategoryid,"");

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

            shopinfo = Shops.GetShopByUserId(user.Uid);
            //��������ύ...
            if (!ispost)
            {
                if (shopgoodscategoryid <= 0)
                    goodslist = Goods.GetGoodsListBySellerUID(userid, true, pagesize, pageid, "lastupdate", 1);
                else
                    goodslist = Goods.GetGoodsInfoListByShopCategory(shopgoodscategoryid, pagesize, pageid, "", "lastupdate", 1);

                pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid, 8);
                shopcategorydt = ShopCategories.GetShopCategoryTable(shopinfo.Shopid);
                shopcategorydt_count = shopcategorydt.Rows.Count;
                shopcategorydata = ShopCategories.GetShopCategoryJson(shopcategorydt);
                categoryoptions = ShopCategories.GetShopCategoryOption(shopcategorydt, true);
            }
            else
            {
                string operation = DNTRequest.GetString("operation");

                if (operation == "")
                    operation = "movecategory";

                switch (operation)
                {
                    case "movecategory": //�ƶ�����Ʒ����
                        {
                            if (goodsidlist == "")
                            {
                                AddErrLine("��δѡ���κ���Ʒ");
                                return;
                            }

                            int selectcategoryid = DNTRequest.GetInt("selectcategoryid", 0);
                            if (selectcategoryid <= 0)
                            {
                                AddErrLine("��δѡ��Ҫ�ƶ�������Ʒ����");
                                return;
                            }
                            if (Goods.IsSeller(goodsidlist, userid))
                            {
                                if (Goods.MoveGoodsShopCategory(goodsidlist, selectcategoryid) > 0)
                                {
                                    SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                                    SetMetaRefresh();
                                    AddMsgLine("�����ɹ�. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">������ﷵ��</a>)<br />");
                                }
                                else
                                {
                                    AddErrLine("��Ʒ������Ϣ��Ч����ѡ��Ʒ���ڸ÷�����");
                                    return;
                                }
                            }
                            else
                            {
                                AddErrLine("�㲻�ǵ�ǰ��Ʒ�����ң�����޷��ƶ�����Ʒ��ָ���ķ���");
                                return;
                            }
                            break;
                        }
                    case "removecategory": //�Ƴ���Ʒ����
                        {
                            int removeshopgoodscategoryid = DNTRequest.GetInt("removeshopgoodscategoryid", 0);

                            int removegoodsid = DNTRequest.GetInt("removegoodsid", 0);

                            if (removeshopgoodscategoryid <= 0 || removegoodsid <= 0)
                            {
                                AddErrLine("�Ƴ�������Ϣ����");
                                return;
                            }

                            if (Goods.IsSeller(removegoodsid.ToString(), userid))
                            {
                                if (Goods.RemoveGoodsShopCategory(removegoodsid, removeshopgoodscategoryid) > 0)
                                {
                                    SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                                    SetMetaRefresh();
                                    AddMsgLine("�����ɹ�. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">������ﷵ��</a>)<br />");
                                }
                                else
                                {
                                    AddErrLine("��Ʒ������Ϣ��Ч����ѡ��Ʒ���ڸ÷�����");
                                    return;
                                }
                            }
                            else
                            {
                                AddErrLine("�����ǵ�ǰ��Ʒ�����ң�����޷��Ƴ�����Ʒ�ķ���");
                                return;
                            }
                            break;
                        }
                    case "recommend": //�Ƽ���Ʒ
                        {
                            if (goodsidlist == "")
                            {
                                AddErrLine("��δѡ���κ���Ʒ");
                                return;
                            }
                            if ((recommendgoodslist.Count + goodsidlist.Split(',').Length) > 5)
                            {
                                AddErrLine("���Ƽ�����Ʒ�����Ѵ���5, ��Ϊ�޷������Ƽ�");
                                return;
                            }

                            if (Goods.IsSeller(goodsidlist, userid))
                            {
                                Goods.RecommendGoods(goodsidlist);
                                SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                                SetMetaRefresh();
                                AddMsgLine("�����ɹ�. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">������ﷵ��</a>)<br />");
                            }
                            else
                            {
                                AddErrLine("�����ǵ�ǰ��Ʒ�����ң�����޷��Ƽ�����Ʒ");
                                return;
                            }
                            break;
                        }
                    case "cancelrecommend": //ȡ���Ƽ���Ʒ
                        {
                            goodsidlist = DNTRequest.GetString("cancelrecommendgoodsid");
                            if (goodsidlist == "")
                            {
                                AddErrLine("��δѡ���κ���Ʒ");
                                return;
                            }

                            if (Goods.IsSeller(goodsidlist, userid))
                            {
                                Goods.CancelRecommendGoods(goodsidlist);
                                SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                                SetMetaRefresh();
                                AddMsgLine("�����ɹ�. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">������ﷵ��</a>)<br />");
                            }
                            else
                            {
                                AddErrLine("�㲻�ǵ�ǰ��Ʒ�����ң�����޷�ȡ���Ƽ�����Ʒ");
                                return;
                            }
                            break;
                        }
                    case "updatedisplayorder": //������Ʒ��ʾ˳��
                        {
                            foreach (Goodsinfo goodsinfo in recommendgoodslist)
                            {
                                //����ʾ˳��ֵ�����仯ʱ���������Ӧ����Ʒ��Ϣ
                                if (goodsinfo.Displayorder != DNTRequest.GetInt("displayorder_" + goodsinfo.Goodsid, 0))
                                {
                                    goodsinfo.Displayorder = DNTRequest.GetInt("displayorder_" + goodsinfo.Goodsid, 0);
                                    Goods.UpdateGoods(goodsinfo);
                                }
                            }

                            SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                            SetMetaRefresh();
                            AddMsgLine("�����ɹ�. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">������ﷵ��</a>)<br />");
                            break;
                        }
                }
            }
        }
    }
}
