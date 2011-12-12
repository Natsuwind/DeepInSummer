using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Natsuhime.Data;
using LiteCMS.Entity;
using LiteCMS.Config;

namespace LiteCMS.Data.Sqlite
{
    public partial class DataProvider : IDataProvider
    {
        #region 取得列表
        /// <summary>
        /// 取得文章列表.
        /// </summary>
        /// <param name="cid">栏目id(如果为0表示取得所有栏目)</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="currentpage">当前页</param>
        /// <returns>列表</returns>
        public System.Data.IDataReader GetArticles(int cid, int pagesize, int currentpage)
        {
            string sql;
            IDataReader dr;
            int recordoffset = (currentpage - 1) * pagesize;

            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@columnid", DbType.Int32, 4,cid),
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            if (cid > 0)
            {
                sql = string.Format("SELECT * FROM {0}articles WHERE del=0 AND columnid=@columnid ORDER BY articleid DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetConfig().Tableprefix);
            }
            else
            {
                sql = string.Format("SELECT * FROM {0}articles WHERE del=0 ORDER BY articleid DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetConfig().Tableprefix);
            }
            dr = DbHelper.ExecuteReader(CommandType.Text, sql, prams);
            return dr;
        }
        public int GetArticleCollectionPageCount(int cid, int pagesize)
        {
            int recordcount;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@columnid", DbType.Int32, 4,cid)
		    };
            if (cid > 0)
            {
                recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(articleid) FROM wy_articles WHERE del=0 AND columnid=@columnid", prams));
            }
            else
            {
                recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(articleid) FROM wy_articles WHERE del=0", prams));
            }
            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }

        /// <summary>
        /// 通过cid列表取得文章列表.
        /// </summary>
        /// <param name="cids">栏目id字符串(,分割)为空字符串表示所有栏目</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="currentpage">当前页</param>
        /// <returns>列表</returns>
        public IDataReader GetArticles(string cids, int pagesize, int currentpage)
        {
            IDataReader dr;
            int recordoffset = (currentpage - 1) * pagesize;

            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@columnids", DbType.String, 100,cids),
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            if (cids.Trim() != "")
            {
                dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_articles WHERE del=0 AND columnid IN (" + cids + ") ORDER BY articleid DESC LIMIT @recordoffset,@pagesize", prams);
            }
            else
            {
                dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_articles WHERE del=0 ORDER BY articleid DESC LIMIT @recordoffset,@pagesize", prams);
            }
            return dr;
        }
        public int GetArticleCollectionPageCount(string cids, int pagesize)
        {
            int recordcount;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@columnids", DbType.String, 100,cids)
		    };
            if (cids.Trim() != "")
            {
                recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(articleid) FROM wy_articles WHERE del=0 AND columnid IN (" + cids + ") ", prams));
            }
            else
            {
                recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(articleid) FROM wy_articles WHERE del=0", prams));
            }
            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }

        public IDataReader GetUserArticles(int uid, int pagesize, int currentpage)
        {
            IDataReader dr;
            int recordoffset = (currentpage - 1) * pagesize;

            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4,uid),
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_articles WHERE del=0 AND uid=@uid ORDER BY articleid DESC LIMIT @recordoffset,@pagesize", prams);

            return dr;
        }
        public int GetUserArticleCollectionPageCount(int uid, int pagesize)
        {
            int recordcount;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4,uid)
		    };
            recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(articleid) FROM wy_articles WHERE del=0 AND uid=@uid", prams));

            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }

        public IDataReader GetRecommendArticles(int pagesize, int currentpage)
        {
            IDataReader dr;
            int recordoffset = (currentpage - 1) * pagesize;

            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM {0}articles WHERE del=0 AND recommend=1 ORDER BY articleid DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetConfig().Tableprefix), prams);

            return dr;
        }
        public int GetRecommendArticleCollectionPageCount(int pagesize)
        {
            int recordcount;
            recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(articleid) FROM {0}articles WHERE del=0 AND recommend=1", BaseConfigs.GetConfig().Tableprefix)));

            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }

        public IDataReader GetHotArticles(int pagesize, int currentpage)
        {
            IDataReader dr;
            int recordoffset = (currentpage - 1) * pagesize;

            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM {0}articles WHERE del=0 ORDER BY commentcount DESC,articleid DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetConfig().Tableprefix), prams);

            return dr;
        }
        public int GetHotArticleCollectionPageCount(int pagesize)
        {
            int recordcount;
            recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(articleid) FROM {0}articles WHERE del=0", BaseConfigs.GetConfig().Tableprefix)));

            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }

        public IDataReader GetSearchArticles(string searchkey, int pagesize, int currentpage)
        {
            if (!Natsuhime.Common.Utils.IsSafeSqlString(searchkey))
            {
                return null;
            }
            IDataReader dr;
            int recordoffset = (currentpage - 1) * pagesize;

            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM {0}articles WHERE del=0 AND title LIKE '%{1}%' ORDER BY articleid DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetConfig().Tableprefix, searchkey), prams);

            return dr;
        }
        public int GetSearchArticleCollectionPageCount(string searchkey, int pagesize)
        {
            if (!Natsuhime.Common.Utils.IsSafeSqlString(searchkey))
            {
                return 0;
            }
            int recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(articleid) FROM {0}articles WHERE del=0 AND title LIKE '%{1}%'", BaseConfigs.GetConfig().Tableprefix, searchkey)));
            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }
        #endregion

        /// <summary>
        /// 取得文章内容
        /// </summary>
        /// <param name="articleid">文章id</param>
        /// <returns>IDataReader</returns>
        public IDataReader GetArticleInfo(int articleid)
        {
            IDataReader dr;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@articleid", DbType.Int32, 4,articleid)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_articles WHERE articleid=@articleid", prams);
            return dr;
        }
        public void CreateArticle(ArticleInfo articleinfo)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@title", DbType.String, 100,articleinfo.Title),
			    DbHelper.MakeInParam("@columnid", DbType.Int32, 4,articleinfo.Columnid),
			    DbHelper.MakeInParam("@highlight", DbType.String, 20,articleinfo.Highlight),
                DbHelper.MakeInParam("@summary", DbType.String, 160,articleinfo.Summary),
			    DbHelper.MakeInParam("@content", DbType.String, 5000,articleinfo.Content),
			    DbHelper.MakeInParam("@postdate", DbType.DateTime, 8,articleinfo.Postdate),
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4,articleinfo.Uid),
			    DbHelper.MakeInParam("@username", DbType.String, 20,articleinfo.Username),
			    DbHelper.MakeInParam("@recommend", DbType.Int32, 4,articleinfo.Recommend)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO wy_articles(title,columnid,highlight,summary,content,postdate,uid,username,recommend) VALUES(@title,@columnid,@highlight,@summary,@content,@postdate,@uid,@username,@recommend)", prams);
        }
        public void EditArticle(ArticleInfo articleinfo)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@title", DbType.String, 100,articleinfo.Title),
			    DbHelper.MakeInParam("@columnid", DbType.Int32, 4,articleinfo.Columnid),
			    DbHelper.MakeInParam("@highlight", DbType.String, 20,articleinfo.Highlight),
			    DbHelper.MakeInParam("@summary", DbType.String, 160,articleinfo.Summary),
			    DbHelper.MakeInParam("@content", DbType.String, 5000,articleinfo.Content),
			    DbHelper.MakeInParam("@postdate", DbType.DateTime, 8,articleinfo.Postdate),
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4,articleinfo.Uid),
			    DbHelper.MakeInParam("@username", DbType.String, 20,articleinfo.Username),
			    DbHelper.MakeInParam("@articleid", DbType.Int32, 4,articleinfo.Articleid),
			    DbHelper.MakeInParam("@recommend", DbType.Int32, 4,articleinfo.Recommend)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_articles SET title=@title,columnid=@columnid,highlight=@highlight,summary=@summary,content=@content,postdate=@postdate,uid=@uid,username=@username,recommend=@recommend WHERE articleid=@articleid", prams);
        }
        public void DeleteArticle(int articleid)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@articleid", DbType.Int32, 4,articleid)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_articles SET del=1 WHERE articleid=@articleid", prams);
        }
        public void ChangeCommentCount(int articleid, int changevalue, int type)
        {
            string sql;
            if (type == 1)
            {
                sql = string.Format("UPDATE {0}articles SET commentcount=commentcount+@increasevalue WHERE articleid=@articleid", BaseConfigs.GetConfig().Tableprefix);
            }
            else
            {
                sql = string.Format("UPDATE {0}articles SET commentcount=commentcount-@increasevalue WHERE articleid=@articleid", BaseConfigs.GetConfig().Tableprefix);
            }
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@articleid", DbType.Int32, 4,articleid),
			    DbHelper.MakeInParam("@increasevalue", DbType.Int32, 4,changevalue)
		    };
            DbHelper.ExecuteReader(CommandType.Text, sql, prams);

        }
    }
}
