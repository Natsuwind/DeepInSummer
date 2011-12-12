using System;
using System.Collections.Generic;
using iTCA.Yuwen.Data;
using System.Data;
using Yuwen.Tools.TinyCache;
using iTCA.Yuwen.Entity;

namespace iTCA.Yuwen.Core
{
    public class Articles
    {
        /// <summary>
        /// 通过cid取得文章列表.
        /// </summary>
        /// <param name="cid">栏目id(如果为0表示取得所有栏目)</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="currentpage">当前页</param>
        /// <returns>列表</returns>
        public static List<ArticleInfo> GetArticleCollection(int cid, int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<ArticleInfo> coll = new List<ArticleInfo>();

            IDataReader reader = DatabaseProvider.GetInstance().GetArticles(cid, pagesize, currentpage);

            while (reader.Read())
            {
                DataReader2ArticleCollection(coll, reader);
            }
            reader.Close();
            return coll;
        }
        /// <summary>
        /// 通过cid取得文章列表.
        /// </summary>
        /// <param name="cids">栏目id字符串(,分割)为空字符串表示所有栏目</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="currentpage">当前页</param>
        /// <returns>列表</returns>
        public static List<ArticleInfo> GetArticleCollection(string cids, int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<ArticleInfo> coll = new List<ArticleInfo>();
            if (Utils.IsNumericString(cids))
            {
                IDataReader reader = DatabaseProvider.GetInstance().GetArticles(cids, pagesize, currentpage);

                while (reader.Read())
                {
                    //DataReader2ArticleCollection(coll, reader);
                    coll.Add(DataReader2ArticleInfo(reader));
                }
                reader.Close();
            }
            return coll;
        }
        /// <summary>
        /// 将DataReader的Article添加到List<ArticleInfo>泛型列表(暂时废除)
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="reader"></param>
        private static void DataReader2ArticleCollection(List<ArticleInfo> coll, IDataReader reader)
        {
            ArticleInfo info = DataReader2ArticleInfo(reader);
            coll.Add(info);
        }
        /// <summary>
        /// 将DataReader的Article转换为ArticleInfo泛型列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static ArticleInfo DataReader2ArticleInfo(IDataReader reader)
        {
            ArticleInfo info = new ArticleInfo();
            info.Articleid = Convert.ToInt32(reader["articleid"]);
            info.Columnid = Convert.ToInt32(reader["columnid"]);
            info.Title = reader["title"].ToString();
            info.Badcount = Convert.ToInt32(reader["badcount"]);
            info.Goodcount = Convert.ToInt32(reader["goodcount"]);
            info.Commentcount = Convert.ToInt32(reader["commentcount"]);
            info.Viewcount = Convert.ToInt32(reader["viewcount"]);
            info.Sort = Convert.ToInt32(reader["sort"]);
            info.Highlight = reader["highlight"].ToString();
            info.Summary = reader["summary"].ToString();
            info.Content = reader["content"].ToString();
            info.Image = reader["image"].ToString();
            info.Uid = Convert.ToInt32(reader["uid"]);
            info.Username = reader["username"].ToString();
            info.Ip = reader["ip"].ToString();
            info.Postdate = Convert.ToDateTime(reader["postdate"]).ToString("yyyy-MM-dd");
            info.Del = Convert.ToInt32(reader["del"]);
            //高亮主题
            if (info.Highlight != "")
            {
                info.Title = string.Format("<span style=\"{0}\">{1}</span>", info.Highlight, info.Title);
            }
            //文章的栏目前缀
            if (info.Columnid > 0)
            {
                info.Columnname = GetColumnName(info.Columnid);
            }
            else
            {
                info.Columnname = "";
            }
            return info;
        }
        /// <summary>
        /// 取得分页数目
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static int GetArticleCollectionPageCount(int cid, int pagesize)
        {
            return DatabaseProvider.GetInstance().GetArticleCollectionPageCount(cid, pagesize);
        }
        /// <summary>
        /// 取得分页数目
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static int GetArticleCollectionPageCount(string cids, int pagesize)
        {
            if (Utils.IsNumericString(cids))
            {
                return DatabaseProvider.GetInstance().GetArticleCollectionPageCount(cids, pagesize);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 取得栏目名称
        /// </summary>
        /// <param name="columnid"></param>
        /// <returns></returns>
        public static string GetColumnName(int columnid)
        {
            SortedList<int, object> columnarray = Articles.GetArticleColumnArray();
            object columnname = null;
            if (columnarray.ContainsKey(columnid))
            {
                columnname = columnarray[columnid];
            }
            if (columnname == null)
            {
                return "";
            }
            else
            {
                return columnname.ToString().Trim();
            }
        }
        /// <summary>
        /// 取得栏目列表
        /// </summary>
        /// <returns></returns>
        private static SortedList<int, object> GetArticleColumnArray()
        {
            SortedList<int, object> columnlist = new SortedList<int,object>();
            List<ColumnInfo> coll = GetColumnCollection();
            foreach (ColumnInfo columninfo in coll)
            {
                columnlist.Add(columninfo.Columnid, columninfo.Columnname);
            }
            //TinyCache cache = new TinyCache();
            //columnlist = cache.RetrieveObject("ColumnList") as SortedList<int, object>;

            //if (columnlist == null)
            //{
            //    columnlist = new SortedList<int, object>();
            //    DataTable dt = DatabaseProvider.GetInstance().GetArticleColumnList();
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            if ((dr["columnid"].ToString() != "") && (dr["columnname"].ToString() != ""))
            //            {
            //                columnlist.Add(Convert.ToInt32(dr["columnid"]), dr["columnname"]);
            //            }
            //        }
            //    }
            //    cache.AddObject("ColumnList", columnlist);
            //}
            return columnlist;
        }
        /// <summary>
        /// 取得栏目列表
        /// </summary>
        /// <returns></returns>
        public static List<ColumnInfo> GetColumnCollection()
        {            
            List<ColumnInfo> coll;
            TinyCache cache = new TinyCache();
            coll = cache.RetrieveObject("ColumnList") as List<ColumnInfo>;
            if (coll == null)
            {
                coll = new List<ColumnInfo>();
                IDataReader reader = DatabaseProvider.GetInstance().GetArticleColumnList();

                while (reader.Read())
                {
                    coll.Add(DataReader2ColumnInfo(reader));
                }
                reader.Close();
                cache.AddObject("ColumnList", coll);
            }
            return coll;
        }
        /// <summary>
        /// 将DataReader的Column转换为ColumnInfo泛型列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static ColumnInfo DataReader2ColumnInfo(IDataReader reader)
        {
            ColumnInfo columninfo = new ColumnInfo();
            columninfo.Columnid = Convert.ToInt32(reader["columnid"]);
            columninfo.Columnname= reader["columnname"].ToString();
            columninfo.Parentid = Convert.ToInt32(reader["parentid"]);
            return columninfo;
        }

        /// <summary>
        /// 取得文章内容
        /// </summary>
        /// <param name="articleid">文章id</param>
        /// <returns>AritlceInfo</returns>
        public static ArticleInfo GetArticleInfo(int articleid)
        {
            ArticleInfo info = new ArticleInfo();
            IDataReader reader = DatabaseProvider.GetInstance().GetArticleInfo(articleid);
            if (reader.Read())
            {
                info = DataReader2ArticleInfo(reader);
            }
            else
            {
                info = null;
            }
            reader.Close();
            return info;
        }
        /// <summary>
        /// 发布新文章
        /// </summary>
        /// <param name="articleinfo"></param>
        public static void CreateArticle(ArticleInfo articleinfo)
        {
            DatabaseProvider.GetInstance().CreateArticle(articleinfo);
        }
        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="articleinfo"></param>
        public static void EditArticle(ArticleInfo articleinfo)
        {
            DatabaseProvider.GetInstance().EditArticle(articleinfo);
        }
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="articleid"></param>
        public static void DeleteArticle(int articleid)
        {
            DatabaseProvider.GetInstance().DeleteArticle(articleid);
        }
        /// <summary>
        /// 新建栏目
        /// </summary>
        /// <param name="columninfo"></param>
        public static void CreateColumn(ColumnInfo columninfo)
        {
            DatabaseProvider.GetInstance().CreateColumn(columninfo);
        }
        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="columnid"></param>
        public static void DeleteColumn(int columnid)
        {
            DatabaseProvider.GetInstance().DeleteColumn(columnid);
        }
        /// <summary>
        /// 编辑栏目
        /// </summary>
        /// <param name="columninfo"></param>
        public static void EditColumn(ColumnInfo columninfo)
        {
            DatabaseProvider.GetInstance().EditColumn(columninfo);
        }
    }
}
