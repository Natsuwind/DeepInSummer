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
    /// �û���������
    /// </summary>
    public class usercpshoplink : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// ��ǰ�û���Ϣ
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// ��ǰ���̵���Ϣ
        /// </summary>
        public Shopinfo shopinfo;
        /// <summary>
        /// ��������������Ϣ
        /// </summary>
        public ShoplinkinfoCollection shoplinkinfolist;
        /// <summary>
        /// ѡ������
        /// </summary>
        public string item = "";
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

            shopinfo = Shops.GetShopByUserId(user.Uid);

            shoplinkinfolist = ShopLinks.GetShopLinkByShopId(shopinfo.Shopid);
            //��������ύ...
            if (ispost)
            { 
                string operation = DNTRequest.GetString("operation");

                if (operation == "")
                    operation = "add";

                switch (operation)
                {
                    case "add": //��ӵ�����������
                        {
                            if (shoplinkinfolist.Count >= 16)
                            {
                                AddErrLine("Ŀǰϵͳ������������ 20 ����������");
                                return;
                            }

                            string addusername = DNTRequest.GetString("username");
                            if (addusername == "")
                            {
                                AddErrLine("�������������!");
                                return;
                            }

                            int adduserid = Users.GetUserId(addusername);
                            if (adduserid < 0)
                            {
                                AddErrLine("�û���������!");
                                return;
                            }
                            if (adduserid == userid)
                            {
                                AddErrLine("�������ܽ����˵�����Ϊ��������!");
                                return;
                            }

                            Shopinfo add_shopinfo = Shops.GetShopByUserId(Users.GetUserId(addusername));
                            if (add_shopinfo == null && add_shopinfo.Shopid <= 0)
                            {
                                AddErrLine("�û�:" + addusername + " δ�ڱ�վ���꣬����޷���Ӹ���������");
                                return;
                            }

                            Shoplinkinfo shoplinkinfo = new Shoplinkinfo();
                            shoplinkinfo.Displayorder = 0;
                            shoplinkinfo.Name = add_shopinfo.Shopname;
                            shoplinkinfo.Linkshopid = add_shopinfo.Shopid;
                            shoplinkinfo.Shopid = shopinfo.Shopid;
                            ShopLinks.CreateShopLink(shoplinkinfo);
                            break;
                        }
                    case "delete": //ɾ��������������
                        {
                            string delshoplink = DNTRequest.GetString("shoplinkid");
                            if (delshoplink == "")
                            {
                                AddErrLine("��δѡ����������");
                                return;
                            }
                            ShopLinks.DeleteShopLink(delshoplink);
                            break;
                        }
                    case "updatedisplayorder": //���µ�������������ʾ˳��
                        {
                            foreach (Shoplinkinfo shoplinkinfo in shoplinkinfolist)
                            {
                                //����ʾ˳��ֵ�����仯ʱ���������Ӧ����Ϣ
                                if (shoplinkinfo.Displayorder != DNTRequest.GetInt("displayorder_" + shoplinkinfo.Id, 0))
                                {
                                    shoplinkinfo.Displayorder = DNTRequest.GetInt("displayorder_" + shoplinkinfo.Id, 0);
                                    ShopLinks.UpdateShopLink(shoplinkinfo);
                                }
                            }
                            break;
                        }
                }

                SetUrl("usercpshoplink.aspx");
                SetMetaRefresh();
                AddMsgLine("�����ɹ�. <br />(<a href=\"usercpshoplink.aspx\">������ﷵ��</a>)<br />");      
            }
        }
    }
}
