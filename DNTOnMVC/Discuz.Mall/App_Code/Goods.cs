using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Mall.Data;
using Discuz.Forum;
using Discuz.Plugin.Preview;

namespace Discuz.Mall
{
    /// <summary>
    /// 商品管理操作类
    /// </summary>
    public class Goods
    {
        #region 正则表达式静态变量声明

        private static Regex regexAttach = new Regex(@"\[attach\](\d+?)\[\/attach\]", RegexOptions.IgnoreCase);

        private static Regex regexHide = new Regex(@"\s*\[hide\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", RegexOptions.IgnoreCase);

        private static Regex regexAttachImg = new Regex(@"\[attachimg\](\d+?)\[\/attachimg\]", RegexOptions.IgnoreCase);

        #endregion

        /// <summary>
        /// 商品信息字段(message)转换
        /// </summary>
        /// <param name="goodsPramsInfo">要转换的信息和相关参数设置</param>
        /// <param name="attColl">当前商品所包括的附件集合</param>
        /// <returns>返回转换后的信息</returns>
        public static string MessgeTranfer(GoodspramsInfo goodsPramsInfo, GoodsattachmentinfoCollection attColl)
        {
            goodsPramsInfo.Hide = 0;
            //先简单判断是否是动网兼容模式
            if (!goodsPramsInfo.Ubbmode)
                goodsPramsInfo.Sdetail = UBB.UBBToHTML((PostpramsInfo)goodsPramsInfo);
            else
                goodsPramsInfo.Sdetail = Utils.HtmlEncode(goodsPramsInfo.Sdetail);

            string message = goodsPramsInfo.Sdetail;
            if (GoodsAttachments.GetGoodsAttachmentsByGoodsid(goodsPramsInfo.Goodsid).Count > 0 || regexAttach.IsMatch(message) || regexAttachImg.IsMatch(message))
            {
                //获取在[hide]标签中的附件id
                string[] attHidArray = GetHiddenAttachIdList(goodsPramsInfo.Sdetail, goodsPramsInfo.Hide);

                for (int i = 0; i < attColl.Count; i++)
                {
                    message = GetMessageWithAttachInfo(goodsPramsInfo, 1, attHidArray, attColl[i], message);
                    if (Utils.InArray(attColl[i].Aid.ToString(), attHidArray))
                        attColl.RemoveAt(i);
                }
                goodsPramsInfo.Sdetail = message;
            }
            return goodsPramsInfo.Sdetail;
        }

        /// <summary>
        /// 获取被包含在[hide]标签内的附件id
        /// </summary>
        /// <param name="content">帖子内容</param>
        /// <param name="hide">隐藏标记</param>
        /// <returns>隐藏的附件id数组</returns>
        private static string[] GetHiddenAttachIdList(string content, int hide)
        {
            if (hide == 0)
                return new string[0];

            StringBuilder tmpStr = new StringBuilder();
            StringBuilder hidContent = new StringBuilder();
            foreach (Match m in regexHide.Matches(content))
            {
                if (hide == 1)
                    hidContent.Append(m.Groups[0].ToString());
            }

            foreach (Match ma in regexAttach.Matches(hidContent.ToString()))
            {
                tmpStr.Append(ma.Groups[1].ToString());
                tmpStr.Append(",");
            }

            foreach (Match ma in regexAttachImg.Matches(hidContent.ToString()))
            {
                tmpStr.Append(ma.Groups[1].ToString());
                tmpStr.Append(",");
            }

            if (tmpStr.Length == 0)
                return new string[0];

            return tmpStr.Remove(tmpStr.Length - 1, 1).ToString().Split(',');
        }

        /// <summary>
        /// 获取加载附件信息的商品内容
        /// </summary>
        /// <param name="goodspramsinfo">参数列表</param>
        /// <param name="allowGetAttach">是否允许获取附件</param>
        /// <param name="attHidArray">隐藏在hide标签中的附件数组</param>
        /// <param name="attinfo">附件信息</param>
        /// <param name="message">内容信息</param>
        /// <returns>商品内容信息</returns>
        private static string GetMessageWithAttachInfo(GoodspramsInfo goodsPramsInfo, int allowGetAttach, string[] attHidArray, Goodsattachmentinfo attInfo, string message)
        {
            string forumPath = BaseConfigs.GetBaseConfig().Forumpath;
            string filesize;
            string replacement;
            if (Utils.InArray(attInfo.Aid.ToString(), attHidArray))
                return message;
        
            attInfo.Filename = attInfo.Filename.ToString().Replace("\\", "/");

            if (message.IndexOf("[attach]" + attInfo.Aid.ToString() + "[/attach]") != -1 || message.IndexOf("[attachimg]" + attInfo.Aid.ToString() + "[/attachimg]") != -1)
            {
                if (attInfo.Filesize > 1024)
                    filesize = Convert.ToString(Math.Round(Convert.ToDecimal(attInfo.Filesize) / 1024, 2)) + " K";
                else
                    filesize = attInfo.Filesize + " B";

                if (Utils.IsImgFilename(attInfo.Attachment))
                {
                    if (attInfo.Filename.ToLower().IndexOf("http") == 0)
                        replacement = "<span style=\"position: absolute; display: none;\" onmouseover=\"showMenu(this.id, 0, 1)\" id=\"attach_" + attInfo.Aid + "\"><img border=\"0\" src=\"" + forumPath + "images/attachicons/attachimg.gif\" /></span><img src=\"" + attInfo.Filename + "\" onload=\"attachimg(this, 'load');\" onmouseover=\"attachimginfo(this, 'attach_" + attInfo.Aid + "', 1);attachimg(this, 'mouseover')\" onclick=\"zoom(this);\" onmouseout=\"attachimginfo(this, 'attach_" + attInfo.Aid + "', 0, event)\" /><div id=\"attach_" + attInfo.Aid + "_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"" + forumPath + "images/attachicons/image.gif\" /><a target=\"_blank\" href=\"" + attInfo.Filename + "\"><strong>" + attInfo.Attachment + "</strong></a>(" + filesize + ")<br/><div class=\"t_smallfont\">" + attInfo.Postdatetime + "</div></div>";
                    else
                        replacement = "<span style=\"position: absolute; display: none;\" onmouseover=\"showMenu(this.id, 0, 1)\" id=\"attach_" + attInfo.Aid + "\"><img border=\"0\" src=\"" + forumPath + "images/attachicons/attachimg.gif\" /></span><img src=\"" + forumPath + "upload/" + attInfo.Filename + "\" onload=\"attachimg(this, 'load');\" onmouseover=\"attachimginfo(this, 'attach_" + attInfo.Aid + "', 1);attachimg(this, 'mouseover')\" onclick=\"zoom(this);\" onmouseout=\"attachimginfo(this, 'attach_" + attInfo.Aid + "', 0, event)\" /><div id=\"attach_" + attInfo.Aid + "_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"" + forumPath + "images/attachicons/image.gif\" /><a target=\"_blank\" href=\"" + forumPath + "upload/" + attInfo.Filename + "\"><strong>" + attInfo.Attachment + "</strong></a>(" + filesize + ")<br/><div class=\"t_smallfont\">" + attInfo.Postdatetime + "</div></div>";
                }
                else
                {
                    if (attInfo.Filename.ToLower().IndexOf("http") == 0)
                        replacement = string.Format("<p><img alt=\"\" src=\"{0}images/attachicons/attachment.gif\" border=\"0\" /><span class=\"bold\">附件</span>: <a href=\""+ attInfo.Filename +"\" target=\"_blank\">{2}</a> ({3}, {4})", forumPath, attInfo.Aid.ToString(), attInfo.Attachment.ToString().Trim(), attInfo.Postdatetime, filesize);
                    else
                        replacement = string.Format("<p><img alt=\"\" src=\"{0}images/attachicons/attachment.gif\" border=\"0\" /><span class=\"bold\">附件</span>: <a href=\"" + forumPath + "upload/" + attInfo.Filename + "\" target=\"_blank\">{2}</a> ({3}, {4})", forumPath, attInfo.Aid.ToString(), attInfo.Attachment.ToString().Trim(), attInfo.Postdatetime, filesize);
                }

                Regex r = new Regex(string.Format(@"\[attach\]{0}\[/attach\]|\[attachimg\]{0}\[/attachimg\]", attInfo.Aid));
                message = r.Replace(message, replacement, 1);
                message = message.Replace("[attach]" + attInfo.Aid.ToString() + "[/attach]", string.Empty);
                message = message.Replace("[attachimg]" + attInfo.Aid.ToString() + "[/attachimg]", string.Empty);
            }
            else if (attInfo.Goodsid == goodsPramsInfo.Goodsid)
            {
                ;
            }
            return message;
        }

        /// <summary>
        /// 更新指定商品数据信息
        /// </summary>
        /// <param name="goodsinfo">商品信息</param>
        /// <returns></returns>
        public static void UpdateGoods(Goodsinfo goodsInfo)
        {
            DbProvider.GetInstance().UpdateGoods(goodsInfo); 
        }

        /// <summary>
        /// 更新指定商品数据信息
        /// </summary>
        /// <param name="goodsinfo">商品信息</param>
        /// <param name="oldgoodscategoryid">商品分类原值</param>
        /// <param name="oldparentcategorylist">商品父分类原值</param>
        public static void UpdateGoods(Goodsinfo goodsInfo, int oldGoodsCategoryId, string oldParentCategoryList)
        {
            if (goodsInfo.Categoryid != oldGoodsCategoryId && goodsInfo.Categoryid >0)
            {
                DbProvider.GetInstance().UpdateCategoryGoodsCounts(goodsInfo.Categoryid, goodsInfo.Parentcategorylist, 1);
                DbProvider.GetInstance().UpdateCategoryGoodsCounts(oldGoodsCategoryId, oldParentCategoryList, -1);
            }

            DbProvider.GetInstance().UpdateGoods(goodsInfo);
        }

        /// <summary>
        /// 创建商品数据信息
        /// </summary>
        /// <param name="goodsinfo">商品信息</param>
        /// <returns>创建商品的id</returns>
        public static int CreateGoods(Goodsinfo goodsInfo)
        {
            int goodsid = DbProvider.GetInstance().CreateGoods(goodsInfo);

            //当成功创建商品信息且可在前台正常显示时
            if (goodsid > 0 && goodsInfo.Displayorder>=0) 
                DbProvider.GetInstance().UpdateCategoryGoodsCounts(goodsInfo.Categoryid, goodsInfo.Parentcategorylist, 1);

            return goodsid;
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="goodsid">商品Id</param>
        public static Goodsinfo GetGoodsInfo(int goodsId)
        {
            return DTO.GetGoodsInfo(DbProvider.GetInstance().GetGoodsInfo(goodsId)); 
        }

        /// <summary>
        /// 输出htmltitle
        /// </summary>
        /// <param name="htmltitle">html标题</param>
        /// <param name="goodsid">商品id</param>
        public static void WriteHtmlSubjectFile(string htmlTitle, int goodsId)
        {
            StringBuilder dir = new StringBuilder();
            dir.Append(BaseConfigs.GetForumPath);
            dir.Append("cache/goods/magic/");

            if (!Directory.Exists(Utils.GetMapPath(dir.ToString())))
                Utils.CreateDir(Utils.GetMapPath(dir.ToString()));

            dir.Append((goodsId / 1000 + 1).ToString());
            dir.Append("/");

            if (!Directory.Exists(Utils.GetMapPath(dir.ToString())))
                Utils.CreateDir(Utils.GetMapPath(dir.ToString()));

            string filename = Utils.GetMapPath(dir.ToString() + goodsId + "_htmltitle.config");
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Byte[] info = System.Text.Encoding.UTF8.GetBytes(Utils.RemoveUnsafeHtml(htmlTitle));
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
            }
            catch{}
        }

        /// <summary>
        /// 获取指定商品Id的商品列表
        /// </summary>
        /// <param name="goodsidlist">商品id列表</param>
        /// <returns>商品列表</returns>
        public static DataTable GetGoodsList(string goodsIdList)
        {
            return DbProvider.GetInstance().GetGoodsList(goodsIdList);
        }


        /// <summary>
        /// 指定用户id是否是商品id列表的卖家
        /// </summary>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="userid">用户id</param>
        /// <returns>是否为卖家</returns>
        public static bool IsSeller(string goodsidlist, int userid)
        {
            bool isseller = true;
            foreach (DataRow dr in GetGoodsList(goodsidlist).Rows)
            {
                if (dr["selleruid"].ToString() != userid.ToString())
                    isseller = false;
            }
            return isseller;
        }


        /// <summary>
        /// 获取推荐商品列表
        /// </summary>
        /// <param name="selleruid">卖家id</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="condition">查询条件</param>
        /// <returns>推荐商品列表</returns>
        public static GoodsinfoCollection GetGoodsRecommendList(int sellerUid, int pageSize, int pageIndex, string condition)
        {
            condition += DbProvider.GetInstance().GetGoodsRecommendCondition((int)MallUtils.OperaCode.MorethanOrEqual, 1);
            condition += DbProvider.GetInstance().GetGoodsCloseCondition((int)MallUtils.OperaCode.Equal, 0);
            condition += DbProvider.GetInstance().GetGoodsExpirationCondition((int)MallUtils.OperaCode.LessthanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsDateLineCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsRemainCondition((int)MallUtils.OperaCode.Morethan, 0);
            condition += DbProvider.GetInstance().GetGoodsDisplayCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);

            return DTO.GetGoodsInfoList(DbProvider.GetInstance().GetGoodsListBySellerUID(sellerUid, pageSize, pageIndex, condition, "displayorder", 0));
        }

        /// <summary>
        /// 获取推荐商品列表
        /// </summary>
        /// <param name="selleruid">卖家id</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="pageindex">当前页面</param>
        /// <param name="condition">查询条件</param>
        /// <returns>推荐商品列表</returns>
        public static GoodsinfoCollection GetGoodsRecommendManageList(int sellerUid, int pageSize, int pageIndex, string condition)
        {
            condition += DbProvider.GetInstance().GetGoodsRecommendCondition((int)MallUtils.OperaCode.MorethanOrEqual, 1);
            return DTO.GetGoodsInfoList(DbProvider.GetInstance().GetGoodsListBySellerUID(sellerUid, pageSize, pageIndex, condition, "displayorder", 0));
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
        /// <returns>商品列表</returns>
        public static GoodsinfoCollection GetGoodsInfoList(int categoryId, int pageSize, int pageIndex, string condition, string orderBy, int ascDesc)
        {
            GoodsinfoCollection coll = new GoodsinfoCollection();

            if (pageIndex <= 0)
                return coll;

            condition += DbProvider.GetInstance().GetGoodsDisplayCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsCloseCondition((int)MallUtils.OperaCode.Equal, 0);
            condition += DbProvider.GetInstance().GetGoodsExpirationCondition((int)MallUtils.OperaCode.LessthanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsDateLineCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsRemainCondition((int)MallUtils.OperaCode.Morethan, 0);

            return DTO.GetGoodsInfoList(DbProvider.GetInstance().GetGoodsList(categoryId, pageSize, pageIndex, condition, orderBy, ascDesc));
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
        /// <returns>商品列表</returns>
        public static GoodsinfoCollection GetGoodsInfoListByShopCategory(int shopCategoryId, int pageSize, int pageIndex, string condition, string orderBy, int ascDesc)
        {
            GoodsinfoCollection coll = new GoodsinfoCollection();

            if (pageIndex <= 0)
                return coll;

            return DTO.GetGoodsInfoList(DbProvider.GetInstance().GetGoodsInfoListByShopCategory(shopCategoryId, pageSize, pageIndex, condition, orderBy, ascDesc));
        }

        /// <summary>
        /// 获取指定条件的商品信息列表
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式</param>
        /// <returns>商品信息列表</returns>
        public static GoodsinfoCollection GetGoodsInfoList(int pageSize, int pageIndex, string condition, string orderBy, int ascDesc)
        {
            GoodsinfoCollection coll = new GoodsinfoCollection();

            if (pageIndex <= 0)
                return coll;

            condition += DbProvider.GetInstance().GetGoodsDisplayCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsCloseCondition((int)MallUtils.OperaCode.Equal, 0);
            condition += DbProvider.GetInstance().GetGoodsExpirationCondition((int)MallUtils.OperaCode.LessthanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsDateLineCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsRemainCondition((int)MallUtils.OperaCode.Morethan, 0);

            return DTO.GetGoodsInfoList(DbProvider.GetInstance().GetGoodsList(pageSize, pageIndex, condition, orderBy, ascDesc));

        }

        /// <summary>
        /// 获取指定条件的商品数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>商品数</returns>
        public static int GetGoodsCount(string condition)
        {
            condition += DbProvider.GetInstance().GetGoodsDisplayCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsCloseCondition((int)MallUtils.OperaCode.Equal, 0);
            condition += DbProvider.GetInstance().GetGoodsExpirationCondition((int)MallUtils.OperaCode.LessthanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsDateLineCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsRemainCondition((int)MallUtils.OperaCode.Morethan, 0);

            return DbProvider.GetInstance().GetGoodsCount(condition);
        }

        /// <summary>
        /// 获取指定店铺商品分类id的商品数
        /// </summary>
        /// <param name="shopcategoryid">店铺商品分类id</param>
        /// <param name="condition">查询条件</param>
        /// <returns>指定店铺商品分类id的商品数</returns>
        public static int GetGoodsCountByShopCategory(int shopCategoryId, string condition)
        {
            return DbProvider.GetInstance().GetGoodsCountByShopCategory(shopCategoryId, condition);
        }

        /// <summary>
        /// 获取指定分类和条件下的商品列表集合
        /// </summary>
        /// <param name="selleruid">卖家uid</param>
        /// <param name="allgoods">是否全部商品</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="ascdesc">排序方式(0:升序, 1:降序)</param>
        /// <returns>商品列表集合</returns>
        public static GoodsinfoCollection GetGoodsListBySellerUID(int sellerUid, bool allGoods, int pageSize, int pageIndex, string orderBy, int ascDesc)
        {
            GoodsinfoCollection coll = new GoodsinfoCollection();

            if (pageIndex <= 0)
                return coll;

            string condition = "";
            if (!allGoods)
            {
                condition += DbProvider.GetInstance().GetGoodsDateLineCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
                condition += DbProvider.GetInstance().GetGoodsDisplayCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            }

            return DTO.GetGoodsInfoList(DbProvider.GetInstance().GetGoodsListBySellerUID(sellerUid, pageSize, pageIndex, condition, orderBy, ascDesc));
        }

        /// <summary>
        /// 获取指定用户id的商品数
        /// </summary>
        /// <param name="selleruid">用户id</param>
        /// <param name="allgoods">是否全部商品</param>
        /// <returns>商品数</returns>
        public static int GetGoodsCountBySellerUid(int sellerUid, bool allGoods)
        {
            string condition = "";
            if (!allGoods)
            {
                condition += DbProvider.GetInstance().GetGoodsDateLineCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
                condition += DbProvider.GetInstance().GetGoodsDisplayCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            }
            return DbProvider.GetInstance().GetGoodsCountBySellerUid(sellerUid, condition);
        }

        /// <summary>
        /// 获取指定分类和条件下的商品数
        /// </summary>
        /// <param name="categoryid">分类id</param>
        /// <param name="condition">查询条件</param>
        /// <returns>商品数</returns>
        public static int GetGoodsCount(int categoryId, string condition)
        {
            condition += DbProvider.GetInstance().GetGoodsDisplayCondition((int) MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsCloseCondition((int) MallUtils.OperaCode.Equal, 0);
            condition += DbProvider.GetInstance().GetGoodsExpirationCondition((int)MallUtils.OperaCode.LessthanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsDateLineCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsRemainCondition((int)MallUtils.OperaCode.Morethan, 0);
            condition += DbProvider.GetInstance().GetGoodsDisplayCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            
            return DbProvider.GetInstance().GetGoodsCount(categoryId, condition);
        }

        /// <summary>
        /// 判断当前goodsidlist是否都在当前分类categoryid下
        /// </summary>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="categoryid">分类id</param>
        /// <returns>是否都在指定分类下</returns>
        public static bool InSameCategory(string goodsIdList, int categoryId)
        {
            return DbProvider.GetInstance().InSameCategory(goodsIdList, categoryId);
        }

        /// <summary>
        /// 将商品高亮显示
        /// </summary>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="intValue">高亮样式及颜色( 0 为解除高亮显示)</param>
        /// <returns>更新商品个数</returns>
        public static int SetHighlight(string goodsIdList, string intValue)
        {
            return SetGoodsStatus(goodsIdList, "highlight", intValue);
        }

        /// <summary>
        /// 设置商品状态属性
        /// </summary>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="field">字段</param>
        /// <param name="intValue">整数值</param>
        /// <returns>执行设置的返回值</returns>
        private static int SetGoodsStatus(string goodsIdList, string field, string intValue)
        {
            if (!Utils.InArray(field.ToLower().Trim(), "displayorder,highlight"))
                return -1;

            if (!Utils.IsNumericList(goodsIdList))
                return -1;

            return DbProvider.GetInstance().SetGoodsStatus(goodsIdList, field, intValue);
        }

        /// <summary>
        /// 设置商品属性
        /// </summary>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="field">要更新的属性</param>
        /// <param name="intValue">要更新的值</param>
        /// <returns>执行设置的返回值</returns>
        private static int SetGoodsStatus(string goodsIdList, string field, int intValue)
        {
            if (!Utils.InArray(field.ToLower().Trim(), "displayorder,highlight"))
                return -1;

            if (!Utils.IsNumericList(goodsIdList))
                return -1;

            foreach (DataRow dr in Goods.GetGoodsList(goodsIdList).Rows)
            {
                DbProvider.GetInstance().UpdateCategoryGoodsCounts(TypeConverter.ObjectToInt(dr["categoryid"]), dr["parentcategorylist"].ToString().Trim(), -1);
            }

            return DbProvider.GetInstance().SetGoodsStatus(goodsIdList, field, intValue);
        }

        /// <summary>
        /// 获取Html标题
        /// </summary>
        /// <param name="goodsid">商品id</param>
        /// <returns>Html标题</returns>
        public static string GetHtmlTitle(int goodsId)
        {
            StringBuilder dir = new StringBuilder();
            dir.Append(BaseConfigs.GetForumPath);
            dir.Append("cache/goods/magic/");
            dir.Append((goodsId / 1000 + 1));
            dir.Append("/");
            string filename = Utils.GetMapPath(dir.ToString() + goodsId + "_htmltitle.config");
            if (!File.Exists(filename))
                return "";

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }


        /// <summary>
        /// 将商品设置关闭/打开
        /// </summary>
        /// <param name="goodsidlist">要设置的商品列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新商品个数</returns>
        public static int SetClose(string goodsIdList, short intValue)
        {
            if (!Utils.IsNumericList(goodsIdList))
                return -1;

            int result = DbProvider.GetInstance().SetGoodsClose(goodsIdList, intValue);

            //更新该商品分类的商品数
            foreach (DataRow dr in Goods.GetGoodsList(goodsIdList).Rows)
            {
                DbProvider.GetInstance().UpdateCategoryGoodsCounts(TypeConverter.ObjectToInt(dr["categoryid"].ToString()), dr["parentcategorylist"].ToString().Trim(), intValue == 0 ? 1 : - 1);
            }
            return result;
        }

   
        /// <summary>
        /// 在数据库中删除指定商品
        /// </summary>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="subtractCredits">是否减少用户积分(0不减少,1减少)</param>
        /// <param name="reserveAttach">是否保留附件</param>
        /// <returns>删除个数</returns>
        public static int DeleteGoods(string goodsIdList, int subtractCredits, bool reserveAttach)
        {
            if (!Utils.IsNumericList(goodsIdList))
                return -1;

            if (!reserveAttach)
            {
                IDataReader reader = DbProvider.GetInstance().GetGoodsAttachmentList(goodsIdList);

                while (reader.Read())
                {
                    if (reader["filename"].ToString().Trim().ToLower().IndexOf("http") < 0)
                    {
                        if ((Utils.FileExists(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/mall/" + reader["filename"].ToString()))))
                        {
                            File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/mall/" + reader["filename"].ToString()));
                        }
                    }
                }
                reader.Close();
                DbProvider.GetInstance().DeleteGoodsAttachments(goodsIdList);
            }

            foreach (DataRow dr in Goods.GetGoodsList(goodsIdList).Rows)
            {
                DbProvider.GetInstance().UpdateCategoryGoodsCounts(TypeConverter.ObjectToInt(dr["categoryid"].ToString()), dr["parentcategorylist"].ToString().Trim(), -1);
            }
            return DbProvider.GetInstance().DeleteGoods(goodsIdList);
        }

       


        /// <summary>
        /// 在数据库中删除指定商品
        /// </summary>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="reserveAttach">是否保留附件</param>
        /// <returns>删除个数</returns>
        public static int DeleteGoods(string goodsIdList, bool reserveAttach)
        {
            return DeleteGoods(goodsIdList, (int)1, reserveAttach);
        }

        /// <summary>
        /// 在删除指定的商品
        /// </summary>
        /// <param name="goodsidlist">商品id列表</param>
        /// <param name="toDustbin">指定商品删除形式(0：直接从数据库中删除,并删除与之关联的信息  1：只将其从论坛列表中删除(将displayorder字段置为-1)将其放入回收站中</param>
        /// <param name="reserveAttach">是否保留附件</param>
        /// <returns>删除个数</returns>
        public static int DeleteGoods(string goodsIdList, byte toDustbin, bool reserveAttach)
        {
            if(toDustbin == 0)
                return DeleteGoods(goodsIdList, reserveAttach);
            else
                return SetGoodsStatus(goodsIdList, "displayorder", -1);
        }


        /// <summary>
        /// 获取指定商品标签id的商品信息集合
        /// </summary>
        /// <param name="tagid">tagid</param>
        /// <param name="pageid">页面id</param>
        /// <param name="pagesize">页面尺寸</param>
        /// <returns>商品信息集合</returns>
        public static GoodsinfoCollection GetGoodsWithSameTag(int tagId, int pageId, int pageSize)
        {
            return DTO.GetGoodsInfoList(DbProvider.GetInstance().GetGoodsWithSameTag(tagId, pageId, pageSize));
        }

        /// <summary>
        /// 更新指定商品的店铺商品分类字段
        /// </summary>
        /// <param name="goodsidlist">指定商品id串(格式:1,2,3)</param>
        /// <param name="shopgoodscategoryid">要绑定的店铺商品分类id</param>
        /// <returns>执行结果</returns>
        public static int MoveGoodsShopCategory(string goodsIdList, int shopGoodsCategoryId)
        {
            if (!Utils.IsNumericList(goodsIdList))
                return -1;

            return DbProvider.GetInstance().MoveGoodsShopCategory(goodsIdList, shopGoodsCategoryId);
        }

        /// <summary>
        /// 移除指定商品的店铺商品分类
        /// </summary>
        /// <param name="removegoodsid">指定的商品id</param>
        /// <param name="removeshopgoodscategoryid">要移除的店铺商品分类id</param>
        /// <returns>被移除商品分类数</returns>
        public static int RemoveGoodsShopCategory(int removeGoodsId, int removeShopGoodsCategoryId)
        {
            return DbProvider.GetInstance().RemoveGoodsShopCategory(removeGoodsId, removeShopGoodsCategoryId);
        }

        /// <summary>
        /// 推荐商品
        /// </summary>
        /// <param name="goodsidlist">指定的商品id串(格式:1,2,3)</param>
        /// <returns>设置推荐商品数</returns>
        public static int RecommendGoods(string goodsIdList)
        {
            if (!Utils.IsNumericList(goodsIdList))
                return -1;

            return DbProvider.GetInstance().RecommendGoods(goodsIdList, 1);
        }

        /// <summary>
        /// 取消推荐商品
        /// </summary>
        /// <param name="goodsidlist">指定的商品id串(格式:1,2,3)</param>
        /// <returns>取消推荐商品数</returns>
        public static int CancelRecommendGoods(string goodsIdList)
        {
            return DbProvider.GetInstance().RecommendGoods(goodsIdList, 0);
        }

        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="datetype">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        /// <returns>返回Json数据</returns>
        public static string GetHotGoodsJsonData(int days, int categroyId, int count)
        {
            StringBuilder sb_goods = new StringBuilder();
            sb_goods.Append("[");

            string condition = DbProvider.GetInstance().GetGoodsCloseCondition((int)MallUtils.OperaCode.Equal, 0);
            condition += DbProvider.GetInstance().GetGoodsExpirationCondition((int)MallUtils.OperaCode.LessthanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsDateLineCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            condition += DbProvider.GetInstance().GetGoodsRemainCondition((int)MallUtils.OperaCode.Morethan, 0);
            condition += DbProvider.GetInstance().GetGoodsDisplayCondition((int)MallUtils.OperaCode.MorethanOrEqual, 0);
            IDataReader idatareader = DbProvider.GetInstance().GetHotGoods(days, categroyId, count, condition);

            while (idatareader.Read())
            {
                sb_goods.Append(string.Format("{{'goodsid' : {0}, 'title' : '{1}', 'goodspic' : '{2}', 'seller' : '{3}', 'selleruid' : {4}, 'price' :{5}}},",
                    idatareader["goodsid"],
                    idatareader["title"].ToString().Trim(),
                    idatareader["goodspic"].ToString().Trim(),
                    idatareader["seller"].ToString().Trim(),
                    idatareader["selleruid"].ToString().Trim(),
                    idatareader["price"]
                    ));
            }
            idatareader.Close();

            if (sb_goods.ToString().EndsWith(","))
                sb_goods.Remove(sb_goods.Length - 1, 1);

            sb_goods.Append("]");
            return sb_goods.ToString();
        }

        /// <summary>
        ///  获取热门商品信息
        /// </summary>
        /// <param name="datetype">天数</param>
        /// <param name="categroyid">商品分类</param>
        /// <param name="count">返回记录条数</param>
        /// <returns>返回Json数据</returns>
        public static string GetGoodsListJsonData(int categroyId, int order, int topNumber)
        {
            StringBuilder sb_goods = new StringBuilder();
            sb_goods.Append("[");

            GoodsinfoCollection goodsinfocoll = GetGoodsInfoList(categroyId, topNumber, 1, "", order == 0 ? "goodsid" : "viewcount", 1);
            foreach (Goodsinfo goodsinfo in goodsinfocoll)
            {
                sb_goods.Append(string.Format("{{'goodsid' : {0}, 'title' : '{1}', 'goodspic' : '{2}', 'seller' : '{3}', 'selleruid' : {4}, 'price' :{5}, 'costprice' :{6}}},",
                    goodsinfo.Goodsid,
                    goodsinfo.Title,
                    goodsinfo.Goodspic,
                    goodsinfo.Seller,
                    goodsinfo.Selleruid,
                    goodsinfo.Price,
                    goodsinfo.Costprice
                    ));
            }
            if (sb_goods.ToString().EndsWith(","))
                sb_goods.Remove(sb_goods.Length - 1, 1);

            sb_goods.Append("]");
            return sb_goods.ToString();
        }
       

        /// <summary>
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {           
            /// <summary>
            /// 设置魔法主题
            /// </summary>
            /// <param name="goodsinfo">要设置的商品信息</param>
            /// <returns></returns>
            public static string SetMagicTitle(Goodsinfo goodsInfo)
            {
                if (Topics.GetMagicValue(goodsInfo.Magic, MagicType.HtmlTitle) == 1)
                    return Goods.GetHtmlTitle(goodsInfo.Goodsid);

                if (goodsInfo.Highlight != "")
                    return "<span style=\"" + goodsInfo.Highlight + "\">" + goodsInfo.Title + "</span>";
                else
                    return goodsInfo.Title;
            }
          
            /// <summary>
            /// 获得商品信息(DTO)
            /// </summary>
            /// <param name="idatareader">要转换的数据</param>
            /// <returns>返回商品信息</returns>
            public static GoodsinfoCollection GetGoodsInfoList(IDataReader reader)
            {
                GoodsinfoCollection goodsinfocoll = new GoodsinfoCollection();
                //StringBuilder tablefield = new StringBuilder().Capacity(2000);
                //tablefield.Append(",");
                //foreach (DataRow dr in __idatareader.GetSchemaTable().Rows)
                //{
                //    tablefield.Append(dr["ColumnName"].ToString().ToLower() + ",");
                //}

                while (reader.Read())
                {
                    Goodsinfo goodsinfo = new Goodsinfo();
                    //if (tablefield.ToString().IndexOf(",goodsid,")>=0)
                    //{
                    goodsinfo.Goodsid = TypeConverter.ObjectToInt(reader["goodsid"]);
                    //}
                    goodsinfo.Shopid = TypeConverter.ObjectToInt(reader["shopid"]);
                    goodsinfo.Parentcategorylist = reader["parentcategorylist"].ToString();
                    goodsinfo.Shopcategorylist = reader["shopcategorylist"].ToString();
                    goodsinfo.Categoryid = TypeConverter.ObjectToInt(reader["categoryid"]);
                    goodsinfo.Recommend = TypeConverter.ObjectToInt(reader["recommend"]);
                    goodsinfo.Discount = TypeConverter.ObjectToInt(reader["discount"]);
                    goodsinfo.Selleruid = TypeConverter.ObjectToInt(reader["selleruid"]);
                    goodsinfo.Seller = reader["seller"].ToString();
                    goodsinfo.Account = reader["account"].ToString();
                    goodsinfo.Magic = TypeConverter.ObjectToInt(reader["magic"]);
                    goodsinfo.Price = Convert.ToDecimal(reader["price"]);
                    goodsinfo.Amount = TypeConverter.ObjectToInt(reader["amount"]);
                    goodsinfo.Quality = TypeConverter.ObjectToInt(reader["quality"]);
                    goodsinfo.Lid = TypeConverter.ObjectToInt(reader["lid"]);
                    goodsinfo.Locus = reader["locus"].ToString();
                    goodsinfo.Transport = TypeConverter.ObjectToInt(reader["transport"]);
                    goodsinfo.Ordinaryfee = Convert.ToDecimal(reader["ordinaryfee"].ToString());
                    goodsinfo.Expressfee = Convert.ToDecimal(reader["expressfee"].ToString());
                    goodsinfo.Emsfee = Convert.ToDecimal(reader["emsfee"].ToString());
                    goodsinfo.Itemtype = TypeConverter.ObjectToInt(reader["itemtype"]);
                    goodsinfo.Dateline = Convert.ToDateTime(reader["dateline"].ToString());
                    goodsinfo.Expiration = Convert.ToDateTime(reader["expiration"].ToString());
                    goodsinfo.Lastbuyer = reader["lastbuyer"].ToString();
                    goodsinfo.Lasttrade = Convert.ToDateTime(reader["lasttrade"].ToString());
                    goodsinfo.Lastupdate = Convert.ToDateTime(reader["lastupdate"].ToString());
                    goodsinfo.Totalitems = TypeConverter.ObjectToInt(reader["totalitems"]);
                    goodsinfo.Tradesum = Convert.ToDecimal(reader["tradesum"].ToString());
                    goodsinfo.Closed = TypeConverter.ObjectToInt(reader["closed"]);
                    goodsinfo.Aid = TypeConverter.ObjectToInt(reader["aid"]);
                    goodsinfo.Goodspic = reader["goodspic"].ToString().Trim();
                    goodsinfo.Displayorder = TypeConverter.ObjectToInt(reader["displayorder"]);
                    goodsinfo.Costprice = Convert.ToDecimal(reader["costprice"].ToString());
                    goodsinfo.Invoice = TypeConverter.ObjectToInt(reader["invoice"]);
                    goodsinfo.Repair = TypeConverter.ObjectToInt(reader["repair"]);
                    goodsinfo.Message = reader["message"].ToString();
                    goodsinfo.Otherlink = reader["otherlink"].ToString();
                    goodsinfo.Readperm = TypeConverter.ObjectToInt(reader["readperm"]);
                    goodsinfo.Tradetype = TypeConverter.ObjectToInt(reader["tradetype"]);
                    goodsinfo.Viewcount = TypeConverter.ObjectToInt(reader["viewcount"]);
                    goodsinfo.Smileyoff = TypeConverter.ObjectToInt(reader["smileyoff"]);
                    goodsinfo.Bbcodeoff = TypeConverter.ObjectToInt(reader["bbcodeoff"]);
                    goodsinfo.Parseurloff = TypeConverter.ObjectToInt(reader["parseurloff"]);
                    goodsinfo.Highlight = reader["highlight"].ToString().Trim();
                    goodsinfo.Title = reader["title"].ToString().Trim();
                    goodsinfo.Htmltitle = SetMagicTitle(goodsinfo);

                    goodsinfocoll.Add(goodsinfo);
                }
                reader.Close();

                return goodsinfocoll;
            }



            /// <summary>
            /// 获得商品信息(DTO)
            /// </summary>
            /// <param name="idatareader">要转换的数据</param>
            /// <returns>返回商品信息</returns>
            public static Goodsinfo GetGoodsInfo(IDataReader reader)
            {
                if (reader.Read())
                {
                    Goodsinfo goodsinfo = new Goodsinfo();
                    goodsinfo.Goodsid = TypeConverter.ObjectToInt(reader["goodsid"]);
                    goodsinfo.Shopid = TypeConverter.ObjectToInt(reader["shopid"]);
                    goodsinfo.Parentcategorylist = reader["parentcategorylist"].ToString();
                    goodsinfo.Shopcategorylist = reader["shopcategorylist"].ToString();
                    goodsinfo.Categoryid = TypeConverter.ObjectToInt(reader["categoryid"]);
                    goodsinfo.Recommend = Convert.ToInt16(reader["recommend"]);
                    goodsinfo.Discount = Convert.ToInt16(reader["discount"]);
                    goodsinfo.Selleruid = TypeConverter.ObjectToInt(reader["selleruid"]);
                    goodsinfo.Seller = reader["seller"].ToString().Trim();
                    goodsinfo.Account = reader["account"].ToString().Trim();
                    goodsinfo.Magic = TypeConverter.ObjectToInt(reader["magic"]);
                    goodsinfo.Price = Convert.ToDecimal(reader["price"].ToString());
                    goodsinfo.Amount = Convert.ToInt16(reader["amount"].ToString());
                    goodsinfo.Quality = Convert.ToInt16(reader["quality"].ToString());
                    goodsinfo.Lid = TypeConverter.ObjectToInt(reader["lid"]);
                    goodsinfo.Locus = reader["locus"].ToString().Trim();
                    goodsinfo.Transport = Convert.ToInt16(reader["transport"].ToString());
                    goodsinfo.Ordinaryfee = Convert.ToDecimal(reader["ordinaryfee"].ToString());
                    goodsinfo.Expressfee = Convert.ToDecimal(reader["expressfee"].ToString());
                    goodsinfo.Emsfee = Convert.ToDecimal(reader["emsfee"].ToString());
                    goodsinfo.Itemtype = Convert.ToInt16(reader["itemtype"].ToString());
                    goodsinfo.Dateline = Convert.ToDateTime(reader["dateline"].ToString());
                    goodsinfo.Expiration = Convert.ToDateTime(reader["expiration"].ToString());
                    goodsinfo.Lastbuyer = reader["lastbuyer"].ToString().Trim();
                    goodsinfo.Lasttrade = Convert.ToDateTime(reader["lasttrade"].ToString());
                    goodsinfo.Lastupdate = Convert.ToDateTime(reader["lastupdate"].ToString());
                    goodsinfo.Totalitems = Convert.ToInt16(reader["totalitems"].ToString());
                    goodsinfo.Tradesum = Convert.ToDecimal(reader["tradesum"].ToString());
                    goodsinfo.Closed = Convert.ToInt16(reader["closed"].ToString());
                    goodsinfo.Aid = TypeConverter.ObjectToInt(reader["aid"]);
                    goodsinfo.Goodspic = reader["goodspic"].ToString().Trim();
                    goodsinfo.Displayorder = Convert.ToInt16(reader["displayorder"].ToString());
                    goodsinfo.Costprice = Convert.ToDecimal(reader["costprice"].ToString());
                    goodsinfo.Invoice = Convert.ToInt16(reader["invoice"].ToString());
                    goodsinfo.Repair = Convert.ToInt16(reader["repair"].ToString());
                    goodsinfo.Message = reader["message"].ToString().Trim();
                    goodsinfo.Otherlink = reader["otherlink"].ToString().Trim();
                    goodsinfo.Readperm = TypeConverter.ObjectToInt(reader["readperm"]);
                    goodsinfo.Tradetype = Convert.ToInt16(reader["tradetype"].ToString());
                    goodsinfo.Viewcount = TypeConverter.ObjectToInt(reader["viewcount"]);
                    goodsinfo.Smileyoff = TypeConverter.ObjectToInt(reader["smileyoff"]);
                    goodsinfo.Bbcodeoff = TypeConverter.ObjectToInt(reader["bbcodeoff"]);
                    goodsinfo.Parseurloff = TypeConverter.ObjectToInt(reader["parseurloff"]);
                    goodsinfo.Highlight = reader["highlight"].ToString().Trim();
                    goodsinfo.Title = reader["title"].ToString().Trim();
                    goodsinfo.Htmltitle = SetMagicTitle(goodsinfo);

                    reader.Close();
                    return goodsinfo;
                }
                return null;
            }


            /// <summary>
            /// 获得商品信息(DTO)
            /// </summary>
            /// <param name="dt">要转换的数据表</param>
            /// <returns>返回商品信息</returns>
            public static Goodsinfo[] GetGoodsInfoArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Goodsinfo[] goodsinfoarray = new Goodsinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    goodsinfoarray[i] = new Goodsinfo();
                    goodsinfoarray[i].Goodsid = TypeConverter.ObjectToInt(dt.Rows[i]["goodsid"]);
                    goodsinfoarray[i].Shopid = TypeConverter.ObjectToInt(dt.Rows[i]["shopid"]);
                    goodsinfoarray[i].Parentcategorylist = dt.Rows[i]["parentcategorylist"].ToString();
                    goodsinfoarray[i].Shopcategorylist = dt.Rows[i]["shopcategorylist"].ToString();
                    goodsinfoarray[i].Categoryid = TypeConverter.ObjectToInt(dt.Rows[i]["categoryid"]);
                    goodsinfoarray[i].Recommend = TypeConverter.ObjectToInt(dt.Rows[i]["recommend"]);
                    goodsinfoarray[i].Discount = TypeConverter.ObjectToInt(dt.Rows[i]["discount"]);
                    goodsinfoarray[i].Selleruid = TypeConverter.ObjectToInt(dt.Rows[i]["selleruid"]);
                    goodsinfoarray[i].Seller = dt.Rows[i]["seller"].ToString();
                    goodsinfoarray[i].Account = dt.Rows[i]["account"].ToString();
                    goodsinfoarray[i].Magic = TypeConverter.ObjectToInt(dt.Rows[i]["magic"]);
                    goodsinfoarray[i].Price = Convert.ToDecimal(dt.Rows[i]["price"].ToString());
                    goodsinfoarray[i].Amount = TypeConverter.ObjectToInt(dt.Rows[i]["amount"]);
                    goodsinfoarray[i].Quality = TypeConverter.ObjectToInt(dt.Rows[i]["quality"]);
                    goodsinfoarray[i].Lid = TypeConverter.ObjectToInt(dt.Rows[i]["lid"]);
                    goodsinfoarray[i].Locus = dt.Rows[i]["locus"].ToString();
                    goodsinfoarray[i].Transport = TypeConverter.ObjectToInt(dt.Rows[i]["transport"]);
                    goodsinfoarray[i].Ordinaryfee = Convert.ToDecimal(dt.Rows[i]["ordinaryfee"].ToString());
                    goodsinfoarray[i].Expressfee = Convert.ToDecimal(dt.Rows[i]["expressfee"].ToString());
                    goodsinfoarray[i].Emsfee = Convert.ToDecimal(dt.Rows[i]["emsfee"].ToString());
                    goodsinfoarray[i].Itemtype = TypeConverter.ObjectToInt(dt.Rows[i]["itemtype"]);
                    goodsinfoarray[i].Dateline = Convert.ToDateTime(dt.Rows[i]["dateline"].ToString());
                    goodsinfoarray[i].Expiration = Convert.ToDateTime(dt.Rows[i]["expiration"].ToString());
                    goodsinfoarray[i].Lastbuyer = dt.Rows[i]["lastbuyer"].ToString();
                    goodsinfoarray[i].Lasttrade = Convert.ToDateTime(dt.Rows[i]["lasttrade"].ToString());
                    goodsinfoarray[i].Lastupdate = Convert.ToDateTime(dt.Rows[i]["lastupdate"].ToString());
                    goodsinfoarray[i].Totalitems = TypeConverter.ObjectToInt(dt.Rows[i]["totalitems"]);
                    goodsinfoarray[i].Tradesum = Convert.ToDecimal(dt.Rows[i]["tradesum"].ToString());
                    goodsinfoarray[i].Closed = TypeConverter.ObjectToInt(dt.Rows[i]["closed"]);
                    goodsinfoarray[i].Aid = TypeConverter.ObjectToInt(dt.Rows[i]["aid"]);
                    goodsinfoarray[i].Goodspic = dt.Rows[i]["goodspic"].ToString();
                    goodsinfoarray[i].Displayorder = TypeConverter.ObjectToInt(dt.Rows[i]["displayorder"]);
                    goodsinfoarray[i].Costprice = Convert.ToDecimal(dt.Rows[i]["costprice"].ToString());
                    goodsinfoarray[i].Invoice = TypeConverter.ObjectToInt(dt.Rows[i]["invoice"]);
                    goodsinfoarray[i].Repair = TypeConverter.ObjectToInt(dt.Rows[i]["repair"]);
                    goodsinfoarray[i].Message = dt.Rows[i]["message"].ToString();
                    goodsinfoarray[i].Otherlink = dt.Rows[i]["otherlink"].ToString();
                    goodsinfoarray[i].Readperm = TypeConverter.ObjectToInt(dt.Rows[i]["readperm"]);
                    goodsinfoarray[i].Tradetype = TypeConverter.ObjectToInt(dt.Rows[i]["tradetype"]);
                    goodsinfoarray[i].Viewcount = TypeConverter.ObjectToInt(dt.Rows[i]["viewcount"]);
                    goodsinfoarray[i].Smileyoff = TypeConverter.ObjectToInt(dt.Rows[i]["smileyoff"]);
                    goodsinfoarray[i].Bbcodeoff = TypeConverter.ObjectToInt(dt.Rows[i]["bbcodeoff"]);
                    goodsinfoarray[i].Parseurloff = TypeConverter.ObjectToInt(dt.Rows[i]["parseurloff"]);
                    goodsinfoarray[i].Highlight = dt.Rows[i]["highlight"].ToString().Trim();
                    goodsinfoarray[i].Title = dt.Rows[i]["title"].ToString();
                    goodsinfoarray[i].Htmltitle = SetMagicTitle(goodsinfoarray[i]);

                }
                dt.Dispose();
                return goodsinfoarray;
            }
        }
    }
}
