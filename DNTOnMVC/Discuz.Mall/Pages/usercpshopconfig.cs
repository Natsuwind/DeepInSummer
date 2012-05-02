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
    public class usercpshopconfig : PageBase
    {
        #region ҳ�����
        /// <summary>
        /// ��ǰ���̵���Ϣ
        /// </summary>
        public Shopinfo shopinfo;
        /// <summary>
        /// ��ǰ�û���Ϣ
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// ѡ������
        /// </summary>
        public string item = "";
        /// <summary>
        /// ��Ʒһ������ѡ��(option��ʽ)
        /// </summary>
        public string categoryoptions = GoodsCategories.GetShopRootCategoryOption();
        /// <summary>
        /// ��ȡ����������Ϣ(option��ʽ)
        /// </summary>
        public string themeoptions = ShopThemes.GetShopThemeOption();
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

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("����������·����ȷ���޷��ύ���������װ��ĳ��Ĭ��������·��Ϣ�ĸ��˷���ǽ���(�� Norton Internet Security)���������䲻Ҫ��ֹ��·��Ϣ�����ԡ�");
                    return;
                }

                shopinfo.Bulletin = Utils.CutString(DNTRequest.GetString("bulletin"), 0, 500);
                shopinfo.Introduce = Utils.CutString(DNTRequest.GetString("introduce"), 0, 500);
                shopinfo.Uid = userid;
                shopinfo.Username = username;
                shopinfo.Shopname = Utils.CutString(DNTRequest.GetString("shopname"), 0, 50);

                int lid = DNTRequest.GetInt("locus_2", 0);
                //���������ڵ���Ϣ�����仯ʱ
                if (shopinfo.Lid != lid && lid > 0)
                {
                    shopinfo.Lid = lid;
                    shopinfo.Locus = Locations.GetLocusByLID(lid);
                }
                
                string uploadfileinfo = MallUtils.SaveRequestFile(1024000, "jpg\r\ngif\r\njpeg", config, "postfile").ToLower();
                //����귢���仯ʱ
                if (uploadfileinfo.EndsWith(".jpg") || uploadfileinfo.EndsWith(".gif"))
                {
                    if (Utils.FileExists(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" + shopinfo.Logo)))
                        System.IO.File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" + shopinfo.Logo));

                    shopinfo.Logo = uploadfileinfo;
                }
                else if (!Utils.StrIsNullOrEmpty(uploadfileinfo))
                {
                    AddErrLine(uploadfileinfo);
                    return;
                }

                int themeid = DNTRequest.GetInt("themeid", 0);
                //���������ⷢ���仯ʱ
                if (shopinfo.Themeid != themeid && themeid > 0)
                {
                    shopinfo.Themeid = themeid;
                    shopinfo.Themepath = ShopThemes.GetShopThemeByThemeId(themeid).Directory;
                }

                Shops.UpdateShop(shopinfo);

                SetUrl("usercpshopconfig.aspx");
                SetMetaRefresh();
                AddMsgLine("�����ɹ�. <br />(<a href=\"usercpshopconfig.aspx\">������ﷵ��</a>)<br />");
            }
        }
    }
}
