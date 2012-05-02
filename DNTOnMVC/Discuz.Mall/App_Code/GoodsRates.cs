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
    /// ��Ʒ���۹��������
    /// </summary>
    public class GoodsRates
    {
        /// <summary>
        /// ������Ʒ������Ϣ
        /// </summary>
        /// <param name="goodsrateinfo">Ҫ��������Ʒ������Ϣ</param>
        /// <returns></returns>
        public static int CreateGoodsRate(Goodsrateinfo goodsRateInfo)
        {
            return DbProvider.GetInstance().CreateGoodsRate(goodsRateInfo);
        }

        /// <summary>
        /// ��ȡָ����Ʒ������־id����Ʒ������Ϣ
        /// </summary>
        /// <param name="goodstradelogid">��Ʒ������־id</param>
        /// <returns></returns>
        public static GoodsrateinfoCollection GetGoodsRatesByTradeLogID(int goodsTradeLogId)
        {
            return DTO.GetGoodsRateInfoList(DbProvider.GetInstance().GetGoodsRateByTradeLogID(goodsTradeLogId));
        }

        /// <summary>
        /// ָ������Ʒ�����ܷ��������
        /// </summary>
        /// <param name="goodstradelogid">��ǰ��Ʒ����id</param>
        /// <param name="uid">Ҫ�������۵��û�id</param>
        /// <returns></returns>
        public static bool CanRate(int goodsTradeLogId, int uId)
        {
            foreach (Goodsrateinfo goodsrateinfo in GetGoodsRatesByTradeLogID(goodsTradeLogId))
            {
                if (goodsrateinfo.Uid == uId)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// ָ������Ʒ����˫���Ƿ�����
        /// </summary>
        /// <param name="goodstradelogid">��ǰ��Ʒ����id</param>
        /// <param name="selleruid">�����û�id</param>
        /// <param name="buyeruid">���</param>
        /// <returns></returns>
        public static bool RateClosed(int goodsTradeLogId, int sellerUid, int buyerUid)
        {
            int ratecount = 0;
            foreach (Goodsrateinfo goodsrateinfo in GetGoodsRatesByTradeLogID(goodsTradeLogId))
            {
                if (goodsrateinfo.Uid == sellerUid || goodsrateinfo.Uid == buyerUid)
                    ratecount = ratecount + 1;
            }
            return ratecount >= 2 ? true : false;
        }
 
        /// <summary>
        /// ��ȡָ����������Ʒ��������(json��ʽ)
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="uidtype">�û�id����(1:����, 2:���, 3:������)</param>
        /// <param name="ratetype">��������(1:����, 2:����, 3:����)</param>
        /// <param name="filter">���й��˵�����(oneweek:1����, onemonth:1����, sixmonth:������, sixmonthago:����֮ǰ)</param>
        /// <returns></returns>
        public static string GetGoodsRatesJson(int uid, int uidtype, int ratetype, string filter)
        {
            StringBuilder sb_categories = new StringBuilder();
            sb_categories.Append("[");

            IDataReader iDataReader = DbProvider.GetInstance().GetGoodsRates(uid, uidtype, ratetype, filter);
            if (iDataReader != null)
            {
                while (iDataReader.Read())
                {
                    sb_categories.Append(string.Format("{{'id' : {0}, 'goodstradelogid' : {1}, 'message' : '{2}', 'uid' : {3}, 'uidtype' : {4}, 'ratetouid' : {5}, 'username' : '{6}', 'postdatetime' : '{7}', 'goodsid' : {8}, 'goodstitle' : '{9}', 'price' : '{10}', 'ratetype' :{11}, 'ratetousername' : '{12}'}},",
                        iDataReader["id"],
                        iDataReader["goodstradelogid"],
                        iDataReader["message"].ToString().Trim(),
                        iDataReader["uid"].ToString().Trim(),
                        iDataReader["uidtype"].ToString().ToLower(),
                        iDataReader["ratetouid"],
                        iDataReader["username"].ToString().Trim(),
                        Convert.ToDateTime(iDataReader["postdatetime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                        iDataReader["goodsid"].ToString().Trim(),
                        iDataReader["goodstitle"].ToString().Trim(),
                        iDataReader["price"].ToString().ToLower(),
                        iDataReader["ratetype"],
                        iDataReader["ratetousername"]));
                }
                iDataReader.Dispose();
            }
            if (sb_categories.ToString().EndsWith(","))
                sb_categories.Remove(sb_categories.Length-1,1);

            sb_categories.Append("]");
            return sb_categories.ToString();
        }

        
        /// <summary>
        /// ����ת��������
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// �����Ʒ������Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת�������ݱ�</param>
            /// <returns>������Ʒ������Ϣ</returns>
            public static Goodsrateinfo GetGoodsRateInfo(IDataReader reader)
            {
                if (reader.Read())
                {
                    Goodsrateinfo goodsRatesInfo = new Goodsrateinfo();
                    goodsRatesInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                    goodsRatesInfo.Goodstradelogid = TypeConverter.ObjectToInt(reader["goodstradelogid"]);
                    goodsRatesInfo.Message = reader["message"].ToString().Trim();
                    goodsRatesInfo.Explain = reader["explain"].ToString().Trim();
                    goodsRatesInfo.Ip = reader["ip"].ToString().Trim();
                    goodsRatesInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                    goodsRatesInfo.Uidtype = Convert.ToInt16(reader["uidtype"].ToString());
                    goodsRatesInfo.Ratetouid = TypeConverter.ObjectToInt(reader["ratetouid"]);
                    goodsRatesInfo.Username = reader["username"].ToString().Trim();
                    goodsRatesInfo.Postdatetime = Convert.ToDateTime(reader["postdatetime"]);
                    goodsRatesInfo.Goodsid = TypeConverter.ObjectToInt(reader["goodsid"]);
                    goodsRatesInfo.Goodstitle = reader["goodstitle"].ToString().Trim();
                    goodsRatesInfo.Price = Convert.ToDecimal(reader["price"].ToString());
                    goodsRatesInfo.Ratetype = Convert.ToInt16(reader["ratetype"].ToString());

                    reader.Close();
                    return goodsRatesInfo;
                }
                return null;
            }


            /// <summary>
            /// �����Ʒ������Ϣ(DTO)
            /// </summary>
            /// <param name="__idatareader">Ҫת��������</param>
            /// <returns>������Ʒ������Ϣ</returns>
            public static GoodsrateinfoCollection GetGoodsRateInfoList(IDataReader reader)
            {
                GoodsrateinfoCollection goodsRateInfoColl = new GoodsrateinfoCollection();

                while (reader.Read())
                {
                    Goodsrateinfo goodsRateInfo = new Goodsrateinfo();
                    goodsRateInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                    goodsRateInfo.Goodstradelogid = TypeConverter.ObjectToInt(reader["goodstradelogid"]);
                    goodsRateInfo.Message = reader["message"].ToString().Trim();
                    goodsRateInfo.Explain = reader["explain"].ToString().Trim();
                    goodsRateInfo.Ip = reader["ip"].ToString().Trim();
                    goodsRateInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                    goodsRateInfo.Uidtype = Convert.ToInt16(reader["uidtype"].ToString());
                    goodsRateInfo.Ratetouid = TypeConverter.ObjectToInt(reader["ratetouid"]);
                    goodsRateInfo.Username = reader["username"].ToString().Trim();
                    goodsRateInfo.Postdatetime = Convert.ToDateTime(reader["postdatetime"]);
                    goodsRateInfo.Goodsid = TypeConverter.ObjectToInt(reader["goodsid"]);
                    goodsRateInfo.Goodstitle = reader["goodstitle"].ToString().Trim();
                    goodsRateInfo.Price = Convert.ToDecimal(reader["price"].ToString());
                    goodsRateInfo.Ratetype = Convert.ToInt16(reader["ratetype"].ToString());

                    goodsRateInfoColl.Add(goodsRateInfo);
                }
                reader.Close();
                return goodsRateInfoColl;
            }

            /// <summary>
            /// �����Ʒ������Ϣ(DTO)
            /// </summary>
            /// <param name="dt">Ҫת�������ݱ�</param>
            /// <returns>������Ʒ������Ϣ</returns>
            public static Goodsrateinfo[] GetGoodsRatesInfoArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Goodsrateinfo[] goodsratesinfoarray = new Goodsrateinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    goodsratesinfoarray[i] = new Goodsrateinfo();
                    goodsratesinfoarray[i].Id = TypeConverter.ObjectToInt(dt.Rows[i]["id"]);
                    goodsratesinfoarray[i].Goodstradelogid = TypeConverter.ObjectToInt(dt.Rows[i]["goodstradelogid"]);
                    goodsratesinfoarray[i].Message = dt.Rows[i]["message"].ToString();
                    goodsratesinfoarray[i].Explain = dt.Rows[i]["explain"].ToString();
                    goodsratesinfoarray[i].Ip = dt.Rows[i]["ip"].ToString();
                    goodsratesinfoarray[i].Uid = TypeConverter.ObjectToInt(dt.Rows[i]["uid"]);
                    goodsratesinfoarray[i].Uidtype = TypeConverter.ObjectToInt(dt.Rows[i]["uidtype"]);
                    goodsratesinfoarray[i].Ratetouid = TypeConverter.ObjectToInt(dt.Rows[i]["ratetouid"]);
                    goodsratesinfoarray[i].Username = dt.Rows[i]["username"].ToString();
                    goodsratesinfoarray[i].Postdatetime = Convert.ToDateTime(dt.Rows[i]["postdatetime"].ToString());
                    goodsratesinfoarray[i].Goodsid = TypeConverter.ObjectToInt(dt.Rows[i]["goodsid"]);
                    goodsratesinfoarray[i].Goodstitle = dt.Rows[i]["goodstitle"].ToString();
                    goodsratesinfoarray[i].Price = Convert.ToDecimal(dt.Rows[i]["price"].ToString());
                    goodsratesinfoarray[i].Ratetype = TypeConverter.ObjectToInt(dt.Rows[i]["ratetype"]);
                }
                dt.Dispose();
                return goodsratesinfoarray;
            }
        }
    }
}
