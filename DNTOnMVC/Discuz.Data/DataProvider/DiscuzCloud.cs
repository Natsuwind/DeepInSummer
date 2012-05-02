using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Data
{
    public class DiscuzCloud
    {
        /// <summary>
        /// 创建用户的互联信息
        /// </summary>
        /// <param name="userConnectInfo"></param>
        /// <returns></returns>
        public static int CreateUserConnectInfo(UserConnectInfo userConnectInfo)
        {
            return DatabaseProvider.GetInstance().CreateUserConnectInfo(userConnectInfo);
        }

        /// <summary>
        /// 更新用户的互联信息
        /// </summary>
        /// <param name="userConnectInfo"></param>
        /// <returns></returns>
        public static int UpdateUserConnectInfo(UserConnectInfo userConnectInfo)
        {
            return DatabaseProvider.GetInstance().UpdateUserConnectInfo(userConnectInfo);
        }

        /// <summary>
        /// 根据openid获取用户的互联信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static UserConnectInfo GetUserConnectInfo(string openId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserConnectInfo(openId);
            UserConnectInfo userConnectInfo = null;
            if (reader.Read())
            {
                userConnectInfo = LoadUserConnectInfo(reader);
                reader.Close();
            }
            return userConnectInfo;
        }

        /// <summary>
        /// 根据uid获取用户的互联信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserConnectInfo GetUserConnectInfo(int userId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserConnectInfo(userId);
            UserConnectInfo userConnectInfo = null;
            if (reader.Read())
            {
                userConnectInfo = LoadUserConnectInfo(reader);
                reader.Close();
            }
            return userConnectInfo;
        }

        private static UserConnectInfo LoadUserConnectInfo(IDataReader reader)
        {
            UserConnectInfo userConnectInfo = new UserConnectInfo();
            userConnectInfo.OpenId = reader["openid"].ToString();
            userConnectInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            userConnectInfo.Token = reader["token"].ToString();
            userConnectInfo.Secret = reader["secret"].ToString();
            userConnectInfo.AllowVisitQQUserInfo = TypeConverter.ObjectToInt(reader["allowvisitqquserinfo"]);
            userConnectInfo.AllowPushFeed = TypeConverter.ObjectToInt(reader["allowpushfeed"]);
            userConnectInfo.IsSetPassword = TypeConverter.ObjectToInt(reader["issetpassword"]);
            userConnectInfo.CallbackInfo = reader["callbackinfo"].ToString();

            return userConnectInfo;
        }

        /// <summary>
        /// 删除指定的用户互联信息
        /// </summary>
        /// <param name="uid">论坛UID</param>
        /// <param name="openId">互联openid</param>
        /// <param name="type">如果type=uid，则openid可以等于空字符串,否则uid可以为-1</param>
        /// <returns></returns>
        public static int DeleteUserConnectInfo(int uid, string openId, string type)
        {
            return DatabaseProvider.GetInstance().DeleteUserConnectInfo(uid, openId, type);
        }

        /// <summary>
        /// 创建主题pushfeed到云平台的日志
        /// </summary>
        /// <param name="feedInfo"></param>
        /// <returns></returns>
        public static int CreateTopicPushFeedLog(TopicPushFeedInfo feedInfo)
        {
            return DatabaseProvider.GetInstance().CreateTopicPushFeedLog(feedInfo);
        }

        /// <summary>
        /// 获取指定主题id的pushfeed日志的信息
        /// (当论坛删除某主题时,需要调用该方法以判断该主题是否有feed到云平台,如果有该主题的信息,
        /// 则需要使用日志中记录的作者accessToken和accessSecret值来调用云平台接口,删除之前的feed)
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static TopicPushFeedInfo GetTopicPushFeedLog(int tid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicPushFeedLog(tid);
            TopicPushFeedInfo feedInfo = null;
            if (reader.Read())
            {
                feedInfo = new TopicPushFeedInfo();
                feedInfo.TopicId = TypeConverter.ObjectToInt(reader["tid"]);
                feedInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                feedInfo.AuthorToken = reader["authortoken"].ToString();
                feedInfo.AuthorSecret = reader["authorsecret"].ToString();
                reader.Close();
            }
            return feedInfo;
        }

        /// <summary>
        /// 删除指定主题id的pushfeed日志信息
        /// (当删除某feed到用户空间和微博的主题时,需要调用云平台接口来移除之前的feed信息,操作之后,该日志中的信息也可以删除了)
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static int DeleteTopicPushFeedLog(int tid)
        {
            return DatabaseProvider.GetInstance().DeleteTopicPushFeedLog(tid);
        }

        /// <summary>
        /// 创建用户绑定QQ的记录
        /// </summary>
        /// <param name="bindLog"></param>
        /// <returns></returns>
        public static int CreateUserConnectBindLog(UserBindConnectLog bindLog)
        {
            return DatabaseProvider.GetInstance().CreateUserConnectBindLog(bindLog);
        }

        /// <summary>
        /// 更新用户绑定QQ的记录
        /// </summary>
        /// <param name="bindLog"></param>
        /// <returns></returns>
        public static int UpdateUserConnectBindLog(UserBindConnectLog bindLog)
        {
            return DatabaseProvider.GetInstance().UpdateUserConnectBindLog(bindLog);
        }

        /// <summary>
        /// 获取用户绑定QQ的记录
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static UserBindConnectLog GetUserConnectBindLog(string openId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetUserConnectBindLog(openId);

            UserBindConnectLog bindLog = null;
            if (reader.Read())
            {
                bindLog = new UserBindConnectLog();
                bindLog.OpenId = reader["openid"].ToString();
                bindLog.Uid = TypeConverter.ObjectToInt(reader["uid"]);
                bindLog.Type = TypeConverter.ObjectToInt(reader["type"]);
                bindLog.BindCount = TypeConverter.ObjectToInt(reader["bindcount"]);
                reader.Close();
            }
            return bindLog;
        }
    }
}
