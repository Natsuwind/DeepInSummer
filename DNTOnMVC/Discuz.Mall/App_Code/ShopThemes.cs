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
    /// 店铺主题管理操作类
    /// </summary>
    public class ShopThemes
    {

        /// <summary>
        /// 创建店铺主题
        /// </summary>
        /// <param name="shopinfo">店铺信息</param>
        /// <returns>创建店铺主题id</returns>
        public static int CreateShop(Shopthemeinfo shopThemeInfo)
        {
            return DbProvider.GetInstance().CreateShopTheme(shopThemeInfo);
        }


        /// <summary>
        /// 更新店铺主题
        /// </summary>
        /// <param name="shopinfo">店铺信息</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateShop(Shopthemeinfo shopThemeInfo)
        {
            return DbProvider.GetInstance().UpdateShopTheme(shopThemeInfo);
        }

        /// <summary>
        /// 获取指定主题ID的店铺风格信息
        /// </summary>
        /// <param name="themeid">主题ID</param>
        /// <returns>店铺风格信息</returns>
        public static Shopthemeinfo GetShopThemeByThemeId(int themeId)
        {
            return DTO.GetShopThemeInfo(DbProvider.GetInstance().GetShopThemeByThemeId(themeId));
        }

        /// <summary>
        /// 将店铺主题表以DataTable的方式存入缓存
        /// </summary>
        /// <returns>商品分类表</returns>
        public static DataTable GetShopThemesTable()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Mall/MallSetting/ShopTheme") as DataTable;
            if (dt == null)
            {
                dt = DbProvider.GetInstance().GetShopThemes();
                cache.AddObject("/Mall/MallSetting/ShopTheme", dt);
            }
            return dt;
        }

        /// <summary>
        /// 获取店铺主题信息(option格式)
        /// </summary>
        /// <returns>店铺主题信息</returns>
        public static string GetShopThemeOption()
        {
            StringBuilder sb_category = new StringBuilder();
            foreach (DataRow dr in GetShopThemesTable().Rows)
            {
                sb_category.AppendFormat("<option value=\"{0}\">{1}</option>", dr["themeid"], dr["name"].ToString().Trim());     
            }
            return sb_category.ToString();
        }


        /// <summary>
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// 获得店铺主题信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回店铺主题信息</returns>
            public static Shopthemeinfo GetShopThemeInfo(IDataReader reader)
            {
                Shopthemeinfo shopThemeInfo = null;
                if (reader.Read())
                {
                    shopThemeInfo = new Shopthemeinfo();
                    shopThemeInfo.Themeid = TypeConverter.ObjectToInt(reader["themeid"]);
                    shopThemeInfo.Directory = reader["directory"].ToString().Trim();
                    shopThemeInfo.Name = reader["name"].ToString().Trim();
                    shopThemeInfo.Author = reader["author"].ToString().Trim();
                    shopThemeInfo.Createdate = reader["createdate"].ToString().Trim();
                    shopThemeInfo.Copyright = reader["copyright"].ToString().Trim();

                    reader.Close();
                }
                return shopThemeInfo;
            }

            /// <summary>
            /// 获得店铺主题信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据表</param>
            /// <returns>返回店铺主题信息</returns>
            public static Shopthemeinfo[] GetShopThemeInfoArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Shopthemeinfo[] shopThemeInfoArray = new Shopthemeinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    shopThemeInfoArray[i] = new Shopthemeinfo();
                    shopThemeInfoArray[i].Themeid = TypeConverter.ObjectToInt(dt.Rows[i]["themeid"]);
                    shopThemeInfoArray[i].Directory = dt.Rows[i]["directory"].ToString();
                    shopThemeInfoArray[i].Name = dt.Rows[i]["name"].ToString();
                    shopThemeInfoArray[i].Author = dt.Rows[i]["author"].ToString();
                    shopThemeInfoArray[i].Createdate = dt.Rows[i]["createdate"].ToString();
                    shopThemeInfoArray[i].Copyright = dt.Rows[i]["copyright"].ToString();
                }
                dt.Dispose();
                return shopThemeInfoArray;
            }
        }
    }
}
