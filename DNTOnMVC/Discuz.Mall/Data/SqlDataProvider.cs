using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Mall.Data
{
    /// <summary>
    /// 数据提供者
    /// </summary>
    public class DataProvider 
    {
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="__goods">要添加的商品信息实例</param>
        /// <returns></returns>
        public int CreateGoods(Goodsinfo goodsInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@shopid", (DbType)SqlDbType.Int, 4,goodsInfo.Shopid),
						DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,goodsInfo.Categoryid),
                        DbHelper.MakeInParam("@parentcategorylist", (DbType)SqlDbType.Char, 300,goodsInfo.Parentcategorylist),
                        DbHelper.MakeInParam("@shopcategorylist", (DbType)SqlDbType.Char, 300,goodsInfo.Shopcategorylist),
						DbHelper.MakeInParam("@recommend", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Recommend),
						DbHelper.MakeInParam("@discount", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Discount),
                        DbHelper.MakeInParam("@selleruid", (DbType)SqlDbType.Int, 4,goodsInfo.Selleruid),
						DbHelper.MakeInParam("@seller", (DbType)SqlDbType.NChar, 20,goodsInfo.Seller),
						DbHelper.MakeInParam("@account", (DbType)SqlDbType.NChar, 50,goodsInfo.Account),
						DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 30,goodsInfo.Title),
						DbHelper.MakeInParam("@magic", (DbType)SqlDbType.Int, 4,goodsInfo.Magic),
						DbHelper.MakeInParam("@price", (DbType)SqlDbType.Decimal, 18,goodsInfo.Price),
						DbHelper.MakeInParam("@amount", (DbType)SqlDbType.SmallInt, 2,goodsInfo.Amount),
						DbHelper.MakeInParam("@quality", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Quality),
						DbHelper.MakeInParam("@lid", (DbType)SqlDbType.Int, 4,goodsInfo.Lid),
						DbHelper.MakeInParam("@locus", (DbType)SqlDbType.NChar, 20,goodsInfo.Locus),
						DbHelper.MakeInParam("@transport", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Transport),
						DbHelper.MakeInParam("@ordinaryfee", (DbType)SqlDbType.Decimal, 18,goodsInfo.Ordinaryfee),
						DbHelper.MakeInParam("@expressfee", (DbType)SqlDbType.Decimal, 18,goodsInfo.Expressfee),
						DbHelper.MakeInParam("@emsfee", (DbType)SqlDbType.Decimal, 18,goodsInfo.Emsfee),
						DbHelper.MakeInParam("@itemtype", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Itemtype),
						DbHelper.MakeInParam("@dateline", (DbType)SqlDbType.DateTime, 8,goodsInfo.Dateline),
						DbHelper.MakeInParam("@expiration", (DbType)SqlDbType.DateTime, 8,goodsInfo.Expiration),
						DbHelper.MakeInParam("@lastbuyer", (DbType)SqlDbType.NChar, 10,goodsInfo.Lastbuyer),
						DbHelper.MakeInParam("@lasttrade", (DbType)SqlDbType.DateTime, 8,goodsInfo.Lasttrade),
						DbHelper.MakeInParam("@lastupdate", (DbType)SqlDbType.DateTime, 8,goodsInfo.Lastupdate),
						DbHelper.MakeInParam("@totalitems", (DbType)SqlDbType.SmallInt, 2,goodsInfo.Totalitems),
						DbHelper.MakeInParam("@tradesum", (DbType)SqlDbType.Decimal, 18,goodsInfo.Tradesum),
						DbHelper.MakeInParam("@closed", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Closed),
						DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4,goodsInfo.Aid),
                        DbHelper.MakeInParam("@goodspic", (DbType)SqlDbType.NChar, 100,goodsInfo.Goodspic),
						DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 1,goodsInfo.Displayorder),
						DbHelper.MakeInParam("@costprice", (DbType)SqlDbType.Decimal, 18,goodsInfo.Costprice),
						DbHelper.MakeInParam("@invoice", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Invoice),
						DbHelper.MakeInParam("@repair", (DbType)SqlDbType.SmallInt, 2,goodsInfo.Repair),
						DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0,goodsInfo.Message),
						DbHelper.MakeInParam("@otherlink", (DbType)SqlDbType.NChar, 250,goodsInfo.Otherlink),
						DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4,goodsInfo.Readperm),
						DbHelper.MakeInParam("@tradetype", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Tradetype),
                        DbHelper.MakeInParam("@viewcount", (DbType)SqlDbType.Int, 4,goodsInfo.Viewcount),
						DbHelper.MakeInParam("@smileyoff", (DbType)SqlDbType.Int, 4,goodsInfo.Smileyoff),
						DbHelper.MakeInParam("@bbcodeoff", (DbType)SqlDbType.Int, 4,goodsInfo.Bbcodeoff),
						DbHelper.MakeInParam("@parseurloff", (DbType)SqlDbType.Int, 4,goodsInfo.Parseurloff),
                        DbHelper.MakeInParam("@highlight", (DbType)SqlDbType.VarChar, 500,goodsInfo.Highlight)
				};
            string commandText = String.Format("INSERT INTO [{0}goods] ([shopid], [categoryid], [parentcategorylist], [shopcategorylist], [recommend], [discount], [selleruid], [seller], [account], [title], [magic], [price], [amount], [quality], [lid], [locus], [transport], [ordinaryfee], [expressfee], [emsfee], [itemtype], [dateline], [expiration], [lastbuyer], [lasttrade], [lastupdate], [totalitems], [tradesum], [closed], [aid], [goodspic], [displayorder], [costprice], [invoice], [repair], [message], [otherlink], [readperm], [tradetype], [viewcount], [smileyoff], [bbcodeoff], [parseurloff], [highlight] ) VALUES (@shopid, @categoryid, @parentcategorylist,@shopcategorylist, @recommend, @discount, @selleruid, @seller, @account, @title, @magic, @price, @amount, @quality, @lid, @locus, @transport, @ordinaryfee, @expressfee, @emsfee, @itemtype, @dateline, @expiration, @lastbuyer, @lasttrade, @lastupdate, @totalitems, @tradesum, @closed, @aid, @goodspic, @displayorder, @costprice, @invoice, @repair, @message, @otherlink, @readperm, @tradetype, @viewcount, @smileyoff, @bbcodeoff, @parseurloff, @highlight);SELECT SCOPE_IDENTITY()  AS 'goodsid'", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="goods">要更新的商品信息实例</param>
        /// <returns></returns>
        public bool UpdateGoods(Goodsinfo goodsInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@shopid", (DbType)SqlDbType.Int, 4,goodsInfo.Shopid),
						DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,goodsInfo.Categoryid),
                        DbHelper.MakeInParam("@parentcategorylist", (DbType)SqlDbType.Char, 300,goodsInfo.Parentcategorylist),
                        DbHelper.MakeInParam("@shopcategorylist", (DbType)SqlDbType.Char, 300,goodsInfo.Shopcategorylist),
						DbHelper.MakeInParam("@recommend", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Recommend),
						DbHelper.MakeInParam("@discount", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Discount),
                        DbHelper.MakeInParam("@selleruid", (DbType)SqlDbType.Int, 4,goodsInfo.Selleruid),
						DbHelper.MakeInParam("@seller", (DbType)SqlDbType.NChar, 20,goodsInfo.Seller),
						DbHelper.MakeInParam("@account", (DbType)SqlDbType.NChar, 50,goodsInfo.Account),
						DbHelper.MakeInParam("@title", (DbType)SqlDbType.NChar, 30,goodsInfo.Title),
						DbHelper.MakeInParam("@magic", (DbType)SqlDbType.Int, 4,goodsInfo.Magic),
						DbHelper.MakeInParam("@price", (DbType)SqlDbType.Decimal, 18,goodsInfo.Price),
						DbHelper.MakeInParam("@amount", (DbType)SqlDbType.SmallInt, 2,goodsInfo.Amount),
						DbHelper.MakeInParam("@quality", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Quality),
						DbHelper.MakeInParam("@lid", (DbType)SqlDbType.Int, 4,goodsInfo.Lid),
						DbHelper.MakeInParam("@locus", (DbType)SqlDbType.NChar, 20,goodsInfo.Locus),
						DbHelper.MakeInParam("@transport", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Transport),
			            DbHelper.MakeInParam("@ordinaryfee", (DbType)SqlDbType.Decimal, 18,goodsInfo.Ordinaryfee),
						DbHelper.MakeInParam("@expressfee", (DbType)SqlDbType.Decimal, 18,goodsInfo.Expressfee),
						DbHelper.MakeInParam("@emsfee", (DbType)SqlDbType.Decimal, 18,goodsInfo.Emsfee),
						DbHelper.MakeInParam("@itemtype", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Itemtype),
						DbHelper.MakeInParam("@dateline", (DbType)SqlDbType.DateTime, 8,goodsInfo.Dateline),
						DbHelper.MakeInParam("@expiration", (DbType)SqlDbType.DateTime, 8,goodsInfo.Expiration),
						DbHelper.MakeInParam("@lastbuyer", (DbType)SqlDbType.NChar, 10,goodsInfo.Lastbuyer),
						DbHelper.MakeInParam("@lasttrade", (DbType)SqlDbType.DateTime, 8,goodsInfo.Lasttrade),
						DbHelper.MakeInParam("@lastupdate", (DbType)SqlDbType.DateTime, 8,goodsInfo.Lastupdate),
						DbHelper.MakeInParam("@totalitems", (DbType)SqlDbType.SmallInt, 2,goodsInfo.Totalitems),
						DbHelper.MakeInParam("@tradesum", (DbType)SqlDbType.Decimal, 18,goodsInfo.Tradesum),
						DbHelper.MakeInParam("@closed", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Closed),
						DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4,goodsInfo.Aid),
                        DbHelper.MakeInParam("@goodspic", (DbType)SqlDbType.NChar, 100,goodsInfo.Goodspic),
						DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 1,goodsInfo.Displayorder),
						DbHelper.MakeInParam("@costprice", (DbType)SqlDbType.Decimal, 18,goodsInfo.Costprice),
						DbHelper.MakeInParam("@invoice", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Invoice),
						DbHelper.MakeInParam("@repair", (DbType)SqlDbType.SmallInt, 2,goodsInfo.Repair),
						DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0,goodsInfo.Message),
						DbHelper.MakeInParam("@otherlink", (DbType)SqlDbType.NChar, 250,goodsInfo.Otherlink),
						DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4,goodsInfo.Readperm),
						DbHelper.MakeInParam("@tradetype", (DbType)SqlDbType.TinyInt, 1,goodsInfo.Tradetype),
						DbHelper.MakeInParam("@viewcount", (DbType)SqlDbType.Int, 4,goodsInfo.Viewcount),
						DbHelper.MakeInParam("@smileyoff", (DbType)SqlDbType.Int, 4,goodsInfo.Smileyoff),
						DbHelper.MakeInParam("@bbcodeoff", (DbType)SqlDbType.Int, 4,goodsInfo.Bbcodeoff),
						DbHelper.MakeInParam("@parseurloff", (DbType)SqlDbType.Int, 4,goodsInfo.Parseurloff),
                        DbHelper.MakeInParam("@highlight", (DbType)SqlDbType.VarChar, 500,goodsInfo.Highlight),
                        DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4,goodsInfo.Goodsid)
				};
            string commandText = String.Format("Update [{0}goods]  Set [shopid] = @shopid, [categoryid] = @categoryid, [parentcategorylist] = @parentcategorylist, [shopcategorylist] = @shopcategorylist, [recommend] = @recommend, [discount] = @discount, selleruid = @selleruid, [seller] = @seller, [account] = @account, [title] = @title, [magic] = @magic, [price] = @price, [amount] = @amount, [quality] = @quality, [lid] = @lid, [locus] = @locus, [transport] = @transport, [ordinaryfee] = @ordinaryfee, [expressfee] = @expressfee, [emsfee] = @emsfee, [itemtype] = @itemtype, [dateline] = @dateline, [expiration] = @expiration, [lastbuyer] = @lastbuyer, [lasttrade] = @lasttrade, [lastupdate] = @lastupdate, [totalitems] = @totalitems, [tradesum] = @tradesum, [closed] = @closed, [aid] = @aid, [goodspic] = @goodspic, [displayorder] = @displayorder, [costprice] = @costprice, [invoice] = @invoice, [repair] = @repair, [message] = @message, [otherlink] = @otherlink, [readperm] = @readperm, [tradetype] = @tradetype, [viewcount] = @viewcount, [smileyoff] = @smileyoff, [bbcodeoff] = @bbcodeoff, [highlight] = @highlight  WHERE [goodsid] = @goodsid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 更新指定商品分类的显示顺序
        /// </summary>
        /// <param name="displayorder">显示顺序值</param>
        /// <param name="categoryid">更新的商品分类id</param>
        public void UpdateGoodsCategoryDisplayorder(int displayOrder, int categoryId)
        {
            DbParameter[] parms =
			{
                DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayOrder),
                DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId)
			};
            DbHelper.ExecuteDataset(CommandType.Text, string.Format("UPDATE [{0}goodscategories] SET [displayorder]=@displayorder WHERE [categoryid]=@categoryid", BaseConfigs.GetTablePrefix), parms);
        }

       
        /// <summary>
        /// 返回商品分类数据表(已绑定fid)
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoriesTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}goodscategories] WHERE [fid] > 0 ORDER BY [categoryid] ASC", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 获取全部商品分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCategoriesTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}goodscategories] ORDER BY [categoryid] ASC", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 返回商品一级(root)分类数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetRootCategoriesTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}goodscategories] WHERE [parentid] = 0 ORDER BY [categoryid] ASC", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 返回用于生成JSON格式串的商品分类数据表
        /// </summary>
        /// <returns>返回JSON数据串</returns>
        public DataTable GetCategoriesTableToJson()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT [categoryid] AS [id], [parentid] AS [pid], [layer] AS [layer], [parentidlist] AS [pidlist], [categoryname] AS [name], [haschild] AS [child], [fid] FROM [{0}goodscategories] ORDER BY [categoryid] ASC", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 通过商品分类id得到其所指向的父id
        /// </summary>
        /// <param name="categoryid">要查询的分类id</param>
        /// <returns>父id</returns>
        public int GetCategoriesParentidByID(int categoryId)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId)
			};
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT TOP 1 [parentid] FROM [{0}goodscategories] WHERE categoryid=@categoryid", BaseConfigs.GetTablePrefix), parms));
        }

        /// <summary>
        /// 创建商品分类
        /// </summary>
        /// <param name="goodscategories">要创建的商品分类信息</param>
        /// <returns>创建的商品分类id</returns>
        public int CreateGoodscategory(Goodscategoryinfo goodsCategoryInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@parentid", (DbType)SqlDbType.Int, 4,goodsCategoryInfo.Parentid),
						DbHelper.MakeInParam("@layer", (DbType)SqlDbType.SmallInt, 2,goodsCategoryInfo.Layer),
						DbHelper.MakeInParam("@parentidlist", (DbType)SqlDbType.Char, 300,goodsCategoryInfo.Parentidlist),
                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,goodsCategoryInfo.Displayorder),
						DbHelper.MakeInParam("@categoryname", (DbType)SqlDbType.NChar, 50,goodsCategoryInfo.Categoryname),
						DbHelper.MakeInParam("@haschild", (DbType)SqlDbType.Bit, 1,goodsCategoryInfo.Haschild),
						DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4,goodsCategoryInfo.Fid),
						DbHelper.MakeInParam("@pathlist", (DbType)SqlDbType.NChar, 3000,goodsCategoryInfo.Pathlist),
						DbHelper.MakeInParam("@goodscount", (DbType)SqlDbType.Int, 4,goodsCategoryInfo.Goodscount)
				};
            string commandText = String.Format("INSERT INTO [{0}goodscategories] ([parentid], [layer], [parentidlist], [displayorder], [categoryname], [haschild], [fid], [pathlist], [goodscount]) VALUES (@parentid, @layer, @parentidlist, @displayorder, @categoryname, @haschild, @fid, @pathlist, @goodscount);SELECT SCOPE_IDENTITY()  AS aid", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), -1);
        }

        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="categoryid">要删除的商品分类ID</param>
        public void DeleteGoodsCategory(int categoryId)
        {
            DbHelper.ExecuteReader(CommandType.Text, 
                                   string.Format("DELETE FROM [{0}goodscategories] WHERE [categoryid] = @categoryid", BaseConfigs.GetTablePrefix), 
                                   DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId));
        }

        /// <summary>
        /// 更新商品分类
        /// </summary>
        /// <param name="goodscategories">要更新的商品分类信息</param>
        /// <returns></returns>
        public bool UpdateGoodscategory(Goodscategoryinfo goodsCategoryInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@parentid", (DbType)SqlDbType.Int, 4,goodsCategoryInfo.Parentid),
						DbHelper.MakeInParam("@layer", (DbType)SqlDbType.SmallInt, 2,goodsCategoryInfo.Layer),
						DbHelper.MakeInParam("@parentidlist", (DbType)SqlDbType.Char, 300,goodsCategoryInfo.Parentidlist),
                        DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,goodsCategoryInfo.Displayorder),
						DbHelper.MakeInParam("@categoryname", (DbType)SqlDbType.NChar, 50,goodsCategoryInfo.Categoryname),
						DbHelper.MakeInParam("@haschild", (DbType)SqlDbType.Bit, 1,goodsCategoryInfo.Haschild),
						DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4,goodsCategoryInfo.Fid),
						DbHelper.MakeInParam("@pathlist", (DbType)SqlDbType.NChar, 3000,goodsCategoryInfo.Pathlist),
						DbHelper.MakeInParam("@goodscount", (DbType)SqlDbType.Int, 4,goodsCategoryInfo.Goodscount),
                        DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,goodsCategoryInfo.Categoryid)
				};
            string commandText = String.Format("Update [{0}goodscategories]  Set [parentid] = @parentid, [layer] = @layer, [parentidlist] = @parentidlist, [displayorder] = @displayorder, [categoryname] = @categoryname, [haschild] = @haschild, [fid] = @fid, [pathlist] = @pathlist, [goodscount] = @goodscount WHERE [categoryid] = @categoryid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 获取指定商品分类ID的数据
        /// </summary>
        /// <param name="categoryid">商品分类ID</param>
        /// <returns></returns>
        public IDataReader GetGoodsCategoryInfoById(int categoryId)
        {
            DbParameter parm = DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}goodscategories] WHERE [categoryid] = @categoryid", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 更新指定商品分类的layer,parentidlist,haschild
        /// </summary>
        /// <param name="categoryid">要更新的商品分类id</param>
        /// <param name="layer">要更新的层</param>
        /// <param name="parentidlist">要更新的父id列表</param>
        /// <param name="haschild">要更新的haschild数据(是否有子分类)</param>
        public void UpdateCategoriesInfo(int categoryId, int layer, string parentIdList, int hasChild)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@layer", (DbType)SqlDbType.SmallInt, 2, layer),
				DbHelper.MakeInParam("@parentidlist", (DbType)SqlDbType.Char, 300, parentIdList),
                DbHelper.MakeInParam("@haschild", (DbType)SqlDbType.Bit, 1, hasChild),
				DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId)
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goodscategories] SET [layer]=@layer, [parentidlist]=@parentidlist, [haschild]=@haschild WHERE [categoryid]=@categoryid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 返回商品所在地数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetLocationsTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}locations] ORDER BY [lid] ASC", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 返回商品所在地的Sql语句
        /// </summary>
        /// <returns></returns>
        public string GetLocationsTableSql()
        {
            return string.Format("SELECT * FROM [{0}locations] ORDER BY [lid] ASC", BaseConfigs.GetTablePrefix);
        }

        /// <summary>
        /// 返回省(州)的数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetStatesTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT DISTINCT [state] FROM  [{0}locations] ORDER BY [state]", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 返回国家的数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetCountriesTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT DISTINCT [country] FROM  [{0}locations] ORDER BY [country]", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        /// 获取指定商品分类的根(root)结点分类
        /// </summary>
        /// <param name="categoryid">商品分类id</param>
        /// <returns></returns>
        public DataTable GetRootCategoryID(int categoryId)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId)
			};
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM  [{0}goodscategories] WHERE [categoryid]=@categoryid", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// 通过指定的LID获取商品所有地信息
        /// </summary>
        /// <param name="lid">所在地的lid</param>
        /// <returns></returns>
        public DataTable GetLocusByLID(int lid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@lid", (DbType)SqlDbType.Int, 4, lid)
			};
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM  [{0}locations] WHERE [lid]=@lid", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }



        /// <summary>
        /// 产生商品附件
        /// </summary>
        /// <param name="attachmentinfo">商品附件描述类实体</param>
        /// <returns>商品附件id</returns>
        public int CreateGoodsAttachment(Goodsattachmentinfo goodsAttachmentInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Uid),
						DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Goodsid),
						DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Categoryid),
						DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,DateTime.Parse(goodsAttachmentInfo.Postdatetime)),
                        DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Readperm),
						DbHelper.MakeInParam("@filename", (DbType)SqlDbType.NChar, 100,goodsAttachmentInfo.Filename),
						DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 100,goodsAttachmentInfo.Description),
						DbHelper.MakeInParam("@filetype", (DbType)SqlDbType.NChar, 50,goodsAttachmentInfo.Filetype),
						DbHelper.MakeInParam("@filesize", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Filesize),
						DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.NChar, 100,goodsAttachmentInfo.Attachment)
				};
            string commandText = String.Format("INSERT INTO [{0}goodsattachments] ([uid], [goodsid], [categoryid], [postdatetime], [readperm], [filename], [description], [filetype], [filesize], [attachment]) VALUES (@uid, @goodsid, @categoryid, @postdatetime, @readperm, @filename, @description, @filetype, @filesize, @attachment);SELECT SCOPE_IDENTITY()  AS aid", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parms), -1);
        }

        /// <summary>
        /// 更新商品附件信息
        /// </summary>
        /// <param name="attachmentinfo">商品附件描述类实体</param>
        /// <returns></returns>
        public bool SaveGoodsAttachment(Goodsattachmentinfo goodsAttachmentInfo)
        {

            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Uid),
						DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Goodsid),
						DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Categoryid),
						DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,DateTime.Parse(goodsAttachmentInfo.Postdatetime)),
                        DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Readperm),
						DbHelper.MakeInParam("@filename", (DbType)SqlDbType.NChar, 100,goodsAttachmentInfo.Filename),
						DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 100,goodsAttachmentInfo.Description),
						DbHelper.MakeInParam("@filetype", (DbType)SqlDbType.NChar, 50,goodsAttachmentInfo.Filetype),
						DbHelper.MakeInParam("@filesize", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Filesize),
						DbHelper.MakeInParam("@attachment", (DbType)SqlDbType.NChar, 100,goodsAttachmentInfo.Attachment),
                    	DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4,goodsAttachmentInfo.Aid)
				};
            string commandText = String.Format("Update [{0}goodsattachments]  Set [uid] = @uid, [goodsid] = @goodsid, [categoryid] = @categoryid, [postdatetime] = @postdatetime, [readperm] = @readperm, [filename] = @filename, [description] = @description, [filetype] = @filetype, [filesize] = @filesize, [attachment] = @attachment  WHERE [aid] = @aid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 更新商品附件信息
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <param name="readperm">读取权限</param>
        /// <param name="description">附件描述</param>
        /// <returns></returns>
        public bool SaveGoodsAttachment(int aid, int readPerm, string description)
        {
            DbParameter[] parms = 
				{
                        DbHelper.MakeInParam("@readperm", (DbType)SqlDbType.Int, 4,readPerm),
						DbHelper.MakeInParam("@description", (DbType)SqlDbType.NChar, 100,description),
                    	DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4,aid)
				};

            DbHelper.ExecuteNonQuery(CommandType.Text, String.Format("Update [{0}goodsattachments]  Set [readperm] = @readperm, [description] = @description  WHERE [aid] = @aid", BaseConfigs.GetTablePrefix), parms);

            return true;
        }


        /// <summary>
        /// 批量删除附件
        /// </summary>
        /// <param name="aidList">附件Id列表，以英文逗号分割</param>
        /// <returns></returns>
        public int DeleteGoodsAttachment(string aidList)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}goodsattachments] WHERE [aid] IN ({1})", BaseConfigs.GetTablePrefix, aidList));
        }

        /// <summary>
        /// 获取指定商品的所有附件信息
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <returns></returns>
        public IDataReader GetGoodsAttachmentsByGoodsid(int goodsId)
        {
            DbParameter parm = DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4, goodsId);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}goodsattachments] WHERE [goodsid] = @goodsid ORDER BY [aid] ASC", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 获取指定附件ID的信息
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <returns></returns>
        public IDataReader GetGoodsAttachmentsByAid(int aid)
        {
            DbParameter parm = DbHelper.MakeInParam("@aid", (DbType)SqlDbType.Int, 4, aid);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}goodsattachments] WHERE [aid] = @aid ", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 获取指定aid列表(","分割)附件
        /// </summary>
        /// <param name="aidList">aid列表(","分割)</param>
        /// <returns></returns>
        public IDataReader GetGoodsAttachmentListByAidList(string aidList)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}goodsattachments] WHERE [aid] IN ({1})", BaseConfigs.GetTablePrefix, aidList));
        }

        /// <summary>
        /// 获取商品所包含的Tag
        /// </summary>
        /// <param name="goodsid">商品Id</param>
        /// <returns></returns>
        public IDataReader GetTagsListByGoods(int goodsId)
        {
            string sql = string.Format("SELECT [{0}tags].* FROM [{0}tags], [{0}goodstags] WHERE [{0}goodstags].[tagid] = [{0}tags].[tagid] AND [{0}goodstags].[goodsid] = @goodsid ORDER BY [orderid]", BaseConfigs.GetTablePrefix);

            DbParameter parm = DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4, goodsId);

            return DbHelper.ExecuteReader(CommandType.Text, sql, parm);
        }

        /// <summary>
        /// 创建商品标签(已存在的标签不会被创建)
        /// </summary>
        /// <param name="tags">标签, 以半角空格分隔</param>
        /// <param name="goodsid">商品Id</param>
        /// <param name="userid">用户Id</param>
        /// <param name="curdatetime">提交时间</param>
        public void CreateGoodsTags(string tags, int goodsId, int userId, string curDateTime)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@tags", (DbType)SqlDbType.NVarChar, 55, tags),
                DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4, goodsId),
                DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4, userId),
                DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8, curDateTime)                
            };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}creategoodstags", BaseConfigs.GetTablePrefix), parms);
        }


        /// <summary>
        /// 设置指定商品分类id的路径信息
        /// </summary>
        /// <param name="pathlist">路径信息</param>
        /// <param name="categoryid">指定商品分类id</param>
        public void SetGoodsCategoryPathList(string pathList, int categoryId)
        {
            DbParameter[] parms = 
            {
			    DbHelper.MakeInParam("@pathlist", (DbType)SqlDbType.VarChar, 3000, pathList),
                DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goodscategories] SET pathlist=@pathlist  WHERE [categoryid]=@categoryid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="goodsid">商品Id</param>
        public IDataReader GetGoodsInfo(int goodsId)
        {
            DbParameter parm = DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4, goodsId);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}goods] WHERE [goodsid] = @goodsid ", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 更新指定商品分类下的商品数
        /// </summary>
        /// <param name="categoryid">指定的商品分类</param>
        /// <param name="parentidlist">指定分类的父分类列表</param>
        /// <param name="goodscount">要更新的商品数</param>
        public void UpdateCategoryGoodsCounts(int categoryId, string parentIdList, int goodsCount)
        {
            string commandText = string.Format("UPDATE [{0}goodscategories] SET [goodscount] = [goodscount] + @goodscount  WHERE  (categoryid = {1}", BaseConfigs.GetTablePrefix, categoryId);
           
            if (!Utils.StrIsNullOrEmpty(parentIdList) && Utils.IsNumericList(parentIdList))
                commandText += "  OR [categoryid] IN (" + parentIdList.Trim() + ")";

            commandText += ")";

            //添加条件判断,以免出现负数情况             
            if (goodsCount < 0)
                commandText += "  AND [goodscount] >= @goodscount ";

            DbParameter[] parms = {
                DbHelper.MakeInParam("@goodscount", (DbType)SqlDbType.Int, 4, goodsCount),
                DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId)
            };
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得指定分类的商品列表
        /// </summary>
        /// <param name="categoryid">指定分类</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式(0:升序 1:降序)</param>
        /// <returns></returns>
        public IDataReader GetGoodsList(int categoryId, int pageSize, int pageIndex, string condition, string orderBy, int ascDesc)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@categoryid",(DbType)SqlDbType.Int,4,categoryId),
									   DbHelper.MakeInParam("@pagesize", (DbType)SqlDbType.Int,4,pageSize),
									   DbHelper.MakeInParam("@pageindex", (DbType)SqlDbType.Int,4,pageIndex),
									   DbHelper.MakeInParam("@condition", (DbType)SqlDbType.VarChar,500, condition),
                                       DbHelper.MakeInParam("@orderby", (DbType)SqlDbType.VarChar,100, orderBy),
                                       DbHelper.MakeInParam("@ascdesc",(DbType)SqlDbType.Int,4,ascDesc)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}getgoodslistbycid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获得指定卖家的商品列表
        /// </summary>
        /// <param name="selleruid">卖家uid</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式(0:升序 1:降序)</param>
        /// <returns></returns>
        public IDataReader GetGoodsListBySellerUID(int sellerUid, int pageSize, int pageIndex, string condition, string orderBy, int ascDesc)
        {
            string sortType = (ascDesc == 0) ?"ASC":"DESC";

            string commandType = "";
  
            if (pageIndex <= 1)
                commandType = string.Format("SELECT TOP {0} * FROM [{1}goods] WHERE [selleruid] = {2} {3}  ORDER BY [{4}] {5} , [goodsid] DESC", 
                                     pageSize, BaseConfigs.GetTablePrefix, sellerUid, condition, orderBy, sortType);
            else
            {
                if (sortType == "DESC")
                    commandType = "SELECT TOP {0} * FROM [{1}goods] WHERE [goodsid] < (SELECT MIN([goodsid])  FROM (SELECT TOP {2} [goodsid] FROM [{1}goods]  WHERE  [selleruid] = {3} {4} ORDER BY [{5}] {6}, [goodsid] DESC) AS tblTmp ) AND [selleruid] = {3} {4} ORDER BY [{5}] {6}, [goodsid] DESC"; 
                else
                    commandType = "SELECT TOP {0} * FROM [{1}goods] WHERE [goodsid] > (SELECT MAX([goodsid])  FROM (SELECT TOP {2} [goodsid] FROM [{1}goods]  WHERE  [selleruid] = {3} {4} ORDER BY {5} {6}) AS tblTmp ) AND [selleruid] = {3} {4} ORDER BY {5} {6}";
                                 
                commandType = string.Format(commandType, pageSize, BaseConfigs.GetTablePrefix, (pageIndex - 1) * pageSize, sellerUid, condition, orderBy, sortType);
            }
            return DbHelper.ExecuteReader(CommandType.Text, commandType);
        }

        /// <summary>
        /// 获取指定条件的商品信息
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns></returns>
        public IDataReader GetGoodsList(int pageSize, int pageIndex, string condition, string orderBy, int ascDesc)
        {
            string sorttype = (ascDesc == 0) ? "ASC" : "DESC";
            string commandType = "";

            condition = condition.Trim().ToUpper().StartsWith("AND") ? condition.Substring(condition.IndexOf("AND") + 3, condition.Length - condition.IndexOf("AND") - 3) : condition;

            if (pageIndex <= 1)
                commandType = string.Format("SELECT TOP {0} * FROM [{1}goods] WHERE {2} ORDER BY [{3}] {4}",
                                             pageSize, BaseConfigs.GetTablePrefix, condition, orderBy, sorttype);
            else
            {
                if (sorttype == "DESC")
                    commandType = string.Format("SELECT TOP {0} * FROM [{1}goods] WHERE [goodsid] < (SELECT MIN([goodsid])  FROM (SELECT TOP {2} [goodsid] FROM [{1}goods]  WHERE  {3} ORDER BY [{4}] {5}) AS tblTmp ) AND {3} ORDER BY [{4}] {5}",
                                                pageSize, BaseConfigs.GetTablePrefix, (pageIndex - 1) * pageSize, condition, orderBy, sorttype) ;
                else
                    commandType = string.Format("SELECT TOP {0} * FROM [{1}goods] WHERE [goodsid] > (SELECT MAX([goodsid])  FROM (SELECT TOP {2} [goodsid] FROM [{1}goods]  WHERE  {3} ORDER BY [{4}] {5}) AS tblTmp ) AND {3} ORDER BY [{4}] {5}",
                                                pageSize, BaseConfigs.GetTablePrefix, (pageIndex - 1) * pageSize, condition, orderBy, sorttype);

                commandType = string.Format(commandType, pageSize, BaseConfigs.GetTablePrefix, (pageIndex - 1) * pageSize, condition, orderBy, sorttype);
            }
            return DbHelper.ExecuteReader(CommandType.Text, commandType);
        }

        /// <summary>
        /// 获取指定条件的商品信息数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetGoodsCount(string condition)
        {
            condition = condition.Trim().ToUpper().StartsWith("AND") ? condition.Substring(condition.IndexOf("AND") + 3, condition.Length - condition.IndexOf("AND") - 3) : condition;
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(goodsid) FROM [{0}goods] WHERE {1}", BaseConfigs.GetTablePrefix, condition)));
        }

        /// <summary>
        /// 获得指定卖家的商品数
        /// </summary>
        /// <param name="selleruid">卖家uid</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetGoodsCountBySellerUid(int sellerUid, string condition)
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(goodsid) FROM [{0}goods] WHERE [selleruid] = {1} {2}", BaseConfigs.GetTablePrefix, sellerUid, condition)));
        }

        /// <summary>
        /// 获取指定店铺商品分类id的商品列表
        /// </summary>
        /// <param name="shopcategoryid">店铺商品分类id</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方法</param>
        /// <returns></returns>
        public IDataReader GetGoodsInfoListByShopCategory(int shopCategoryId, int pageSize, int pageIndex, string condition, string orderBy, int ascDesc)
        {
            string sorttype = (ascDesc == 0) ?"ASC"　:"DESC";
            string commandText = "";
            if (pageIndex <= 1)
                commandText = string.Format("SELECT TOP {0} * FROM [{1}goods] WHERE CHARINDEX(',{2},', RTRIM([shopcategorylist])) > 0 {3} ORDER BY [{4}] {5} , [goodsid] DESC",
                                             pageSize,BaseConfigs.GetTablePrefix, shopCategoryId, condition,orderBy,sorttype);
            else 
            {
                if (sorttype == "DESC")
                    commandText = "SELECT TOP {0} * FROM [{1}goods] WHERE [goodsid] < (SELECT MIN([goodsid])  FROM (SELECT TOP {2} [goodsid] FROM [{1}goods]  WHERE  CHARINDEX(',{3},', RTRIM([shopcategorylist])) > 0 {4} ORDER BY [{5}] {6}, [goodsid] DESC) AS tblTmp ) AND CHARINDEX(',{3},', RTRIM([shopcategorylist])) > 0 {4} ORDER BY [{5}] {6}, [goodsid] DESC";
                                                 
                else
                    commandText = "SELECT TOP {0} * FROM [{1}goods] WHERE [goodsid] > (SELECT MAX([goodsid])  FROM (SELECT TOP {2} [goodsid] FROM [{1}goods]  WHERE  CHARINDEX(',{3},', RTRIM([shopcategorylist])) > 0 {4} ORDER BY [{5}] {6}) AS tblTmp ) AND CHARINDEX(',{3},', RTRIM([shopcategorylist])) > 0 {4} ORDER BY [{5}] {6}";

                commandText = string.Format(commandText, pageSize, BaseConfigs.GetTablePrefix, (pageIndex - 1) * pageSize, shopCategoryId, condition, orderBy, sorttype);
            }

            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获取指定店铺商品分类id的商品数
        /// </summary>
        /// <param name="shopcategoryid">店铺商品分类id</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetGoodsCountByShopCategory(int shopCategoryId, string condition)
        {
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(goodsid) FROM [{0}goods] WHERE CHARINDEX(',{1},', RTRIM([shopcategorylist])) > 0 {2}", BaseConfigs.GetTablePrefix, shopCategoryId, condition)));
        }

        /// <summary>
        /// 根据操作码获取操作符
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        public string GetOperaCode(int opCode)
        {
            switch (opCode)
            {
                case 1: return "=";
                case 2: return "<>";
                case 3: return ">";
                case 4: return ">=";
                case 5: return "<";
                case 6: return "<=";
                default: return "";
            }
        }


        /// <summary>
        /// 获取商品显示字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="displayorder">显示信息</param>
        /// <returns>查询条件</returns>
        public string GetGoodsDisplayCondition(int opCode, int displayOrder)
        {
            string condition = "";
            if (displayOrder > -3 && displayOrder <= 6)
            {
                condition = GetOperaCode(opCode);
                if (!Utils.StrIsNullOrEmpty(condition))
                {
                    condition = string.Format(" AND [displayorder] {0} {1} ", condition, displayOrder);
                }
            }
            return condition;
        }

        /// <summary>
        /// 获取商品关闭字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="closed">关闭信息</param>
        /// <returns>查询条件</returns>
        public string GetGoodsCloseCondition(int opCode, int closed)
        {
            string condition = "";
            if (closed == 0 || closed == 1)
            {
                condition = GetOperaCode(opCode);
                if (!Utils.StrIsNullOrEmpty(condition))
                {
                    condition = string.Format(" AND [closed] {0} {1} ", condition, closed);
                }
            }
            return condition;
        }

        /// <summary>
        /// 获取推荐商品字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="recommend">推荐信息</param>
        /// <returns>查询条件</returns>
        public string GetGoodsRecommendCondition(int opCode, int recommend)
        {
            string condition = "";
            if (recommend == 0 || recommend == 1)
            {
                condition = GetOperaCode(opCode);
                if (!Utils.StrIsNullOrEmpty(condition))
                {
                    condition = string.Format(" AND [recommend] {0} {1} ", condition, recommend);
                }
            }
            return condition;
        }


        /// <summary>
        /// 获取商品Id字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="goodsid">商品id</param>
        /// <returns>查询条件</returns>
        public string GetGoodsIdCondition(int opCode, int goodsId)
        {
            string condition = "";
            if (goodsId > 0)
            {
                condition = GetOperaCode(opCode);
                if (!Utils.StrIsNullOrEmpty(condition))
                {
                    condition = string.Format(" AND [goodsid] {0} {1} ", condition, goodsId);
                }
            }
            return condition;
        }

        /// <summary>
        /// 获取商品到期日期条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="day">天数</param>
        /// <returns>查询条件</returns>
        public string GetGoodsExpirationCondition(int opCode, int day)
        {
            string condition = "";
            condition = GetOperaCode(opCode);
            if (!Utils.StrIsNullOrEmpty(condition))
            {
                condition = string.Format(" AND DATEDIFF(day, [expiration], getdate()) {0} {1} ", condition, day);
            }
            return condition;
        }

   
        /// <summary>
        /// 获取商品类型(全新,二手)字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="quality">数量</param>
        /// <returns>查询条件</returns>
        public string GetGoodsQualityCondition(int opCode, int quality)
        {
            string condition = "";
            if (quality > -3 && quality <= 6)
            {
                condition = GetOperaCode(opCode);
                if (!Utils.StrIsNullOrEmpty(condition))
                {
                    condition = string.Format(" AND [quality] {0} {1} ", condition, quality);
                }
            }
            return condition;
        }

        /// <summary>
        /// 获取商品开始日期条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="day">天数</param>
        /// <returns>查询条件</returns>
        public string GetGoodsDateLineCondition(int opCode, int day)
        {
            string condition = GetOperaCode(opCode);
            if (!Utils.StrIsNullOrEmpty(condition))
                condition = string.Format(" AND DATEDIFF(day, [dateline], getdate()) {0} {1} ", condition, day);

            return condition;
        }

        /// <summary>
        /// 获取剩余商品数条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="amount">数量</param>
        /// <returns>查询条件</returns>
        public string GetGoodsRemainCondition(int opCode, int amount)
        {
            string condition = GetOperaCode(opCode);
            if (!Utils.StrIsNullOrEmpty(condition))
                condition = string.Format(" AND [amount] {0} {1} ", condition, amount);

            return condition;
        }

        /// <summary>
        /// 获得指定分类的商品数
        /// </summary>
        /// <param name="categoryid">指定分类</param>
        /// <param name="condition">条件</param>
        /// <returns>商品数</returns>
        public int GetGoodsCount(int categoryId, string condition)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@categoryid",(DbType)SqlDbType.Int,4,categoryId),
									   DbHelper.MakeInParam("@condition", (DbType)SqlDbType.VarChar,500,condition)									   
								   };
            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}getgoodscountbycid", BaseConfigs.GetTablePrefix), parms));
        }

        /// <summary>
        /// 获得指定商品分类下的子分类
        /// </summary>
        /// <param name="categoryid">指定分类</param>
        /// <returns>子分类数据对象</returns>
        public IDataReader GetSubGoodsCategories(int categoryId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@categoryid",(DbType)SqlDbType.Int,4,categoryId)
								   };
            string commandText = string.Format("SELECT * FROM [{0}goodscategories] WHERE [parentid] = @categoryid", BaseConfigs.GetTablePrefix);
            if (GeneralConfigs.GetConfig().Enablemall == 1) //当开启普通模式时
                commandText += " AND [fid]>0 ";

            commandText += " ORDER BY [displayorder] ";
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获取指定层数的商品分类 
        /// </summary>
        /// <param name="layer">层数</param>
        /// <returns>商品分类信息</returns>
        public IDataReader GetGoodsCategoriesByLayer(int layer)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@layer",(DbType)SqlDbType.Int,4,layer)
								   };
            string commandText = string.Format("SELECT * FROM [{0}goodscategories] WHERE [layer] <= @layer ORDER BY [displayorder]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 判断商品列表是否都在当前分类下
        /// </summary>
        /// <param name="goodsidlist">商品Id列表，以英文逗号分割</param>
        /// <param name="categoryid">指定分类</param>
        /// <returns></returns>
        public bool InSameCategory(string goodsIdList, int categoryId)
        {
            return Utils.SplitString(goodsIdList, ",").Length == GetGoodsCount(categoryId, " AND [goodsid] IN (" + goodsIdList + ")");
        }


        /// <summary>
        /// 将商品设置关闭/打开
        /// </summary>
        /// <param name="goodsidlist">商品Id列表,以英文逗号分割</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新商品个数</returns>
        public int SetGoodsClose(string goodsList, short intValue)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@field", (DbType)SqlDbType.TinyInt, 1, intValue)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goods] SET [closed] = @field WHERE [goodsid] IN ({1}) AND [closed] IN (0,1)", BaseConfigs.GetTablePrefix, goodsList), parms);
        }

        /// <summary>
        /// 删除指定的商品
        /// </summary>
        /// <param name="goodsidlist">商品Id列表(以","分割)</param>
        /// <returns></returns>
        public int DeleteGoods(string goodsList)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE [{0}goods] WHERE [goodsid] IN ({1})", BaseConfigs.GetTablePrefix, goodsList));
        }

        /// <summary>
        /// 删除指定的商品附件
        /// </summary>
        /// <param name="goodsidlist">要删除的商品Id列表(以","分割)</param>
        /// <returns></returns>
        public int DeleteGoodsAttachments(string goodsList)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE [{0}goodsattachments] WHERE [goodsid] IN ({1})", BaseConfigs.GetTablePrefix, goodsList));
        }

        /// <summary>
        /// 设置商品指定字段的属性值
        /// </summary>
        /// <param name="goodsidlist">要设置的商品Id列表(以","分割)</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        public int SetGoodsStatus(string goodsList, string field, int intValue)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@field", (DbType)SqlDbType.Int, 1, intValue)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goods] SET [{1}] = @field WHERE [goodsid] IN ({2})", BaseConfigs.GetTablePrefix, field, goodsList), parms);
        }

        /// <summary>
        /// 设置商品指定字段的属性值
        /// </summary>
        /// <param name="goodsidlist">要设置的商品Id列表(以","分割)</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        public int SetGoodsStatus(string goodsList, string field, string intValue)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@field", (DbType)SqlDbType.VarChar, 500, intValue)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goods] SET [{1}] = @field WHERE [goodsid] IN ({2})", BaseConfigs.GetTablePrefix, field, goodsList), parms);
        }

        /// <summary>
        /// 获得指定商品的所有附件
        /// </summary>
        /// <param name="goodsidlist">商品Id列表(以","分割)</param>
        /// <returns></returns>
        public IDataReader GetGoodsAttachmentList(string goodsIdList)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT [aid],[filename] FROM [{0}goodsattachments] WHERE [goodsid] IN ({1})", BaseConfigs.GetTablePrefix, goodsIdList));
        }

        /// <summary>
        /// 获得指定商品ID的商品列表
        /// </summary>
        /// <param name="goodsidlist">商品Id列表(以","分割)</param>
        /// <returns></returns>
        public DataTable GetGoodsList(string goodsIdList)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}goods] WHERE [goodsid] IN ({1})", BaseConfigs.GetTablePrefix, goodsIdList)).Tables[0];
        }


        /// <summary>
        /// 创建商品交易信息
        /// </summary>
        /// <param name="goodstradelog">要创建的交易信息</param>
        /// <returns></returns>
        public int CreateGoodsTradeLog(Goodstradeloginfo goodsTradeLog)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4,goodsTradeLog.Goodsid),
						DbHelper.MakeInParam("@orderid", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Orderid),
						DbHelper.MakeInParam("@tradeno", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Tradeno),
						DbHelper.MakeInParam("@subject", (DbType)SqlDbType.NChar, 60,goodsTradeLog.Subject),
						DbHelper.MakeInParam("@price", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Price),
						DbHelper.MakeInParam("@quality", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Quality),
						DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,goodsTradeLog.Categoryid),
						DbHelper.MakeInParam("@number", (DbType)SqlDbType.SmallInt, 2,goodsTradeLog.Number),
						DbHelper.MakeInParam("@tax", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Tax),
						DbHelper.MakeInParam("@locus", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Locus),
						DbHelper.MakeInParam("@sellerid", (DbType)SqlDbType.Int, 4,goodsTradeLog.Sellerid),
						DbHelper.MakeInParam("@seller", (DbType)SqlDbType.NChar, 20,goodsTradeLog.Seller),
						DbHelper.MakeInParam("@selleraccount", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Selleraccount),
						DbHelper.MakeInParam("@buyerid", (DbType)SqlDbType.Int, 4,goodsTradeLog.Buyerid),
						DbHelper.MakeInParam("@buyer", (DbType)SqlDbType.NChar, 20,goodsTradeLog.Buyer),
						DbHelper.MakeInParam("@buyercontact", (DbType)SqlDbType.NChar, 100,goodsTradeLog.Buyercontact),
						DbHelper.MakeInParam("@buyercredit", (DbType)SqlDbType.SmallInt, 2,goodsTradeLog.Buyercredit),
						DbHelper.MakeInParam("@buyermsg", (DbType)SqlDbType.NChar, 100,goodsTradeLog.Buyermsg),
						DbHelper.MakeInParam("@status", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Status),
						DbHelper.MakeInParam("@lastupdate", (DbType)SqlDbType.DateTime, 8,goodsTradeLog.Lastupdate),
						DbHelper.MakeInParam("@offline", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Offline),
						DbHelper.MakeInParam("@buyername", (DbType)SqlDbType.NChar, 20,goodsTradeLog.Buyername),
						DbHelper.MakeInParam("@buyerzip", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Buyerzip),
						DbHelper.MakeInParam("@buyerphone", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Buyerphone),
						DbHelper.MakeInParam("@buyermobile", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Buyermobile),
						DbHelper.MakeInParam("@transport", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Transport),
                        DbHelper.MakeInParam("@transportpay", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Transportpay),
						DbHelper.MakeInParam("@transportfee", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Transportfee),
                        DbHelper.MakeInParam("@tradesum", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Tradesum),
						DbHelper.MakeInParam("@baseprice", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Baseprice),
						DbHelper.MakeInParam("@discount", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Discount),
						DbHelper.MakeInParam("@ratestatus", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Ratestatus),
						DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0,goodsTradeLog.Message)
				};
            string commandText = String.Format("INSERT INTO [{0}goodstradelogs] ([goodsid], [orderid], [tradeno], [subject], [price], [quality], [categoryid], [number], [tax], [locus], [sellerid], [seller], [selleraccount], [buyerid], [buyer], [buyercontact], [buyercredit], [buyermsg], [status], [lastupdate], [offline], [buyername], [buyerzip], [buyerphone], [buyermobile], [transport], [transportpay], [transportfee], [tradesum], [baseprice], [discount], [ratestatus], [message]) VALUES (@goodsid, @orderid, @tradeno, @subject, @price, @quality, @categoryid, @number, @tax, @locus, @sellerid, @seller, @selleraccount, @buyerid, @buyer, @buyercontact, @buyercredit, @buyermsg, @status, @lastupdate, @offline, @buyername, @buyerzip, @buyerphone, @buyermobile, @transport, @transportpay, @transportfee, @tradesum, @baseprice, @discount, @ratestatus, @message);SELECT SCOPE_IDENTITY()  AS 'id'", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }

        /// <summary>
        /// 更新商品交易信息
        /// </summary>
        /// <param name="goodstradelog">要更新的交易信息</param>
        /// <returns></returns>
        public bool UpdateGoodsTradeLog(Goodstradeloginfo goodsTradeLog)
        {

            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4,goodsTradeLog.Goodsid),
						DbHelper.MakeInParam("@orderid", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Orderid),
						DbHelper.MakeInParam("@tradeno", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Tradeno),
						DbHelper.MakeInParam("@subject", (DbType)SqlDbType.NChar, 60,goodsTradeLog.Subject),
						DbHelper.MakeInParam("@price", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Price),
						DbHelper.MakeInParam("@quality", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Quality),
						DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,goodsTradeLog.Categoryid),
						DbHelper.MakeInParam("@number", (DbType)SqlDbType.SmallInt, 2,goodsTradeLog.Number),
						DbHelper.MakeInParam("@tax", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Tax),
						DbHelper.MakeInParam("@locus", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Locus),
						DbHelper.MakeInParam("@sellerid", (DbType)SqlDbType.Int, 4,goodsTradeLog.Sellerid),
						DbHelper.MakeInParam("@seller", (DbType)SqlDbType.NChar, 20,goodsTradeLog.Seller),
						DbHelper.MakeInParam("@selleraccount", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Selleraccount),
						DbHelper.MakeInParam("@buyerid", (DbType)SqlDbType.Int, 4,goodsTradeLog.Buyerid),
						DbHelper.MakeInParam("@buyer", (DbType)SqlDbType.NChar, 20,goodsTradeLog.Buyer),
						DbHelper.MakeInParam("@buyercontact", (DbType)SqlDbType.NChar, 100,goodsTradeLog.Buyercontact),
						DbHelper.MakeInParam("@buyercredit", (DbType)SqlDbType.SmallInt, 2,goodsTradeLog.Buyercredit),
						DbHelper.MakeInParam("@buyermsg", (DbType)SqlDbType.NChar, 100,goodsTradeLog.Buyermsg),
						DbHelper.MakeInParam("@status", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Status),
						DbHelper.MakeInParam("@lastupdate", (DbType)SqlDbType.DateTime, 8,goodsTradeLog.Lastupdate),
						DbHelper.MakeInParam("@offline", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Offline),
						DbHelper.MakeInParam("@buyername", (DbType)SqlDbType.NChar, 20,goodsTradeLog.Buyername),
						DbHelper.MakeInParam("@buyerzip", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Buyerzip),
						DbHelper.MakeInParam("@buyerphone", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Buyerphone),
						DbHelper.MakeInParam("@buyermobile", (DbType)SqlDbType.VarChar, 50,goodsTradeLog.Buyermobile),
						DbHelper.MakeInParam("@transport", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Transport),
                        DbHelper.MakeInParam("@transportpay", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Transportpay),
						DbHelper.MakeInParam("@transportfee", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Transportfee),
                        DbHelper.MakeInParam("@tradesum", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Tradesum),
						DbHelper.MakeInParam("@baseprice", (DbType)SqlDbType.Decimal, 18,goodsTradeLog.Baseprice),
						DbHelper.MakeInParam("@discount", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Discount),
						DbHelper.MakeInParam("@ratestatus", (DbType)SqlDbType.TinyInt, 1,goodsTradeLog.Ratestatus),
						DbHelper.MakeInParam("@message", (DbType)SqlDbType.NText, 0,goodsTradeLog.Message),
                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4,goodsTradeLog.Id)
				};
            string commandText = String.Format("Update [{0}goodstradelogs]  Set [goodsid] = @goodsid, [orderid] = @orderid, [tradeno] = @tradeno, [subject] = @subject, [price] = @price, [quality] = @quality, [categoryid] = @categoryid, [number] = @number, [tax] = @tax, [locus] = @locus, [sellerid] = @sellerid, [seller] = @seller, [selleraccount] = @selleraccount, [buyerid] = @buyerid, [buyer] = @buyer, [buyercontact] = @buyercontact, [buyercredit] = @buyercredit, [buyermsg] = @buyermsg, [status] = @status, [lastupdate] = @lastupdate, [offline] = @offline, [buyername] = @buyername, [buyerzip] = @buyerzip, [buyerphone] = @buyerphone, [buyermobile] = @buyermobile, [transport] = @transport, [transportpay] = @transportpay, [transportfee] = @transportfee, [tradesum] = @tradesum, [baseprice] = @baseprice, [discount] = @discount, [ratestatus] = @ratestatus, [message] = @message WHERE [id] = @id", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 设置商品交易状态条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="status">交易状态</param>
        /// <returns>查询条件</returns>
        public string SetGoodsTradeStatusCond(int opCode, int status)
        {
            string condition = GetOperaCode(opCode);
            if (!Utils.StrIsNullOrEmpty(condition))
                condition = string.Format("  AND [status] {0} {1} ", condition, status);

            return condition;
        }

        /// <summary>
        /// 获取指定商品的交易日志
        /// </summary>
        /// <param name="goodsid">商品Id</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式(0:升序 1:降序)</param>
        /// <returns></returns>
        public IDataReader GetGoodsTradeLogByGid(int goodsId, int pageSize, int pageIndex, string condition, string orderBy, int ascDesc)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@goodsid",(DbType)SqlDbType.Int,4,goodsId)
								  };

            string commandText = "";
            if (pageIndex <= 1)
                commandText = "SELECT TOP {0} * FROM [{1}goodstradelogs] WHERE [goodsid] = @goodsid  {2} ORDER BY  [id] {4}, {3} {4}";
            else
            {
                if (ascDesc == 1)
                    commandText = "SELECT TOP {0} * FROM [{1}goodstradelogs] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + ((pageIndex - 1) * pageSize) + " [id] FROM [{1}goodstradelogs]  WHERE  [goodsid] = @goodsid  {2}  ORDER BY [id] {4},{3} {4}) AS tblTmp ) AND [goodsid] = @goodsid  {2}  ORDER BY [id] {4},{3} {4}";
                else
                    commandText = "SELECT TOP {0} * FROM [{1}goodstradelogs] WHERE [id] > (SELECT MAX([id])  FROM (SELECT TOP " + ((pageIndex - 1) * pageSize) + " [id] FROM [{1}goodstradelogs]  WHERE  [goodsid] = @goodsid  {2}  ORDER BY [id] {4},{3} {4}) AS tblTmp ) AND [goodsid] = @goodsid  {2}  ORDER BY [id] {4},{3} {4}";
            }
            return DbHelper.ExecuteReader(CommandType.Text, string.Format(commandText, pageSize, BaseConfigs.GetTablePrefix, condition, orderBy, ascDesc == 0 ? "ASC" : "DESC"), parms);
        }

        /// <summary>
        /// 获取指定用户(或商品id，过滤filter)的交易信息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="goodsidlist">商品id串(格式:1,2,3)</param>
        /// <param name="uidtype">用户类型(1卖家, 2买家)</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <returns></returns>
        public DataTable GetGoodsTradeLogList(int userId, string goodsIdList, int uidType, string fileter, int pageSize, int pageIndex)
        {
            string commandText = "";
            string condition = " [buyerid] = " + userId;
            if (uidType == 1) //卖家
                condition = " [sellerid] = " + userId;

            if (!Utils.StrIsNullOrEmpty(goodsIdList) && Utils.IsNumericArray(goodsIdList.Split(',')))
                condition += string.Format(" AND [{0}goodstradelogs].[goodsid] IN ({1})", BaseConfigs.GetTablePrefix, goodsIdList);

            condition += GetTradeStatus(fileter);
            condition += string.Format(" ORDER BY [{0}goodstradelogs].[id] DESC", BaseConfigs.GetTablePrefix);

            if (pageIndex <= 1)
                commandText = "SELECT TOP {0} * FROM [{1}goodstradelogs] LEFT JOIN [{1}goods] ON [{1}goodstradelogs].[goodsid] = [{1}goods].[goodsid] WHERE {2}";
            else
                commandText = "SELECT TOP {0} * FROM [{1}goodstradelogs] LEFT JOIN [{1}goods] ON [{1}goodstradelogs].[goodsid] = [{1}goods].[goodsid] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + ((pageIndex - 1) * pageSize) + " [id] FROM [{1}goodstradelogs]  WHERE  {2}) AS tblTmp ) AND {2}";

            return DbHelper.ExecuteDataset(CommandType.Text, string.Format(commandText, pageSize, BaseConfigs.GetTablePrefix, condition)).Tables[0];
        }

        /// <summary>
        /// 将过滤条件转换成为查询条件
        /// </summary>
        /// <param name="filter">过滤参数</param>
        /// <returns>查询过滤条件</returns>
        public string GetTradeStatus(string filter)
        {
            switch (filter)
            {
                case "attention": return " AND [status] IN (1,2,3,5,6,10,11,12,13) "; //关注的交易
                case "eccredit": return " AND [status] IN (7,17) "; //评价的交易
                case "success": return " AND [status] = 7 "; //成功的交易
                case "refund": return " AND [status] IN (10,16,17,18) "; //退款的交易
                case "closed": return " AND [status] IN (8,17,18) "; //失败的交易
                case "unstart": return " AND [status] = 0 "; //未生效的交易
                case "all": return ""; // 全部交易
                default: return " AND [status] IN (1,2,3,4,5,6,10,11,12,13) "; //进行中的交易
            }
        }

        /// <summary>
        /// 获取指定用户的商品交易统计数据
        /// </summary>
        /// <returns></returns>
        public IDataReader GetTradeStatistic(int userId)
        {
            string commandText = string.Format("SELECT (SELECT COUNT(id) FROM [{0}goodstradelogs] WHERE [sellerid] = {1} AND [status] IN (1,2,3,5,6,10,11,12,13)) AS SellerAttention," + //卖家关注交易数
                                              "(SELECT COUNT(id) FROM [{0}goodstradelogs] WHERE [sellerid] = {1} AND [status] IN (1,2,3,4,5,6,10,11,12,13)) AS SellerTrading," + //卖家交易进行中的交易数
                                              "(SELECT COUNT(id) FROM [{0}goodstradelogs] WHERE [sellerid] = {1} AND [ratestatus] IN (0,2) AND [status] IN (7,17)) AS SellerRate," + //需卖家评价的交易数
                                              "ISNULL((SELECT SUM(number) FROM [{0}goodstradelogs] WHERE [sellerid] = {1} AND [status]=7),0)  AS SellNumberSum," + //卖家售出商品总数
                                              "ISNULL((SELECT SUM(tradesum) FROM [{0}goodstradelogs] WHERE [sellerid] = {1} AND [status]=7),0)  AS SellTradeSum," + //卖家销售成交总额

                                              "(SELECT COUNT(id) FROM [{0}goodstradelogs] WHERE [buyerid] = {1} AND [status] IN (1,2,3,5,6,10,11,12,13)) AS BuyERAttention," + //买家关注交易数
                                              "(SELECT COUNT(id) FROM [{0}goodstradelogs] WHERE [buyerid] = {1} AND [status] IN (1,2,3,4,5,6,10,11,12,13)) AS BuyerTrading," + //买家交易进行中的交易数
                                              "(SELECT COUNT(id) FROM [{0}goodstradelogs] WHERE [buyerid] = {1} AND [ratestatus] IN (0,2) AND [status] IN (7,17)) AS BuyerRate," +  //需买家评价的交易数
                                              "ISNULL((SELECT SUM(number) FROM [{0}goodstradelogs] WHERE [buyerid] = {1} AND [status]=7),0)  AS BuyNumberSum," + //买入商品总数
                                              "ISNULL((SELECT SUM(tradesum) FROM [{0}goodstradelogs] WHERE [buyerid] = {1} AND [status]=7),0)  AS BuyTradeSum",  //买入成交总额
                                              BaseConfigs.GetTablePrefix,
                                              userId);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获取指定用户(或商品id列表或过滤filter)的交易信息数
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="goodsidlist">商品id串(格式:1,2,3)</param>
        /// <param name="uidtype">用户类型(1卖家, 2买家)</param>
        /// <param name="fileter">过滤条件</param>
        /// <returns></returns>
        public int GetGoodsTradeLogCount(int userId, string goodsIdList, int uidType, string fileter)
        {
            string condition = " [buyerid] = " + userId;
            if (uidType == 1) //卖家
                condition = " [sellerid] = " + userId;

            if (!Utils.StrIsNullOrEmpty(goodsIdList) && Utils.IsNumericList(goodsIdList))
                condition += " AND [goodsid] IN (" + goodsIdList + ")";

            if (!Utils.StrIsNullOrEmpty(fileter))
                condition += GetTradeStatus(fileter);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT count(id) FROM [{0}goodstradelogs] WHERE {1}", BaseConfigs.GetTablePrefix, condition)));
        }

        /// <summary>
        /// 获取指定商品的交易日志数
        /// </summary>
        /// <param name="goodsid">商品Id</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int GetTradeLogCountByGid(int goodsId, string condition)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@goodsid",(DbType)SqlDbType.Int,4,goodsId),
								  };

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT count(id) FROM [{0}goodstradelogs] WHERE [goodsid] = @goodsid {1}", BaseConfigs.GetTablePrefix, condition), parms));
        }

        /// <summary>
        /// 获得指定交易日志ID的商品交易日志
        /// </summary>
        /// <param name="goodstradelogid">交易日志ID</param>
        /// <returns></returns>
        public IDataReader GetGoodsTradeLogByID(int goodsTradeLogId)
        {
            DbParameter parm = DbHelper.MakeInParam("@goodstradelogid", (DbType)SqlDbType.Int, 4, goodsTradeLogId);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}goodstradelogs] WHERE [id] = @goodstradelogid", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 获得指定交易号的商品交易日志
        /// </summary>
        /// <param name="tradeno"></param>
        /// <returns></returns>
        public IDataReader GetGoodsTradeLogByTradeNo(string tradeNo)
        {
            DbParameter parm = DbHelper.MakeInParam("@tradeno", (DbType)SqlDbType.VarChar, 50, tradeNo);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}goodstradelogs] WHERE [tradeno] = @tradeno", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 创建留言
        /// </summary>
        /// <param name="goodsleaveword">要创建的留言信息</param>
        /// <returns></returns>
        public int CreateGoodsLeaveWord(Goodsleavewordinfo goodsLeaveWord)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Goodsid),
						DbHelper.MakeInParam("@tradelogid", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Tradelogid),
						DbHelper.MakeInParam("@isbuyer", (DbType)SqlDbType.TinyInt, 1,goodsLeaveWord.Isbuyer),
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Uid),
						DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20,goodsLeaveWord.Username),
						DbHelper.MakeInParam("@message", (DbType)SqlDbType.NChar, 200,goodsLeaveWord.Message),
						DbHelper.MakeInParam("@invisible", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Invisible),
						DbHelper.MakeInParam("@ip", (DbType)SqlDbType.NVarChar, 15,goodsLeaveWord.Ip),
						DbHelper.MakeInParam("@usesig", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Usesig),
						DbHelper.MakeInParam("@htmlon", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Htmlon),
						DbHelper.MakeInParam("@smileyoff", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Smileyoff),
						DbHelper.MakeInParam("@parseurloff", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Parseurloff),
						DbHelper.MakeInParam("@bbcodeoff", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Bbcodeoff),
						DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,goodsLeaveWord.Postdatetime)
				};
            string commandText = String.Format("INSERT INTO [{0}goodsleavewords] ([goodsid], [tradelogid], [isbuyer], [uid], [username], [message], [invisible], [ip], [usesig], [htmlon], [smileyoff], [parseurloff], [bbcodeoff], [postdatetime]) VALUES (@goodsid, @tradelogid, @isbuyer, @uid, @username, @message, @invisible, @ip, @usesig, @htmlon, @smileyoff, @parseurloff, @bbcodeoff, @postdatetime);SELECT SCOPE_IDENTITY()  AS 'id'", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }

        /// <summary>
        /// 更新留言
        /// </summary>
        /// <param name="goodsleaveword">要更新的留言信息</param>
        /// <returns></returns>
        public bool UpdateGoodsLeaveWord(Goodsleavewordinfo goodsLeaveWord)
        {

            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Goodsid),
						DbHelper.MakeInParam("@tradelogid", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Tradelogid),
						DbHelper.MakeInParam("@isbuyer", (DbType)SqlDbType.TinyInt, 1,goodsLeaveWord.Isbuyer),
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Uid),
						DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20,goodsLeaveWord.Username),
						DbHelper.MakeInParam("@message", (DbType)SqlDbType.NChar, 200,goodsLeaveWord.Message),
						DbHelper.MakeInParam("@invisible", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Invisible),
						DbHelper.MakeInParam("@ip", (DbType)SqlDbType.NVarChar, 15,goodsLeaveWord.Ip),
						DbHelper.MakeInParam("@usesig", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Usesig),
						DbHelper.MakeInParam("@htmlon", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Htmlon),
						DbHelper.MakeInParam("@smileyoff", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Smileyoff),
						DbHelper.MakeInParam("@parseurloff", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Parseurloff),
						DbHelper.MakeInParam("@bbcodeoff", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Bbcodeoff),
						DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,goodsLeaveWord.Postdatetime),
                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4,goodsLeaveWord.Id)
				};
            string commandText = String.Format("Update [{0}goodsleavewords]  Set [goodsid] = @goodsid, [tradelogid] = @tradelogid, [isbuyer] = @isbuyer, [uid] = @uid, [username] = @username, [message] = @message, [invisible] = @invisible, [ip] = @ip, [usesig] = @usesig, [htmlon] = @htmlon, [smileyoff] = @smileyoff, [parseurloff] = @parseurloff, [bbcodeoff] = @bbcodeoff, [postdatetime] = @postdatetime WHERE [id] = @id", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 通过交易日志id获得留言列表
        /// </summary>
        /// <param name="goodstradelogid">交易日志id</param>
        /// <returns></returns>
        public IDataReader GetGoodsLeaveWordListByTradeLogId(int goodsTradeLogId)
        {
            DbParameter parm = DbHelper.MakeInParam("@tradelogid", (DbType)SqlDbType.Int, 4, goodsTradeLogId);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}goodsleavewords] WHERE [tradelogid] = @tradelogid AND [invisible] = 0 ORDER BY [postdatetime] ASC", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 获取指定商品的留言
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <returns></returns>
        public int GetGoodsLeaveWordCountByGid(int goodsId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@goodsid",(DbType)SqlDbType.Int,4,goodsId),
								  };

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT count(id) FROM [{0}goodsleavewords] WHERE [goodsid] = @goodsid AND [tradelogid]=0 AND [invisible] = 0", BaseConfigs.GetTablePrefix), parms));
        }

        /// <summary>
        /// 获取指定商品分页的留言
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="pageindex">当前分页</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方向(0:asc 1:desc)</param>
        /// <returns></returns>
        public IDataReader GetGoodsLeaveWordByGid(int goodsId, int pageSize, int pageIndex, string orderBy, int ascDesc)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@goodsid",(DbType)SqlDbType.Int,4,goodsId)
    							  };

            string commandText = "";
            if (pageIndex <= 1)
                commandText = "SELECT TOP {0} * FROM [{1}goodsleavewords] WHERE [goodsid] = @goodsid AND [tradelogid]=0 AND [invisible] = 0 ORDER BY  {2} {3}";
            else
            {
                if (ascDesc == 1)
                    commandText = "SELECT TOP {0} * FROM [{1}goodsleavewords] WHERE [id] < (SELECT MIN([id])  FROM (SELECT TOP " + ((pageIndex - 1) * pageSize) + " [id] FROM [{1}goodsleavewords]  WHERE  [goodsid] = @goodsid AND [tradelogid]=0 AND [invisible] = 0  ORDER BY  {2} {3}) AS tblTmp ) AND [goodsid] = @goodsid AND [tradelogid]=0 AND [invisible] = 0 ORDER BY  {2} {3}";
                else
                    commandText = "SELECT TOP {0} * FROM [{1}goodsleavewords] WHERE [id] > (SELECT MAX([id])  FROM (SELECT TOP " + ((pageIndex - 1) * pageSize) + " [id] FROM [{1}goodsleavewords]  WHERE  [goodsid] = @goodsid AND [tradelogid]=0 AND [invisible] = 0  ORDER BY  {2} {3}) AS tblTmp ) AND [goodsid] = @goodsid AND [tradelogid]=0 AND [invisible] = 0 ORDER BY  {2} {3}";
            }
            return DbHelper.ExecuteReader(CommandType.Text, string.Format(commandText, pageSize, BaseConfigs.GetTablePrefix, orderBy, ascDesc == 0 ? "ASC" : "DESC"), parms);
        }

        /// <summary>
        /// 获取指定ID的留言信息
        /// </summary>
        /// <param name="id">留言id</param>
        /// <returns></returns>
        public IDataReader GetGoodsLeaveWordById(int id)
        {
            DbParameter parm = DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}goodsleavewords] WHERE [id] = @id", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 删除指定ID的留言信息
        /// </summary>
        /// <param name="id">留言id</param>
        /// <returns></returns>
        public bool DeleteGoodsLeaveWordById(int id)
        {
            DbParameter parm = DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id);

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}goodsleavewords] WHERE [id] = @id", BaseConfigs.GetTablePrefix), parm);

            return true;
        }
      
        /// <summary>
        /// 初始化用户评价信息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public int InitGoodsUserCredit(int userId)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,userId),
				};
            StringBuilder sb_sql = new StringBuilder();
            sb_sql.Append("INSERT INTO [{0}goodsusercredits] ([uid],[ratefrom],[ratetype]) VALUES (@uid, 2, 1);");
            sb_sql.Append("INSERT INTO [{0}goodsusercredits] ([uid],[ratefrom],[ratetype]) VALUES (@uid, 2, 2);");
            sb_sql.Append("INSERT INTO [{0}goodsusercredits] ([uid],[ratefrom],[ratetype]) VALUES (@uid, 2, 3);");
            sb_sql.Append("INSERT INTO [{0}goodsusercredits] ([uid],[ratefrom],[ratetype]) VALUES (@uid, 1, 1);");
            sb_sql.Append("INSERT INTO [{0}goodsusercredits] ([uid],[ratefrom],[ratetype]) VALUES (@uid, 1, 2);");
            sb_sql.Append("INSERT INTO [{0}goodsusercredits] ([uid],[ratefrom],[ratetype]) VALUES (@uid, 1, 3);SELECT SCOPE_IDENTITY()  AS 'id';");

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, string.Format(sb_sql.ToString(), BaseConfigs.GetTablePrefix), parms).Tables[0].Rows[0][0], -1);
        }

        /// <summary>
        /// 更新商品用户信用记录
        /// </summary>
        /// <param name="goodsusercredits">要更新的用户信用信息</param>
        /// <returns></returns>
        public bool UpdateGoodsUserCredit(Goodsusercreditinfo goodsUserCredits)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,goodsUserCredits.Uid),
						DbHelper.MakeInParam("@oneweek", (DbType)SqlDbType.Int, 4,goodsUserCredits.Oneweek),
						DbHelper.MakeInParam("@onemonth", (DbType)SqlDbType.Int, 4,goodsUserCredits.Onemonth),
						DbHelper.MakeInParam("@sixmonth", (DbType)SqlDbType.Int, 4,goodsUserCredits.Sixmonth),
						DbHelper.MakeInParam("@sixmonthago", (DbType)SqlDbType.Int, 4,goodsUserCredits.Sixmonthago),
						DbHelper.MakeInParam("@ratefrom", (DbType)SqlDbType.TinyInt, 1,goodsUserCredits.Ratefrom),
						DbHelper.MakeInParam("@ratetype", (DbType)SqlDbType.TinyInt, 1,goodsUserCredits.Ratetype),
                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4,goodsUserCredits.Id)
				};
            string commandText = String.Format("Update [{0}goodsusercredits]  Set [uid] = @uid, [oneweek] = @oneweek, [onemonth] = @onemonth, [sixmonth] = @sixmonth, [sixmonthago] = @sixmonthago, [ratefrom] = @ratefrom, [ratetype] = @ratetype WHERE [id] = @id", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 获取指定用户id的评价信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public IDataReader GetGoodsUserCreditByUid(int uid)
        {
            DbParameter parm = DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}goodsusercredits] WHERE [uid] = @uid ORDER BY [id] ASC", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 发表商品评价
        /// </summary>
        /// <param name="goodsrates">要创建的商品评价信息</param>
        /// <returns></returns>
        public int CreateGoodsRate(Goodsrateinfo goodsRates)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@goodstradelogid", (DbType)SqlDbType.Int, 4,goodsRates.Goodstradelogid),
						DbHelper.MakeInParam("@message", (DbType)SqlDbType.NChar, 200,goodsRates.Message),
						DbHelper.MakeInParam("@explain", (DbType)SqlDbType.NChar, 200,goodsRates.Explain),
						DbHelper.MakeInParam("@ip", (DbType)SqlDbType.NVarChar, 15,goodsRates.Ip),
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,goodsRates.Uid),
						DbHelper.MakeInParam("@uidtype", (DbType)SqlDbType.TinyInt, 1,goodsRates.Uidtype),
                        DbHelper.MakeInParam("@ratetouid", (DbType)SqlDbType.Int, 4,goodsRates.Ratetouid),
                        DbHelper.MakeInParam("@ratetousername", (DbType)SqlDbType.NChar, 20,goodsRates.Ratetousername),
						DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20,goodsRates.Username),
						DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,goodsRates.Postdatetime),
						DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4,goodsRates.Goodsid),
						DbHelper.MakeInParam("@goodstitle", (DbType)SqlDbType.NChar, 60,goodsRates.Goodstitle),
						DbHelper.MakeInParam("@price", (DbType)SqlDbType.Decimal, 18,goodsRates.Price),
						DbHelper.MakeInParam("@ratetype", (DbType)SqlDbType.TinyInt, 1,goodsRates.Ratetype)
				};
            string commandText = String.Format("INSERT INTO [{0}goodsrates] ([goodstradelogid], [message], [explain], [ip], [uid], [uidtype], [ratetouid], [ratetousername], [username], [postdatetime], [goodsid], [goodstitle], [price], [ratetype]) VALUES (@goodstradelogid, @message, @explain, @ip, @uid, @uidtype, @ratetouid, @ratetousername, @username, @postdatetime, @goodsid, @goodstitle, @price, @ratetype);SELECT SCOPE_IDENTITY()  AS 'id'", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }
      
        /// <summary>
        /// 通过交易日志id获取评价记录
        /// </summary>
        /// <param name="goodstradelogid">交易日志id</param>
        /// <returns></returns>
        public IDataReader GetGoodsRateByTradeLogID(int goodsTradeLogId)
        {
            DbParameter parm = DbHelper.MakeInParam("@goodstradelogid", (DbType)SqlDbType.Int, 4, goodsTradeLogId);

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}goodsrates] WHERE [goodstradelogid] = @goodstradelogid", BaseConfigs.GetTablePrefix), parm);
        }

        /// <summary>
        /// 获取指定条件的评价数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="uidtype">用户类型</param>
        /// <param name="ratetype">评价类型</param>
        /// <returns></returns>
        public IDataReader GetGoodsRateCount(int uid, int uidType, int rateType)
        {
            string sql = string.Format("SELECT  (SELECT COUNT([id])  FROM  [{0}goodsrates]  WHERE  [ratetouid]={1} AND [uidtype]={2} AND [ratetype]={3} AND DATEDIFF(day, GETDATE(), [postdatetime]) < 7) AS oneweek," +
                                               "(SELECT COUNT([id])  FROM  [{0}goodsrates]  WHERE  [ratetouid]={1} AND [uidtype]={2} AND [ratetype]={3} AND DATEDIFF(month, GETDATE(), [postdatetime]) < 1) AS onemonth," +
                                               "(SELECT COUNT([id])  FROM  [{0}goodsrates]  WHERE  [ratetouid]={1} AND [uidtype]={2} AND [ratetype]={3} AND DATEDIFF(month, GETDATE(), postdatetime) < 6) AS sixmonth," +
                                               "(SELECT COUNT([id])  FROM  [{0}goodsrates]  WHERE  [ratetouid]={1} AND [uidtype]={2} AND [ratetype]={3} AND DATEDIFF(month, GETDATE(), postdatetime) > 6) AS sixmonthago",
                                               BaseConfigs.GetTablePrefix,
                                               uid,
                                               uidType,
                                               rateType);
            return DbHelper.ExecuteReader(CommandType.Text, sql);
        }

        /// <summary>
        /// 获取指定用户的评价记录
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="uidtype">用户类型</param>
        /// <param name="ratetype">评价类型</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public IDataReader GetGoodsRates(int uid, int uidType, int rateType, string filter)
        {
            string commandText = "";
            DbParameter[] parms = 
                {
						DbHelper.MakeInParam("@userid", (DbType)SqlDbType.Int, 4,uid)
                };

            switch (uidType)
            {
                case 0: commandText = string.Format("SELECT * FROM [{0}goodsrates] WHERE [ratetouid] = @userid ", BaseConfigs.GetTablePrefix); break; //收到的所有评价
                case 3: commandText = string.Format("SELECT * FROM [{0}goodsrates] WHERE [uid] = @userid ", BaseConfigs.GetTablePrefix); break; //给他人的评价
                default: commandText = string.Format("SELECT * FROM [{0}goodsrates] WHERE [ratetouid] = @userid AND [uidtype] = " + uidType, BaseConfigs.GetTablePrefix); break; //收到卖家(1)或买家(2) 的评价
            }

            if (rateType > 0 && rateType <= 3)
                commandText += " AND [ratetype] = " + rateType;

            switch (filter.ToLower().Trim())
            {
                case "oneweek": commandText += " AND DATEDIFF(day, GETDATE(), [postdatetime]) < 7 "; break; //一周内
                case "onemonth": commandText += " AND DATEDIFF(month, GETDATE(), [postdatetime]) < 1 "; break; //一月内
                case "sixmonth": commandText += " AND DATEDIFF(month, GETDATE(), [postdatetime]) < 6 "; break; //半年内
                case "sixmonthago": commandText += " AND DATEDIFF(month, GETDATE(), [postdatetime]) > 6 "; break; //半年之前
            }

            commandText += " ORDER BY [id] DESC ";
            return DbHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获取诚信规则列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetGoodsCreditRules()
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 15 * FROM [{0}goodscreditrules] ORDER BY [id]", BaseConfigs.GetTablePrefix));
        }

        /// <summary>
        /// 创建所在地信息
        /// </summary>
        /// <param name="locations">要创建的所在地信息</param>
        /// <returns></returns>
        public int CreateLocations(Locationinfo locations)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@city", (DbType)SqlDbType.NVarChar, 50,locations.City),
						DbHelper.MakeInParam("@state", (DbType)SqlDbType.NVarChar, 50,locations.State),
						DbHelper.MakeInParam("@country", (DbType)SqlDbType.NVarChar, 50,locations.Country),
						DbHelper.MakeInParam("@zipcode", (DbType)SqlDbType.NVarChar, 20,locations.Zipcode)
				};
            string commandText = String.Format("INSERT INTO [{0}locations] ([city], [state], [country], [zipcode]) VALUES (@city, @state, @country, @zipcode);SELECT SCOPE_IDENTITY()  AS 'lid'", BaseConfigs.GetTablePrefix);
            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }

        /// <summary>
        /// 更新所在地信息
        /// </summary>
        /// <param name="locations">要更新的所在地信息</param>
        /// <returns></returns>
        public bool UpdateLocations(Locationinfo locations)
        {

            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@city", (DbType)SqlDbType.NVarChar, 50,locations.City),
						DbHelper.MakeInParam("@state", (DbType)SqlDbType.NVarChar, 50,locations.State),
						DbHelper.MakeInParam("@country", (DbType)SqlDbType.NVarChar, 50,locations.Country),
						DbHelper.MakeInParam("@zipcode", (DbType)SqlDbType.NVarChar, 20,locations.Zipcode),
                        DbHelper.MakeInParam("@lid", (DbType)SqlDbType.Int, 4,locations.Lid)
				};
            string commandText = String.Format("Update [{0}locations]  Set [city] = @city, [state] = @state, [country] = @country, [zipcode] = @zipcode WHERE [lid] = @lid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            return true;
        }

        /// <summary>
        /// 删除所在地信息
        /// </summary>
        /// <param name="lidlist">要删除的所在地id列表(以","分割)</param>
        public void DeleteLocations(string lidList)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}locations] WHERE [lid] IN ({1})", BaseConfigs.GetTablePrefix, lidList));
        }

        /// <summary>
        /// 更新指定诚信规则id的信息
        /// </summary>
        /// <param name="id">诚信规则id</param>
        /// <param name="lowerlimit">下限</param>
        /// <param name="upperlimit">上限</param>
        public void UpdateCreditRules(int id, int lowerLimit, int upperLimit)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@lowerlimit", (DbType)SqlDbType.Int, 4,lowerLimit),
						DbHelper.MakeInParam("@upperlimit", (DbType)SqlDbType.Int,4,upperLimit),
                        DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4,id)
				};

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goodscreditrules]  SET [lowerlimit] = @lowerlimit, [upperlimit] = @upperlimit WHERE [id] = @id", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获取指定返回数的商品TAG数据
        /// </summary>
        /// <param name="count">返回数</param>
        /// <returns></returns>
        public IDataReader GetHotTagsListForGoods(int count)
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP {0} * FROM [{1}tags] WHERE [gcount] > 0 ORDER BY [gcount] DESC,[orderid]", count, BaseConfigs.GetTablePrefix));
        }

        /// <summary>
        /// 获取指定标签商品数量
        /// </summary>
        /// <param name="tagid">TAG id</param>
        /// <returns></returns>
        public int GetGoodsCountWithSameTag(int tagId)
        {
            DbParameter parm = DbHelper.MakeInParam("@tagid", (DbType)SqlDbType.Int, 4, tagId);

            string commandText = string.Format("SELECT COUNT(1) FROM [{0}goodstags] AS [gt],[{0}goods] AS [g] WHERE [gt].[tagid] = @tagid AND [g].[goodsid] = [gt].[goodsid] AND [g].[displayorder]>=0 AND [g].[closed]=0 ", BaseConfigs.GetTablePrefix);

            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText, parm), 0);
        }

        /// <summary>
        /// 获取指定标签的商品数据列表
        /// </summary>
        /// <param name="tagid">TAG id</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">页面尺寸</param>
        /// <returns></returns>
        public IDataReader GetGoodsWithSameTag(int tagId, int pageIndex, int pageSize)
        {
            string commandText = "";
            if (pageIndex <= 1)
                commandText = "SELECT TOP {1} [g].* FROM [{0}goods] AS [g], [{0}goodstags] AS [gt]	WHERE [g].[goodsid] = [gt].[goodsid] AND [g].[displayorder]>=0 AND [g].[closed]=0 AND [gt].[tagid] = {2} ORDER BY [g].[goodsid] DESC";
            else
                commandText = "SELECT TOP {1} [g].* FROM [{0}goods] AS [g], [{0}goodstags] AS [gt]	WHERE [g].[goodsid] = [gt].[goodsid] AND [g].[displayorder]>=0 AND [g].[closed]=0 AND [gt].[tagid] = {2} AND [g].[goodsid] < (SELECT MIN([goodsid]) FROM (SELECT TOP " + ((pageIndex - 1) * pageSize) + " [g].[goodsid] FROM [{0}goods] AS [g], [{0}_goodstags] AS [gt] WHERE [g].[goodsid] = [gt].[goodsid] AND [g].[displayorder]>=0 AND [gt].[tagid] = {2}	ORDER BY [g].[goodsid] DESC) as tblTmp) ORDER BY [g].[goodsid] DESC";

            return DbHelper.ExecuteReader(CommandType.Text, string.Format(commandText, BaseConfigs.GetTablePrefix, pageSize, tagId));
        }

        /// <summary>
        /// 创建店铺分类
        /// </summary>
        /// <param name="shopcategoryinfo">店铺分类信息</param>
        /// <returns></returns>
        public int CreateShopCategory(Shopcategoryinfo shopCategoryInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@parentid", (DbType)SqlDbType.Int, 4,shopCategoryInfo.Parentid),
                        DbHelper.MakeInParam("@parentidlist", (DbType)SqlDbType.Char, 300,shopCategoryInfo.Parentidlist),
                        DbHelper.MakeInParam("@layer", (DbType)SqlDbType.Char, 300,shopCategoryInfo.Layer),
                        DbHelper.MakeInParam("@childcount", (DbType)SqlDbType.Char, 300,shopCategoryInfo.Childcount),
						DbHelper.MakeInParam("@syscategoryid", (DbType)SqlDbType.Int, 4,shopCategoryInfo.Syscategoryid),
						DbHelper.MakeInParam("@name", (DbType)SqlDbType.NChar, 50,shopCategoryInfo.Name),
						DbHelper.MakeInParam("@categorypic", (DbType)SqlDbType.NVarChar, 100,shopCategoryInfo.Categorypic),
						DbHelper.MakeInParam("@shopid", (DbType)SqlDbType.Int, 4,shopCategoryInfo.Shopid),
						DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,shopCategoryInfo.Displayorder)
				};
            string commandText = String.Format("INSERT INTO [{0}shopcategories] ( [parentid], [parentidlist], [layer], [childcount], [syscategoryid], [name], [categorypic], [shopid], [displayorder]) VALUES (@parentid, @parentidlist, @layer, @childcount, @syscategoryid, @name, @categorypic, @shopid, @displayorder);SELECT SCOPE_IDENTITY()  AS 'id'", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }

        /// <summary>
        /// 更新店铺分类
        /// </summary>
        /// <param name="shopcategoryinfo">店铺分类信息</param>
        /// <returns></returns>
        public bool UpdateShopCategory(Shopcategoryinfo shopCategoryInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@parentid", (DbType)SqlDbType.Int, 4,shopCategoryInfo.Parentid),
                        DbHelper.MakeInParam("@parentidlist", (DbType)SqlDbType.Char, 300,shopCategoryInfo.Parentidlist),
                        DbHelper.MakeInParam("@layer", (DbType)SqlDbType.Char, 300,shopCategoryInfo.Layer),
                        DbHelper.MakeInParam("@childcount", (DbType)SqlDbType.Char, 300,shopCategoryInfo.Childcount),
						DbHelper.MakeInParam("@syscategoryid", (DbType)SqlDbType.Int, 4,shopCategoryInfo.Syscategoryid),
                        DbHelper.MakeInParam("@name", (DbType)SqlDbType.NChar, 50,shopCategoryInfo.Name),
						DbHelper.MakeInParam("@categorypic", (DbType)SqlDbType.NVarChar, 100,shopCategoryInfo.Categorypic),
						DbHelper.MakeInParam("@shopid", (DbType)SqlDbType.Int, 4,shopCategoryInfo.Shopid),
						DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,shopCategoryInfo.Displayorder),
                    	DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,shopCategoryInfo.Categoryid)
				};
            string commandText = String.Format("Update [{0}shopcategories]  Set  [parentid] = @parentid, [parentidlist] = @parentidlist, [layer] = @layer, [childcount] = @childcount, [syscategoryid] = @syscategoryid, [name] = @name, [categorypic] = @categorypic, [shopid] = @shopid, [displayorder] = @displayorder WHERE [categoryid] = @categoryid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 更新指定店铺商品分类的子分类数字段
        /// </summary>
        /// <param name="childcount">更新的子分类数</param>
        /// <param name="categoryid">店铺商品分类id</param>
        public void UpdateShopCategoryChildCount(int childCount, int categoryId)
        {
            DbParameter[] parms =
			{
                DbHelper.MakeInParam("@childcount", (DbType)SqlDbType.Int, 4, childCount),
                DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId)
			};

            DbHelper.ExecuteDataset(CommandType.Text, string.Format("UPDATE [{0}shopcategories] SET [childcount]=@childcount WHERE [categoryid]=@categoryid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 创建店铺友情链接
        /// </summary>
        /// <param name="shoplink">店铺友情链接信息</param>
        /// <returns></returns>
        public int CreateShopLink(Shoplinkinfo shopLink)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,shopLink.Displayorder),
						DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 100,shopLink.Name),
						DbHelper.MakeInParam("@linkshopid", (DbType)SqlDbType.Int, 4,shopLink.Linkshopid),
						DbHelper.MakeInParam("@shopid", (DbType)SqlDbType.Int, 4,shopLink.Shopid)
				};
            string commandText = String.Format("INSERT INTO [{0}shoplinks] ([displayorder], [name], [linkshopid], [shopid]) VALUES (@displayorder, @name, @linkshopid, @shopid);SELECT SCOPE_IDENTITY()  AS 'id'", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }


        /// <summary>
        /// 更新店铺友情链接
        /// </summary>
        /// <param name="shoplink">店铺友情链接信息</param>
        /// <returns></returns>
        public bool UpdateShopLink(Shoplinkinfo shopLink)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4,shopLink.Displayorder),
						DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 100,shopLink.Name),
						DbHelper.MakeInParam("@linkshopid", (DbType)SqlDbType.Int, 4,shopLink.Linkshopid),
						DbHelper.MakeInParam("@shopid", (DbType)SqlDbType.Int, 4,shopLink.Shopid),
   						DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4,shopLink.Id)
				};
            string commandText = String.Format("Update [{0}shoplinks]  Set [displayorder] = @displayorder, [name] = @name, [linkshopid] = @linkshopid, [shopid] = @shopid WHERE [id] = @id", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 获取指定店铺的友情链接信息
        /// </summary>
        /// <param name="shopid">店铺id</param>
        /// <returns></returns>
        public IDataReader GetShopLinkByShopId(int shopId)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@shopid",(DbType)SqlDbType.Int,4,shopId)
								  };

            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT  * FROM [{0}shoplinks] WHERE [shopid] = @shopid  ORDER BY  [displayorder] ASC", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 删除指定id的店铺友情链接信息
        /// </summary>
        /// <param name="shoplinkidlist">店铺链接id串(格式:1,2,3)</param>
        /// <returns></returns>
        public int DeleteShopLink(string shopLinkIdList)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}shoplinks] WHERE [id] IN (" + shopLinkIdList + ")", BaseConfigs.GetTablePrefix));
        }
        /// <summary>
        /// 创建店铺
        /// </summary>
        /// <param name="shopinfo">店铺信息</param>
        /// <returns></returns>
        public int CreateShop(Shopinfo shopInfo)
        {
            DbParameter[] parms = 
				{
                        DbHelper.MakeInParam("@logo", (DbType)SqlDbType.NVarChar, 50,shopInfo.Logo),
						DbHelper.MakeInParam("@shopname", (DbType)SqlDbType.NVarChar, 100,shopInfo.Shopname),
						DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4,shopInfo.Themeid),
						DbHelper.MakeInParam("@themepath", (DbType)SqlDbType.NChar, 50,shopInfo.Themepath),
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,shopInfo.Uid),
						DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20,shopInfo.Username),
						DbHelper.MakeInParam("@introduce", (DbType)SqlDbType.NVarChar, 500,shopInfo.Introduce),
						DbHelper.MakeInParam("@lid", (DbType)SqlDbType.Int, 4,shopInfo.Lid),
						DbHelper.MakeInParam("@locus", (DbType)SqlDbType.NChar, 20,shopInfo.Locus),
						DbHelper.MakeInParam("@bulletin", (DbType)SqlDbType.NVarChar, 500,shopInfo.Bulletin),
						DbHelper.MakeInParam("@createdatetime", (DbType)SqlDbType.DateTime, 8,shopInfo.Createdatetime),
						DbHelper.MakeInParam("@invisible", (DbType)SqlDbType.Int, 4,shopInfo.Invisible),
                        DbHelper.MakeInParam("@viewcount", (DbType)SqlDbType.Int, 4,shopInfo.Viewcount)
				};
            string commandText = String.Format("INSERT INTO [{0}shops] ([logo], [shopname], [themeid], [themepath], [uid], [username], [introduce], [lid], [locus], [bulletin], [createdatetime], [invisible], [viewcount]) VALUES (@logo, @shopname, @themeid, @themepath, @uid, @username, @introduce, @lid, @locus, @bulletin, @createdatetime, @invisible, @viewcount);SELECT SCOPE_IDENTITY()  AS 'id'", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }

        /// <summary>
        /// 更新店铺信息
        /// </summary>
        /// <param name="shopinfo">店铺信息</param>
        public bool UpdateShop(Shopinfo shopInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@logo", (DbType)SqlDbType.NVarChar, 50,shopInfo.Logo),
                        DbHelper.MakeInParam("@shopname", (DbType)SqlDbType.NVarChar, 100,shopInfo.Shopname),
						DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4,shopInfo.Themeid),
						DbHelper.MakeInParam("@themepath", (DbType)SqlDbType.NChar, 50,shopInfo.Themepath),
						DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,shopInfo.Uid),
						DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20,shopInfo.Username),
						DbHelper.MakeInParam("@introduce", (DbType)SqlDbType.NVarChar, 500,shopInfo.Introduce),
						DbHelper.MakeInParam("@lid", (DbType)SqlDbType.Int, 4,shopInfo.Lid),
						DbHelper.MakeInParam("@locus", (DbType)SqlDbType.NChar, 20,shopInfo.Locus),
						DbHelper.MakeInParam("@bulletin", (DbType)SqlDbType.NVarChar, 500,shopInfo.Bulletin),
						DbHelper.MakeInParam("@createdatetime", (DbType)SqlDbType.DateTime, 8,shopInfo.Createdatetime),
						DbHelper.MakeInParam("@invisible", (DbType)SqlDbType.Int, 4,shopInfo.Invisible),
                        DbHelper.MakeInParam("@viewcount", (DbType)SqlDbType.Int, 4,shopInfo.Viewcount),
                        DbHelper.MakeInParam("@shopid", (DbType)SqlDbType.Int, 4,shopInfo.Shopid)
				};
            string commandText = String.Format("Update [{0}shops]  Set [logo] = @logo, [shopname] = @shopname, [themeid] = @themeid, [themepath] = @themepath, [uid] = @uid, [username] = @username, [introduce] = @introduce, [lid] = @lid, [locus] = @locus, [bulletin] = @bulletin, [createdatetime] = @createdatetime, [invisible] = @invisible, [viewcount] = @viewcount WHERE [shopid] = @shopid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 创建店铺主题
        /// </summary>
        /// <param name="shopthemeinfo">店铺主题信息</param>
        /// <returns></returns>
        public int CreateShopTheme(Shopthemeinfo shopThemeInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@directory", (DbType)SqlDbType.VarChar, 100,shopThemeInfo.Directory),
						DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50,shopThemeInfo.Name),
						DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 100,shopThemeInfo.Author),
						DbHelper.MakeInParam("@createdate", (DbType)SqlDbType.NVarChar, 50,shopThemeInfo.Createdate),
						DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.NVarChar, 100,shopThemeInfo.Copyright)
				};
            string commandText = String.Format("INSERT INTO [{0}shopthemes] ([directory], [name], [author], [createdate], [copyright]) VALUES (@directory, @name, @author, @createdate, @copyright);SELECT SCOPE_IDENTITY()  AS 'id'", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }

        /// <summary>
        /// 更新店铺主题
        /// </summary>
        /// <param name="shopthemeinfo">店铺主题信息</param>
        /// <returns></returns>
        public bool UpdateShopTheme(Shopthemeinfo shopThemeInfo)
        {
            DbParameter[] parms = 
				{
						DbHelper.MakeInParam("@directory", (DbType)SqlDbType.VarChar, 100,shopThemeInfo.Directory),
						DbHelper.MakeInParam("@name", (DbType)SqlDbType.NVarChar, 50,shopThemeInfo.Name),
						DbHelper.MakeInParam("@author", (DbType)SqlDbType.NVarChar, 100,shopThemeInfo.Author),
						DbHelper.MakeInParam("@createdate", (DbType)SqlDbType.NVarChar, 50,shopThemeInfo.Createdate),
						DbHelper.MakeInParam("@copyright", (DbType)SqlDbType.NVarChar, 100,shopThemeInfo.Copyright),
                    	DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4,shopThemeInfo.Themeid)
				};
            string commandText = String.Format("Update [{0}shopthemes]  Set [directory] = @directory, [name] = @name, [author] = @author, [createdate] = @createdate, [copyright] = @copyright WHERE [themeid] = @themeid", BaseConfigs.GetTablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

            return true;
        }

        /// <summary>
        /// 获取热门或新开的店铺信息
        /// </summary>
        /// <param name="shoptype">热门店铺(1:热门店铺, 2 :新开店铺)</param>
        /// <param name="topnumber">返回数</param>
        /// <returns></returns>
        public IDataReader GetHotOrNewShops(int shopType, int topNumber)
        {
            string commandText = string.Format("SELECT TOP {0} * FROM [{1}shops] WHERE [invisible] = 0 ORDER BY ", topNumber, BaseConfigs.GetTablePrefix);

            if (shopType == 1)
                commandText += "[viewcount] DESC ";
            else
                commandText += "[createdatetime] DESC ";

            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获取指定主题ID的店铺风格信息
        /// </summary>
        /// <param name="themeid">主题ID</param>
        /// <returns></returns>
        public IDataReader GetShopThemeByThemeId(int themeId)
        {
            DbParameter[] parms = 
				{
                    	DbHelper.MakeInParam("@themeid", (DbType)SqlDbType.Int, 4,themeId)
				};
            return DbHelper.ExecuteReader(CommandType.Text, String.Format("SELECT  TOP 1 * FROM  [{0}shopthemes]  WHERE  [themeid] = @themeid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 通过指定的论坛版块id获取相应的商品分类
        /// </summary>
        /// <param name="forumid">版块id</param>
        /// <returns></returns>
        public int GetGoodsCategoryIdByFid(int forumId)
        {
            DbParameter[] parms = 
				{
                    	DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4,forumId)
				};
            string commandText = String.Format("SELECT  ISNULL  ((SELECT  TOP 1 [categoryid]  FROM  [{0}goodscategories]  WHERE  [fid] = @fid), 0)", BaseConfigs.GetTablePrefix);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        }

        /// <summary>
        /// 获取绑定论坛版块ID的商品分类
        /// </summary>
        /// <returns></returns>
        public IDataReader GetGoodsCategoryWithFid()
        {
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT  [fid], [categoryid] FROM  [{0}goodscategories] GROUP BY [fid], [layer], [categoryid] HAVING      (fid > 0) AND (layer <= 1) ORDER BY [layer]", BaseConfigs.GetTablePrefix));
        }

        /// <summary>
        /// 获取指定店铺的商品类型数据(json格式)
        /// </summary>
        /// <param name="shopid">店铺id</param>
        /// <returns></returns>
        public DataTable GetShopCategoryTableToJson(int shopId)
        {
            DbParameter[] parms = 
				{
                    	DbHelper.MakeInParam("@shopid", (DbType)SqlDbType.Int, 4,shopId)
				};
            return DbHelper.ExecuteDataset(CommandType.Text, String.Format("SELECT  * FROM  [{0}shopcategories]  WHERE  [shopid] = @shopid ORDER BY [displayorder] ASC ", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// 获取指定shopid的店铺信息
        /// </summary>
        /// <returns></returns>
        public IDataReader GetShopByUserId(int userId)
        {
            DbParameter[] parms = 
				{
                    	DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,userId)
				};
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}shops] WHERE [uid] = @uid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获取指定分类id的店铺商品类型数据
        /// </summary>
        /// <param name="categoryid">分类id</param>
        /// <returns></returns>
        public IDataReader GetShopCategoryByCategoryId(int categoryId)
        {
            DbParameter[] parms = 
				{
                    	DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4,categoryId)
				};
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}shopcategories] WHERE [categoryid] = @categoryid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 指定的商品分类下有无子分类
        /// </summary>
        /// <param name="shopcategoryinfo">指定的商品分类</param>
        /// <returns></returns>
        public bool IsExistSubShopCategory(Shopcategoryinfo shopCategoryInfo)
        {
            DbParameter[] parms = 
            {
                DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, shopCategoryInfo.Categoryid)
		    };

            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}shopcategories] WHERE [parentid]=@categoryid", BaseConfigs.GetTablePrefix), parms).Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// 删除指定的商品分类信息
        /// </summary>
        /// <param name="shopcategoryinfo"></param>
        /// <returns></returns>
        public bool DeleteShopCategory(Shopcategoryinfo shopCategoryInfo)
        {
            bool result = false;
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    //调整在当前节点排序位置之后的节点,做减1操作
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, string.Format("UPDATE [{0}shopcategories] SET [displayorder]=[displayorder]-1 WHERE [displayorder]>{1}", BaseConfigs.GetTablePrefix, shopCategoryInfo.Displayorder));
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, string.Format("UPDATE [{0}goods] SET [shopcategorylist] = REPLACE([shopcategorylist], ',{1},', ',') WHERE  CHARINDEX(',{1},', RTRIM([shopcategorylist])) > 0", BaseConfigs.GetTablePrefix, shopCategoryInfo.Categoryid));
                    DbHelper.ExecuteNonQuery(trans, CommandType.Text, string.Format("DELETE FROM [{0}shopcategories] WHERE [categoryid] = {1}", BaseConfigs.GetTablePrefix, shopCategoryInfo.Categoryid));
                    trans.Commit();
                    result = true;
                }
                catch
                {
                    trans.Rollback();
                }
            }
            conn.Close();
            return result;
        }

        /// <summary>
        /// 移动商品分类
        /// </summary>
        /// <param name="shopcategoryinfo">源分类</param>
        /// <param name="targetshopcategoryinfo">目标分类</param>
        /// <param name="isaschildnode">是否作为子节点</param>
        public void MovingShopCategoryPos(Shopcategoryinfo shopCategoryInfo, Shopcategoryinfo targetShopCategoryInfo, bool isAsChildNode)
        {
            SqlConnection conn = new SqlConnection(DbHelper.ConnectionString);
            conn.Open();

            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    //当前商品分类带子分类时
                    if (DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 [categoryid] FROM [{0}shopcategories] WHERE [parentid]={1}", BaseConfigs.GetTablePrefix, shopCategoryInfo.Categoryid)).Tables[0].Rows.Count > 0)
                    {
                        #region

                        string commandText = "";
                        if (isAsChildNode) //作为商品分类子分类插入
                        {
                            //让位于当前商品分类(分类)显示顺序之后的商品分类全部加1(为新加入的商品分类让位结果)
                            commandText = string.Format("UPDATE [{0}shopcategories] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={1} AND [shopid] = {2}",
                                                      BaseConfigs.GetTablePrefix,
                                                      targetShopCategoryInfo.Displayorder + 1,
                                                      targetShopCategoryInfo.Shopid);
                            DbHelper.ExecuteDataset(trans, CommandType.Text, commandText);

                            //更新当前商品分类的相关信息
                            commandText = string.Format("UPDATE [{0}shopcategories] SET [parentid]={1},[displayorder]={2} WHERE [categoryid]={3}", 
                                                       BaseConfigs.GetTablePrefix, 
                                                       targetShopCategoryInfo.Categoryid, 
                                                       targetShopCategoryInfo.Displayorder + 1, 
                                                       shopCategoryInfo.Categoryid);
                            DbHelper.ExecuteDataset(trans, CommandType.Text, commandText);
                        }
                        else //作为同级商品分类,在目标商品分类之前插入
                        {
                            //让位于包括当前商品分类显示顺序之后的商品分类全部加1(为新加入的商品分类让位结果)
                            commandText = string.Format("UPDATE [{3}shopcategories] SET [displayorder]=[displayorder]+1 WHERE ([displayorder]>={0} AND [shopid] = {1}) OR [categoryid]={2}",
                                                      targetShopCategoryInfo.Shopid,
                                                      targetShopCategoryInfo.Displayorder,
                                                      targetShopCategoryInfo.Categoryid,
                                                      BaseConfigs.GetTablePrefix);
                            DbHelper.ExecuteDataset(trans, CommandType.Text, commandText);

                            //更新当前商品分类的相关信息
                            commandText = string.Format("UPDATE [{3}shopcategories] SET [parentid]={1},[displayorder]={2}  WHERE [categoryid]={0}", 
                                                      shopCategoryInfo.Categoryid, 
                                                      targetShopCategoryInfo.Parentid, 
                                                      targetShopCategoryInfo.Displayorder,
                                                      BaseConfigs.GetTablePrefix);
                            DbHelper.ExecuteDataset(trans, CommandType.Text, commandText);
                        }

                        #endregion
                    }
                    else //当前商品分类不带子分类
                    {
                        #region

                        //让位于当前节点显示顺序之后的节点全部减1 [起到删除节点的效果]
                        if (isAsChildNode) //作为子分类插入
                        {
                            //让位于当前商品分类显示顺序之后的商品分类全部加1(为新加入的商品分类让位结果)
                            string commandText = string.Format("UPDATE [{2}shopcategories] SET [displayorder]=[displayorder]+1 WHERE [displayorder]>={0}  AND [shopid] = {1}",
                                                             targetShopCategoryInfo.Displayorder + 1,
                                                             targetShopCategoryInfo.Shopid,
                                                             BaseConfigs.GetTablePrefix);
                            DbHelper.ExecuteDataset(trans, CommandType.Text, commandText);

                            string parentidlist = null;
                            if (targetShopCategoryInfo.Parentidlist == "0")
                                parentidlist = targetShopCategoryInfo.Categoryid.ToString();
                            else
                                parentidlist = targetShopCategoryInfo.Parentidlist.Trim() + "," + targetShopCategoryInfo.Categoryid;

                            //更新当前商品分类的相关信息
                            commandText = string.Format("UPDATE [{5}shopcategories] SET [parentid]={1},[layer]={2},[parentidlist]='{3}',[displayorder]={4} WHERE [categoryid]={0}",
                                                      shopCategoryInfo.Categoryid,
                                                      targetShopCategoryInfo.Categoryid,
                                                      parentidlist.Split(',').Length,
                                                      parentidlist,
                                                      targetShopCategoryInfo.Displayorder + 1,
                                                      BaseConfigs.GetTablePrefix
                                );
                            DbHelper.ExecuteDataset(trans, CommandType.Text, commandText);

                        }
                        else //作为同级商品分类,在目标商品分类之前插入
                        {
                            //让位于包括当前商品分类显示顺序之后的商品分类全部加1(为新加入的商品分类让位结果)
                            string commandText = string.Format("UPDATE [{3}shopcategories] SET [displayorder]=[displayorder]+1 WHERE ([displayorder]>={0} AND [shopid] = {1}) OR [categoryid]={2}",
                                                             targetShopCategoryInfo.Displayorder + 1,
                                                             targetShopCategoryInfo.Shopid,
                                                             targetShopCategoryInfo.Categoryid,
                                                             BaseConfigs.GetTablePrefix);
                            DbHelper.ExecuteDataset(trans, CommandType.Text, commandText);



                            //更新当前商品分类的相关信息
                            commandText = string.Format("UPDATE [{5}shopcategories]  SET [parentid]={1},[layer]={2},[parentidlist]='{3}',[displayorder]={4} WHERE [categoryid]={0}",
                                                      shopCategoryInfo.Categoryid,
                                                      targetShopCategoryInfo.Parentid,
                                                      targetShopCategoryInfo.Parentidlist.Trim().Split(',').Length,
                                                      targetShopCategoryInfo.Parentidlist.Trim(),
                                                      targetShopCategoryInfo.Displayorder,
                                                      BaseConfigs.GetTablePrefix
                                );
                            DbHelper.ExecuteDataset(trans, CommandType.Text, commandText);
                        }

                        #endregion
                    }
                    trans.Commit();
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                conn.Close();
            }
        }

        /// <summary>
        /// 更新商品分类的显示顺序
        /// </summary>
        /// <param name="displayorder">显示信息</param>
        /// <param name="categoryid">商品分类id</param>
        public void UpdateShopCategoryDisplayOrder(int displayOrder, int categoryId)
        {
            DbParameter[] parms =
			{
                DbHelper.MakeInParam("@displayorder", (DbType)SqlDbType.Int, 4, displayOrder),
                DbHelper.MakeInParam("@categoryid", (DbType)SqlDbType.Int, 4, categoryId)
			};

            DbHelper.ExecuteDataset(CommandType.Text, string.Format("UPDATE [{0}shopcategories] SET [displayorder]=@displayorder WHERE [categoryid] = @categoryid", BaseConfigs.GetTablePrefix), parms);
        }


        /// <summary>
        /// 获取指定店铺的商品分类
        /// </summary>
        /// <param name="shopid">店铺id</param>
        /// <returns></returns>
        public DataTable GetShopCategoryByShopId(int shopId)
        {
            DbParameter[] parms = 
				{
                    	DbHelper.MakeInParam("@shopid", (DbType)SqlDbType.Int, 4,shopId)
				};
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT [categoryid], [parentid] FROM [{0}shopcategories] WHERE [shopid] = @shopid", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// 获取以当前分类为父分类的所有分类数据
        /// </summary>
        /// <param name="parentid">当前分类id</param>
        /// <returns></returns>
        public DataTable GetCategoryidInShopByParentid(int parentId)
        {
            DbParameter[] parms =
			{
                DbHelper.MakeInParam("@parentid", (DbType)SqlDbType.Int, 4, parentId)
			};
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT [categoryid] FROM [{0}shopcategories] WHERE [parentid]=@parentid ORDER BY [displayorder] ASC", BaseConfigs.GetTablePrefix), parms).Tables[0];
        }

        /// <summary>
        /// 更新指定商品的店铺商品分类字段
        /// </summary>
        /// <param name="goodsidlist">指定商品id串(格式:1,2,3)</param>
        /// <param name="shopgoodscategoryid">要绑定的店铺商品分类id</param>
        /// <returns></returns>
        public int MoveGoodsShopCategory(string goodsIdList, int shopGoodsCategoryId)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goods] SET [shopcategorylist] = RTRIM([shopcategorylist]) + '{1},' WHERE [goodsid] IN ({2}) AND CHARINDEX(',{1},', RTRIM([shopcategorylist])) <= 0 ", BaseConfigs.GetTablePrefix, shopGoodsCategoryId, goodsIdList));
        }

        /// <summary>
        /// 移除指定商品的店铺商品分类
        /// </summary>
        /// <param name="removegoodsid">指定的商品id</param>
        /// <param name="removeshopgoodscategoryid">要移除的店铺商品分类id</param>
        /// <returns></returns>
        public int RemoveGoodsShopCategory(int removeGoodsId, int removeShopGoodsCategoryId)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goods] SET [shopcategorylist] = REPLACE([shopcategorylist], ',{1},', ',')  WHERE [goodsid] = {2} AND CHARINDEX(',{1},', RTRIM([shopcategorylist])) >= 0 ", BaseConfigs.GetTablePrefix, removeShopGoodsCategoryId, removeGoodsId));
        }

        /// <summary>
        /// 设置指定商品的推荐值信息
        /// </summary>
        /// <param name="goodsidlist">指定的商品id串(格式:1,2,3)</param>
        /// <param name="recommendvalue">推荐值</param>
        /// <returns></returns>
        public int RecommendGoods(string goodsIdList, int recommendValue)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goods] SET [recommend] = {1} WHERE [goodsid] IN ({2})", BaseConfigs.GetTablePrefix, recommendValue, goodsIdList));
        }

        /// <summary>
        /// 获取店铺主题信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetShopThemes()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}shopthemes] ORDER BY [themeid] ASC", BaseConfigs.GetTablePrefix)).Tables[0];
        }

        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="datetype">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public IDataReader GetHotGoods(int days, int categoryId, int count, string condition)
        {
            string commandText = "";
            if (categoryId <= 0)
                commandText = string.Format("SELECT * FROM  [{0}goods] WHERE [goodsid] IN (SELECT TOP {1} [goodsid] FROM  [{0}goodstradelogs]  WHERE DATEDIFF(day, [lastupdate], GETDATE()) <= {2} GROUP BY [goodsid] ORDER BY COUNT(goodsid) DESC) {3}", BaseConfigs.GetTablePrefix, count, days, condition);
            else
                commandText = string.Format("SELECT * FROM  [{0}goods] WHERE [goodsid] IN (SELECT TOP {1} [goodsid] FROM  [{0}goodstradelogs]  WHERE DATEDIFF(day, [lastupdate], GETDATE()) <= {2} AND [categoryid] = {3}  GROUP BY [goodsid] ORDER BY COUNT(goodsid) DESC) {4}", BaseConfigs.GetTablePrefix, count, days, categoryId, condition);

            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获取版块在商品分类中绑定的个数
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <returns></returns>
        public int GetCategoriesFidCount(int fid)
        {
            DbParameter parms = DbHelper.MakeInParam("@fid", (DbType)SqlDbType.Int, 4, fid);

            return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(*) FROM [{0}goodscategories] WHERE [fid]=@fid", BaseConfigs.GetTablePrefix), parms));
        }

        /// <summary>
        /// 获取所有商品交易记录的Sql语句
        /// </summary>
        /// <returns></returns>
        public string GetAllGoodstradelogs(string status)
        {
            if (status == "")
                return string.Format("SELECT * FROM  [{0}goodstradelogs]", BaseConfigs.GetTablePrefix);
            else
                return string.Format("SELECT * FROM  [{0}goodstradelogs] WHERE [status]={1}", BaseConfigs.GetTablePrefix, status);
        }

        /// <summary>
        /// 获取所有回收站中的商品语句
        /// </summary>
        /// <returns></returns>
        public string GetAllRecyclebinGoods()
        {
            return string.Format("SELECT g.*,c.categoryname FROM [{0}goods] g LEFT JOIN [{0}goodscategories] c ON g.[categoryid]=c.[categoryid] WHERE g.[displayorder]=-1", BaseConfigs.GetTablePrefix);
        }

        /// <summary>
        /// 获取全部审核的商品
        /// </summary>
        /// <returns></returns>
        public string GetAllAuditGoods()
        {
            return string.Format("SELECT g.*,c.categoryname FROM [{0}goods] g LEFT JOIN [{0}goodscategories] c ON g.[categoryid]=c.[categoryid] WHERE g.[displayorder]=-2", BaseConfigs.GetTablePrefix);
        }

        /// <summary>
        /// 恢复回收站中的商品
        /// </summary>
        /// <param name="goodsid">要恢复商品的ID列表</param>
        public void ResetRecyclebinGoods(string goodsId)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goods] SET [displayorder]=0 WHERE [goodsid] IN ({1})", BaseConfigs.GetTablePrefix, goodsId));
        }

        /// <summary>
        /// 更新指定商品为通过审核
        /// </summary>
        /// <param name="goodsid"></param>
        public void PassAuditGoods(string goodsId)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}goods] SET [displayorder]=0 WHERE [goodsid] IN ({1})", BaseConfigs.GetTablePrefix, goodsId));
        }

        /// <summary>
        /// 清除商品分类绑定的版块
        /// </summary>
        /// <param name="fid"></param>
        public void EmptyGoodsCategoryFid(int fid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text,string.Format("UPDATE [{0}goodscategories] SET [fid]=0 WHERE [fid]={1}",BaseConfigs.GetTablePrefix, fid));
        }

        #region 注释的代码
        /// <summary>
        /// 获取指定分类的最新商品列表
        /// </summary>
        /// <param name="categoryid">商品分类ID</param>
        /// <param name="topnumber">获取记录条数</param>
        /// <returns></returns>
        //public IDataReader GetNewGoodsList(int categoryId, int topNumber)
        //{
        //    if (topNumber > 0)
        //    {
        //        if (categoryId > 0)
        //        {
        //            return GetGoodsList(categoryId, topNumber, 1, " AND [closed] = 0 AND [displayorder] >=0 ", "goodsid", 1);
        //        }
        //        else
        //        {
        //            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}goods] WHERE [closed] = 0 AND [displayorder] >=0 AND CHARINDEX('','{1}','' , '',[parentcategorylist],'')>0) ORDER BY [goodsid] DESC", BaseConfigs.GetTablePrefix, categoryId));
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 创建商品用户信用记录
        /// </summary>
        /// <param name="goodsusercredits">要创建的用户信用信息</param>
        /// <returns></returns>
        //public int CreateGoodsUserCredit(Goodsusercreditinfo goodsUserCredits)
        //{
        //    DbParameter[] parms = 
        //        {
        //                DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,goodsUserCredits.Uid),
        //                DbHelper.MakeInParam("@oneweek", (DbType)SqlDbType.Int, 4,goodsUserCredits.Oneweek),
        //                DbHelper.MakeInParam("@onemonth", (DbType)SqlDbType.Int, 4,goodsUserCredits.Onemonth),
        //                DbHelper.MakeInParam("@sixmonth", (DbType)SqlDbType.Int, 4,goodsUserCredits.Sixmonth),
        //                DbHelper.MakeInParam("@sixmonthago", (DbType)SqlDbType.Int, 4,goodsUserCredits.Sixmonthago),
        //                DbHelper.MakeInParam("@ratefrom", (DbType)SqlDbType.TinyInt, 1,goodsUserCredits.Ratefrom),
        //                DbHelper.MakeInParam("@ratetype", (DbType)SqlDbType.TinyInt, 1,goodsUserCredits.Ratetype)
        //        };
        //    string commandText = String.Format("INSERT INTO [{0}goodsusercredits] ([uid], [oneweek], [onemonth], [sixmonth], [sixmonthago], [ratefrom], [ratetype]) VALUES (@uid, @oneweek, @onemonth, @sixmonth, @sixmonthago, @ratefrom, @ratetype);SELECT SCOPE_IDENTITY()  AS 'id';", BaseConfigs.GetTablePrefix);

        //    return TypeConverter.ObjectToInt(DbHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0].Rows[0][0], -1);
        //}

        /// <summary>
        /// 删除指定附件
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <returns></returns>
        //public int DeleteGoodsAttachment(int aid)
        //{
        //    DbParameter[] parms = {
        //                               DbHelper.MakeInParam("@aid",(DbType)SqlDbType.Int,4,aid)
        //                           };

        //    return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM [{0}goodsattachments] WHERE [aid]=@aid", BaseConfigs.GetTablePrefix), parms);
        //}

        /// <summary>
        /// 获取指定用户商品字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="selleruid">卖家信息</param>
        /// <returns>查询条件</returns>
        //public string GetGoodsSellerUidCondition(int opCode, int sellerUid)
        //{
        //    string condition = "";
        //    if (sellerUid > 0)
        //    {
        //        condition = GetOperaCode(opCode);
        //        if (!Utils.StrIsNullOrEmpty(condition))
        //        {
        //            condition = string.Format(" AND [selleruid] {0} {1} ", condition, sellerUid);
        //        }
        //    }
        //    return condition;
        //}

        /// <summary>
        /// 获取商品价格字段条件
        /// </summary>
        /// <param name="opcode">操作码</param>
        /// <param name="price">价格</param>
        /// <returns>查询条件</returns>
        //public string GetGoodsPriceCondition(int opCode, decimal price)
        //{
        //    string condition = "";
        //    if (price > -3 && price <= 6)
        //    {
        //        condition = GetOperaCode(opCode);
        //        if (!Utils.StrIsNullOrEmpty(condition))
        //        {
        //            condition = string.Format(" AND [price] {0} {1} ", condition, price);
        //        }
        //    }
        //    return condition;
        //}

        /// <summary>
        /// 更新商品评价
        /// </summary>
        /// <param name="goodsrates">要更新的商品评价信息</param>
        /// <returns></returns>
        //public bool UpdateGoodsRate(Goodsrateinfo goodsRates)
        //{
        //    DbParameter[] parms = 
        //        {
        //                DbHelper.MakeInParam("@goodstradelogid", (DbType)SqlDbType.Int, 4,goodsRates.Goodstradelogid),
        //                DbHelper.MakeInParam("@message", (DbType)SqlDbType.NChar, 200,goodsRates.Message),
        //                DbHelper.MakeInParam("@explain", (DbType)SqlDbType.NChar, 200,goodsRates.Explain),
        //                DbHelper.MakeInParam("@ip", (DbType)SqlDbType.NVarChar, 15,goodsRates.Ip),
        //                DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4,goodsRates.Uid),
        //                DbHelper.MakeInParam("@uidtype", (DbType)SqlDbType.TinyInt, 1,goodsRates.Uidtype),
        //                DbHelper.MakeInParam("@ratetouid", (DbType)SqlDbType.Int, 4,goodsRates.Ratetouid),
        //                DbHelper.MakeInParam("@username", (DbType)SqlDbType.NChar, 20,goodsRates.Username),
        //                DbHelper.MakeInParam("@postdatetime", (DbType)SqlDbType.DateTime, 8,goodsRates.Postdatetime),
        //                DbHelper.MakeInParam("@goodsid", (DbType)SqlDbType.Int, 4,goodsRates.Goodsid),
        //                DbHelper.MakeInParam("@goodstitle", (DbType)SqlDbType.NChar, 60,goodsRates.Goodstitle),
        //                DbHelper.MakeInParam("@price", (DbType)SqlDbType.Decimal, 18,goodsRates.Price),
        //                DbHelper.MakeInParam("@ratetype", (DbType)SqlDbType.TinyInt, 1,goodsRates.Ratetype),
        //                DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4,goodsRates.Id)
        //        };
        //    string commandText = String.Format("Update [{0}goodsrates]  Set [goodstradelogid] = @goodstradelogid, [message] = @message, [explain] = @explain, [ip] = @ip, [uid] = @uid, [uidtype] = @uidtype, [ratetouid] = @ratetouid, [ratetousername] = @ratetousername, [username] = @username, [postdatetime] = @postdatetime, [goodsid] = @goodsid, [goodstitle] = @goodstitle, [price] = @price, [ratetype] = @ratetype WHERE [id] = @id", BaseConfigs.GetTablePrefix);

        //    DbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);

        //    return true;
        //}
        #endregion
    }
}