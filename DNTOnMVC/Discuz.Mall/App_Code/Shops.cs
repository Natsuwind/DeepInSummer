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
    /// ���̹��������
    /// </summary>
    public class Shops
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="shopinfo">������Ϣ</param>
        /// <returns>��������id</returns>
        public static int CreateShop(Shopinfo shopInfo)
        {
            return DbProvider.GetInstance().CreateShop(shopInfo);
        }


        /// <summary>
        /// ���µ���
        /// </summary>
        /// <param name="shopinfo">������Ϣ</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool UpdateShop(Shopinfo shopInfo)
        {
            return DbProvider.GetInstance().UpdateShop(shopInfo);
        }


        /// <summary>
        /// ��ȡָ��userid�ĵ�����Ϣ
        /// </summary>
        /// <param name="userid">�û�id</param>
        /// <returns>������Ϣ</returns>
        public static Shopinfo GetShopByUserId(int userId)
        {
            Shopinfo shopinfo = DTO.GetShopInfo(DbProvider.GetInstance().GetShopByUserId(userId));

            //���޸õ���ʱ, �򴴽��õ���
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
        /// ��ȡ���Ż��¿��ĵ�����Ϣ
        /// </summary>
        /// <param name="shoptype">���ŵ���(1:���ŵ���, 2 :�¿�����)</param>
        /// <returns>����json��Ϣ</returns>
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
        /// ����ת��������
        /// </summary>
        public class DTO
        {

            /// <summary>
            /// ��õ�����Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת��������</param>
            /// <returns>���ص�����Ϣ</returns>
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
            /// ��õ�����Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת�������ݱ�</param>
            /// <returns>���ص�����Ϣ</returns>
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
