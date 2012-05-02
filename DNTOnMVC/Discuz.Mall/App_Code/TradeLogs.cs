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
    public class TradeLogs
    {
        /// <summary>
        /// 获取交易流水号  
        /// </summary>
        public static string GetOrderID()
        {
            string _out_trade_no;
            //构造订单号 (形如:20080104140009iwGampfQkzFgMZ0yoT)
            _out_trade_no = Discuz.Common.Utils.GetDateTime();
            _out_trade_no = _out_trade_no.Replace("-", "");
            _out_trade_no = _out_trade_no.Replace(":", "");
            _out_trade_no = _out_trade_no.Replace(" ", "");

            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rnd = new Random();
            for (int i = 1; i <= 32; i++)
            {
                _out_trade_no += chars.Substring(rnd.Next(chars.Length), 1);
            }
            return _out_trade_no;
        }

        /// <summary>
        /// 创建商品交易日志
        /// </summary>
        /// <param name="__goodstradelog">要创建的商品交易日志</param>
        /// <returns>创建的商品交易日志id</returns>
        public static int CreateTradeLog(Goodstradeloginfo goodsTradeLog)
        {
            //当为支付宝付款方式时,将订单号绑定到tradeno字段
            if (goodsTradeLog.Offline == 0)
                goodsTradeLog.Tradeno = goodsTradeLog.Orderid;

            if (goodsTradeLog.Buyermsg.Length > 100)
                goodsTradeLog.Buyermsg = goodsTradeLog.Buyermsg.Substring(0, 100);

            if (goodsTradeLog.Buyercontact.Length > 100)
                goodsTradeLog.Buyercontact = goodsTradeLog.Buyercontact.Substring(0, 100);

            if (goodsTradeLog.Number > 0)
            { 
                //更新商品数量和最近交易信息
                Goodsinfo goodsInfo = Goods.GetGoodsInfo(goodsTradeLog.Goodsid);
                if (goodsInfo != null && goodsInfo.Goodsid > 0)
                {
                    //当商品库存变为0(负)库存时
                    if (goodsInfo.Amount > 0 && (goodsInfo.Amount - goodsTradeLog.Number) <= 0)
                        DbProvider.GetInstance().UpdateCategoryGoodsCounts(goodsInfo.Categoryid, goodsInfo.Parentcategorylist, -1);

                    goodsInfo.Totalitems = goodsInfo.Totalitems + goodsTradeLog.Number; //累加总交易量
                    goodsInfo.Amount = goodsInfo.Amount - goodsTradeLog.Number; //减少当前商品数量
                    goodsInfo.Tradesum = goodsInfo.Tradesum + goodsTradeLog.Tradesum;  //累加总交易额
                    goodsInfo.Lastbuyer = goodsTradeLog.Buyer;
                    goodsInfo.Lasttrade = DateTime.Now;

                    Goods.UpdateGoods(goodsInfo);
                }
            }
            goodsTradeLog.Id = DbProvider.GetInstance().CreateGoodsTradeLog(goodsTradeLog);

            SendNotice(goodsTradeLog);

            return goodsTradeLog.Id;
        }

        /// <summary>
        /// 更新交易信息
        /// </summary>
        /// <param name="__goodstradelog">要更新的交易信息</param>
        /// <param name="oldstatus">本次更新之前的状态</param>
        /// <returns>是否更新成功</returns>
        public static bool UpdateTradeLog(Goodstradeloginfo goodsTradeLog, int oldstatus)
        {
            if (goodsTradeLog.Buyermsg.Length > 100)
                goodsTradeLog.Buyermsg = goodsTradeLog.Buyermsg.Substring(0, 100);

            if (goodsTradeLog.Buyercontact.Length > 100)
                goodsTradeLog.Buyercontact = goodsTradeLog.Buyercontact.Substring(0, 100);
         
            goodsTradeLog.Tradesum = goodsTradeLog.Number * goodsTradeLog.Price + (goodsTradeLog.Transportpay == 2 ? goodsTradeLog.Transportfee : 0);
 
            //当交易状态发生变化时
            if (goodsTradeLog.Status != oldstatus)
            {
                if (goodsTradeLog.Number > 0)
                {
                    //获取当前交易的商品信息
                    Goodsinfo goodsInfo = Goods.GetGoodsInfo(goodsTradeLog.Goodsid);

                    //当交易从中途关闭(未完成)状态变为生效(Status: 1为生效, 4为买家已付款等待卖家发货)时更新商品数量)
                    if (oldstatus == 8 && (goodsTradeLog.Status == 1 || goodsTradeLog.Status == 4))
                    {
                        //当商品库存变为0(负)库存时
                        if (goodsInfo.Amount > 0 && (goodsInfo.Amount - goodsTradeLog.Number) <= 0)
                            DbProvider.GetInstance().UpdateCategoryGoodsCounts(goodsInfo.Categoryid, goodsInfo.Parentcategorylist, -1);

                        goodsInfo.Totalitems = goodsInfo.Totalitems + goodsTradeLog.Number; //累加总交易量
                        goodsInfo.Amount = goodsInfo.Amount - goodsTradeLog.Number; //减少当前商品数量
                        goodsInfo.Tradesum = goodsInfo.Tradesum + goodsTradeLog.Tradesum;  //累加总交易额
                    }

                    //当退款成功后(Status = 17, 表示此次交易无效,同时更新商品信息并还原商品数目)
                    //或交易中途关闭,未完成(Status = 8, 更新商品数量)
                    if (goodsTradeLog.Status == 17 || goodsTradeLog.Status == 8)
                    {
                        //当商品库存从0(负)库存变为有效库存时
                        if (goodsInfo.Amount <= 0 && (goodsInfo.Amount + goodsTradeLog.Number) > 0)
                            DbProvider.GetInstance().UpdateCategoryGoodsCounts(goodsInfo.Categoryid, goodsInfo.Parentcategorylist, 1);

                        goodsInfo.Totalitems = goodsInfo.Totalitems - goodsTradeLog.Number; //减少总交易量
                        goodsInfo.Amount = goodsInfo.Amount + goodsTradeLog.Number; //还原当前商品数量
                        goodsInfo.Tradesum = goodsInfo.Tradesum - goodsTradeLog.Tradesum;//减少总交易额
                    }
                  
                    goodsInfo.Lastbuyer = goodsTradeLog.Buyer;
                    goodsInfo.Lasttrade = DateTime.Now;

                    Goods.UpdateGoods(goodsInfo);
                }
            }
            return DbProvider.GetInstance().UpdateGoodsTradeLog(goodsTradeLog);
        }


        /// <summary>
        /// 更新交易信息
        /// </summary>
        /// <param name="__goodstradelog">要更新的交易信息</param>
        /// <param name="oldstatus">更新之前的状态</param>
        /// <param name="issendpm">更新交易信息成功后, 是否发送短消息</param>
        /// <returns>是否更新成功</returns>
        public static bool UpdateTradeLog(Goodstradeloginfo goodsTradeLog, int oldStatus, bool isSendPm)
        {
            bool result = UpdateTradeLog(goodsTradeLog, oldStatus);
            if (result && isSendPm)
                SendNotice(goodsTradeLog);

            return result;
        }
        
        /// <summary>
        /// 根据交易日志的状态发送相应通知
        /// </summary>
        /// <param name="__goodstradelog">交易日志信息</param>
        /// <returns>是否发送成功</returns>
        public static bool SendNotice(Goodstradeloginfo goodsTradeLog)
        {
            string noticeContent = "这是由论坛系统自动发送的通知短消息.<BR />";
            bool isSendNotice = false;
            int noticeUid = 0;
            int posterId = 0;
            string poster = "";
            string pageName = goodsTradeLog.Offline == 1 ? "offlinetrade.aspx" : "onlinetrade.aspx";
            switch ((TradeStatusEnum)goodsTradeLog.Status)
            {
                case TradeStatusEnum.UnStart:
                    {
                        noticeContent = string.Format("买家 {0} 购买您的商品 {1}. 但交易尚未生效, 等待您的确认, 请<a href =\"" + pageName + "?goodstradelogid={2}\">点击这里</a>查看详情.",
                                    goodsTradeLog.Buyer,
                                    goodsTradeLog.Subject,
                                    goodsTradeLog.Id);
                        isSendNotice = true;
                        noticeUid = goodsTradeLog.Sellerid;
                        posterId = goodsTradeLog.Buyerid;
                        poster = goodsTradeLog.Buyername;
                        break;
                    }
                case TradeStatusEnum.WAIT_SELLER_SEND_GOODS:
                    {
                        noticeContent = string.Format("买家 {0} 购买您的商品 {1}. 买家已付款, 等待您发货, 请<a href =\"" + pageName + "?goodstradelogid={2}\">点击这里</a>查看详情.",
                                    goodsTradeLog.Buyer,
                                    goodsTradeLog.Subject,
                                    goodsTradeLog.Id);
                        isSendNotice = true;
                        noticeUid = goodsTradeLog.Sellerid;
                        posterId = goodsTradeLog.Buyerid;
                        poster = goodsTradeLog.Buyername;
                        break;
                    }
                case TradeStatusEnum.WAIT_BUYER_CONFIRM_GOODS:
                    {
                        noticeContent = string.Format("您购买的商品 {0} . 卖家 {1} 已发货, 等待您的确认, 请<a href =\"" + pageName + "?goodstradelogid={2}\">点击这里</a>查看详情.",
                                    goodsTradeLog.Subject,
                                    goodsTradeLog.Seller,
                                    goodsTradeLog.Id);
                        noticeUid = goodsTradeLog.Buyerid;
                        posterId = goodsTradeLog.Sellerid;
                        poster = goodsTradeLog.Seller;
                        isSendNotice = true; 
                        break;
                    }
                case TradeStatusEnum.WAIT_SELLER_AGREE:
                    {
                        noticeContent = string.Format("买家 {0} 等待你同意退款, 请<a href =\"" + pageName + "?goodstradelogid={1}\">点击这里</a>查看详情.",
                                     goodsTradeLog.Buyer,
                                    goodsTradeLog.Id);
                        isSendNotice = true;
                        noticeUid = goodsTradeLog.Sellerid;
                        posterId = goodsTradeLog.Buyerid;
                        poster = goodsTradeLog.Buyername;
                        break; 
                    }
                case TradeStatusEnum.SELLER_REFUSE_BUYER:
                    {
                        noticeContent = string.Format("卖家 {0} 拒绝您的条件, 等待您修改条件, 请<a href =\"" + pageName + "?goodstradelogid={1}\">点击这里</a>查看详情.",
                                     goodsTradeLog.Seller,
                                    goodsTradeLog.Id);
                        isSendNotice = true;
                        noticeUid = goodsTradeLog.Buyerid;
                        posterId = goodsTradeLog.Sellerid;
                        poster = goodsTradeLog.Seller;
                        break; 
                    }
                case TradeStatusEnum.WAIT_BUYER_RETURN_GOODS:
                    {
                        noticeContent = string.Format("卖家 {0} 同意退款, 等待您退货, 请<a href =\"" + pageName + "?goodstradelogid={1}\">点击这里</a>查看详情.",
                                     goodsTradeLog.Seller,
                                    goodsTradeLog.Id);
                        noticeUid = goodsTradeLog.Buyerid;
                        posterId = goodsTradeLog.Sellerid;
                        poster = goodsTradeLog.Seller;
                        isSendNotice = true;
                        break; 
                    }
                case TradeStatusEnum.WAIT_SELLER_CONFIRM_GOODS:
                    {
                        noticeContent = string.Format("买家 {0} 已退货, 等待您收货, 请<a href =\"" + pageName + "?goodstradelogid={1}\">点击这里</a>查看详情.",
                                     goodsTradeLog.Buyer,
                                    goodsTradeLog.Id);
                        noticeUid = goodsTradeLog.Sellerid;
                        posterId = goodsTradeLog.Buyerid;
                        poster = goodsTradeLog.Buyername;
                        isSendNotice = true;
                        break; 
                    }
                case TradeStatusEnum.TRADE_FINISHED:
                    {
                        noticeContent = string.Format("商品 {0} 已交易成功, 请<a href =\"goodsrate.aspx?goodstradelogid={1}\">点击这里</a>给对方评分.",
                                    goodsTradeLog.Subject,
                                    goodsTradeLog.Id);
                        noticeUid = goodsTradeLog.Sellerid;
                        posterId = goodsTradeLog.Buyerid;
                        poster = goodsTradeLog.Buyername;
                        isSendNotice = true; 
                        break;
                    }
                case TradeStatusEnum.TRADE_CLOSED:
                    {
                        noticeContent = string.Format("商品 {0} 交易失败, 卖家取消交易, 请<a href =\"goodsrate.aspx?goodstradelogid={1}\">点击这里</a>查看详情.",
                                    goodsTradeLog.Subject,
                                    goodsTradeLog.Id);
                        noticeUid = goodsTradeLog.Sellerid;
                        posterId = goodsTradeLog.Buyerid;
                        poster = goodsTradeLog.Buyername;
                        isSendNotice = true;
                        break;
                    }          
                case TradeStatusEnum.REFUND_SUCCESS:
                    {
                        noticeContent = string.Format("商品 {0} 已退款成功, 请<a href =\"goodsrate.aspx?goodstradelogid={1}\">点击这里</a>给对方评分.",
                                    goodsTradeLog.Subject,
                                    goodsTradeLog.Id);
                        noticeUid = goodsTradeLog.Buyerid;
                        posterId = goodsTradeLog.Sellerid;
                        poster = goodsTradeLog.Seller;
                        isSendNotice = true;
                        break;
                    }
            }

            //发送通知
            if (isSendNotice)
            {
                NoticeInfo noticeInfo = new NoticeInfo();
                //商品交易通知
                noticeInfo.Note = Utils.HtmlEncode(noticeContent);
                noticeInfo.Uid = noticeUid;
                noticeInfo.Type = NoticeType.GoodsTradeNotice;
                noticeInfo.New = 1;
                noticeInfo.Posterid = posterId;
                noticeInfo.Poster = poster;
                noticeInfo.Postdatetime = Utils.GetDateTime();

                Notices.CreateNoticeInfo(noticeInfo);
            }
            return true;
        }

      
        /// <summary>
        /// 获取指定商品交易日志id的交易信息
        /// </summary>
        /// <param name="goodstradelogid">商品交易日志id</param>
        /// <returns>交易信息</returns>
        public static Goodstradeloginfo GetGoodsTradeLogInfo(int goodsTradeLogId)
        {
            return DTO.GetGoodsTradeLogInfo(DbProvider.GetInstance().GetGoodsTradeLogByID(goodsTradeLogId));
        }

        /// <summary>
        /// 根据交易单的流水号来获取交易信息
        /// </summary>
        /// <param name="tradeno">交易单的流水号</param>
        /// <returns>交易信息</returns>
        public static Goodstradeloginfo GetGoodsTradeLogInfo(string tradeNo)
        {
            return DTO.GetGoodsTradeLogInfo(DbProvider.GetInstance().GetGoodsTradeLogByTradeNo(tradeNo));
        }

        

        /// <summary>
        /// 获取指定商品id和相关条件下的商品交易信息集合
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式(0:升序, 1:降序)</param>
        /// <returns>商品交易信息集合</returns>
        public static GoodstradeloginfoCollection GetGoodsTradeLog(int goodsId, int pageSize, int pageIndex, string orderBy, int ascDesc)
        {
            GoodstradeloginfoCollection coll = new GoodstradeloginfoCollection();

            if (pageIndex <= 0)
                return coll;

            string condition = DbProvider.GetInstance().SetGoodsTradeStatusCond((int)MallUtils.OperaCode.Equal, 7);
            return DTO.GetGoodsTradeLogInfoList(DbProvider.GetInstance().GetGoodsTradeLogByGid(goodsId, pageSize, pageIndex, condition, orderBy, ascDesc));
        }


        /// <summary>
        /// 获取指定商品id和条件下的商品交易数
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <returns>交易日志数</returns>
        public static int GetGoodsTradeLogCount(int goodsid)
        {
            string condition = DbProvider.GetInstance().SetGoodsTradeStatusCond((int)MallUtils.OperaCode.Equal, 7);
            return DbProvider.GetInstance().GetTradeLogCountByGid(goodsid, condition);
        }

        /// <summary>
        /// 获取交易日志数
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="uidtype">用户类型</param>
        /// <param name="filter">过滤方式</param>
        /// <param name="pagesize">页面尺寸</param>
        /// <param name="pageindex">当前页面</param>
        /// <returns>交易日志数</returns>
        public static DataTable GetGoodsTradeLogList(int userId, string goodsIdList, int uidType, string filter, int pageSize, int pageIndex)
        {
            return DbProvider.GetInstance().GetGoodsTradeLogList(userId, goodsIdList, uidType, filter, pageSize, pageIndex);
        }

        /// <summary>
        /// 获取交易日志数
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="uidtype">用户类型</param>
        /// <param name="filter">过滤方式</param>
        /// <returns>交易日志数</returns>
        public static int GetGoodsTradeLogCount(int userId, string goodsIdList, int uidType, string filter)
        {
            return DbProvider.GetInstance().GetGoodsTradeLogCount(userId, goodsIdList, uidType, filter);
        }

        /// <summary>
        /// 获取指定商品id和相关条件下的商品交易信息(json数据串)
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns>交易json数据</returns>
        public static StringBuilder GetTradeLogJson(int goodsId, int pageSize, int pageIndex, string orderBy, int ascDesc)
        {
            StringBuilder tradeLogJson = new StringBuilder("[");
            foreach (Goodstradeloginfo goodsTradeLogInfo in GetGoodsTradeLog(goodsId, pageSize, pageIndex, orderBy, ascDesc))
            {
                tradeLogJson.Append(string.Format("{{'buyerid' : {0}, 'buyer' : '{1}', 'price' : {2}, 'number' : {3}, 'lastupdate' : '{4}', 'buyercredit' : {5}, 'status' : {6}}},",
                                    goodsTradeLogInfo.Buyerid,
                                    goodsTradeLogInfo.Buyer, 
                                    goodsTradeLogInfo.Price, 
                                    goodsTradeLogInfo.Number, 
                                    goodsTradeLogInfo.Lastupdate, 
                                    goodsTradeLogInfo.Buyercredit, 
                                    goodsTradeLogInfo.Status));
            }
            if (tradeLogJson.ToString().EndsWith(","))
                tradeLogJson.Remove(tradeLogJson.Length - 1, 1);

            tradeLogJson.Append("]");
            return tradeLogJson;
        }

        /// <summary>
        /// 获取指定用户的商品交易统计信息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns>商品交易统计信息</returns>
        public static Goosdstradestatisticinfo GetTradeStatistic(int userId)
        {
            Goosdstradestatisticinfo goodstradestatistic = null;
            IDataReader iDataReader = DbProvider.GetInstance().GetTradeStatistic(userId);

            //绑定新的查询数据
            if (iDataReader.Read())
            {
                goodstradestatistic = new Goosdstradestatisticinfo();
                goodstradestatistic.Userid = userId;
                goodstradestatistic.Sellerattention = TypeConverter.ObjectToInt(iDataReader["SellerAttention"]);
                goodstradestatistic.Sellertrading = TypeConverter.ObjectToInt(iDataReader["SellerTrading"]);
                goodstradestatistic.Sellerrate = TypeConverter.ObjectToInt(iDataReader["SellerRate"]);
                goodstradestatistic.Sellnumbersum = Convert.ToDecimal(iDataReader["SellNumberSum"].ToString());
                goodstradestatistic.Selltradesum = Convert.ToDecimal(iDataReader["SellTradeSum"].ToString());
                goodstradestatistic.Buyerattention = TypeConverter.ObjectToInt(iDataReader["BuyerAttention"]);
                goodstradestatistic.Buyertrading = TypeConverter.ObjectToInt(iDataReader["BuyerTrading"]);
                goodstradestatistic.Buyerrate = TypeConverter.ObjectToInt(iDataReader["BuyerRate"]);
                goodstradestatistic.Buynumbersum = Convert.ToDecimal(iDataReader["BuyNumberSum"].ToString());
                goodstradestatistic.Buytradesum = Convert.ToDecimal(iDataReader["BuyTradeSum"].ToString());
            }
            iDataReader.Close();
            return goodstradestatistic;
        }

        /// <summary>
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// 获得商品交易信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回商品交易信息</returns>
            public static Goodstradeloginfo GetGoodsTradeLogInfo(IDataReader reader)
            {
                Goodstradeloginfo goodsTradeLogInfo = null;
                if (reader.Read())
                {
                    goodsTradeLogInfo = new Goodstradeloginfo();
                    goodsTradeLogInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                    goodsTradeLogInfo.Goodsid = TypeConverter.ObjectToInt(reader["goodsid"]);
                    goodsTradeLogInfo.Orderid = reader["orderid"].ToString().Trim();
                    goodsTradeLogInfo.Tradeno = reader["tradeno"].ToString().Trim();
                    goodsTradeLogInfo.Subject = reader["subject"].ToString().Trim();
                    goodsTradeLogInfo.Price = Convert.ToDecimal(reader["price"].ToString());
                    goodsTradeLogInfo.Quality = Convert.ToInt16(reader["quality"].ToString());
                    goodsTradeLogInfo.Categoryid = TypeConverter.ObjectToInt(reader["categoryid"]);
                    goodsTradeLogInfo.Number = Convert.ToInt16(reader["number"].ToString());
                    goodsTradeLogInfo.Tax = Convert.ToDecimal(reader["tax"].ToString());
                    goodsTradeLogInfo.Locus = reader["locus"].ToString().Trim();
                    goodsTradeLogInfo.Sellerid = TypeConverter.ObjectToInt(reader["sellerid"]);
                    goodsTradeLogInfo.Seller = reader["seller"].ToString().Trim();
                    goodsTradeLogInfo.Selleraccount = reader["selleraccount"].ToString().Trim();
                    goodsTradeLogInfo.Buyerid = TypeConverter.ObjectToInt(reader["buyerid"]);
                    goodsTradeLogInfo.Buyer = reader["buyer"].ToString().Trim();
                    goodsTradeLogInfo.Buyercontact = reader["buyercontact"].ToString().Trim();
                    goodsTradeLogInfo.Buyercredit = Convert.ToInt16(reader["buyercredit"].ToString());
                    goodsTradeLogInfo.Buyermsg = reader["buyermsg"].ToString().Trim();
                    goodsTradeLogInfo.Status = Convert.ToInt16(reader["status"].ToString());
                    goodsTradeLogInfo.Lastupdate = Convert.ToDateTime(reader["lastupdate"].ToString());
                    goodsTradeLogInfo.Offline = Convert.ToInt16(reader["offline"].ToString());
                    goodsTradeLogInfo.Buyername = reader["buyername"].ToString().Trim();
                    goodsTradeLogInfo.Buyerzip = reader["buyerzip"].ToString().Trim();
                    goodsTradeLogInfo.Buyerphone = reader["buyerphone"].ToString().Trim();
                    goodsTradeLogInfo.Buyermobile = reader["buyermobile"].ToString().Trim();
                    goodsTradeLogInfo.Transport = Convert.ToInt16(reader["transport"].ToString());
                    goodsTradeLogInfo.Transportpay = Convert.ToInt16(reader["transportpay"].ToString());
                    goodsTradeLogInfo.Transportfee = Convert.ToDecimal(reader["transportfee"].ToString());
                    goodsTradeLogInfo.Tradesum = Convert.ToDecimal(reader["tradesum"].ToString());
                    goodsTradeLogInfo.Baseprice = Convert.ToDecimal(reader["baseprice"].ToString());
                    goodsTradeLogInfo.Discount = Convert.ToInt16(reader["discount"].ToString());
                    goodsTradeLogInfo.Ratestatus = Convert.ToInt16(reader["ratestatus"].ToString());
                    goodsTradeLogInfo.Message = reader["message"].ToString().Trim();

                    reader.Close();
                }
                return goodsTradeLogInfo;
            }

            /// <summary>
            /// 获得商品交易信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回商品交易信息</returns>
            public static GoodstradeloginfoCollection GetGoodsTradeLogInfoList(IDataReader reader)
            {
                GoodstradeloginfoCollection goodsTradeLogInfoColl = new GoodstradeloginfoCollection();

                while (reader.Read())
                {
                    Goodstradeloginfo goodsTradeLogInfo = new Goodstradeloginfo();
                    goodsTradeLogInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
                    goodsTradeLogInfo.Goodsid = TypeConverter.ObjectToInt(reader["goodsid"]);
                    goodsTradeLogInfo.Orderid = reader["orderid"].ToString().Trim();
                    goodsTradeLogInfo.Tradeno = reader["tradeno"].ToString().Trim();
                    goodsTradeLogInfo.Subject = reader["subject"].ToString().Trim();
                    goodsTradeLogInfo.Price = Convert.ToDecimal(reader["price"].ToString());
                    goodsTradeLogInfo.Quality = Convert.ToInt16(reader["quality"].ToString());
                    goodsTradeLogInfo.Categoryid = TypeConverter.ObjectToInt(reader["categoryid"]);
                    goodsTradeLogInfo.Number = Convert.ToInt16(reader["number"].ToString());
                    goodsTradeLogInfo.Tax = Convert.ToDecimal(reader["tax"].ToString());
                    goodsTradeLogInfo.Locus = reader["locus"].ToString().Trim();
                    goodsTradeLogInfo.Sellerid = TypeConverter.ObjectToInt(reader["sellerid"]);
                    goodsTradeLogInfo.Seller = reader["seller"].ToString().Trim();
                    goodsTradeLogInfo.Selleraccount = reader["selleraccount"].ToString().Trim();
                    goodsTradeLogInfo.Buyerid = TypeConverter.ObjectToInt(reader["buyerid"]);
                    goodsTradeLogInfo.Buyer = reader["buyer"].ToString().Trim();
                    goodsTradeLogInfo.Buyercontact = reader["buyercontact"].ToString().Trim();
                    goodsTradeLogInfo.Buyercredit = Convert.ToInt16(reader["buyercredit"].ToString());
                    goodsTradeLogInfo.Buyermsg = reader["buyermsg"].ToString().Trim();
                    goodsTradeLogInfo.Status = Convert.ToInt16(reader["status"].ToString());
                    goodsTradeLogInfo.Lastupdate = Convert.ToDateTime(reader["lastupdate"].ToString());
                    goodsTradeLogInfo.Offline = Convert.ToInt16(reader["offline"].ToString());
                    goodsTradeLogInfo.Buyername = reader["buyername"].ToString().Trim();
                    goodsTradeLogInfo.Buyerzip = reader["buyerzip"].ToString().Trim();
                    goodsTradeLogInfo.Buyerphone = reader["buyerphone"].ToString().Trim();
                    goodsTradeLogInfo.Buyermobile = reader["buyermobile"].ToString().Trim();
                    goodsTradeLogInfo.Transport = Convert.ToInt16(reader["transport"].ToString());
                    goodsTradeLogInfo.Transportpay = Convert.ToInt16(reader["transportpay"].ToString());
                    goodsTradeLogInfo.Transportfee = Convert.ToDecimal(reader["transportfee"].ToString());
                    goodsTradeLogInfo.Tradesum = Convert.ToDecimal(reader["tradesum"].ToString());
                    goodsTradeLogInfo.Baseprice = Convert.ToDecimal(reader["baseprice"].ToString());
                    goodsTradeLogInfo.Discount = Convert.ToInt16(reader["discount"].ToString());
                    goodsTradeLogInfo.Ratestatus = Convert.ToInt16(reader["ratestatus"].ToString());
                    goodsTradeLogInfo.Message = reader["message"].ToString().Trim();

                    goodsTradeLogInfoColl.Add(goodsTradeLogInfo);
                }
                reader.Close();

                return goodsTradeLogInfoColl;
            }

            /// <summary>
            /// 获得商品交易信息(DTO)
            /// </summary>
            /// <param name="dt">要转换的数据表</param>
            /// <returns>返回商品交易信息</returns>
            public static Goodstradeloginfo[] GetGoodsTradeLogInfoArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Goodstradeloginfo[] goodsTradeLogInfoArray = new Goodstradeloginfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    goodsTradeLogInfoArray[i] = new Goodstradeloginfo();
                    goodsTradeLogInfoArray[i].Id = TypeConverter.ObjectToInt(dt.Rows[i]["id"]);
                    goodsTradeLogInfoArray[i].Goodsid = TypeConverter.ObjectToInt(dt.Rows[i]["goodsid"]);
                    goodsTradeLogInfoArray[i].Orderid = dt.Rows[i]["orderid"].ToString();
                    goodsTradeLogInfoArray[i].Tradeno = dt.Rows[i]["tradeno"].ToString();
                    goodsTradeLogInfoArray[i].Subject = dt.Rows[i]["subject"].ToString();
                    goodsTradeLogInfoArray[i].Price = Convert.ToDecimal(dt.Rows[i]["price"].ToString());
                    goodsTradeLogInfoArray[i].Quality = TypeConverter.ObjectToInt(dt.Rows[i]["quality"]);
                    goodsTradeLogInfoArray[i].Categoryid = TypeConverter.ObjectToInt(dt.Rows[i]["categoryid"]);
                    goodsTradeLogInfoArray[i].Number = TypeConverter.ObjectToInt(dt.Rows[i]["number"]);
                    goodsTradeLogInfoArray[i].Tax = Convert.ToDecimal(dt.Rows[i]["tax"].ToString());
                    goodsTradeLogInfoArray[i].Locus = dt.Rows[i]["locus"].ToString();
                    goodsTradeLogInfoArray[i].Sellerid = TypeConverter.ObjectToInt(dt.Rows[i]["sellerid"]);
                    goodsTradeLogInfoArray[i].Seller = dt.Rows[i]["seller"].ToString();
                    goodsTradeLogInfoArray[i].Selleraccount = dt.Rows[i]["selleraccount"].ToString();
                    goodsTradeLogInfoArray[i].Buyerid = TypeConverter.ObjectToInt(dt.Rows[i]["buyerid"]);
                    goodsTradeLogInfoArray[i].Buyer = dt.Rows[i]["buyer"].ToString();
                    goodsTradeLogInfoArray[i].Buyercontact = dt.Rows[i]["buyercontact"].ToString();
                    goodsTradeLogInfoArray[i].Buyercredit = TypeConverter.ObjectToInt(dt.Rows[i]["buyercredit"]);
                    goodsTradeLogInfoArray[i].Buyermsg = dt.Rows[i]["buyermsg"].ToString();
                    goodsTradeLogInfoArray[i].Status = TypeConverter.ObjectToInt(dt.Rows[i]["status"]);
                    goodsTradeLogInfoArray[i].Lastupdate = Convert.ToDateTime(dt.Rows[i]["lastupdate"].ToString());
                    goodsTradeLogInfoArray[i].Offline = TypeConverter.ObjectToInt(dt.Rows[i]["offline"]);
                    goodsTradeLogInfoArray[i].Buyername = dt.Rows[i]["buyername"].ToString();
                    goodsTradeLogInfoArray[i].Buyerzip = dt.Rows[i]["buyerzip"].ToString();
                    goodsTradeLogInfoArray[i].Buyerphone = dt.Rows[i]["buyerphone"].ToString();
                    goodsTradeLogInfoArray[i].Buyermobile = dt.Rows[i]["buyermobile"].ToString();
                    goodsTradeLogInfoArray[i].Transport = TypeConverter.ObjectToInt(dt.Rows[i]["transport"]);
                    goodsTradeLogInfoArray[i].Transportpay = TypeConverter.ObjectToInt(dt.Rows[i]["transportpay"]);
                    goodsTradeLogInfoArray[i].Transportfee = Convert.ToDecimal(dt.Rows[i]["transportfee"].ToString());
                    goodsTradeLogInfoArray[i].Tradesum = Convert.ToDecimal(dt.Rows[i]["tradesum"].ToString());
                    goodsTradeLogInfoArray[i].Baseprice = Convert.ToDecimal(dt.Rows[i]["baseprice"].ToString());
                    goodsTradeLogInfoArray[i].Discount = TypeConverter.ObjectToInt(dt.Rows[i]["discount"]);
                    goodsTradeLogInfoArray[i].Ratestatus = TypeConverter.ObjectToInt(dt.Rows[i]["ratestatus"]);
                    goodsTradeLogInfoArray[i].Message = dt.Rows[i]["message"].ToString();

                }
                dt.Dispose();
                return goodsTradeLogInfoArray;
            }
        }
    }
}
