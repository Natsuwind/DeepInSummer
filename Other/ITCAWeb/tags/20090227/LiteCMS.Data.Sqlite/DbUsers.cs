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
        /// <summary>
        /// 取得用户信息(用于登录)
        /// </summary>
        /// <param name="loginid">登录id(UserName或者Email)</param>
        /// <param name="password">密码</param>
        /// <param name="logintype">登录类型(0为邮箱地址登录,1为UserName登录)</param>
        /// <returns>用户记录</returns>
        public IDataReader GetUserInfo(string loginid, string password, int logintype)
        {
            string sql;
            if (logintype == 0)
            {
                sql = "SELECT * FROM wy_users WHERE email=@loginid AND password=@password";
            }
            else
            {
                sql = "SELECT * FROM wy_users WHERE username=@loginid AND password=@password";
            }
            IDataReader dr;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@loginid", DbType.String, 100, loginid),
                DbHelper.MakeInParam("@password", DbType.String, 32, password)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, sql, prams);
            return dr;
        }

        public IDataReader GetUserInfo(int uid)
        {
            IDataReader dr;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_users WHERE uid=@uid", prams);
            return dr;
        }

        public IDataReader GetUserInfo(int uid, string password)
        {
            IDataReader dr;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@uid", DbType.String, 100, uid),
                DbHelper.MakeInParam("@password", DbType.String, 32, password)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM {0}users WHERE uid=@uid AND password=@password", BaseConfigs.GetConfig().Tableprefix), prams);
            return dr;
        }

        public IDataReader GetUserInfo(string loginid, int logintype)
        {
            string sql;
            if (logintype == 0)
            {
                sql = "SELECT * FROM wy_users WHERE email=@loginid";
            }
            else
            {
                sql = "SELECT * FROM wy_users WHERE username=@loginid";
            }
            IDataReader dr;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@loginid", DbType.String, 100, loginid),
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, sql, prams);
            return dr;
        }



        public IDataReader GetUsers(int pagesize, int currentpage)
        {
            IDataReader dr;
            int recordoffset = (currentpage - 1) * pagesize;

            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("@recordoffset", DbType.Int32, 4,recordoffset),
			    DbHelper.MakeInParam("@pagesize", DbType.Int32, 4,pagesize)
		    };
            dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM wy_users ORDER BY uid LIMIT @recordoffset,@pagesize", prams);
            return dr;
        }

        public int GetArticleCollectionPageCount(int pagesize)
        {
            int recordcount;
            recordcount = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(uid) FROM wy_users"));
            return recordcount % pagesize == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
        }



        public void AddUser(UserInfo info)
        {
            DbParameter[] dbparams = 
		    {
			    DbHelper.MakeInParam("@username", DbType.String, 100, info.Username),                
			    DbHelper.MakeInParam("@password", DbType.String, 100, info.Password),
			    DbHelper.MakeInParam("@groupid", DbType.Int32, 4, info.Groupid),
			    DbHelper.MakeInParam("@adminid", DbType.Int32, 4, info.Adminid),
			    DbHelper.MakeInParam("@qq", DbType.String, 100, info.Qq),
			    DbHelper.MakeInParam("@email", DbType.String, 100, info.Email),
			    DbHelper.MakeInParam("@secques", DbType.String, 50, info.Secquestion),
			    DbHelper.MakeInParam("@secans", DbType.String, 32, info.Secanswer),
			    DbHelper.MakeInParam("@msn", DbType.String, 100, info.Msn),
			    DbHelper.MakeInParam("@hi", DbType.String, 100, info.Hi),
			    DbHelper.MakeInParam("@nickname", DbType.String, 100, info.Nickname),
			    DbHelper.MakeInParam("@realname", DbType.String, 100, info.Realname),
			    DbHelper.MakeInParam("@regip", DbType.String, 100, info.Regip),
			    DbHelper.MakeInParam("@del", DbType.Int32, 4, info.Del),
			    DbHelper.MakeInParam("@articlecount", DbType.Int32, 4, info.Articlecount),
			    DbHelper.MakeInParam("@topiccount", DbType.Int32, 4, info.Topiccount),
			    DbHelper.MakeInParam("@replycount", DbType.Int32, 4, info.Replycount),
			    DbHelper.MakeInParam("@lastlogip", DbType.String, 100, info.Lastlogip),
			    DbHelper.MakeInParam("@bdday", DbType.DateTime, 8, info.Bdday),
			    DbHelper.MakeInParam("@lastlogdate", DbType.DateTime, 8, info.Lastlogdate),
			    DbHelper.MakeInParam("@regdate", DbType.DateTime, 8, info.Regdate)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO wy_users(username,password,groupid,adminid,qq,email,secques,secans,msn,hi,nickname,realname,regip,del,articlecount,topiccount,replycount,lastlogip,bdday,lastlogdate,regdate) VALUES(@username,@password,@groupid,@adminid,@qq,@email,@secques,@secans,@msn,@hi,@nickname,@realname,@regip,@del,@articlecount,@topiccount,@replycount,@lastlogip,@bdday,@lastlogdate,@regdate)", dbparams);
        }

        public void EditUser(UserInfo info)
        {
            DbParameter[] dbparams = 
		    {
			    DbHelper.MakeInParam("@uid", DbType.String, 4, info.Uid),   
			    DbHelper.MakeInParam("@username", DbType.String, 100, info.Username),                
			    DbHelper.MakeInParam("@password", DbType.String, 100, info.Password),
			    DbHelper.MakeInParam("@groupid", DbType.Int32, 4, info.Groupid),
			    DbHelper.MakeInParam("@adminid", DbType.Int32, 4, info.Adminid),
			    DbHelper.MakeInParam("@qq", DbType.String, 100, info.Qq),
			    DbHelper.MakeInParam("@email", DbType.String, 100, info.Email),
			    DbHelper.MakeInParam("@secques", DbType.String, 50, info.Secquestion),
			    DbHelper.MakeInParam("@secans", DbType.String, 32, info.Secanswer),
			    DbHelper.MakeInParam("@msn", DbType.String, 100, info.Msn),
			    DbHelper.MakeInParam("@hi", DbType.String, 100, info.Hi),
			    DbHelper.MakeInParam("@nickname", DbType.String, 100, info.Nickname),
			    DbHelper.MakeInParam("@realname", DbType.String, 100, info.Realname),
			    DbHelper.MakeInParam("@regip", DbType.String, 100, info.Regip),
			    DbHelper.MakeInParam("@del", DbType.Int32, 4, info.Del),
			    DbHelper.MakeInParam("@articlecount", DbType.Int32, 4, info.Articlecount),
			    DbHelper.MakeInParam("@topiccount", DbType.Int32, 4, info.Topiccount),
			    DbHelper.MakeInParam("@replycount", DbType.Int32, 4, info.Replycount),
			    DbHelper.MakeInParam("@lastlogip", DbType.String, 100, info.Lastlogip),
			    DbHelper.MakeInParam("@bdday", DbType.DateTime, 8, info.Bdday),
			    DbHelper.MakeInParam("@lastlogdate", DbType.DateTime, 8, info.Lastlogdate),
			    DbHelper.MakeInParam("@regdate", DbType.DateTime, 8, info.Regdate)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE wy_users SET username=@username,password=@password,groupid=@groupid,adminid=@adminid,qq=@qq,email=@email,secques=@secques,secans=@secans,msn=@msn,hi=@hi,nickname=@nickname,realname=@realname,regip=@regip,del=@del,articlecount=@articlecount,topiccount=@topiccount,replycount=@replycount,lastlogip=@lastlogip,bdday=@bdday,lastlogdate=@lastlogdate,regdate=@regdate WHERE uid=@uid", dbparams);
        }
    }
}
