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
    /// ����������������
    /// </summary>
    public class ShopThemes
    {

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="shopinfo">������Ϣ</param>
        /// <returns>������������id</returns>
        public static int CreateShop(Shopthemeinfo shopThemeInfo)
        {
            return DbProvider.GetInstance().CreateShopTheme(shopThemeInfo);
        }


        /// <summary>
        /// ���µ�������
        /// </summary>
        /// <param name="shopinfo">������Ϣ</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool UpdateShop(Shopthemeinfo shopThemeInfo)
        {
            return DbProvider.GetInstance().UpdateShopTheme(shopThemeInfo);
        }

        /// <summary>
        /// ��ȡָ������ID�ĵ��̷����Ϣ
        /// </summary>
        /// <param name="themeid">����ID</param>
        /// <returns>���̷����Ϣ</returns>
        public static Shopthemeinfo GetShopThemeByThemeId(int themeId)
        {
            return DTO.GetShopThemeInfo(DbProvider.GetInstance().GetShopThemeByThemeId(themeId));
        }

        /// <summary>
        /// �������������DataTable�ķ�ʽ���뻺��
        /// </summary>
        /// <returns>��Ʒ�����</returns>
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
        /// ��ȡ����������Ϣ(option��ʽ)
        /// </summary>
        /// <returns>����������Ϣ</returns>
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
        /// ����ת��������
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// ��õ���������Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת��������</param>
            /// <returns>���ص���������Ϣ</returns>
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
            /// ��õ���������Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת�������ݱ�</param>
            /// <returns>���ص���������Ϣ</returns>
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
