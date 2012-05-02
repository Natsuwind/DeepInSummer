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
    /// 用户店铺分类
    /// </summary>
    public class usercpshopcategory : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前店铺下的商品分类数据
        /// </summary>
        public string shopcategorydata = "";
        /// <summary>
        /// 选项名称
        /// </summary>
        public string item = DNTRequest.GetString("item");
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 当前店铺的ID
        /// </summary>
        public int shopid = 0;
        /// <summary>
        /// 当前店铺的信息
        /// </summary>
        public Shopinfo shopinfo;
        /// <summary>
        /// 店铺的商品分类信息
        /// </summary>
        public Shopcategoryinfo shopcategoryinfo;
        /// <summary>
        /// 当前店铺下的商品分类选项
        /// </summary>
        public string categoryoptions;
        /// <summary>
        /// 店铺商品分类数据表
        /// </summary>
        public DataTable shopcategorydt = new DataTable();
        /// <summary>
        /// 店铺商品分类数据表记录数
        /// </summary>
        public int shopcategorydt_count = 0;
        /// <summary>
        /// 目标分类
        /// </summary>
        public Shopcategoryinfo targetshopcategoryinfo = null;
        #endregion

        protected override void ShowPage()
        {
            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            if (config.Enablemall <2)
            {
                AddErrLine("当前操作只有在开启商城(高级)模式时才可以使用!");
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
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                string operation = DNTRequest.GetFormString("operation");
                if (operation != "add")
                {
                    int shopcategoryid = DNTRequest.GetFormInt("categoryid", 0);
                    if (shopcategoryid <= 0)
                    {
                        AddErrLine("店铺商品分类参数无效<br />");
                        return;
                    }
                
                    shopcategoryinfo = ShopCategories.GetShopCategoryByCategoryId(shopcategoryid);
                }

                shopid = 0;
                item = DNTRequest.GetString("item");

                switch (operation)
                {
                    case "delete": //删除分类
                    {
                        if (shopcategoryinfo == null || shopcategoryinfo.Categoryid <= 0)
                        {
                            AddErrLine("要删除的店铺商品分类参数无效<br />");
                            return;
                        }
                        if (!ShopCategories.DeleteCategoryByCategoryId(shopcategoryinfo))
                        {
                            AddErrLine("对不起,当前节点下面还有子结点,因此不能删除<br />");
                            return;
                        }
                        break; 
                    }
                    case "edit": //编辑分类名称
                    {
                        if (shopcategoryinfo == null || shopcategoryinfo.Categoryid <=0)
                        {
                            AddErrLine("要修改的店铺商品分类参数无效<br />");
                            return;
                        }
                        string editname = DNTRequest.GetString("editcategoryname");
                        if (editname == "")
                        {
                            AddErrLine("店铺商品分类名称未变更或不能为空<br />");
                            return;
                        }
                        shopcategoryinfo.Name = editname;
                        ShopCategories.UpdateShopCategory(shopcategoryinfo);
                        break; 
                    }
                    case "add": //添加分类
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
                                AddErrLine("要添加到的目标分类参数无效<br />");
                                return;
                            }

                            targetshopcategoryinfo = ShopCategories.GetShopCategoryByCategoryId(targetcategoryid);
                            if (targetshopcategoryinfo == null || targetshopcategoryinfo.Categoryid <= 0)
                            {
                                AddErrLine("要添加到的目标分类参数无效<br />");
                                return;
                            }
                        }

                        ShopCategories.CreateShopCategory(shopcategoryinfo, targetshopcategoryinfo, addtype);
                        break; 
                    }
                    case "move": //移动分类
                    {
                        int targetcategoryid = DNTRequest.GetFormInt("targetcategoryid", 0);

                        if (targetcategoryid <= 0)
                        {
                            AddErrLine("要移动到的目标分类参数无效<br />");
                            return;
                        }

                        targetshopcategoryinfo = ShopCategories.GetShopCategoryByCategoryId(targetcategoryid);
                        if (targetshopcategoryinfo == null || targetshopcategoryinfo.Categoryid <= 0)
                        {
                            AddErrLine("要移动到的目标分类参数无效<br />");
                            return;
                        }
                        string target_parentidlist = "," + targetshopcategoryinfo.Parentidlist.Trim() + ",";
                        if (target_parentidlist.IndexOf("," + shopcategoryinfo.Categoryid.ToString() + ",") > 0)
                        {
                            AddErrLine("不能将当前分类移动到其子分类<br />");
                            return;
                        }
                        ShopCategories.MoveShopCategory(shopcategoryinfo, targetshopcategoryinfo, DNTRequest.GetInt("isaschildnode",0) == 1 ? true : false);
                        break; 
                    }
                }

                SetUrl("usercpshopcategory.aspx?item=" + item);
                SetMetaRefresh();
                AddMsgLine("操作成功. <br />(<a href=\"usercpmygoods.aspx?item=" + item + "\">点击这里返回</a>)<br />");
            }
        }
    }
}