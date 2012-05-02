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
    /// 店铺友情链接管理操作类
    /// </summary>
    public class ShopLinks
    {
        /// <summary>
        /// 创建店铺友情链接
        /// </summary>
        /// <param name="shoplinkinfo">店铺友情链接信息</param>
        /// <returns>创建店铺友情链接id</returns>
        public static int CreateShopLink(Shoplinkinfo shopLinkInfo)
        {
            return DbProvider.GetInstance().CreateShopLink(shopLinkInfo);
        }


        /// <summary>
        /// 更新店铺友情链接
        /// </summary>
        /// <param name="shoplinkinfo">店铺友情链接信息</param>
        /// <returns>是否更新成功</returns>
        public static bool UpdateShopLink(Shoplinkinfo shopLinkInfo)
        {
            return DbProvider.GetInstance().UpdateShopLink(shopLinkInfo);
        }


         /// <summary>
        /// 获取指定店铺的友情链接信息集合
        /// </summary>
        /// <param name="shopid">店铺id</param>
        /// <returns>友情链接信息集合</returns>
        public static ShoplinkinfoCollection GetShopLinkByShopId(int shopId)
        {
            return DTO.GetShopLinkList(DbProvider.GetInstance().GetShopLinkByShopId(shopId));
        }

        /// <summary>
        /// 删除指定id的店铺友情链接信息
        /// </summary>
        /// <param name="shoplinkidlist">店铺链接id串(格式:1,2,3)</param>
        /// <returns>删除店铺数</returns>
        public static int DeleteShopLink(string shopLinkIdList)
        {
            if (!Utils.IsNumericList(shopLinkIdList))
                return -1;

            return DbProvider.GetInstance().DeleteShopLink(shopLinkIdList);
        }

        /// <summary>
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// 获得店铺友情链接信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回店铺友情链接信息</returns>
            public static Shoplinkinfo GetShopLinkInfo(IDataReader reader)
            {
                if (reader == null)
                    return null;

                Shoplinkinfo shopLinkInfo = new Shoplinkinfo();
                if (reader.Read())
                {
                    shopLinkInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                    shopLinkInfo.Displayorder = TypeConverter.ObjectToInt(reader["displayorder"]);
                    shopLinkInfo.Name = reader["name"].ToString().Trim();
                    shopLinkInfo.Linkshopid = TypeConverter.ObjectToInt(reader["linkshopid"]);
                    shopLinkInfo.Shopid = TypeConverter.ObjectToInt(reader["shopid"]);
                }
                reader.Close();
                return shopLinkInfo;
            }

             /// <summary>
            /// 获得店铺友情链接信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回店铺友情链接信息</returns>
            public static ShoplinkinfoCollection GetShopLinkList(IDataReader reader)
            {
                ShoplinkinfoCollection shopLinkInfoColl = new ShoplinkinfoCollection();

                while (reader.Read())
                {
                    Shoplinkinfo shoplinkinfo = new Shoplinkinfo();
                    shoplinkinfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                    shoplinkinfo.Displayorder = TypeConverter.ObjectToInt(reader["displayorder"]);
                    shoplinkinfo.Name = reader["name"].ToString().Trim();
                    shoplinkinfo.Linkshopid = TypeConverter.ObjectToInt(reader["linkshopid"]);
                    shoplinkinfo.Shopid = TypeConverter.ObjectToInt(reader["shopid"]);

                    shopLinkInfoColl.Add(shoplinkinfo);
                }
                reader.Close();
                return shopLinkInfoColl;
            }


            /// <summary>
            /// 获得店铺友情链接信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据表</param>
            /// <returns>返回店铺友情链接信息</returns>
            public static Shoplinkinfo[] GetShopLinkInfoArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Shoplinkinfo[] shoplinkarray = new Shoplinkinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    shoplinkarray[i] = new Shoplinkinfo();
                    shoplinkarray[i].Id = TypeConverter.ObjectToInt(dt.Rows[i]["id"]);
                    shoplinkarray[i].Displayorder = TypeConverter.ObjectToInt(dt.Rows[i]["displayorder"]);
                    shoplinkarray[i].Name = dt.Rows[i]["name"].ToString();
                    shoplinkarray[i].Linkshopid = TypeConverter.ObjectToInt(dt.Rows[i]["linkshopid"]);
                    shoplinkarray[i].Shopid = TypeConverter.ObjectToInt(dt.Rows[i]["shopid"]);
                }
                dt.Dispose();
                return shoplinkarray;
            }
        }
    }   
}
