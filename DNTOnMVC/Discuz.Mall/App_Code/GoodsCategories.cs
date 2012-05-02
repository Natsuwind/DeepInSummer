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
    /// 商品分类管理操作类
    /// </summary>
    public class GoodsCategories : WriteFile
    {
        #region 私有变量
        private static volatile GoodsCategories instance = null;
        private static object lockHelper = new object();
        private static string jsonPath = "";
        #endregion

        #region 返回唯一实例
        private GoodsCategories()
        {
            jsonPath = Utils.GetMapPath(BaseConfigs.GetForumPath + "\\javascript\\goodscategories.js");
        }

        public static GoodsCategories GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new GoodsCategories();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 更新指定商品数据信息
        /// </summary>
        /// <param name="goodsinfo">商品信息</param>
        /// <returns></returns>
        public static void UpdateGoodsCategory(Goodscategoryinfo goodsCategoryInfo)
        {
            DbProvider.GetInstance().UpdateGoodscategory(goodsCategoryInfo);
        }

        /// <summary>
        /// 删除指定商品分类
        /// </summary>
        /// <param name="categoryid">分类ID</param>
        public static void DeleteGoodsCategory(int categoryId)
        {
            DbProvider.GetInstance().DeleteGoodsCategory(categoryId);
        }

        /// <summary>
        /// 创建商品分类数据信息
        /// </summary>
        /// <param name="goodsinfo">商品信息</param>
        /// <returns>商品分类id</returns>
        public static int CreateGoodsCategory(Goodscategoryinfo goodsCategoryInfo)
        {
            return DbProvider.GetInstance().CreateGoodscategory(goodsCategoryInfo);
        }

        /// <summary>
        /// 获取指定分类id的分类信息
        /// </summary>
        /// <param name="categoryid">分类id</param>
        /// <returns>分类信息</returns>
        public static Goodscategoryinfo GetGoodsCategoryInfoById(int categoryId)
        {
            return DTO.GetGoodsCategoryInfo(DbProvider.GetInstance().GetGoodsCategoryInfoById(categoryId));
        }

        /// <summary>
        /// 设置商品分类列表中层数(layer)和父列表(parentidlist)字段
        /// </summary>
        public static void SetGoodsCategoryeslayer()
        {
            DataTable dt = DbProvider.GetInstance().GetCategoriesTable();
            foreach (DataRow dr in dt.Rows)
            {
                int layer = 0;
                string parentIdList = "";
                string parentId = dr["parentid"].ToString().Trim();

                //如果是(分类)顶层则直接更新数据库
                if (parentId == "0")
                {
                    DbProvider.GetInstance().UpdateCategoriesInfo(TypeConverter.ObjectToInt(dr["categoryid"]),
                        layer,
                        "0",
                        HasChild(dt, dr["categoryid"].ToString().Trim()));
                    continue;
                }

                do
                { //更新子分类的层数(layer)和父列表(parentidlist)字段
                    string temp = parentId;

                    parentId = DbProvider.GetInstance().GetCategoriesParentidByID(TypeConverter.ObjectToInt(parentId)).ToString();

                    layer++;
                    if (parentId != "0")
                        parentIdList = temp + "," + parentIdList;
                    else
                    {
                        parentIdList = temp + "," + parentIdList;
                        DbProvider.GetInstance().UpdateCategoriesInfo(TypeConverter.ObjectToInt(dr["categoryid"]),
                            layer,
                            parentIdList.Substring(0, parentIdList.Length - 1),
                            HasChild(dt, dr["categoryid"].ToString().Trim()));
                        break;
                    }
                } while (true);
            }
            dt.Dispose();
        }

        #region  递归指定分类下的所有子分类

        public static string ChildNode = "0";

        /// <summary>
        /// 递归所有子节点并返回字符串
        /// </summary>
        /// <param name="correntfid">当前</param>
        /// <returns>子版块的集合,格式:1,2,3,4,</returns>
        public static string FindChildNode(string currentCId)
        {
            lock (ChildNode)
            {
                IDataReader iDataReader = DbProvider.GetInstance().GetSubGoodsCategories(TypeConverter.ObjectToInt(currentCId));

                ChildNode = ChildNode + "," + currentCId;
                //有子节点
                if (iDataReader != null)
                {                    
                    while(iDataReader.Read())
                    {
                        FindChildNode(iDataReader["categoryid"].ToString());
                    }
                    iDataReader.Dispose();
                }
                return ChildNode;
            }
        }

        #endregion

        /// <summary>
        /// 判断当前分类是否还有子分类
        /// </summary>
        /// <param name="dt">商品分类表</param>
        /// <param name="categoryid">商品分类</param>
        /// <returns>是否有子分类</returns>
        public static int HasChild(DataTable dt, string categoryId)
        {
            int haschild = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["parentid"].ToString().Trim() == categoryId)
                {
                    haschild = 1;
                    break;
                }
            }
            return haschild;
        }

        /// <summary>
        /// 设置显示顺序
        /// </summary>
        public static void SetDispalyorder()
        {
            DataTable dt = DbProvider.GetInstance().GetCategoriesTable();

            if (dt.Rows.Count == 1) return;

            int displayorder = 1;
            string cidlist;
            foreach (DataRow dr in dt.Select("parentid=0"))
            {
                if (dr["parentid"].ToString() == "0")
                {
                    ChildNode = "0";
                    cidlist = ("," + FindChildNode(dr["categoryid"].ToString())).Replace(",0,", "");

                    foreach (string cidstr in cidlist.Split(','))
                    {
                        DbProvider.GetInstance().UpdateGoodsCategoryDisplayorder(displayorder, TypeConverter.ObjectToInt(cidstr));
                        displayorder++;
                    }
                }
            }
        }

      
        /// <summary>
        /// 设置商品分类表中路径(pathlist)字段
        /// </summary>
        public static void SetCategoryPathList()
        {
            string extName = GeneralConfigs.Deserialize(Utils.GetMapPath(BaseConfigs.GetForumPath + "config/general.config")).Extname;
            SetCategoryPathList(true, extName);
        }


        /// <summary>
        /// 按指定的文件扩展名称设置商品分类表中路径(pathlist)字段
        /// </summary>
        /// <param name="extname">扩展名称,如:aspx , html 等</param>
        public static void SetCategoryPathList(bool isAspxRewrite, string extName)
        {
            DataTable dt = DbProvider.GetInstance().GetCategoriesTable();

            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            foreach (DataRow dr in dt.Rows)
            {
                string pathList = "";

                if (dr["parentidlist"].ToString().Trim() == "0")
                {
                    if (isAspxRewrite)
                        pathList = "<a href=\"showgoodslist-" + dr["categoryid"] + extName + "\">" + dr["categoryname"].ToString().Trim() + "</a>";
                    else
                        pathList = "<a href=\"showgoodslist.aspx?categoryid=" + dr["categoryid"] + "\">" + dr["categoryname"].ToString().Trim() + "</a>";
                }
                else
                {
                    foreach (string parentid in dr["parentidlist"].ToString().Trim().Split(','))
                    {
                        if (parentid.Trim() != "")
                        {
                            DataRow[] drs = dt.Select("[categoryid]=" + parentid);
                            if (drs.Length > 0)
                            {
                                if (isAspxRewrite)
                                    pathList += "<a href=\"showgoodslist-" + drs[0]["categoryid"] + extName + "\">" + drs[0]["categoryname"].ToString().Trim() + "</a>";
                                else
                                    pathList += "<a href=\"showgoodslist.aspx?categoryid=" + drs[0]["categoryid"] + "\">" + drs[0]["categoryname"].ToString().Trim() + "</a>";
                            }
                        }
                    }
                    if (isAspxRewrite)
                        pathList += "<a href=\"showgoodslist-" + dr["categoryid"] + extName + "\">" + dr["categoryname"].ToString().Trim() + "</a>";
                    else
                        pathList += "<a href=\"showgoodslist.aspx?categoryid=" + dr["categoryid"] + "\">" + dr["categoryname"].ToString().Trim() + "</a>";
                }

                DbProvider.GetInstance().SetGoodsCategoryPathList(pathList, TypeConverter.ObjectToInt(dr["categoryid"]));
            }
        }


        /// <summary>
        /// 将商品分类表以DataTable的方式存入缓存
        /// </summary>
        /// <returns>商品分类表</returns>
        public static DataTable GetCategoriesTable()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Mall/MallSetting/GoodsCategories") as DataTable;
            if (dt == null)
            {
                dt = DbProvider.GetInstance().GetCategoriesTable();
                cache.AddObject("/Mall/MallSetting/GoodsCategories", dt);
            }
            return dt;
        }


        /// <summary>
        /// 更新指定分类的有效商品数量
        /// </summary>
        /// <param name="goodscategoryinfo">指定分类的信息</param>
        /// <returns>更新商品分类数</returns>
        public static bool UpdateCategoryGoodsCount(Goodscategoryinfo goodsCategoryInfo)
        {
            if (goodsCategoryInfo != null && goodsCategoryInfo.Categoryid > 0)
            {
                goodsCategoryInfo.Goodscount = Goods.GetGoodsCount(goodsCategoryInfo.Categoryid, "");
                GoodsCategories.UpdateGoodsCategory(goodsCategoryInfo);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 更新分类的有效商品数量
        /// </summary>
        /// <returns>更新商品分类数</returns>
        public static bool UpdateCategoryGoodsCount()
        {
            Goodscategoryinfo[] goodsArray = DTO.GetGoodsCategoryInfoArray(GoodsCategories.GetCategoriesTable());
            if(goodsArray == null)
                return true;
            foreach (Goodscategoryinfo goodsCategoryInfo in goodsArray)
            {
                if (goodsCategoryInfo != null && goodsCategoryInfo.Categoryid > 0)
                {
                    goodsCategoryInfo.Goodscount = Goods.GetGoodsCount(goodsCategoryInfo.Categoryid, "");
                    GoodsCategories.UpdateGoodsCategory(goodsCategoryInfo);
                }
            }
            return true;
        }

        /// <summary>
        /// 获取指定分类的fid(版块id)字段信息
        /// </summary>
        /// <param name="categoryid">指定的分类id</param>
        /// <returns>版块id</returns>
        public static int GetCategoriesFid(int categoryId)
        {
            int forumid = -1;
            DataTable dt = GetCategoriesTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Select("categoryid=" + categoryId))
                { 
                    forumid = TypeConverter.ObjectToInt(dr["fid"].ToString());
                    break;
                }
            }
            return forumid;
        }

        /// <summary>
        /// 通过指定的论坛版块id获取相应的商品分类
        /// </summary>
        /// <param name="forumid">版块id</param>
        /// <returns>商品分类id</returns>
        public static int GetGoodsCategoryIdByFid(int forumId)
        {
            return DbProvider.GetInstance().GetGoodsCategoryIdByFid(forumId);
        }

        /// <summary>
        /// 获取指定分类id下的所有子分类的json格式信息
        /// </summary>
        /// <param name="categoryid">指定的分类id</param>
        /// <returns>json格式信息串</returns>
        public static string GetSubCategoriesJson(int categoryId)
        {
            return GetGoodsCategoryJsonData(DbProvider.GetInstance().GetSubGoodsCategories(categoryId));
        }


        /// <summary>
        /// 获取指定层数的商品分类 
        /// </summary>
        /// <returns>json格式信息串</returns>
        public static string GetRootGoodsCategoriesJson()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            string goodsCategoriesJson = cache.RetrieveObject("/Mall/MallSetting/RootGoodsCategories") as string;
            if (goodsCategoriesJson == null)
            {
                goodsCategoriesJson = GetGoodsCategoryJsonData(DbProvider.GetInstance().GetGoodsCategoriesByLayer(1));
                cache.AddObject("/Mall/MallSetting/RootGoodsCategories", goodsCategoriesJson);
            }
            return goodsCategoriesJson;
        }

        /// <summary>
        /// 获取JSON格式的商品分类信息
        /// </summary>
        /// <param name="__idatareader">数据信息</param>
        /// <returns>返回Json数据</returns>
        private static string GetGoodsCategoryJsonData(IDataReader reader)
        {
            StringBuilder sb_categories = new StringBuilder();
            sb_categories.Append("var cats = ");
            sb_categories.Append("[\r\n");

            while (reader.Read())
            {
                sb_categories.Append(string.Format("{{'id' : {0}, 'pid' : {1}, 'pidlist' : '{2}', 'name' : '{3}', 'child' : {4}, 'gcount' :{5}, 'layer' : {6}, 'fid' : {7}}},",
                    reader["categoryid"],
                    reader["parentid"],
                    reader["parentidlist"].ToString().Trim(),
                    reader["categoryname"].ToString().Trim().Replace("\\", "\\\\").Replace("'", "\\'"),
                    reader["haschild"].ToString().ToLower(),
                    reader["goodscount"],
                    reader["layer"],
                    reader["fid"]
                    ));
            }
            reader.Close();

            if (sb_categories.ToString().EndsWith(","))
                sb_categories.Remove(sb_categories.Length - 1, 1);

            sb_categories.Append("\r\n];");
            return sb_categories.ToString();
        }

        /// <summary>
        /// 返回fid与categoryid对应的JSON数据
        /// </summary>
        /// <returns>JSON数据</returns>
        public static string GetGoodsCategoryWithFid()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            string  categoryfid = cache.RetrieveObject("/Mall/MallSetting/GoodsCategoryWithFid") as string;
            if (categoryfid == null)
            {
                categoryfid = "[";

                IDataReader iDataReader = DbProvider.GetInstance().GetGoodsCategoryWithFid();
                if (iDataReader != null)
                {
                    while (iDataReader.Read())
                    {
                        categoryfid += string.Format("{{'fid' : {0}, 'categoryid' : {1}}}," , 
                            iDataReader["fid"],
                            iDataReader["categoryid"]);
                    }
                    iDataReader.Dispose();
                }
                if (categoryfid.EndsWith(","))
                    categoryfid = categoryfid.Substring(0, categoryfid.Length - 1);

                categoryfid += "]";
                cache.AddObject("/Mall/MallSetting/GoodsCategoryWithFid", categoryfid);
            }

            return categoryfid;
        }
        

        /// <summary>
        /// 生成商品分类表的JSON文件
        /// </summary>
        /// <returns>是否写入成功</returns>
        public override bool WriteJsonFile()
        {
            StringBuilder sb_categories = new StringBuilder("var cats = ");

            sb_categories.Append(
                Utils.DataTableToJSON(
                      DbProvider.GetInstance().GetCategoriesTableToJson()));

            return base.WriteJsonFile(jsonPath , sb_categories);
        }

        /// <summary>
        /// 获取指定商品分类的parentidlist数据
        /// </summary>
        /// <param name="categoryid">指定的分类id</param>
        /// <returns>parentidlist数据</returns>
        public static string GetParentCategoryList(int categoryid)
        {
            DataTable dt = DbProvider.GetInstance().GetRootCategoryID(categoryid);
            string parentIdList = "";
            if(dt.Rows.Count>0)
                parentIdList = dt.Rows[0]["parentidlist"].ToString().Trim();

            dt.Dispose();
            return parentIdList;
        }

        /// <summary>
        /// 获取指定的商品类型数据(option格式)
        /// </summary>
        /// <returns>商品类型数据</returns>
        public static string GetShopRootCategoryOption()
        {
            StringBuilder sb_category = new StringBuilder();
            DataTable dt = DbProvider.GetInstance().GetRootCategoriesTable();
            foreach (DataRow dr in dt.Rows)
            {
                sb_category.AppendFormat("<option value=\"{0}\">{1}</option>", dr["categoryid"], dr["categoryname"].ToString().Trim());
            }
            dt.Dispose();
            return sb_category.ToString();
        }

        /// <summary>
        /// 获取(根)商品分类信息
        /// </summary>
        /// <returns></returns>
        public static Goodscategoryinfo[] GetShopRootCategory()
        {
            return DTO.GetGoodsCategoryInfoArray(DbProvider.GetInstance().GetRootCategoriesTable());
        }
      
        /// <summary>
        /// 数据转换对象类
        /// </summary>
        public class DTO
        {
            /// <summary>
            /// 获得商品分类信息(DTO)
            /// </summary>
            /// <param name="__idatareader">要转换的数据</param>
            /// <returns>返回商品分类信息</returns>
            public static Goodscategoryinfo GetGoodsCategoryInfo(IDataReader reader)
            {
                if (reader.Read())
                {
                    Goodscategoryinfo goodscategoryinfo = new Goodscategoryinfo();
                    goodscategoryinfo.Categoryid = TypeConverter.ObjectToInt(reader["categoryid"]);
                    goodscategoryinfo.Parentid = TypeConverter.ObjectToInt(reader["parentid"]);
                    goodscategoryinfo.Layer = Convert.ToInt16(reader["layer"].ToString());
                    goodscategoryinfo.Parentidlist = reader["parentidlist"].ToString().Trim();
                    goodscategoryinfo.Displayorder = Convert.ToInt16(reader["displayorder"].ToString().Trim());
                    goodscategoryinfo.Categoryname = reader["categoryname"].ToString().Trim();
                    goodscategoryinfo.Haschild = reader["haschild"].ToString() == "True" ? 1 : 0;
                    goodscategoryinfo.Fid = TypeConverter.ObjectToInt(reader["fid"]);
                    goodscategoryinfo.Pathlist = reader["pathlist"].ToString().Trim().Replace("a><a", "a> &raquo; <a");
                    goodscategoryinfo.Goodscount = TypeConverter.ObjectToInt(reader["goodscount"]);

                    reader.Close();
                    return goodscategoryinfo;
                }
                return null;
            }

            /// <summary>
            /// 获得商品分类信息(DTO)
            /// </summary>
            /// <param name="dt">要转换的数据表</param>
            /// <returns>返回商品分类信息</returns>
            public static Goodscategoryinfo[] GetGoodsCategoryInfoArray(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;

                Goodscategoryinfo[] goodscategoryinfoarray = new Goodscategoryinfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    goodscategoryinfoarray[i] = new Goodscategoryinfo();
                    goodscategoryinfoarray[i].Categoryid = TypeConverter.ObjectToInt(dt.Rows[i]["categoryid"]);
                    goodscategoryinfoarray[i].Parentid = TypeConverter.ObjectToInt(dt.Rows[i]["parentid"]);
                    goodscategoryinfoarray[i].Layer = TypeConverter.ObjectToInt(dt.Rows[i]["layer"]);
                    goodscategoryinfoarray[i].Parentidlist = dt.Rows[i]["parentidlist"].ToString();
                    goodscategoryinfoarray[i].Displayorder = Convert.ToInt16(dt.Rows[i]["displayorder"].ToString().Trim());
                    goodscategoryinfoarray[i].Categoryname = dt.Rows[i]["categoryname"].ToString();
                    goodscategoryinfoarray[i].Haschild = dt.Rows[i]["haschild"].ToString() == "True" ? 1 : 0;
                    goodscategoryinfoarray[i].Fid = TypeConverter.ObjectToInt(dt.Rows[i]["fid"]);
                    goodscategoryinfoarray[i].Pathlist = dt.Rows[i]["pathlist"].ToString().Replace("a><a", "a> &raquo; <a");
                    goodscategoryinfoarray[i].Goodscount = TypeConverter.ObjectToInt(dt.Rows[i]["goodscount"]);
                }
                dt.Dispose();
                return goodscategoryinfoarray;
            }
        }

        /// <summary>
        /// 清除商品分类绑定的版块
        /// </summary>
        /// <param name="fid"></param>
        public static void EmptyGoodsCategoryFid(int fid)
        {
            DbProvider.GetInstance().EmptyGoodsCategoryFid(fid);
        }

        /// <summary>
        /// 生成商品分类的json文件
        /// </summary>
        public static void StaticWriteJsonFile()
        {
            new GoodsCategories().WriteJsonFile();
        }
    }

}
