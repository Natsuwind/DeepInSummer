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

        public static int IsExits(string username, string email)
        {
            bool IsExitsUsername = false;
            bool IsExitsEmail = false;
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("?username", (DbType)MySqlDbType.String, 15,username),
			    DbHelper.MakeInParam("?email", (DbType)MySqlDbType.String, 32,email)
		    };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT username,email FROM members WHERE username=?username OR email=?email", prams);
            while (dr.Read() && !IsExitsUsername && !IsExitsEmail)
            {
                if (!IsExitsUsername && dr["username"].ToString().Trim() == username)
                {
                    IsExitsUsername = true;
                }
                if (!IsExitsEmail && dr["email"].ToString().Trim() == email)
                {
                    IsExitsEmail = true;
                }
            }
            dr.Close();


            if (IsExitsUsername && IsExitsEmail)
                return 3;
            if (IsExitsUsername)
                return 1;
            if (IsExitsEmail)
                return 2;
            return 0;
        }

        public static ShortUserInfo GetUserInfo(int uid)
        {
            if (uid <= 0)
            {
                return null;
            }

            DbParameter[] prams = 
                {
                    DbHelper.MakeInParam("?uid", (DbType)MySqlDbType.Int24, 4,uid)
                };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM members WHERE uid=?uid", prams);
            ShortUserInfo shortUserInfo = BindUserInfo(dr);
            dr.Close();
            return shortUserInfo;
        }

        public static ShortUserInfo GetUserInfo(string email)
        {
            if (email.Trim() == string.Empty)
            {
                return null;
            }
            
            DbParameter[] prams = 
                {
			    DbHelper.MakeInParam("?email", (DbType)MySqlDbType.String, 32,email)
                };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM members WHERE email=?email", prams);
            ShortUserInfo shortUserInfo = BindUserInfo(dr);
            dr.Close();
            return shortUserInfo;
        }

        static ShortUserInfo BindUserInfo(IDataReader dr)
        {
            ShortUserInfo shortUserInfo = null;
            while (dr.Read())
            {
                shortUserInfo = new ShortUserInfo();
                shortUserInfo.Uid = Convert.ToInt32(dr["uid"]);
                shortUserInfo.Username = dr["username"].ToString();
                shortUserInfo.Password = dr["password"].ToString();
                shortUserInfo.Email = dr["email"].ToString();
                shortUserInfo.RegIp = dr["regip"].ToString();
                shortUserInfo.RegDate = Convert.ToDateTime(dr["regdate"]);
                shortUserInfo.LastLoginIp = dr["lastloginip"].ToString();
                shortUserInfo.LastLoginDate = Convert.ToDateTime(dr["lastlogindate"]);
                shortUserInfo.Salt = dr["salt"].ToString();
                shortUserInfo.SecQues = dr["secques"].ToString();
            }
            return shortUserInfo;
        }

        public static void SetLoginStatus(ShortUserInfo shortUserInfo)
        {
            HttpContext.Current.Session["login"] = shortUserInfo.Uid;
        }

        internal static void SetLoginStatus(string email)
        {
            ShortUserInfo shortUserInfo = GetUserInfo(email);
            SetLoginStatus(shortUserInfo);
        }
    }
}