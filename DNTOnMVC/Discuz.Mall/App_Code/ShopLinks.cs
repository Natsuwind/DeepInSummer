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
    /// �����������ӹ��������
    /// </summary>
    public class ShopLinks
    {
        /// <summary>
        /// ����������������
        /// </summary>
        /// <param name="shoplinkinfo">��������������Ϣ</param>
        /// <returns>����������������id</returns>
        public static int CreateShopLink(Shoplinkinfo shopLinkInfo)
        {
            return DbProvider.GetInstance().CreateShopLink(shopLinkInfo);
        }


        /// <summary>
        /// ���µ�����������
        /// </summary>
        /// <param name="shoplinkinfo">��������������Ϣ</param>
        /// <returns>�Ƿ���³ɹ�</returns>
        public static bool UpdateShopLink(Shoplinkinfo shopLinkInfo)
        {
            return DbProvider.GetInstance().UpdateShopLink(shopLinkInfo);
        }


         /// <summary>
        /// ��ȡָ�����̵�����������Ϣ����
        /// </summary>
        /// <param name="shopid">����id</param>
        /// <returns>����������Ϣ����</returns>
        public static ShoplinkinfoCollection GetShopLinkByShopId(int shopId)
        {
            return DTO.GetShopLinkList(DbProvider.GetInstance().GetShopLinkByShopId(shopId));
        }

        /// <summary>
        /// ɾ��ָ��id�ĵ�������������Ϣ
        /// </summary>
        /// <param name="shoplinkidlist">��������id��(��ʽ:1,2,3)</param>
        /// <returns>ɾ��������</returns>
        public static int DeleteShopLink(string shopLinkIdList)
        {
            if (!Utils.IsNumericList(shopLinkIdList))
                return -1;

            return DbProvider.GetInstance().DeleteShopLink(shopLinkIdList);
        }

        /// <summary>
        /// ����ת��������
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// ��õ�������������Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת��������</param>
            /// <returns>���ص�������������Ϣ</returns>
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
            /// ��õ�������������Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת��������</param>
            /// <returns>���ص�������������Ϣ</returns>
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
            /// ��õ�������������Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת�������ݱ�</param>
            /// <returns>���ص�������������Ϣ</returns>
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
