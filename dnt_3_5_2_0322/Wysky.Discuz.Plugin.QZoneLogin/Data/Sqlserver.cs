using System;
using System.Collections.Generic;
using System.Web;
using System.Data.Common;
using Discuz.Data;
using System.Data;

namespace Wysky.Discuz.Plugin.QZoneLogin.Data
{
    public class Sqlserver
    {
        public static int GetUIDByQqOpenid(string openid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@qqopenid", (DbType)SqlDbType.Char, 50, openid)
			};
            string sql = "SELECT uid FROM wysky_plugin_qzlogin WHERE qqopenid=@qqopenid";
            object uid = DbHelper.ExecuteScalar(CommandType.Text, sql, parms);
            return uid != null ? Convert.ToInt32(uid) : -1;
        }

        public static int CreateQqUserInfo(string openid, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@qqopenid", (DbType)SqlDbType.Char, 50, openid),
				DbHelper.MakeInParam("@uid", (DbType)SqlDbType.Int, 4, uid)
			};
            string sql = "INSERT INTO wysky_plugin_qzlogin(uid,qqopenid) VALUES (@uid,@qqopenid)";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public static int Install()
        {
            string sql = 
@"CREATE TABLE [wysky_plugin_qzlogin](
[uid] [int] NOT NULL,
[qqopenid] [char](50) NOT NULL, 
CONSTRAINT [PK_qzlogin_qqopenid] PRIMARY KEY CLUSTERED 
(
	[qqopenid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }
    }
}