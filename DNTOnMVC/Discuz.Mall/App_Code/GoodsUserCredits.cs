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
    /// 商品用户信用管理操作类
    /// </summary>
    public class GoodsUserCredits
    {

        /// <summary>
        /// 设置用户信用(该方法会在用户进行评价之后调用)
        /// </summary>
        /// <param name="goodsrateinfo">评价信息</param>
        /// <param name="uid">被评价人的uid</param>
        /// <returns></returns>
        public static bool SetUserCredit(Goodsrateinfo goodsRateInfo, int uid)
        {
            //获取被评价人的信用信息
            GoodsusercreditinfoCollection goodsUserCreditInfoColl = GetUserCreditList(uid);

            //如果信用表中不存在, 则创建被评价人的信息
            if (goodsUserCreditInfoColl.Count == 0)
            {
                //当初始化信息失败时则返回
                if (DbProvider.GetInstance().InitGoodsUserCredit(uid) <= 0)
                    return false;

                //再次获取被评价人的信用信息
                goodsUserCreditInfoColl = GetUserCreditList(uid);
            }

            //用于绑定要更新的用户信用
            Goodsusercreditinfo cur_creditInfo = null;
            foreach (Goodsusercreditinfo goodsUserCreditInfo in goodsUserCreditInfoColl)
            {
                //查找符合条件的用户信用
                if (goodsRateInfo.Uidtype == goodsUserCreditInfo.Ratefrom && goodsRateInfo.Ratetype == goodsUserCreditInfo.Ratetype)
                    cur_creditInfo = goodsUserCreditInfo; break;
            }

            //当不为空, 表示找到了要更新的用户信用信息, 则进行下面的绑定操作
            if (cur_creditInfo != null)
            {
                IDataReader iDataReader = DbProvider.GetInstance().GetGoodsRateCount(uid, goodsRateInfo.Uidtype, goodsRateInfo.Ratetype);
                //绑定新的查询数据
                if (iDataReader.Read())
                {
                    cur_creditInfo.Ratefrom = goodsRateInfo.Uidtype;
                    cur_creditInfo.Ratetype = goodsRateInfo.Ratetype;
                    cur_creditInfo.Oneweek = TypeConverter.ObjectToInt(iDataReader["oneweek"]);
                    cur_creditInfo.Onemonth = TypeConverter.ObjectToInt(iDataReader["onemonth"]);
                    cur_creditInfo.Sixmonth = TypeConverter.ObjectToInt(iDataReader["sixmonth"]);
                    cur_creditInfo.Sixmonthago = TypeConverter.ObjectToInt(iDataReader["sixmonthago"]);
                    UpdateUserCredit(cur_creditInfo);                    
                }
                iDataReader.Close();
            }
            return true;
        }

        /// <summary>
        /// 获取指定用户id的信用信息(json格式串)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static StringBuilder GetUserCreditJsonData(int uid)
        {
            StringBuilder sb_usercreditJson = new StringBuilder();
            sb_usercreditJson.Append("[");

            //获取被评价人的信用信息
            GoodsusercreditinfoCollection goodsusercreditinfocoll = GetUserCreditList(uid);

            //如果信用表中不存在, 则创建被评价人的信息
            if (goodsusercreditinfocoll.Count == 0)
            {
                //当初始化信息失败时则返回
                DbProvider.GetInstance().InitGoodsUserCredit(uid);
                 //再次获取被评价人的信用信息
                goodsusercreditinfocoll = GetUserCreditList(uid);
            }

            foreach (Goodsusercreditinfo __goodsusercreditinfo in goodsusercreditinfocoll)
            {
                sb_usercreditJson.Append(string.Format("{{'id' : {0}, 'uid' : {1}, 'oneweek' : {2}, 'onemonth' : {3}, 'sixmonth' : {4}, 'sixmonthago' : {5}, 'ratefrom' : {6}, 'ratetype' : {7}}},",
                                __goodsusercreditinfo.Id,
                                __goodsusercreditinfo.Uid,
                                __goodsusercreditinfo.Oneweek,
                                __goodsusercreditinfo.Onemonth,
                                __goodsusercreditinfo.Sixmonth,
                                __goodsusercreditinfo.Sixmonthago,
                                __goodsusercreditinfo.Ratefrom,
                                __goodsusercreditinfo.Ratetype));
            }
            if (sb_usercreditJson.ToString().EndsWith(","))
                sb_usercreditJson.Remove(sb_usercreditJson.Length - 1, 1);

            sb_usercreditJson.Append("]");
            return sb_usercreditJson;
        }

        /// <summary>
        /// 更新用户信用信息
        /// </summary>
        /// <param name="goodsusercreditinfo">要更新的用户信用信息</param>
        /// <returns></returns>
        public static bool UpdateUserCredit(Goodsusercreditinfo goodsusercreditinfo)
        {
            return DbProvider.GetInstance().UpdateGoodsUserCredit(goodsusercreditinfo);
        }

        /// <summary>
        /// 获取指定用户id的信用信息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public static GoodsusercreditinfoCollection GetUserCreditList(int userid)
        {
            return DTO.GetGoodsUserCreditList(DbProvider.GetInstance().GetGoodsUserCreditByUid(userid));
        }

        /// <summary>
        /// 获取诚信规则的json数据串
        /// </summary>
        /// <returns></returns>
        public static StringBuilder GetCreditRulesJsonData()
        {
            StringBuilder sb_usercreditJson = new StringBuilder();
            sb_usercreditJson.Append("[");

            IDataReader iDataReader = DbProvider.GetInstance().GetGoodsCreditRules();

            while (iDataReader.Read())
            {
                sb_usercreditJson.Append(string.Format("{{'id' : {0}, 'lowerlimit' : {1}, 'upperlimit' : {2}, 'sellericon' : '{3}', 'buyericon' : '{4}'}},",
                                iDataReader["id"],
                                iDataReader["lowerlimit"],
                                iDataReader["upperlimit"],
                                iDataReader["sellericon"],
                                iDataReader["buyericon"]));
            }
            iDataReader.Close();
            
            if (sb_usercreditJson.ToString().EndsWith(","))
                sb_usercreditJson.Remove(sb_usercreditJson.Length - 1, 1);

            sb_usercreditJson.Append("]");
            return sb_usercreditJson;
        }


        /// <summary>
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {

            /// <summary>
            /// 获得(商品)用户信用信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回(商品)用户信用信息</returns>
            public static Goodsusercreditinfo GetGoodsUserCreditInfo(IDataReader reader)
            {
                Goodsusercreditinfo goodsUserCreditsInfo = null;
                if (reader.Read())
                {
                    goodsUserCreditsInfo = new Goodsusercreditinfo();
                    goodsUserCreditsInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                    goodsUserCreditsInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                    goodsUserCreditsInfo.Oneweek = TypeConverter.ObjectToInt(reader["oneweek"]);
                    goodsUserCreditsInfo.Onemonth = TypeConverter.ObjectToInt(reader["onemonth"]);
                    goodsUserCreditsInfo.Sixmonth = TypeConverter.ObjectToInt(reader["sixmonth"]);
                    goodsUserCreditsInfo.Sixmonthago = TypeConverter.ObjectToInt(reader["sixmonthago"]);
                    goodsUserCreditsInfo.Ratefrom = Convert.ToInt16(reader["ratefrom"].ToString());
                    goodsUserCreditsInfo.Ratetype = Convert.ToInt16(reader["ratetype"].ToString());

                    reader.Close();
                }
                return goodsUserCreditsInfo;
            }


            /// <summary>
            /// 获得(商品)用户信用信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回(商品)用户信用信息</returns>
            public static GoodsusercreditinfoCollection GetGoodsUserCreditList(IDataReader reader)
            {
                GoodsusercreditinfoCollection goodsUserCreditInfoColl = new GoodsusercreditinfoCollection();

                while (reader.Read())
                {
                    Goodsusercreditinfo goodsUserCreditsInfo = new Goodsusercreditinfo();
                    goodsUserCreditsInfo.Id = TypeConverter.ObjectToInt(reader["id"].ToString());
                    goodsUserCreditsInfo.Uid = TypeConverter.ObjectToInt(reader["uid"].ToString());
                    goodsUserCreditsInfo.Oneweek = TypeConverter.ObjectToInt(reader["oneweek"].ToString());
                    goodsUserCreditsInfo.Onemonth = TypeConverter.ObjectToInt(reader["onemonth"].ToString());
                    goodsUserCreditsInfo.Sixmonth = TypeConverter.ObjectToInt(reader["sixmonth"].ToString());
                    goodsUserCreditsInfo.Sixmonthago = TypeConverter.ObjectToInt(reader["sixmonthago"].ToString());
                    goodsUserCreditsInfo.Ratefrom = Convert.ToInt16(reader["ratefrom"].ToString());
                    goodsUserCreditsInfo.Ratetype = Convert.ToInt16(reader["ratetype"].ToString());

                    goodsUserCreditInfoColl.Add(goodsUserCreditsInfo);
                }
                reader.Close();
                return goodsUserCreditInfoColl;
            }

           
            /// <summary>
            /// 获得(商品)用户信用信息(DTO)
            /// </summary>
            /// <param name="dt">要转换的数据表</param>
            /// <returns>返回(商品)用户信用信息</returns>
            public static Goodsusercreditinfo[] GetGoodsUserCreditArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Goodsusercreditinfo[] goodsUserCreditsInfoArray = new Goodsusercreditinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    goodsUserCreditsInfoArray[i] = new Goodsusercreditinfo();
                    goodsUserCreditsInfoArray[i].Id = TypeConverter.ObjectToInt(dt.Rows[i]["id"].ToString());
                    goodsUserCreditsInfoArray[i].Uid = TypeConverter.ObjectToInt(dt.Rows[i]["uid"].ToString());
                    goodsUserCreditsInfoArray[i].Oneweek = TypeConverter.ObjectToInt(dt.Rows[i]["oneweek"].ToString());
                    goodsUserCreditsInfoArray[i].Onemonth = TypeConverter.ObjectToInt(dt.Rows[i]["onemonth"].ToString());
                    goodsUserCreditsInfoArray[i].Sixmonth = TypeConverter.ObjectToInt(dt.Rows[i]["sixmonth"].ToString());
                    goodsUserCreditsInfoArray[i].Sixmonthago = TypeConverter.ObjectToInt(dt.Rows[i]["sixmonthago"].ToString());
                    goodsUserCreditsInfoArray[i].Ratefrom = TypeConverter.ObjectToInt(dt.Rows[i]["ratefrom"].ToString());
                    goodsUserCreditsInfoArray[i].Ratetype = TypeConverter.ObjectToInt(dt.Rows[i]["ratetype"].ToString());

                }
                dt.Dispose();
                return goodsUserCreditsInfoArray;
            }
        }
    }
}
