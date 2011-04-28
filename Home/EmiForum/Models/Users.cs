using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmiForum.Models.Entity;
using System.Data.Common;
using Natsuhime.Data;
using System.Data;
using MySql.Data.MySqlClient;

namespace EmiForum.Models
{
    public class Users
    {
        public static void CreateUser(ShortUserInfo newUserInfo)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("?username", (DbType)MySqlDbType.String, 15,newUserInfo.Username),
			    DbHelper.MakeInParam("?password", (DbType)MySqlDbType.String, 32,newUserInfo.Password),
			    DbHelper.MakeInParam("?email", (DbType)MySqlDbType.String, 32,newUserInfo.Email),
			    DbHelper.MakeInParam("?regip", (DbType)MySqlDbType.String, 30,newUserInfo.RegIp),
			    DbHelper.MakeInParam("?regdate", (DbType)MySqlDbType.DateTime, 8,newUserInfo.RegDate),
			    DbHelper.MakeInParam("?lastloginip", (DbType)MySqlDbType.String, 30,newUserInfo.LastLoginIp),
			    DbHelper.MakeInParam("?lastlogindate", (DbType)MySqlDbType.DateTime, 8,newUserInfo.LastLoginDate),
			    DbHelper.MakeInParam("?salt", (DbType)MySqlDbType.String,6,newUserInfo.Salt),
			    DbHelper.MakeInParam("?secques", (DbType)MySqlDbType.String, 8,newUserInfo.SecQues)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO members (`username`, `password`, `email`, `regip`, `regdate`, `lastloginip`, `lastlogindate`, `salt`, `secques`) VALUES(?username,?password,?email,?regip,?regdate,?lastloginip,?lastlogindate,?salt,?secques)", prams);
        }
    }
}