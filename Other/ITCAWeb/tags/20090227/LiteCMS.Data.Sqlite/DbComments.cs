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
        public IDataReader GetComments(int articleid, int pagesize, int currentpage)
        {
            IDataReader dr;
            string sql;
            int recordoffset = (currentpage - 1) * pagesize;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@articleid", DbType.Int32, 4,articleid),
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            if (articleid > 0)
            {
                sql = string.Format("SELECT * FROM {0}comments WHERE del=0 AND articleid=@articleid ORDER BY commentid DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetConfig().Tableprefix);
            }
            else
            {
                sql = string.Format("SELECT * FROM {0}comments WHERE del=0 ORDER BY commentid DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetConfig().Tableprefix);
            }
            dr = DbHelper.ExecuteReader(CommandType.Text, sql, prams);
            return dr;
        }
        public int GetCommentsPageCount(int articleid, int pagesize)
        {
            int recordcount;
            string sql;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@articleid", DbType.Int32, 4,articleid)
		    };
            if (articleid > 0)
            {
                sql = string.Format("SELECT COUNT(commentid) FROM {0}comments WHERE del=0 AND articleid=@articleid", BaseConfigs.GetConfig().Tableprefix);
            }
            else
            {
                sql = string.Format("SELECT COUNT(commentid) FROM {0}comments WHERE del=0", BaseConfigs.GetConfig().Tableprefix);
            }
            recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql, prams));
            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }

        public IDataReader GetUserComments(int uid, int pagesize, int currentpage)
        {
            IDataReader dr;
            int recordoffset = (currentpage - 1) * pagesize;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4,uid),
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM {0}comments WHERE del=0 AND uid=@uid ORDER BY commentid DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetConfig().Tableprefix), prams);
            return dr;
        }
        public int GetUserCommentsPageCount(int uid, int pagesize)
        {
            int recordcount;
            string sql;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4,uid)
		    };
            sql = string.Format("SELECT COUNT(commentid) FROM {0}comments WHERE del=0 AND uid=@uid", BaseConfigs.GetConfig().Tableprefix);
            recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql, prams));
            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }

        public IDataReader GetMostGradComments(int pagesize, int currentpage)
        {
            IDataReader dr;
            string sql;
            int recordoffset = (currentpage - 1) * pagesize;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            sql = string.Format("SELECT * FROM {0}comments WHERE del=0 ORDER BY goodcount DESC, badcount DESC, commentid DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetConfig().Tableprefix);
            dr = DbHelper.ExecuteReader(CommandType.Text, sql, prams);
            return dr;
        }
        public int GetMostGradCommentsPageCount(int pagesize)
        {
            int recordcount;
            string sql;
            sql = string.Format("SELECT COUNT(commentid) FROM {0}comments WHERE del=0", BaseConfigs.GetConfig().Tableprefix);
            recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql));
            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }
        #endregion

        public IDataReader GetCommentInfo(int commentid)
        {
            IDataReader dr;
            DbParameter[] dbparams =
            {
                DbHelper.MakeInParam("@commentid", DbType.Int32, 4, commentid)
            };
            dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM {0}comments WHERE commentid=@commentid", BaseConfigs.GetConfig().Tableprefix), dbparams);
            return dr;
        }

        public void CreateComment(CommentInfo commentinfo)
        {
            DbParameter[] dbparams = 
		    {
			    DbHelper.MakeInParam("@articleid", DbType.Int32, 4, commentinfo.Articleid),
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4, commentinfo.Uid),
			    DbHelper.MakeInParam("@username", DbType.String, 50, commentinfo.Username),
			    DbHelper.MakeInParam("@postdate", DbType.DateTime, 8, commentinfo.Postdate),
			    DbHelper.MakeInParam("@del", DbType.Int32, 4, commentinfo.Del),
			    DbHelper.MakeInParam("@content", DbType.String, 1000, commentinfo.Content),
			    DbHelper.MakeInParam("@goodcount", DbType.Int32, 4, commentinfo.Goodcount),
			    DbHelper.MakeInParam("@badcount", DbType.Int32, 4, commentinfo.Badcount),
			    DbHelper.MakeInParam("@articletitle", DbType.String, 100, commentinfo.Articletitle)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("INSERT INTO {0}comments(articleid,uid,username,postdate,del,content,goodcount,badcount,articletitle) VALUES(@articleid,@uid,@username,@postdate,@del,@content,@goodcount,@badcount,@articletitle)", BaseConfigs.GetConfig().Tableprefix), dbparams);
        }
        public void DeleteComment(int commentid)
        {
            DbParameter[] dbparams = 
		    {
			    DbHelper.MakeInParam("@commentid", DbType.Int32, 4, commentid),
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("DELETE FROM {0}comments WHERE commentid=@commentid", BaseConfigs.GetConfig().Tableprefix), dbparams);
        }
        public void GradeComment(int commentid, int type)
        {
            string sql;
            DbParameter[] dbparams = 
		    {
			    DbHelper.MakeInParam("@commentid", DbType.Int32, 4, commentid),
		    };
            if (type == 1)
            {
                sql = string.Format("UPDATE {0}comments SET goodcount=goodcount+1 WHERE commentid=@commentid", BaseConfigs.GetConfig().Tableprefix);
            }
            else
            {
                sql = string.Format("UPDATE {0}comments SET badcount=badcount+1 WHERE commentid=@commentid", BaseConfigs.GetConfig().Tableprefix);
            }
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, dbparams);
        }
    }
}
