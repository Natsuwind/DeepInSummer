using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Config;
using Discuz.Mall.Data;
using Discuz.Entity;
using Discuz.Cache;
using Discuz.Forum;

namespace Discuz.Mall
{
    /// <summary>
    /// 商品留言管理操作类
    /// </summary>
    public class GoodsLeaveWords
    {
        /// <summary>
        /// 获取指定商品交易日志id的留言信息
        /// </summary>
        /// <param name="goodstradelogid">商品交易日志id</param>
        /// <returns></returns>
        public static GoodsleavewordinfoCollection GetLeaveWordList(int goodsTradeLogId)
        {
            return DTO.GetGoodsLeaveWordInfoList(DbProvider.GetInstance().GetGoodsLeaveWordListByTradeLogId(goodsTradeLogId));
        }

        /// <summary>
        /// 获取指定留言id的留言信息
        /// </summary>
        /// <param name="id">留言id</param>
        /// <returns></returns>
        public static Goodsleavewordinfo GetGoodsLeaveWordById(int id)
        {
            return DTO.GetGoodsLeaveWordInfo(DbProvider.GetInstance().GetGoodsLeaveWordById(id));
        }

        /// <summary>
        /// 创建留言
        /// </summary>
        /// <param name="goodsleavewordinfo">要创建的留言信息</param>
        /// <param name="selleruid">卖家id</param>
        /// <returns></returns>
        public static int CreateLeaveWord(Goodsleavewordinfo goodsLeaveWordInfo, int sellerUid)
        {
            return CreateLeaveWord(goodsLeaveWordInfo, sellerUid, true);
        }


        /// <summary>
        /// 创建留言
        /// </summary>
        /// <param name="goodsleavewordinfo">要创建的留言信息</param>
        /// <param name="selleruid">卖家id</param>
        /// <param name="sendnotice">是否发送通知</param>
        /// <returns></returns>
        public static int CreateLeaveWord(Goodsleavewordinfo goodsLeaveWordInfo, int sellerUid, bool sendNotice)
        {
            goodsLeaveWordInfo.Postdatetime = DateTime.Now;
            goodsLeaveWordInfo.Usesig = 0;
            goodsLeaveWordInfo.Invisible = 0;
            goodsLeaveWordInfo.Htmlon = 0;
            goodsLeaveWordInfo.Smileyoff = 1;
            goodsLeaveWordInfo.Parseurloff = 1;
            goodsLeaveWordInfo.Bbcodeoff = 1;

            //当为买家时，则发送消息给卖家
            if (sendNotice && goodsLeaveWordInfo.Isbuyer == 1)
            {
                NoticeInfo noticeInfo = new NoticeInfo();
                //商品交易通知
                noticeInfo.Note = Utils.HtmlEncode(string.Format("有买家 <a href=\"userinfo.aspx?userid={0}\">{1}</a> 给您留言, 请<a href =\"showgoods.aspx?goodsid={2}\">点击这里</a>查看详情.", goodsLeaveWordInfo.Uid, goodsLeaveWordInfo.Username, sellerUid));
                noticeInfo.Uid = sellerUid;
                noticeInfo.Type = NoticeType.GoodsLeaveWordNotice;
                noticeInfo.New = 1;
                noticeInfo.Posterid = goodsLeaveWordInfo.Uid;
                noticeInfo.Poster = goodsLeaveWordInfo.Username;
                noticeInfo.Postdatetime = Utils.GetDateTime();

                Notices.CreateNoticeInfo(noticeInfo);
            }
            return DbProvider.GetInstance().CreateGoodsLeaveWord(goodsLeaveWordInfo);
       }

      

        /// <summary>
        /// 删除指定留言id的留言信息
        /// </summary>
        /// <param name="id">留言id</param>
        /// <param name="userid">当前留言的提交人</param>
        /// <param name="selleruid">当前商品的卖家</param>
        /// <returns></returns>
        public static bool DeleteLeaveWordById(int id, int userId, int sellerUid, int userAdminId)
        { 
            //删除留言的操作
            Goodsleavewordinfo goodsleaveword = GetGoodsLeaveWordById(id);

            //当为管理组身份 或 留言的uid与当前用户相同时
            if (userAdminId == 1 || (goodsleaveword != null && goodsleaveword.Uid == userId) || sellerUid == userId)
                return DbProvider.GetInstance().DeleteGoodsLeaveWordById(id);
            else
                return false;
        }

        /// <summary>
        /// 更新指定的留言信息
        /// </summary>
        /// <param name="goodsleavewordinfo">要更新的留言信息</param>
        /// <returns></returns>
        public static bool UpdateLeaveWord(Goodsleavewordinfo goodsLeaveWordInfo)
        {
            return DbProvider.GetInstance().UpdateGoodsLeaveWord(goodsLeaveWordInfo);
        }

        /// <summary>
        /// 获取指定分类和条件下的商品列表集合
        /// </summary>
        /// <param name="categoryid">商品分类</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式(0:升序, 1:降序)</param>
        /// <returns></returns>
        public static GoodsleavewordinfoCollection GetGoodsLeaveWord(int goodsId, int pageSize, int pageIndex, string orderBy, int ascDesc)
        {
            GoodsleavewordinfoCollection coll = new GoodsleavewordinfoCollection();

            if (pageIndex <= 0)
                return coll;

            return DTO.GetGoodsLeaveWordInfoList(DbProvider.GetInstance().GetGoodsLeaveWordByGid(goodsId, pageSize, pageIndex, orderBy, ascDesc));
        }


        /// <summary>
        /// 获取指定商品的留言
        /// </summary>
        /// <param name="categoryid">分类id</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static int GetGoodsLeaveWordCount(int goodsId)
        {
            return DbProvider.GetInstance().GetGoodsLeaveWordCountByGid(goodsId);
        }

        /// <summary>
        /// 获取指定商品的交易日志JSON数据
        /// </summary>
        /// <param name="goodsid">指定商品</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public static StringBuilder GetLeaveWordJson(int goodsId, int pageSize, int pageIndex, string orderBy, int ascDesc)
        {
            StringBuilder leaveWordJson = new StringBuilder();
            leaveWordJson.Append("[");
            foreach (Goodsleavewordinfo goodsLeaveWordInfo in GetGoodsLeaveWord(goodsId, pageSize, pageIndex, orderBy, ascDesc))
            {
                goodsLeaveWordInfo.Message = ParseSmilies(goodsLeaveWordInfo.Message);

                leaveWordJson.Append(string.Format("{{'id' : {0}, 'isbuyer' : {1}, 'uid' : {2}, 'username' : '{3}', 'postdatetime' : '{4}', 'message' : '{5}'}},",
                                goodsLeaveWordInfo.Id,
                                goodsLeaveWordInfo.Isbuyer == 1 ? "true": "false",
                                goodsLeaveWordInfo.Uid,
                                goodsLeaveWordInfo.Username,
                                goodsLeaveWordInfo.Postdatetime.ToString("yyyy-MM-dd"),
                                goodsLeaveWordInfo.Message.Replace("\r\n", "<br />")));
            }
            if (leaveWordJson.ToString().EndsWith(","))
                leaveWordJson.Remove(leaveWordJson.Length - 1, 1);

            leaveWordJson.Append("]");

            return leaveWordJson;
        }

        /// <summary>
        /// 获取指定商品的交易日志JSON数据
        /// </summary>
        /// <param name="goodsid">指定商品</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public static StringBuilder GetLeaveWordJson(int leaveWordId)
        {
            StringBuilder leaveWordJson = new StringBuilder();
            leaveWordJson.Append("[");

            if (leaveWordId <= 0)
                leaveWordJson.Append("{{'id' : 0, 'isbuyer' : 0, 'uid' : 0, 'username' : '', 'postdatetime' : '', 'message' : ''}}");
            else
            {
                Goodsleavewordinfo goodsleavewordinfo = GoodsLeaveWords.GetGoodsLeaveWordById(leaveWordId);

                if (goodsleavewordinfo == null || goodsleavewordinfo.Id <= 0)
                    leaveWordJson.Append("{{'id' : 0, 'isbuyer' : 0, 'uid' : 0, 'username' : '', 'postdatetime' : '', 'message' : ''}}");
                else
                {
                    leaveWordJson.Append(string.Format("{{'id' : {0}, 'isbuyer' : {1}, 'uid' : {2}, 'username' : '{3}', 'postdatetime' : '{4}', 'message' : '{5}'}}",
                                    goodsleavewordinfo.Id,
                                    goodsleavewordinfo.Isbuyer == 1 ? "true" : "false",
                                    goodsleavewordinfo.Uid,
                                    goodsleavewordinfo.Username,
                                    goodsleavewordinfo.Postdatetime.ToString("yyyy-MM-dd"),
                                    goodsleavewordinfo.Message.Replace("\r\n", "<br />")));
                }
            }
            return leaveWordJson.Append("]");
        }

        /// <summary>
        /// 转换表情
        /// </summary>
        /// <param name="sDetail">内容</param>
        /// <returns>帖子内容</returns>
        private static string ParseSmilies(string sDetail)
        {
            RegexOptions options = RegexOptions.IgnoreCase;

            SmiliesInfo[] smiliesInfo = Smilies.GetSmiliesListWithInfo(); //表情数组
            int smiliesMax = GeneralConfigs.GetConfig().Smiliesmax;
            if (smiliesInfo == null)
                return sDetail;

            string smilieFormatStr = "[smilie]{0}editor/images/smilies/{1}[/smilie]";
            for (int i = 0; i < Smilies.regexSmile.Length; i++)
            {
                if (smiliesMax > 0)
                    sDetail = Smilies.regexSmile[i].Replace(sDetail, string.Format(smilieFormatStr, BaseConfigs.GetForumPath, smiliesInfo[i].Url), smiliesMax);
                else
                    sDetail = Smilies.regexSmile[i].Replace(sDetail, string.Format(smilieFormatStr, BaseConfigs.GetForumPath, smiliesInfo[i].Url));
            }
            return Regex.Replace(sDetail, @"\[smilie\](.+?)\[\/smilie\]", "<img src=\"$1\" />", options);
        }

        /// <summary>
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// 获得商品留言信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回商品留言信息</returns>
            public static Goodsleavewordinfo GetGoodsLeaveWordInfo(IDataReader reader)
            {
                Goodsleavewordinfo goodsLeaveWordInfo = null;
                if (reader.Read())
                {
                    goodsLeaveWordInfo = new Goodsleavewordinfo();
                    goodsLeaveWordInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                    goodsLeaveWordInfo.Goodsid = TypeConverter.ObjectToInt(reader["goodsid"]);
                    goodsLeaveWordInfo.Tradelogid = TypeConverter.ObjectToInt(reader["tradelogid"]);
                    goodsLeaveWordInfo.Isbuyer = Convert.ToInt16(reader["isbuyer"].ToString());
                    goodsLeaveWordInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                    goodsLeaveWordInfo.Username = reader["username"].ToString().Trim();
                    goodsLeaveWordInfo.Message = reader["message"].ToString().Trim();
                    goodsLeaveWordInfo.Invisible = TypeConverter.ObjectToInt(reader["invisible"]);
                    goodsLeaveWordInfo.Ip = reader["ip"].ToString().Trim();
                    goodsLeaveWordInfo.Usesig = TypeConverter.ObjectToInt(reader["usesig"]);
                    goodsLeaveWordInfo.Htmlon = TypeConverter.ObjectToInt(reader["htmlon"]);
                    goodsLeaveWordInfo.Smileyoff = TypeConverter.ObjectToInt(reader["smileyoff"]);
                    goodsLeaveWordInfo.Parseurloff = TypeConverter.ObjectToInt(reader["parseurloff"]);
                    goodsLeaveWordInfo.Bbcodeoff = TypeConverter.ObjectToInt(reader["bbcodeoff"]);
                    goodsLeaveWordInfo.Postdatetime = Convert.ToDateTime(reader["postdatetime"].ToString());

                    reader.Close();
                }
                return goodsLeaveWordInfo;
            }

            /// <summary>
            /// 获得商品留言信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回商品留言信息</returns>
            public static GoodsleavewordinfoCollection GetGoodsLeaveWordInfoList(IDataReader reader)
            {
                GoodsleavewordinfoCollection goodsLeaveWordInfoColl = new GoodsleavewordinfoCollection();

                while (reader.Read())
                {
                    Goodsleavewordinfo goodsLeaveWordInfo = new Goodsleavewordinfo();
                    goodsLeaveWordInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                    goodsLeaveWordInfo.Goodsid = TypeConverter.ObjectToInt(reader["goodsid"]);
                    goodsLeaveWordInfo.Tradelogid = TypeConverter.ObjectToInt(reader["tradelogid"]);
                    goodsLeaveWordInfo.Isbuyer = Convert.ToInt16(reader["isbuyer"].ToString());
                    goodsLeaveWordInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                    goodsLeaveWordInfo.Username = reader["username"].ToString().Trim();
                    goodsLeaveWordInfo.Message = reader["message"].ToString().Trim();
                    goodsLeaveWordInfo.Invisible = TypeConverter.ObjectToInt(reader["invisible"]);
                    goodsLeaveWordInfo.Ip = reader["ip"].ToString().Trim();
                    goodsLeaveWordInfo.Usesig = TypeConverter.ObjectToInt(reader["usesig"]);
                    goodsLeaveWordInfo.Htmlon = TypeConverter.ObjectToInt(reader["htmlon"]);
                    goodsLeaveWordInfo.Smileyoff = TypeConverter.ObjectToInt(reader["smileyoff"]);
                    goodsLeaveWordInfo.Parseurloff = TypeConverter.ObjectToInt(reader["parseurloff"]);
                    goodsLeaveWordInfo.Bbcodeoff = TypeConverter.ObjectToInt(reader["bbcodeoff"]);
                    goodsLeaveWordInfo.Postdatetime = Convert.ToDateTime(reader["postdatetime"].ToString());

                    goodsLeaveWordInfoColl.Add(goodsLeaveWordInfo);
                }
                reader.Close();
                return goodsLeaveWordInfoColl;
            }


            /// <summary>
            /// 获得商品留言信息(DTO)
            /// </summary>
            /// <param name="dt">要转换的数据表</param>
            /// <returns>返回商品留言信息</returns>
            public static Goodsleavewordinfo[] GetGoodsLeaveWordInfoArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Goodsleavewordinfo[] goodsLeaveWordInfoArray = new Goodsleavewordinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    goodsLeaveWordInfoArray[i] = new Goodsleavewordinfo();
                    goodsLeaveWordInfoArray[i].Id = TypeConverter.ObjectToInt(dt.Rows[i]["id"]);
                    goodsLeaveWordInfoArray[i].Goodsid = TypeConverter.ObjectToInt(dt.Rows[i]["goodsid"]);
                    goodsLeaveWordInfoArray[i].Tradelogid = TypeConverter.ObjectToInt(dt.Rows[i]["tradelogid"]);
                    goodsLeaveWordInfoArray[i].Isbuyer = TypeConverter.ObjectToInt(dt.Rows[i]["isbuyer"]);
                    goodsLeaveWordInfoArray[i].Uid = TypeConverter.ObjectToInt(dt.Rows[i]["uid"]);
                    goodsLeaveWordInfoArray[i].Username = dt.Rows[i]["username"].ToString();
                    goodsLeaveWordInfoArray[i].Message = dt.Rows[i]["message"].ToString();
                    goodsLeaveWordInfoArray[i].Invisible = TypeConverter.ObjectToInt(dt.Rows[i]["invisible"]);
                    goodsLeaveWordInfoArray[i].Ip = dt.Rows[i]["ip"].ToString();
                    goodsLeaveWordInfoArray[i].Usesig = TypeConverter.ObjectToInt(dt.Rows[i]["usesig"]);
                    goodsLeaveWordInfoArray[i].Htmlon = TypeConverter.ObjectToInt(dt.Rows[i]["htmlon"]);
                    goodsLeaveWordInfoArray[i].Smileyoff = TypeConverter.ObjectToInt(dt.Rows[i]["smileyoff"]);
                    goodsLeaveWordInfoArray[i].Parseurloff = TypeConverter.ObjectToInt(dt.Rows[i]["parseurloff"]);
                    goodsLeaveWordInfoArray[i].Bbcodeoff = TypeConverter.ObjectToInt(dt.Rows[i]["bbcodeoff"]);
                    goodsLeaveWordInfoArray[i].Postdatetime = Convert.ToDateTime(dt.Rows[i]["postdatetime"].ToString());
                }
                dt.Dispose();
                return goodsLeaveWordInfoArray;
            }
        }
    }
}
