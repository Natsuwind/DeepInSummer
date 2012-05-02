using System;
using System.Collections.Generic;
using System.Text;

using Discuz.Config;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Services.API
{
    public class CommandParameter
    {
        private string commandName;

        /// <summary>
        /// 方法名称
        /// </summary>
        public string CommandName
        {
            get { return commandName; }
            set { commandName = value; }
        }

        private List<DNTParam> paramList;

        /// <summary>
        /// 方法参数列表
        /// </summary>
        public List<DNTParam> ParamList
        {
            get { return paramList; }
            set { paramList = value; }
        }

        private ApplicationInfo appInfo;

        /// <summary>
        /// 客户端应用配置信息
        /// </summary>
        public ApplicationInfo AppInfo
        {
            get { return appInfo; }
            set { appInfo = value; }
        }

        /// <summary>
        /// 当前登录用户的uid
        /// </summary>
        public int LocalUid
        {
            get
            {
                if (!CheckRequiredParams("session_key"))
                    return -1;//游客
                return GetUidFromSessionKey(GetDNTParam("session_key").ToString(), AppInfo.Secret);
            }
        }

        /// <summary>
        /// 当前请求的CallId
        /// </summary>
        public long CallId
        {
            get
            {
                long callId = 0;
                if (!CheckRequiredParams("call_id"))
                    return 0;//无效或非法请求

                return long.TryParse(GetDNTParam("call_id").ToString(), out callId) ? callId : 0;
            }
        }

        public FormatType Format
        {
            get
            {
                if (GetDNTParam("format") != null)
                    return Utils.GetEnum<FormatType>(GetDNTParam("format").ToString(), FormatType.XML);
                else
                    return FormatType.XML;
            }
        }

        public GeneralConfigInfo GeneralConfig
        {
            get
            {
                return GeneralConfigs.GetConfig();
            }
        }

        public CommandParameter(string commandName, List<DNTParam> paramList, ApplicationInfo appInfo)
        {
            this.commandName = commandName;
            this.paramList = paramList;
            this.appInfo = appInfo;
        }

        /// <summary>
        /// 根据SessionKey获得Uid
        /// </summary>
        /// <param name="session_key">会话key</param>
        /// <param name="secret">整合程序密码</param>
        /// <returns>uid</returns>
        private int GetUidFromSessionKey(string session_key, string secret)
        {
            if (session_key.Trim() == string.Empty)
                return -1;
            string[] sessionArray = session_key.Split('-');
            if (sessionArray.Length != 2)
                return -1;
            int uid = Utils.StrToInt(sessionArray[1], -1);
            int olid = OnlineUsers.GetOlidByUid(uid);

            byte[] md5_result = System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(olid.ToString() + secret));

            StringBuilder sessionkey_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sessionkey_builder.Append(b.ToString("x2"));
            if (sessionkey_builder.ToString() != sessionArray[0])
                return -1;
            return uid > 0 ? uid : -1;
        }

        /// <summary>
        /// 获得参数对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object GetDNTParam(string key)
        {
            if (paramList == null)
                return null;
            foreach (DNTParam p in paramList)
            {
                if (p.Name.ToLower() == key.ToLower())
                {
                    return p.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 获得整形参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetIntParam(string key)
        {
            return TypeConverter.ObjectToInt(GetDNTParam(key));
        }

        /// <summary>
        /// 获得整形参数，如果没有则返回默认值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetIntParam(string key, int defaultValue)
        {
            return TypeConverter.ObjectToInt(GetDNTParam(key), defaultValue);
        }


        /// <summary>
        /// 检查需要的参数是否存在
        /// </summary>
        /// <param name="paramArray">参数数组字符串</param>
        /// <returns></returns>
        public bool CheckRequiredParams(string paramArray)
        {
            string[] parms = paramArray.Split(',');
            for (int i = 0; i < parms.Length; i++)
            {
                if (GetDNTParam(parms[i]) == null || GetDNTParam(parms[i]).ToString().Trim() == string.Empty)
                    return false;
            }
            return true;
        }
    }
}
