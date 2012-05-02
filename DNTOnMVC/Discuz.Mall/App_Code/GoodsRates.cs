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
    /// 商品评价管理操作类
    /// </summary>
    public class GoodsRates
    {
        /// <summary>
        /// 创建商品评价信息
        /// </summary>
        /// <param name="goodsrateinfo">要创建的商品评价信息</param>
        /// <returns></returns>
        public static int CreateGoodsRate(Goodsrateinfo goodsRateInfo)
        {
            return DbProvider.GetInstance().CreateGoodsRate(goodsRateInfo);
        }

        /// <summary>
        /// 获取指定商品交易日志id的商品评价信息
        /// </summary>
        /// <param name="goodstradelogid">商品交易日志id</param>
        /// <returns></returns>
        public static GoodsrateinfoCollection GetGoodsRatesByTradeLogID(int goodsTradeLogId)
        {
            return DTO.GetGoodsRateInfoList(DbProvider.GetInstance().GetGoodsRateByTradeLogID(goodsTradeLogId));
        }

        /// <summary>
        /// 指定的商品交易能否进行评价
        /// </summary>
        /// <param name="goodstradelogid">当前商品交易id</param>
        /// <param name="uid">要进行评价的用户id</param>
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
        /// 指定的商品交易双方是否已评
        /// </summary>
        /// <param name="goodstradelogid">当前商品交易id</param>
        /// <param name="selleruid">卖家用户id</param>
        /// <param name="buyeruid">买家</param>
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
        /// 获取指定条件的商品评价数据(json格式)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="uidtype">用户id类型(1:卖家, 2:买家, 3:给他人)</param>
        /// <param name="ratetype">评价类型(1:好评, 2:中评, 3:差评)</param>
        /// <param name="filter">进行过滤的条件(oneweek:1周内, onemonth:1月内, sixmonth:半年内, sixmonthago:半年之前)</param>
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
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// 获得商品评价信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据表</param>
            /// <returns>返回商品评价信息</returns>
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
            /// 获得商品评价信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回商品评价信息</returns>
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
            /// 获得商品评价信息(DTO)
            /// </summary>
            /// <param name="dt">要转换的数据表</param>
            /// <returns>返回商品评价信息</returns>
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
