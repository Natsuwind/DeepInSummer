using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Common;
using Discuz.Data;
using System.Data;
using Discuz.Config;

namespace Wysky.Discuz.Plugin.QZoneLogin.Data
{
    public class Sqlserver
    {
        public static int DbGetUIDByQqOpenid(string openid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@qqopenid", (DbType)SqlDbType.Char, 50, openid)
			};
            string sql = string.Format(
                "SELECT uid FROM {0}wysky_plugin_qzlogin WHERE qqopenid=@qqopenid",
                BaseConfigs.GetTablePrefix
                );
            object uid = DbHelper.ExecuteScalar(CommandType.Text, sql, parms);
            return uid != null ? Convert.ToInt32(uid) : -1;
        }

        public static string DbGetQqOpenidByUID(int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
            string sql = string.Format(
                "SELECT qqopenid FROM {0}wysky_plugin_qzlogin WHERE uid=@uid",
                BaseConfigs.GetTablePrefix
                );
            object openid = DbHelper.ExecuteScalar(CommandType.Text, sql, parms);
            return openid != null ? openid.ToString().Trim() : "";
        }

        public static void DbDeleteQqLoginInfo(string openid, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@qqopenid", (DbType)SqlDbType.Char, 50, openid),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
            string sql = string.Format(
                "DELETE FROM {0}wysky_plugin_qzlogin WHERE uid=@uid OR qqopenid=@qqopenid",
                BaseConfigs.GetTablePrefix
                );
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public static bool DbIsNullPasswordUser(int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
            string sql = string.Format(
                "SELECT password FROM {0}users WHERE uid=@uid",
                BaseConfigs.GetTablePrefix
                );
            object password = DbHelper.ExecuteScalar(CommandType.Text, sql, parms);
            return (password == null || password.ToString().Trim() == string.Empty || password.ToString().Trim() == "qqlogin_by_wysky.org");
        }

        public static int DbCreateQqUserInfo(string openid, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@qqopenid", (DbType)SqlDbType.Char, 50, openid),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
            string sql = string.Format(
                "INSERT INTO {0}wysky_plugin_qzlogin(uid,qqopenid) VALUES (@uid,@qqopenid)",
                BaseConfigs.GetTablePrefix
                );
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public static int DbInstall()
        {
            string sql = string.Format(
@"CREATE TABLE [{0}wysky_plugin_qzlogin](
[uid] [int] NOT NULL,
[qqopenid] [char](50) NOT NULL, 
CONSTRAINT [PK_{0}wysky_plugin_qzlogin_qqopenid] PRIMARY KEY CLUSTERED 
(
	[qqopenid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }
    }
}