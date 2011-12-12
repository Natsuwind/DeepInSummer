using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Yuwen.Tools.Data;
using iTCA.Yuwen.Entity;

namespace iTCA.Yuwen.Data.Sqlite
{
    public class DataProvider : IDataProvider
    {
        /// <summary>
        /// 取得文章列表.
        /// </summary>
        /// <param name="cid">栏目id(如果为0表示取得所有栏目)</param>
        /// <param name="pagesize">分页大小</param>
        /// <param name="currentpage">当前页</param>
        /// <returns>列表</returns>
        public System.Data.IDataReader GetArticles(int cid, int pagesize, int currentpage)
        {
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
                dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_articles WHERE del=0 AND columnid=@columnid ORDER BY articleid DESC LIMIT @recordoffset,@pagesize", prams);
            }
            else
            {
                dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_articles WHERE del=0 ORDER BY articleid DESC LIMIT @recordoffset,@pagesize", prams);
            }
            return dr;
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

        /// <summary>
        /// 取得栏目列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetArticleColumnList()
        {
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_columns WHERE del=0 ORDER BY [columnid]");
        }

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
			    DbHelper.MakeInParam("@content", DbType.String, 5000,articleinfo.Content),
			    DbHelper.MakeInParam("@postdate", DbType.DateTime, 8,articleinfo.Postdate),
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4,articleinfo.Uid),
			    DbHelper.MakeInParam("@username", DbType.String, 20,articleinfo.Username)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO wy_articles(title,columnid,highlight,content,postdate,uid,username) VALUES(@title,@columnid,@highlight,@content,@postdate,@uid,@username)", prams);
        }

        public void EditArticle(ArticleInfo articleinfo)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@title", DbType.String, 100,articleinfo.Title),
			    DbHelper.MakeInParam("@columnid", DbType.Int32, 4,articleinfo.Columnid),
			    DbHelper.MakeInParam("@highlight", DbType.String, 20,articleinfo.Highlight),
			    DbHelper.MakeInParam("@content", DbType.String, 5000,articleinfo.Content),
			    DbHelper.MakeInParam("@postdate", DbType.DateTime, 8,articleinfo.Postdate),
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4,articleinfo.Uid),
			    DbHelper.MakeInParam("@username", DbType.String, 20,articleinfo.Username),
			    DbHelper.MakeInParam("@articleid", DbType.Int32, 4,articleinfo.Articleid)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_articles SET title=@title,columnid=@columnid,highlight=@highlight,content=@content,postdate=@postdate,uid=@uid,username=@username WHERE articleid=@articleid", prams);
        }

        public void DeleteArticle(int articleid)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@articleid", DbType.Int32, 4,articleid)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_articles SET del=1 WHERE articleid=@articleid", prams);
        }

        
        public void CreateColumn(ColumnInfo columninfo)
        {
            DbParameter[] prams = 
		    {
			    //DbHelper.MakeInParam("@columnid", DbType.Int32, 4,columninfo.Columnid),
			    DbHelper.MakeInParam("@columnname", DbType.String, 50,columninfo.Columnname),
			    DbHelper.MakeInParam("@parentid", DbType.Int32, 4,columninfo.Parentid)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO wy_columns(columnname,parentid) VALUES(@columnname,@columnname)", prams);
        }

        public void DeleteColumn(int columnid)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@columnid", DbType.Int32, 4,columnid)
		    };
            //DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM wy_columns WHERE columnid=@columnid", prams);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_columns SET del=1 WHERE columnid=@columnid", prams);
        }

        public void EditColumn(ColumnInfo columninfo)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@columnname", DbType.String, 100,columninfo.Columnname),
			    DbHelper.MakeInParam("@columnid", DbType.Int32, 4,columninfo.Columnid),
			    DbHelper.MakeInParam("@parentid", DbType.Int32, 4,columninfo.Parentid)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_columns SET columnname=@columnname,parentid=@parentid WHERE columnid=@columnid", prams);
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

    }
}
