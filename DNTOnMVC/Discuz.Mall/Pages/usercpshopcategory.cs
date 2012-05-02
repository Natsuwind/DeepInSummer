using System;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Mall;

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// �û����̷���
    /// </summary>
    public class usercpshopcategory : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// ��ǰ�����µ���Ʒ��������
        /// </summary>
        public string shopcategorydata = "";
        /// <summary>
        /// ѡ������
        /// </summary>
        public string item = DNTRequest.GetString("item");
        /// <summary>
        /// ��ǰ�û���Ϣ
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// ��ǰ���̵�ID
        /// </summary>
        public int shopid = 0;
        /// <summary>
        /// ��ǰ���̵���Ϣ
        /// </summary>
        public Shopinfo shopinfo;
        /// <summary>
        /// ���̵���Ʒ������Ϣ
        /// </summary>
        public Shopcategoryinfo shopcategoryinfo;
        /// <summary>
        /// ��ǰ�����µ���Ʒ����ѡ��
        /// </summary>
        public string categoryoptions;
        /// <summary>
        /// ������Ʒ�������ݱ�
        /// </summary>
        public DataTable shopcategorydt = new DataTable();
        /// <summary>
        /// ������Ʒ�������ݱ��¼��
        /// </summary>
        public int shopcategorydt_count = 0;
        /// <summary>
        /// Ŀ�����
        /// </summary>
        public Shopcategoryinfo targetshopcategoryinfo = null;
        #endregion

        protected override void ShowPage()
        {
            if (userid == -1)
            {
                AddErrLine("����δ��¼");
                return;
            }
            if (config.Enablemall <2)
            {
                AddErrLine("��ǰ����ֻ���ڿ����̳�(�߼�)ģʽʱ�ſ���ʹ��!");
                return;
            }

            user = Users.GetUserInfo(userid);
            shopinfo = Shops.GetShopByUserId(user.Uid);

            if (!DNTRequest.IsPost())
            {
                shopcategorydt = ShopCategories.GetShopCategoryTable(shopinfo.Shopid);
                shopcategorydt_count = shopcategorydt.Rows.Count;
                shopcategorydata = ShopCategories.GetShopCategoryJson(shopcategorydt);
                categoryoptions = ShopCategories.GetShopCategoryOption(shopcategorydt, false);
            }
            else
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("����������·����ȷ���޷��ύ���������װ��ĳ��Ĭ��������·��Ϣ�ĸ��˷���ǽ���(�� Norton Internet Security)���������䲻Ҫ��ֹ��·��Ϣ�����ԡ�");
                    return;
                }
                string operation = DNTRequest.GetFormString("operation");
                if (operation != "add")
                {
                    int shopcategoryid = DNTRequest.GetFormInt("categoryid", 0);
                    if (shopcategoryid <= 0)
                    {
                        AddErrLine("������Ʒ���������Ч<br />");
                        return;
                    }
                
                    shopcategoryinfo = ShopCategories.GetShopCategoryByCategoryId(shopcategoryid);
                }

                shopid = 0;
                item = DNTRequest.GetString("item");

                switch (operation)
                {
                    case "delete": //ɾ������
                    {
                        if (shopcategoryinfo == null || shopcategoryinfo.Categoryid <= 0)
                        {
                            AddErrLine("Ҫɾ���ĵ�����Ʒ���������Ч<br />");
                            return;
                        }
                        if (!ShopCategories.DeleteCategoryByCategoryId(shopcategoryinfo))
                        {
                            AddErrLine("�Բ���,��ǰ�ڵ����滹���ӽ��,��˲���ɾ��<br />");
                            return;
                        }
                        break; 
                    }
                    case "edit": //�༭��������
                    {
                        if (shopcategoryinfo == null || shopcategoryinfo.Categoryid <=0)
                        {
                            AddErrLine("Ҫ�޸ĵĵ�����Ʒ���������Ч<br />");
                            return;
                        }
                        string editname = DNTRequest.GetString("editcategoryname");
                        if (editname == "")
                        {
                            AddErrLine("������Ʒ��������δ�������Ϊ��<br />");
                            return;
                        }
                        shopcategoryinfo.Name = editname;
                        ShopCategories.UpdateShopCategory(shopcategoryinfo);
                        break; 
                    }
                    case "add": //��ӷ���
                    {
                        shopcategoryinfo = new Shopcategoryinfo();
                        shopcategoryinfo.Name = DNTRequest.GetFormString("addcategoryname");
                        shopcategoryinfo.Shopid = shopinfo.Shopid;
                        
                        int addtype = DNTRequest.GetInt("addtype", 0);
                        if (addtype > 0 && addtype <= 2)
                        {
                            int targetcategoryid = DNTRequest.GetFormInt("selectcategoryid", 0);

                            if (targetcategoryid <= 0)
                            {
                                AddErrLine("Ҫ��ӵ���Ŀ����������Ч<br />");
                                return;
                            }

                            targetshopcategoryinfo = ShopCategories.GetShopCategoryByCategoryId(targetcategoryid);
                            if (targetshopcategoryinfo == null || targetshopcategoryinfo.Categoryid <= 0)
                            {
                                AddErrLine("Ҫ��ӵ���Ŀ����������Ч<br />");
                                return;
                            }
                        }

                        ShopCategories.CreateShopCategory(shopcategoryinfo, targetshopcategoryinfo, addtype);
                        break; 
                    }
                    case "move": //�ƶ�����
                    {
                        int targetcategoryid = DNTRequest.GetFormInt("targetcategoryid", 0);

                        if (targetcategoryid <= 0)
                        {
                            AddErrLine("Ҫ�ƶ�����Ŀ����������Ч<br />");
                            return;
                        }

                        targetshopcategoryinfo = ShopCategories.GetShopCategoryByCategoryId(targetcategoryid);
                        if (targetshopcategoryinfo == null || targetshopcategoryinfo.Categoryid <= 0)
                        {
                            AddErrLine("Ҫ�ƶ�����Ŀ����������Ч<br />");
                            return;
                        }
                        string target_parentidlist = "," + targetshopcategoryinfo.Parentidlist.Trim() + ",";
                        if (target_parentidlist.IndexOf("," + shopcategoryinfo.Categoryid.ToString() + ",") > 0)
                        {
                            AddErrLine("���ܽ���ǰ�����ƶ������ӷ���<br />");
                            return;
                        }
                        ShopCategories.MoveShopCategory(shopcategoryinfo, targetshopcategoryinfo, DNTRequest.GetInt("isaschildnode",0) == 1 ? true : false);
                        break; 
                    }
                }

                SetUrl("usercpshopcategory.aspx?item=" + item);
                SetMetaRefresh();
                AddMsgLine("�����ɹ�. <br />(<a href=\"usercpmygoods.aspx?item=" + item + "\">������ﷵ��</a>)<br />");
            }
        }
    }
}