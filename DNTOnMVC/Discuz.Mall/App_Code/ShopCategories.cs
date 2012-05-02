using System;
using System.Data;
using System.IO;
using System.Text;

using Discuz.Common;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Mall.Data;
using Discuz.Forum;

namespace Discuz.Mall
{
    /// <summary>
    /// 店铺分类管理操作类
    /// </summary>
    public class ShopCategories
    {
        /// <summary>
        /// 创建店铺分类
        /// </summary>
        /// <param name="shopcategoryinfo">店铺分类信息</param>
        /// <param name="targetshopcategoryinfo">要加入的目标分类信息</param>
        /// <param name="addtype">添加方式(1:作为同级分类 2:作为子分类 其它:根结店)</param>
        /// <returns>创建店铺分类id</returns>
        public static int CreateShopCategory(Shopcategoryinfo shopCategoryInfo, Shopcategoryinfo targetShopCategoryInfo, int addType)
        {
            switch(addType)
            {
                case 1: //作为同级分类
                    {
                        shopCategoryInfo.Parentid = targetShopCategoryInfo.Parentid;
                        shopCategoryInfo.Parentidlist = targetShopCategoryInfo.Parentidlist;
                        shopCategoryInfo.Layer = targetShopCategoryInfo.Layer;
                        break;
                    }
                case 2: //作为子分类
                    {
                        shopCategoryInfo.Parentid = targetShopCategoryInfo.Categoryid;
                        shopCategoryInfo.Parentidlist = targetShopCategoryInfo.Parentidlist == "0" ? targetShopCategoryInfo.Categoryid.ToString() : targetShopCategoryInfo.Parentidlist + "," + targetShopCategoryInfo.Categoryid;
                        shopCategoryInfo.Layer = targetShopCategoryInfo.Layer + 1;
                        break;
                    }
                default:
                    {
                        shopCategoryInfo.Parentid = 0;
                        shopCategoryInfo.Parentidlist = "0";
                        shopCategoryInfo.Layer = 0;
                        break;
                    }
            }
            return CreateShopCategory(shopCategoryInfo);
        }

        /// <summary>
        /// 创建店铺分类
        /// </summary>
        /// <param name="shopcategoryinfo">店铺分类信息</param>
        /// <returns>创建店铺分类id</returns>
        public static int CreateShopCategory(Shopcategoryinfo shopCategoryInfo)
        {
            int returnval =  DbProvider.GetInstance().CreateShopCategory(shopCategoryInfo);
            SetShopCategoryDispalyorder(shopCategoryInfo.Shopid);
            return returnval;
        }

        /// <summary>
        /// 获取指定店铺的商品分类
        /// </summary>
        /// <param name="shopid">店铺id</param>
        /// <returns>店铺商品分类表</returns>
        public static DataTable GetShopCategoryTable(int shopId)
        {
            return DbProvider.GetInstance().GetShopCategoryTableToJson(shopId);
        }

        /// <summary>
        /// 获取店铺的商品类型数据(json格式)
        /// </summary>
        /// <param name="shopid">店铺id</param>
        /// <returns>店铺的商品类型数据</returns>
        public static string GetShopCategoryJson(DataTable dt)
        {
            StringBuilder sb_category = new StringBuilder();
            sb_category.Append(Utils.DataTableToJSON(dt));
            return sb_category.ToString();
        }

        /// <summary>
        /// 获取店铺的商品类型数据(option格式)
        /// </summary>
        /// <param name="shopid">店铺id</param>
        /// <returns>商品类型数据</returns>
        public static string GetShopCategoryOption(DataTable dt, bool optGroup)
        {
            StringBuilder sb_category = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                if (optGroup && dr["childcount"].ToString() != "0")
                    sb_category.AppendFormat("<optgroup label=\"{0}\"></optgroup>", dr["name"]);
                else
                    sb_category.AppendFormat("<option value=\"{0}\">{1}{2}</option>", 
                                             dr["categoryid"], 
                                             Utils.GetSpacesString(TypeConverter.ObjectToInt(dr["layer"])), 
                                             dr["name"].ToString().Trim());
            }
            return sb_category.ToString();
        }

        /// <summary>
        /// 获取指定分类id的店铺商品类型数据
        /// </summary>
        /// <param name="categoryid">分类id</param>
        /// <returns>店铺商品类型信息</returns>
        public static Shopcategoryinfo GetShopCategoryByCategoryId(int categoryId)
        {
            return DTO.GetShopCategoryInfo(DbProvider.GetInstance().GetShopCategoryByCategoryId(categoryId));
        }

        /// <summary>
        /// 删除指定的店铺商品分类
        /// </summary>
        /// <param name="categoryid">要删除的店铺商品分类id</param>
        /// <returns>是否删除成功</returns>
        public static bool DeleteCategoryByCategoryId(Shopcategoryinfo shopCategoryInfo)
        {
            if (DbProvider.GetInstance().IsExistSubShopCategory(shopCategoryInfo))
                return false;

            DbProvider.GetInstance().DeleteShopCategory(shopCategoryInfo);
            return true;
        }

        /// <summary>
        /// 移动商品分类
        /// </summary>
        /// <param name="shopcategoryinfo">源店铺商品分类</param>
        /// <param name="targetshopcategoryinfo">目标店铺商品分类</param>
        /// <param name="isaschildnode">是否作为子节点</param>
        /// <returns>是否移动成功</returns>
        public static bool MoveShopCategory(Shopcategoryinfo shopCategoryInfo, Shopcategoryinfo targetShopCategoryInfo, bool isAsChildNode)
        {
            DbProvider.GetInstance().MovingShopCategoryPos(shopCategoryInfo, targetShopCategoryInfo, isAsChildNode);
            SetShopCategoryDispalyorder(targetShopCategoryInfo.Shopid);
            return true;
        }

        /// <summary>
        /// 设置店铺商品分类显示顺序
        /// </summary>
        public static void SetShopCategoryDispalyorder(int shopId)
        {
            DataTable dt = DbProvider.GetInstance().GetShopCategoryByShopId(shopId);

            //更新店铺商品分类下的子分类数
            foreach (DataRow dr in dt.Rows)
            {
                DbProvider.GetInstance().UpdateShopCategoryChildCount(dt.Select("parentid=" + dr["categoryid"]).Length, TypeConverter.ObjectToInt(dr["categoryid"]));
            }

            if (dt.Rows.Count == 1) 
                return;

            int displayorder = 1;
            string categoryidlist;
            foreach (DataRow dr in dt.Select("parentid=0"))
            {
                if (dr["parentid"].ToString() == "0")
                {
                    ChildNode = "0";
                    categoryidlist = ("," + FindChildNode(dr["categoryid"].ToString())).Replace(",0,", "");

                    foreach (string categoryid in categoryidlist.Split(','))
                    {
                        DbProvider.GetInstance().UpdateShopCategoryDisplayOrder(displayorder, TypeConverter.ObjectToInt(categoryid));
                        displayorder++;
                    }
                }
            }
        }


        #region  递归指定论坛版块下的所有子版块

        public static string ChildNode = "0";

        /// <summary>
        /// 递归所有子节点并返回字符串
        /// </summary>
        /// <param name="correntfid">当前</param>
        /// <returns>子版块的集合,格式:1,2,3,4,</returns>
        public static string FindChildNode(string categoryid)
        {
            lock (ChildNode)
            {
                DataTable dt = DbProvider.GetInstance().GetCategoryidInShopByParentid(int.Parse(categoryid));

                ChildNode = ChildNode + "," + categoryid;

                if (dt.Rows.Count > 0)
                {
                    //有子节点
                    foreach (DataRow dr in dt.Rows)
                    {
                        FindChildNode(dr["categoryid"].ToString());
                    }
                    dt.Dispose();
                }
                else
                {
                    dt.Dispose();
                }
                return ChildNode;
            }
        }

        #endregion

        /// <summary>
        /// 更新店铺分类
        /// </summary>
        /// <param name="shopcategoryinfo">店铺分类信息</param>
        /// <returns>是否更新成功</returns>
        public static bool UpdateShopCategory(Shopcategoryinfo shopCategoryInfo)
        {
            return DbProvider.GetInstance().UpdateShopCategory(shopCategoryInfo);
        }

        /// <summary>
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {

            /// <summary>
            /// 获得店铺分类信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回店铺分类信息</returns>
            public static Shopcategoryinfo GetShopCategoryInfo(IDataReader reader)
            {
                Shopcategoryinfo shopCategoryInfo = null;
                if (reader.Read())
                {
                    shopCategoryInfo = new Shopcategoryinfo();
                    shopCategoryInfo.Categoryid = TypeConverter.ObjectToInt(reader["categoryid"]);
                    shopCategoryInfo.Parentid = TypeConverter.ObjectToInt(reader["parentid"]);
                    shopCategoryInfo.Parentidlist = reader["parentidlist"].ToString().Trim();
                    shopCategoryInfo.Layer = TypeConverter.ObjectToInt(reader["layer"]);
                    shopCategoryInfo.Childcount = TypeConverter.ObjectToInt(reader["childcount"]);
                    shopCategoryInfo.Syscategoryid = TypeConverter.ObjectToInt(reader["syscategoryid"]);
                    shopCategoryInfo.Name = reader["name"].ToString().Trim();
                    shopCategoryInfo.Categorypic = reader["categorypic"].ToString().Trim();
                    shopCategoryInfo.Shopid = TypeConverter.ObjectToInt(reader["shopid"]);
                    shopCategoryInfo.Displayorder = TypeConverter.ObjectToInt(reader["displayorder"]);

                    reader.Close();
                }
                return shopCategoryInfo;
            }

            /// <summary>
            /// 获得店铺分类信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据表</param>
            /// <returns>返回店铺分类信息</returns>
            public static Shopcategoryinfo[] GetShopCategoryArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Shopcategoryinfo[] shopcategoryinfoarray = new Shopcategoryinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    shopcategoryinfoarray[i] = new Shopcategoryinfo();
                    shopcategoryinfoarray[i].Categoryid = TypeConverter.ObjectToInt(dt.Rows[i]["categoryid"]);
                    shopcategoryinfoarray[i].Parentid = TypeConverter.ObjectToInt(dt.Rows[i]["parentid"]);
                    shopcategoryinfoarray[i].Parentidlist = dt.Rows[i]["parentidlist"].ToString().Trim();
                    shopcategoryinfoarray[i].Layer = TypeConverter.ObjectToInt(dt.Rows[i]["layer"]);
                    shopcategoryinfoarray[i].Childcount = TypeConverter.ObjectToInt(dt.Rows[i]["childcount"]);
                    shopcategoryinfoarray[i].Syscategoryid = TypeConverter.ObjectToInt(dt.Rows[i]["syscategoryid"]);
                    shopcategoryinfoarray[i].Name = dt.Rows[i]["name"].ToString();
                    shopcategoryinfoarray[i].Categorypic = dt.Rows[i]["categorypic"].ToString();
                    shopcategoryinfoarray[i].Shopid = TypeConverter.ObjectToInt(dt.Rows[i]["shopid"]);
                    shopcategoryinfoarray[i].Displayorder = TypeConverter.ObjectToInt(dt.Rows[i]["displayorder"]);
                }
                dt.Dispose();
                return shopcategoryinfoarray;
            }

        }
    }

    
}
