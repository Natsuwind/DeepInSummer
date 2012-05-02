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
    /// ���̷�����������
    /// </summary>
    public class ShopCategories
    {
        /// <summary>
        /// �������̷���
        /// </summary>
        /// <param name="shopcategoryinfo">���̷�����Ϣ</param>
        /// <param name="targetshopcategoryinfo">Ҫ�����Ŀ�������Ϣ</param>
        /// <param name="addtype">��ӷ�ʽ(1:��Ϊͬ������ 2:��Ϊ�ӷ��� ����:�����)</param>
        /// <returns>�������̷���id</returns>
        public static int CreateShopCategory(Shopcategoryinfo shopCategoryInfo, Shopcategoryinfo targetShopCategoryInfo, int addType)
        {
            switch(addType)
            {
                case 1: //��Ϊͬ������
                    {
                        shopCategoryInfo.Parentid = targetShopCategoryInfo.Parentid;
                        shopCategoryInfo.Parentidlist = targetShopCategoryInfo.Parentidlist;
                        shopCategoryInfo.Layer = targetShopCategoryInfo.Layer;
                        break;
                    }
                case 2: //��Ϊ�ӷ���
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
        /// �������̷���
        /// </summary>
        /// <param name="shopcategoryinfo">���̷�����Ϣ</param>
        /// <returns>�������̷���id</returns>
        public static int CreateShopCategory(Shopcategoryinfo shopCategoryInfo)
        {
            int returnval =  DbProvider.GetInstance().CreateShopCategory(shopCategoryInfo);
            SetShopCategoryDispalyorder(shopCategoryInfo.Shopid);
            return returnval;
        }

        /// <summary>
        /// ��ȡָ�����̵���Ʒ����
        /// </summary>
        /// <param name="shopid">����id</param>
        /// <returns>������Ʒ�����</returns>
        public static DataTable GetShopCategoryTable(int shopId)
        {
            return DbProvider.GetInstance().GetShopCategoryTableToJson(shopId);
        }

        /// <summary>
        /// ��ȡ���̵���Ʒ��������(json��ʽ)
        /// </summary>
        /// <param name="shopid">����id</param>
        /// <returns>���̵���Ʒ��������</returns>
        public static string GetShopCategoryJson(DataTable dt)
        {
            StringBuilder sb_category = new StringBuilder();
            sb_category.Append(Utils.DataTableToJSON(dt));
            return sb_category.ToString();
        }

        /// <summary>
        /// ��ȡ���̵���Ʒ��������(option��ʽ)
        /// </summary>
        /// <param name="shopid">����id</param>
        /// <returns>��Ʒ��������</returns>
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
        /// ��ȡָ������id�ĵ�����Ʒ��������
        /// </summary>
        /// <param name="categoryid">����id</param>
        /// <returns>������Ʒ������Ϣ</returns>
        public static Shopcategoryinfo GetShopCategoryByCategoryId(int categoryId)
        {
            return DTO.GetShopCategoryInfo(DbProvider.GetInstance().GetShopCategoryByCategoryId(categoryId));
        }

        /// <summary>
        /// ɾ��ָ���ĵ�����Ʒ����
        /// </summary>
        /// <param name="categoryid">Ҫɾ���ĵ�����Ʒ����id</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
        public static bool DeleteCategoryByCategoryId(Shopcategoryinfo shopCategoryInfo)
        {
            if (DbProvider.GetInstance().IsExistSubShopCategory(shopCategoryInfo))
                return false;

            DbProvider.GetInstance().DeleteShopCategory(shopCategoryInfo);
            return true;
        }

        /// <summary>
        /// �ƶ���Ʒ����
        /// </summary>
        /// <param name="shopcategoryinfo">Դ������Ʒ����</param>
        /// <param name="targetshopcategoryinfo">Ŀ�������Ʒ����</param>
        /// <param name="isaschildnode">�Ƿ���Ϊ�ӽڵ�</param>
        /// <returns>�Ƿ��ƶ��ɹ�</returns>
        public static bool MoveShopCategory(Shopcategoryinfo shopCategoryInfo, Shopcategoryinfo targetShopCategoryInfo, bool isAsChildNode)
        {
            DbProvider.GetInstance().MovingShopCategoryPos(shopCategoryInfo, targetShopCategoryInfo, isAsChildNode);
            SetShopCategoryDispalyorder(targetShopCategoryInfo.Shopid);
            return true;
        }

        /// <summary>
        /// ���õ�����Ʒ������ʾ˳��
        /// </summary>
        public static void SetShopCategoryDispalyorder(int shopId)
        {
            DataTable dt = DbProvider.GetInstance().GetShopCategoryByShopId(shopId);

            //���µ�����Ʒ�����µ��ӷ�����
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


        #region  �ݹ�ָ����̳����µ������Ӱ��

        public static string ChildNode = "0";

        /// <summary>
        /// �ݹ������ӽڵ㲢�����ַ���
        /// </summary>
        /// <param name="correntfid">��ǰ</param>
        /// <returns>�Ӱ��ļ���,��ʽ:1,2,3,4,</returns>
        public static string FindChildNode(string categoryid)
        {
            lock (ChildNode)
            {
                DataTable dt = DbProvider.GetInstance().GetCategoryidInShopByParentid(int.Parse(categoryid));

                ChildNode = ChildNode + "," + categoryid;

                if (dt.Rows.Count > 0)
                {
                    //���ӽڵ�
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
        /// ���µ��̷���
        /// </summary>
        /// <param name="shopcategoryinfo">���̷�����Ϣ</param>
        /// <returns>�Ƿ���³ɹ�</returns>
        public static bool UpdateShopCategory(Shopcategoryinfo shopCategoryInfo)
        {
            return DbProvider.GetInstance().UpdateShopCategory(shopCategoryInfo);
        }

        /// <summary>
        /// ����ת��������
        /// </summary>
        public class DTO
        {

            /// <summary>
            /// ��õ��̷�����Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת��������</param>
            /// <returns>���ص��̷�����Ϣ</returns>
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
            /// ��õ��̷�����Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת�������ݱ�</param>
            /// <returns>���ص��̷�����Ϣ</returns>
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
