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
    /// 用户店铺配置
    /// </summary>
    public class usercpshopconfig : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前店铺的信息
        /// </summary>
        public Shopinfo shopinfo;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 选项名称
        /// </summary>
        public string item = "";
        /// <summary>
        /// 商品一级分类选项(option格式)
        /// </summary>
        public string categoryoptions = GoodsCategories.GetShopRootCategoryOption();
        /// <summary>
        /// 获取店铺主题信息(option格式)
        /// </summary>
        public string themeoptions = ShopThemes.GetShopThemeOption();
        #endregion

        protected override void ShowPage()
        {
            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            if (config.Enablemall < 2)
            {
                AddErrLine("当前操作只有在开启商城(高级)模式时才可以使用!");
                return;
            }

            user = Users.GetUserInfo(userid);
            shopinfo = Shops.GetShopByUserId(user.Uid);

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                shopinfo.Bulletin = Utils.CutString(DNTRequest.GetString("bulletin"), 0, 500);
                shopinfo.Introduce = Utils.CutString(DNTRequest.GetString("introduce"), 0, 500);
                shopinfo.Uid = userid;
                shopinfo.Username = username;
                shopinfo.Shopname = Utils.CutString(DNTRequest.GetString("shopname"), 0, 50);

                int lid = DNTRequest.GetInt("locus_2", 0);
                //当店铺所在地信息发生变化时
                if (shopinfo.Lid != lid && lid > 0)
                {
                    shopinfo.Lid = lid;
                    shopinfo.Locus = Locations.GetLocusByLID(lid);
                }
                
                string uploadfileinfo = MallUtils.SaveRequestFile(1024000, "jpg\r\ngif\r\njpeg", config, "postfile").ToLower();
                //当店标发生变化时
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
                //当店铺主题发生变化时
                if (shopinfo.Themeid != themeid && themeid > 0)
                {
                    shopinfo.Themeid = themeid;
                    shopinfo.Themepath = ShopThemes.GetShopThemeByThemeId(themeid).Directory;
                }

                Shops.UpdateShop(shopinfo);

                SetUrl("usercpshopconfig.aspx");
                SetMetaRefresh();
                AddMsgLine("操作成功. <br />(<a href=\"usercpshopconfig.aspx\">点击这里返回</a>)<br />");
            }
        }
    }
}
