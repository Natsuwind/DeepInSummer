using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmiForum.Models.Entity;
using System.Data.Common;
using Natsuhime.Data;
using System.Data;
using MySql.Data.MySqlClient;
using System.Web.Security;

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
			    DbHelper.MakeInParam("?secques", (DbType)MySqlDbType.String, 8,newUserInfo.SecQues),
			    DbHelper.MakeInParam("?qqopenid", (DbType)MySqlDbType.String, 45,newUserInfo.QqOpenId)
		    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO members (`username`, `password`, `email`, `regip`, `regdate`, `lastloginip`, `lastlogindate`, `salt`, `secques`,`qqopenid`) VALUES(?username,?password,?email,?regip,?regdate,?lastloginip,?lastlogindate,?salt,?secques,?qqopenid)", prams);
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
            while (dr.Read() && (!IsExitsUsername || !IsExitsEmail))
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

        public static List<ShortUserInfo> GetUserList()
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM members");
            List<ShortUserInfo> userList = BindUserInfoList(dr);
            dr.Close();
            return userList;
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
        public static ShortUserInfo GetUserInfoByUsername(string username)
        {
            if (username.Trim() == string.Empty)
            {
                return null;
            }

            DbParameter[] prams = 
                {
			    DbHelper.MakeInParam("?username", (DbType)MySqlDbType.String, 15,username)
                };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM members WHERE username=?username", prams);
            ShortUserInfo shortUserInfo = BindUserInfo(dr);
            dr.Close();
            return shortUserInfo;
        }

        public static ShortUserInfo GetUserInfoByQqOpenid(string openid)
        {
            if (openid == null || openid.Trim() == string.Empty)
            {
                return null;
            }

            DbParameter[] prams = 
                {
			    DbHelper.MakeInParam("?qqopenid", (DbType)MySqlDbType.String, 45,openid)
                };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM members WHERE qqopenid=?qqopenid", prams);
            ShortUserInfo shortUserInfo = BindUserInfo(dr);
            dr.Close();
            return shortUserInfo;
        }

        public static ShortUserInfo CheckUserLogin(string email, string password)
        {
            if (email.Trim() == string.Empty)
            {
                return null;
            }

            DbParameter[] prams = 
                {
			    DbHelper.MakeInParam("?email", (DbType)MySqlDbType.String, 32,email),
			    DbHelper.MakeInParam("?password", (DbType)MySqlDbType.String, 32,FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5"))
                };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM members WHERE email=?email AND password=?password", prams);
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
                shortUserInfo.QqOpenId = dr["qqopenid"] != DBNull.Value ? dr["qqopenid"].ToString() : "null";
            }
            return shortUserInfo;
        }
        static List<ShortUserInfo> BindUserInfoList(IDataReader dr)
        {
            List<ShortUserInfo> userList = new List<ShortUserInfo>();
            while (dr.Read())
            {
                ShortUserInfo shortUserInfo = new ShortUserInfo();
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

                userList.Add(shortUserInfo);
            }
            return userList;
        }

        public static void SetLoginStatus(ShortUserInfo shortUserInfo)
        {
            HttpContext.Current.Session["login"] = shortUserInfo.Uid;
        }

        public static void SetLoginStatus(string email)
        {
            ShortUserInfo shortUserInfo = GetUserInfo(email);
            SetLoginStatus(shortUserInfo);
        }

        public static void Logout()
        {
            HttpContext.Current.Session.Abandon();
        }

        public static ShortUserInfo GetLoginStatus()
        {
            int uid = -1;
            if (HttpContext.Current.Session["login"] != null && int.TryParse(HttpContext.Current.Session["login"].ToString(), out uid))
            {
                return GetUserInfo(uid);
            }
            return null;
        }
    }
}