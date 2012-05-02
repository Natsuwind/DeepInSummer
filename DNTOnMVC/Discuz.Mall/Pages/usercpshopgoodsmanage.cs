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
    /// 用户店铺商品管理
    /// </summary>
    public class usercpshopgoodsmanage : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前店铺下的商品分类数据
        /// </summary>
        public string shopcategorydata = "";
        /// <summary>
        /// 商品列表
        /// </summary>
        public GoodsinfoCollection goodslist;
        /// <summary>
        /// 选项名称
        /// </summary>
        public string item = DNTRequest.GetString("item");
        /// <summary>
        /// 分页总数
        /// </summary>
        public int pagecount = 1;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 商品id字符串(格式:1,2,3)
        /// </summary>
        public string goodsidlist = DNTRequest.GetString("goodsid");
        /// <summary>
        /// 记录总数
        /// </summary>
        public int reccount = 0;
        /// <summary>
        /// 当前店铺的信息
        /// </summary>
        public Shopinfo shopinfo;
        /// <summary>
        /// 店铺商品分类数据表
        /// </summary>
        public DataTable shopcategorydt = new DataTable();
        /// <summary>
        /// 店铺商品分类数据表记录数
        /// </summary>
        public int shopcategorydt_count = 0;
        /// <summary>
        /// 当前店铺下的商品分类选项
        /// </summary>
        public string categoryoptions;
        /// <summary>
        /// 店铺商品分类id
        /// </summary>
        public int shopgoodscategoryid = DNTRequest.GetInt("shopgoodscategoryid", 0);
        /// <summary>
        /// 推荐商品列表
        /// </summary>
        public GoodsinfoCollection recommendgoodslist;
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

            if (item == "")
                item = "shopcategory";

            if(item == "recommend")
                recommendgoodslist = Goods.GetGoodsRecommendManageList(userid, 6, 1, "");
            
            reccount = (shopgoodscategoryid <= 0) ? Goods.GetGoodsCountBySellerUid(userid, true) : Goods.GetGoodsCountByShopCategory(shopgoodscategoryid,"");

            // 得到分页大小设置
            int pagesize = 10;

            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;

            //获取总页数
            pagecount = reccount % pagesize == 0 ? reccount / pagesize : reccount / pagesize + 1;
            if (pagecount == 0)
                pagecount = 1;

            if (pageid > pagecount)
                pageid = pagecount;

            shopinfo = Shops.GetShopByUserId(user.Uid);
            //如果不是提交...
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
                    case "movecategory": //移动到商品分类
                        {
                            if (goodsidlist == "")
                            {
                                AddErrLine("你未选中任何商品");
                                return;
                            }

                            int selectcategoryid = DNTRequest.GetInt("selectcategoryid", 0);
                            if (selectcategoryid <= 0)
                            {
                                AddErrLine("你未选择要移动到的商品分类");
                                return;
                            }
                            if (Goods.IsSeller(goodsidlist, userid))
                            {
                                if (Goods.MoveGoodsShopCategory(goodsidlist, selectcategoryid) > 0)
                                {
                                    SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                                    SetMetaRefresh();
                                    AddMsgLine("操作成功. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">点击这里返回</a>)<br />");
                                }
                                else
                                {
                                    AddErrLine("商品参数信息无效或所选商品已在该分类下");
                                    return;
                                }
                            }
                            else
                            {
                                AddErrLine("你不是当前商品的卖家，因此无法移动该商品到指定的分类");
                                return;
                            }
                            break;
                        }
                    case "removecategory": //移除商品分类
                        {
                            int removeshopgoodscategoryid = DNTRequest.GetInt("removeshopgoodscategoryid", 0);

                            int removegoodsid = DNTRequest.GetInt("removegoodsid", 0);

                            if (removeshopgoodscategoryid <= 0 || removegoodsid <= 0)
                            {
                                AddErrLine("移除分类信息错误");
                                return;
                            }

                            if (Goods.IsSeller(removegoodsid.ToString(), userid))
                            {
                                if (Goods.RemoveGoodsShopCategory(removegoodsid, removeshopgoodscategoryid) > 0)
                                {
                                    SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                                    SetMetaRefresh();
                                    AddMsgLine("操作成功. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">点击这里返回</a>)<br />");
                                }
                                else
                                {
                                    AddErrLine("商品参数信息无效或所选商品已在该分类下");
                                    return;
                                }
                            }
                            else
                            {
                                AddErrLine("您不是当前商品的卖家，因此无法移除该商品的分类");
                                return;
                            }
                            break;
                        }
                    case "recommend": //推荐商品
                        {
                            if (goodsidlist == "")
                            {
                                AddErrLine("您未选中任何商品");
                                return;
                            }
                            if ((recommendgoodslist.Count + goodsidlist.Split(',').Length) > 5)
                            {
                                AddErrLine("您推荐的商品总数已大于5, 因为无法进行推荐");
                                return;
                            }

                            if (Goods.IsSeller(goodsidlist, userid))
                            {
                                Goods.RecommendGoods(goodsidlist);
                                SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                                SetMetaRefresh();
                                AddMsgLine("操作成功. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">点击这里返回</a>)<br />");
                            }
                            else
                            {
                                AddErrLine("您不是当前商品的卖家，因此无法推荐该商品");
                                return;
                            }
                            break;
                        }
                    case "cancelrecommend": //取消推荐商品
                        {
                            goodsidlist = DNTRequest.GetString("cancelrecommendgoodsid");
                            if (goodsidlist == "")
                            {
                                AddErrLine("您未选中任何商品");
                                return;
                            }

                            if (Goods.IsSeller(goodsidlist, userid))
                            {
                                Goods.CancelRecommendGoods(goodsidlist);
                                SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                                SetMetaRefresh();
                                AddMsgLine("操作成功. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">点击这里返回</a>)<br />");
                            }
                            else
                            {
                                AddErrLine("你不是当前商品的卖家，因此无法取消推荐该商品");
                                return;
                            }
                            break;
                        }
                    case "updatedisplayorder": //更新商品显示顺序
                        {
                            foreach (Goodsinfo goodsinfo in recommendgoodslist)
                            {
                                //当显示顺序值发生变化时，则更新相应的商品信息
                                if (goodsinfo.Displayorder != DNTRequest.GetInt("displayorder_" + goodsinfo.Goodsid, 0))
                                {
                                    goodsinfo.Displayorder = DNTRequest.GetInt("displayorder_" + goodsinfo.Goodsid, 0);
                                    Goods.UpdateGoods(goodsinfo);
                                }
                            }

                            SetUrl("usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid);
                            SetMetaRefresh();
                            AddMsgLine("操作成功. <br />(<a href=\"usercpshopgoodsmanage.aspx?item=" + item + "&shopgoodscategoryid=" + shopgoodscategoryid + "\">点击这里返回</a>)<br />");
                            break;
                        }
                }
            }
        }
    }
}
