using System;
using System.Data;
using System.Collections.Generic;
using Natsuhime;
using LiteCMS.Data;
using LiteCMS.Entity;

namespace LiteCMS.Core
{
    public class Users
    {        
        private static UserInfo DataReader2UserInfo(IDataReader reader)
        {
            UserInfo info = new UserInfo();
            info.Uid = Convert.ToInt32(reader["uid"]);
            info.Username = reader["username"].ToString();
            info.Password = reader["password"].ToString();
            info.Groupid = Convert.ToInt32(reader["groupid"]);
            info.Adminid = Convert.ToInt32(reader["adminid"]);
            info.Qq = reader["qq"].ToString();
            info.Email = reader["email"].ToString();
            info.Secquestion = reader["secques"].ToString();
            info.Secanswer = reader["secans"].ToString();
            info.Msn = reader["msn"].ToString();
            info.Hi = reader["hi"].ToString();
            info.Nickname = reader["nickname"].ToString();
            info.Realname = reader["realname"].ToString();
            info.Bdday = Convert.ToDateTime(reader["bdday"]).ToString("yyyy-MM-dd");
            info.Regip = reader["regip"].ToString();
            info.Regdate = Convert.ToDateTime(reader["regdate"]).ToString("yyyy-MM-dd");
            info.Lastlogip = reader["lastlogip"].ToString();
            info.Lastlogdate = Convert.ToDateTime(reader["lastlogdate"]).ToString("yyyy-MM-dd");
            info.Del = Convert.ToInt32(reader["del"]);
            info.Articlecount = Convert.ToInt32(reader["articlecount"]);
            info.Topiccount = Convert.ToInt32(reader["topiccount"]);
            info.Replycount = Convert.ToInt32(reader["replycount"]);
            return info;
        }
        /// <summary>
        /// 取得用户信息(用于登录)
        /// </summary>
        /// <param name="loginid">登录id(UserName或者Email)</param>
        /// <param name="password">密码</param>
        /// <param name="logintype">登录类型(0为邮箱地址登录,1为UserName登录)</param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string loginid, string password, int logintpye)
        {
            UserInfo info;
            IDataReader reader = DatabaseProvider.GetInstance().GetUserInfo(loginid, password, logintpye);
            if (reader.Read())
            {
                info = DataReader2UserInfo(reader);
            }
            else
            {
                info = null;
            }
            reader.Close();
            return info;
        }
        public static UserInfo GetUserInfo(int uid, string password)
        {
            UserInfo info;
            IDataReader reader = DatabaseProvider.GetInstance().GetUserInfo(uid, password);
            if (reader.Read())
            {
                info = DataReader2UserInfo(reader);
            }
            else
            {
                info = null;
            }
            reader.Close();
            return info;
        }
        public static UserInfo GetUserInfo(string loginid, int logintype)
        {
            UserInfo info;
            IDataReader reader = DatabaseProvider.GetInstance().GetUserInfo(loginid, logintype);
            if (reader.Read())
            {
                info = DataReader2UserInfo(reader);
            }
            else
            {
                info = null;
            }
            reader.Close();
            return info;
        }



        public static void AddUser(UserInfo info)
        {
            DatabaseProvider.GetInstance().AddUser(info);
        }

        public static void EditUser(UserInfo info)
        {
            DatabaseProvider.GetInstance().EditUser(info);
        }
    }
}
