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
    /// 用户店铺链接
    /// </summary>
    public class usercpshoplink : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 当前店铺的信息
        /// </summary>
        public Shopinfo shopinfo;
        /// <summary>
        /// 店铺友情链接信息
        /// </summary>
        public ShoplinkinfoCollection shoplinkinfolist;
        /// <summary>
        /// 选项名称
        /// </summary>
        public string item = "";
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

            shoplinkinfolist = ShopLinks.GetShopLinkByShopId(shopinfo.Shopid);
            //如果不是提交...
            if (ispost)
            { 
                string operation = DNTRequest.GetString("operation");

                if (operation == "")
                    operation = "add";

                switch (operation)
                {
                    case "add": //添加店铺友情链接
                        {
                            if (shoplinkinfolist.Count >= 16)
                            {
                                AddErrLine("目前系统允许您最多添加 20 个友情链接");
                                return;
                            }

                            string addusername = DNTRequest.GetString("username");
                            if (addusername == "")
                            {
                                AddErrLine("请输入店主姓名!");
                                return;
                            }

                            int adduserid = Users.GetUserId(addusername);
                            if (adduserid < 0)
                            {
                                AddErrLine("用户名不存在!");
                                return;
                            }
                            if (adduserid == userid)
                            {
                                AddErrLine("店主不能将本人店铺作为友情链接!");
                                return;
                            }

                            Shopinfo add_shopinfo = Shops.GetShopByUserId(Users.GetUserId(addusername));
                            if (add_shopinfo == null && add_shopinfo.Shopid <= 0)
                            {
                                AddErrLine("用户:" + addusername + " 未在本站开店，因此无法添加该友情链接");
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
                    case "delete": //删除店铺友情链接
                        {
                            string delshoplink = DNTRequest.GetString("shoplinkid");
                            if (delshoplink == "")
                            {
                                AddErrLine("您未选中友情链接");
                                return;
                            }
                            ShopLinks.DeleteShopLink(delshoplink);
                            break;
                        }
                    case "updatedisplayorder": //更新店铺友情链接显示顺序
                        {
                            foreach (Shoplinkinfo shoplinkinfo in shoplinkinfolist)
                            {
                                //当显示顺序值发生变化时，则更新相应的信息
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
                AddMsgLine("操作成功. <br />(<a href=\"usercpshoplink.aspx\">点击这里返回</a>)<br />");      
            }
        }
    }
}
