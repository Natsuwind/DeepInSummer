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
    /// 店铺管理操作类
    /// </summary>
    public class Shops
    {
        /// <summary>
        /// 创建店铺
        /// </summary>
        /// <param name="shopinfo">店铺信息</param>
        /// <returns>创建店铺id</returns>
        public static int CreateShop(Shopinfo shopInfo)
        {
            return DbProvider.GetInstance().CreateShop(shopInfo);
        }


        /// <summary>
        /// 更新店铺
        /// </summary>
        /// <param name="shopinfo">店铺信息</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateShop(Shopinfo shopInfo)
        {
            return DbProvider.GetInstance().UpdateShop(shopInfo);
        }


        /// <summary>
        /// 获取指定userid的店铺信息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns>店铺信息</returns>
        public static Shopinfo GetShopByUserId(int userId)
        {
            Shopinfo shopinfo = DTO.GetShopInfo(DbProvider.GetInstance().GetShopByUserId(userId));

            //当无该店铺时, 则创建该店铺
            if (shopinfo == null)
            {
                shopinfo = new Shopinfo();
                shopinfo.Bulletin = "";
                shopinfo.Createdatetime = DateTime.Now;
                shopinfo.Introduce = "";
                shopinfo.Lid = -1;
                shopinfo.Locus = "";
                shopinfo.Logo = "";
                shopinfo.Shopname = "";
                shopinfo.Themeid = 0;
                shopinfo.Themepath = "";
                shopinfo.Uid = userId;
                shopinfo.Username = "";
                Shops.CreateShop(shopinfo);

                shopinfo = Shops.GetShopByUserId(userId);
            }
            return shopinfo;
        }
        
        /// <summary>
        /// 获取热门或新开的店铺信息
        /// </summary>
        /// <param name="shoptype">热门店铺(1:热门店铺, 2 :新开店铺)</param>
        /// <returns>店铺json信息</returns>
        public static string GetShopInfoJson(int shopType)
        {
            StringBuilder sb_shops = new StringBuilder("[");
            IDataReader iDataReader = DbProvider.GetInstance().GetHotOrNewShops(shopType, 4);

            while (iDataReader.Read())
            {
                sb_shops.Append(string.Format("{{'shopid' : {0}, 'logo' : '{1}', 'shopname' : '{2}', 'uid' : {3}, 'username' : '{4}'}},",
                    iDataReader["shopid"],
                    iDataReader["logo"],
                    iDataReader["shopname"].ToString().Trim(),
                    iDataReader["uid"].ToString().Trim(),
                    iDataReader["username"].ToString().ToLower()));
            }
            iDataReader.Close();

            if (sb_shops.ToString().EndsWith(","))
                sb_shops.Remove(sb_shops.Length - 1, 1);

            sb_shops.Append("]");
            return sb_shops.ToString();
        }

        /// <summary>
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {

            /// <summary>
            /// 获得店铺信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回店铺信息</returns>
            public static Shopinfo GetShopInfo(IDataReader reader)
            {
                Shopinfo shopInfo = null;
                if (reader.Read())
                {
                    shopInfo = new Shopinfo();
                    shopInfo.Shopid = TypeConverter.ObjectToInt(reader["shopid"].ToString());
                    shopInfo.Logo = reader["logo"].ToString().Trim();
                    shopInfo.Shopname = reader["shopname"].ToString().Trim();
                    shopInfo.Themeid = TypeConverter.ObjectToInt(reader["themeid"].ToString());
                    shopInfo.Themepath = reader["themepath"].ToString().Trim();
                    shopInfo.Uid = TypeConverter.ObjectToInt(reader["uid"].ToString());
                    shopInfo.Username = reader["username"].ToString().Trim();
                    shopInfo.Introduce = reader["introduce"].ToString().Trim();
                    shopInfo.Lid = TypeConverter.ObjectToInt(reader["lid"].ToString());
                    shopInfo.Locus = reader["locus"].ToString().Trim();
                    shopInfo.Bulletin = reader["bulletin"].ToString().Trim();
                    shopInfo.Createdatetime = Convert.ToDateTime(reader["createdatetime"].ToString());
                    shopInfo.Invisible = TypeConverter.ObjectToInt(reader["invisible"].ToString());
                    shopInfo.Viewcount = TypeConverter.ObjectToInt(reader["viewcount"].ToString());

                    reader.Close();
                }
                return shopInfo;
            }

            /// <summary>
            /// 获得店铺信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据表</param>
            /// <returns>返回店铺信息</returns>
            public static Shopinfo[] GetShopInfoArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Shopinfo[] shopInfoArray = new Shopinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    shopInfoArray[i] = new Shopinfo();
                    shopInfoArray[i].Shopid = TypeConverter.ObjectToInt(dt.Rows[i]["shopid"]);
                    shopInfoArray[i].Logo = dt.Rows[i]["logo"].ToString();
                    shopInfoArray[i].Shopname = dt.Rows[i]["shopname"].ToString();
                    shopInfoArray[i].Themeid = TypeConverter.ObjectToInt(dt.Rows[i]["themeid"]);
                    shopInfoArray[i].Themepath = dt.Rows[i]["themepath"].ToString();
                    shopInfoArray[i].Uid = TypeConverter.ObjectToInt(dt.Rows[i]["uid"]);
                    shopInfoArray[i].Username = dt.Rows[i]["username"].ToString();
                    shopInfoArray[i].Introduce = dt.Rows[i]["introduce"].ToString();
                    shopInfoArray[i].Lid = TypeConverter.ObjectToInt(dt.Rows[i]["lid"]);
                    shopInfoArray[i].Locus = dt.Rows[i]["locus"].ToString();
                    shopInfoArray[i].Bulletin = dt.Rows[i]["bulletin"].ToString();
                    shopInfoArray[i].Createdatetime = Convert.ToDateTime(dt.Rows[i]["createdatetime"].ToString());
                    shopInfoArray[i].Invisible = TypeConverter.ObjectToInt(dt.Rows[i]["invisible"]);
                    shopInfoArray[i].Viewcount = TypeConverter.ObjectToInt(dt.Rows[i]["viewcount"]);
                }
                dt.Dispose();
                return shopInfoArray;
            }
        }
    }
}
