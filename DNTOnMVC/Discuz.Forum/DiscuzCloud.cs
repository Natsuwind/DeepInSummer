using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Newtonsoft.Json;

namespace Discuz.Forum
{
    public class DiscuzCloud
    {
        private static string productType = "Discuz!NT";
        private static string productVersion = Utils.ASSEMBLY_VERSION;
        private static string oauthCallback = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "connect.aspx?action=access";

        private const string CLOUD_URL = "http://api.discuz.qq.com/site.php";
        private const string CLOUD_CP_URL = "http://cp.discuz.qq.com/";
        private const string CONNECT_URL = "http://connect.discuz.qq.com/";
        private const string API_CONNECT_URL = "http://api.discuz.qq.com/";
        private const string PRODUCT_RELEASE = "20110701";


        private const string REQUEST_TOKEN_URL = API_CONNECT_URL + "oauth/requestToken";
        private const string AUTHORIZE_URL = CONNECT_URL + "oauth/authorize";
        private const string ACCESS_TOKEN_URL = API_CONNECT_URL + "oauth/accessToken";
        private const string UNBIND_URL = API_CONNECT_URL + "connect/user/unbind";

        private const string FORMAT = "json";
        private const string CHARSET = "utf-8";

        /// <summary>
        /// 从云平台注册站点ID和KEY并保存在dzcloud.config中
        /// </summary>
        /// <returns></returns>
        public static string RegisterSite()
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            DiscuzCloudMethodParameter mParams = new DiscuzCloudMethodParameter();
            mParams.Add("sName", GeneralConfigs.GetConfig().Forumtitle);
            mParams.Add("sSiteKey", config.Sitekey);
            mParams.Add("sCharset", "utf-8");
            mParams.Add("sTimeZone", "8");
            mParams.Add("sLanguage", "zh_CN");
            mParams.Add("sProductType", productType);
            mParams.Add("sProductVersion", productVersion);
            mParams.Add("sTimestamp", Utils.ConvertToUnixTimestamp(DateTime.Now).ToString());
            mParams.Add("sApiVersion", "0.4");
            mParams.Add("sSiteUid", BaseConfigs.GetFounderUid > 0 ? BaseConfigs.GetFounderUid.ToString() : "1");
            mParams.Add("sProductRelease", PRODUCT_RELEASE);
#if DEBUG
            mParams.Add("sUrl", "http://247.mydev.com/~cailong/");
            mParams.Add("sUCenterUrl", "http://247.mydev.com/~cailong/");
#else
            mParams.Add("sUrl", Utils.GetRootUrl(BaseConfigs.GetForumPath));
            mParams.Add("sUCenterUrl", Utils.GetRootUrl(BaseConfigs.GetForumPath));
#endif

            BaseCloudResponse<RegisterCloud> regCloudResult = GetCloudResponse<RegisterCloud>("site.register", mParams);

            if (regCloudResult.ErrCode == 0)
            {
                config.Cloudsiteid = regCloudResult.Result.CloudSiteId;
                config.Cloudsitekey = regCloudResult.Result.CloudSiteKey;

                DiscuzCloudConfigs.SaveConfig(config);
                DiscuzCloudConfigs.ResetConfig();
            }

            return regCloudResult.ErrMessage;
        }

        /// <summary>
        /// 获取云平台后台服务管理列表Iframe地址
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetCloudAppListIFrame(int userId)
        {

            DiscuzCloudMethodParameter mParams = new DiscuzCloudMethodParameter(false);
            mParams.Add("refer", Utils.GetRootUrl(BaseConfigs.GetForumPath));
            mParams.Add("s_id", DiscuzCloudConfigs.GetConfig().Cloudsiteid);
            mParams.Add("s_site_uid", userId.ToString());
            return GetCloudCpUrl("cloud/appList/", mParams);
        }

        /// <summary>
        /// 获取云平台绑定QQ号的页面地址
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetCloudBindUrl(int userId)
        {
            DiscuzCloudMethodParameter mParams = new DiscuzCloudMethodParameter(false);
            mParams.Add("s_id", DiscuzCloudConfigs.GetConfig().Cloudsiteid);
            mParams.Add("s_site_uid", userId.ToString());

            return GetCloudCpUrl("bind/index", mParams);
        }

        /// <summary>
        /// 获取云平台上传站点logo的页面地址
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetCloudUploadLogoIFrame(int userId)
        {
            DiscuzCloudMethodParameter mParams = new DiscuzCloudMethodParameter(false);
            mParams.Add("s_id", DiscuzCloudConfigs.GetConfig().Cloudsiteid);
            mParams.Add("s_site_uid", userId.ToString());
            mParams.Add("link_url", "admin/global/global_connectset.aspx");
            mParams.Add("self_url", "admin/global/global_connectset.aspx?upload=1");

            return GetCloudCpUrl("connect/service", mParams);
        }



        //public static string ResumeSite()
        //{
        //    DiscuzCloudMethodParameter mParams = new DiscuzCloudMethodParameter();
        //    mParams.Add("sUrl", Utils.GetRootUrl(BaseConfigs.GetForumPath));
        //    mParams.Add("sCharset", CHARSET);
        //    mParams.Add("sProductType", productType);
        //    mParams.Add("sProductVersion", productVersion);

        //    BaseCloudResponse<RegisterCloud> resumeCloudResult = GetCloudResponse<RegisterCloud>("site.resume", mParams);

        //    if (resumeCloudResult.ErrCode == 0 &&
        //        (resumeCloudResult.Result.CloudSiteId != config.Cloudsiteid || resumeCloudResult.Result.CloudSiteKey != config.Cloudsitekey))
        //    {
        //        config.Cloudsiteid = resumeCloudResult.Result.CloudSiteId;
        //        config.Cloudsitekey = resumeCloudResult.Result.CloudSiteKey;

        //        GeneralConfigs.SaveConfig(config);
        //        GeneralConfigs.ResetConfig();
        //    }

        //    return resumeCloudResult.ErrMessage;
        //}

        /// <summary>
        /// 同步站点信息,包括站点名称和站点地址
        /// </summary>
        /// <returns></returns>
        public static string SyncSite()
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            DiscuzCloudMethodParameter mParams = new DiscuzCloudMethodParameter();
            mParams.Add("sId", config.Cloudsiteid);
            mParams.Add("sName", GeneralConfigs.GetConfig().Forumtitle);
            mParams.Add("sSiteKey", config.Sitekey);
            mParams.Add("sCharset", "utf-8");
            mParams.Add("sTimeZone", "8");
            mParams.Add("sLanguage", "zh_CN");
            mParams.Add("sProductType", productType);
            mParams.Add("sProductVersion", productVersion);
            mParams.Add("sApiVersion", "0.4");
            mParams.Add("sSiteUid", BaseConfigs.GetFounderUid > 0 ? BaseConfigs.GetFounderUid.ToString() : "1");
            mParams.Add("sProductRelease", PRODUCT_RELEASE);
            mParams.Add("sTimestamp", Utils.ConvertToUnixTimestamp(DateTime.Now).ToString());

#if DEBUG
            mParams.Add("sUrl", "http://247.mydev.com/~cailong/");
            mParams.Add("sUCenterUrl", "http://247.mydev.com/~cailong/");
#else
            mParams.Add("sUrl", Utils.GetRootUrl(BaseConfigs.GetForumPath));
            mParams.Add("sUCenterUrl", Utils.GetRootUrl(BaseConfigs.GetForumPath));
#endif
            BaseCloudResponse<bool> syncSiteResult = GetCloudResponse<bool>("site.sync", mParams);
            return syncSiteResult.ErrMessage;
        }

        /// <summary>
        /// 更换站点KEY
        /// </summary>
        /// <returns></returns>
        public static string ResetSiteKey()
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            DiscuzCloudMethodParameter mParams = new DiscuzCloudMethodParameter();
            mParams.Add("sId", config.Cloudsiteid);
            BaseCloudResponse<RegisterCloud> resetCloudResult = GetCloudResponse<RegisterCloud>("site.resetKey", mParams);

            if (resetCloudResult.ErrCode == 0)
            {
                config.Cloudsitekey = resetCloudResult.Result.CloudSiteKey;
                DiscuzCloudConfigs.SaveConfig(config);
                DiscuzCloudConfigs.ResetConfig();
            }
            return resetCloudResult.ErrMessage;
        }

        /// <summary>
        /// 获取指定云平台服务的开启状态,true为开启
        /// </summary>
        /// <param name="serviceName">服务名称(connect,connect_reg)</param>
        /// <returns></returns>
        public static bool GetCloudServiceEnableStatus(string serviceName)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            if (config.Cloudenabled == 0)
                return false;

            switch (serviceName)
            {
                case "connect": return config.Connectenabled == 1;
                case "connect_reg": return config.Allowconnectregister == 1;
                default: return false;
            }
        }

#if DEBUG
        public static string GetCloudTestResponse(string method, DiscuzCloudMethodParameter mParams)
        {
            string timeStamp = mParams.Find("sTimestamp");
            timeStamp = string.IsNullOrEmpty(timeStamp) ? Utils.ConvertToUnixTimestamp(DateTime.Now).ToString() : timeStamp;
            string postData = string.Format("format={0}&method={1}&sId={2}&sig={3}&ts={4}{5}"
                , FORMAT, method, DiscuzCloudConfigs.GetConfig().Cloudsiteid,
                    GetCloudMethodSignature(method, timeStamp, mParams), timeStamp, mParams.GetPostData());
            return Utils.GetHttpWebResponse(CLOUD_URL, postData);
        }
#endif

        /// <summary>
        /// 获取QQ Connect 授权页面地址
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetConnectLoginPageUrl(int userId)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            List<DiscuzOAuthParameter> paramList = new List<DiscuzOAuthParameter>();
            paramList.Add(new DiscuzOAuthParameter("client_ip", DNTRequest.GetIP()));
            paramList.Add(new DiscuzOAuthParameter("type", userId > 0 ? "loginbind" : "login"));

            DiscuzOAuth oauth = new DiscuzOAuth();
            string queryStr = "";
            string requestTokenUrl = oauth.GetOAuthUrl(REQUEST_TOKEN_URL, "POST", config.Connectappid, config.Connectappkey,
                "", "", "", oauthCallback, paramList, out queryStr);

            string response = Utils.GetHttpWebResponse(requestTokenUrl, queryStr);

            try
            {
                ConnectResponse<OAuthTokenInfo> tokenInfo = JavaScriptConvert.DeserializeObject<ConnectResponse<OAuthTokenInfo>>(response);
                Utils.WriteCookie("connect", "token", tokenInfo.Result.Token);
                Utils.WriteCookie("connect", "secret", tokenInfo.Result.Secret);

                string authorizeUrl = oauth.GetOAuthUrl(AUTHORIZE_URL, "GET", config.Connectappid,
                                                        config.Connectappkey, tokenInfo.Result.Token,
                                                        tokenInfo.Result.Secret, "", oauthCallback,
                                                        new List<DiscuzOAuthParameter>(), out queryStr);
                return authorizeUrl + "?" + queryStr;
            }
            catch
            {
                return "?Failed to get tmptoken";
            }
        }

        /// <summary>
        /// 获取当前Oauth用户的accessTokenInfo
        /// </summary>
        /// <returns></returns>
        public static OAuthAccessTokenInfo GetConnectAccessTokenInfo()
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            List<DiscuzOAuthParameter> paramList = new List<DiscuzOAuthParameter>();
            paramList.Add(new DiscuzOAuthParameter("client_ip", DNTRequest.GetIP()));

            DiscuzOAuth oauth = new DiscuzOAuth();
            string queryStr = "";
            string accessTokenUrl = oauth.GetOAuthUrl(ACCESS_TOKEN_URL, "POST", config.Connectappid, config.Connectappkey,
                                                       Utils.GetCookie("connect", "token"), Utils.GetCookie("connect", "secret"),
                                                       DNTRequest.GetString("con_oauth_verifier"), "", paramList, out queryStr);
            string response = Utils.GetHttpWebResponse(accessTokenUrl, queryStr);

            try
            {
                ConnectResponse<OAuthAccessTokenInfo> accessTokenInfo = JavaScriptConvert.DeserializeObject<ConnectResponse<OAuthAccessTokenInfo>>(response);
                return accessTokenInfo.Result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取云平台通知已绑定用户的接口地址
        /// </summary>
        /// <param name="connectInfo"></param>
        /// <param name="userName"></param>
        /// <param name="birthday"></param>
        /// <param name="gender"></param>
        /// <param name="email"></param>
        /// <param name="isPublicEmail"></param>
        /// <param name="isUsedQQAvatar"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetBindUserNotifyUrl(UserConnectInfo connectInfo, string userName, string birthday, int gender,
                string email, int isPublicEmail, int isUsedQQAvatar, string type)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            List<DiscuzOAuthParameter> paramList = new List<DiscuzOAuthParameter>();
            paramList.Add(new DiscuzOAuthParameter("s_id", string.Empty));
            paramList.Add(new DiscuzOAuthParameter("openid", connectInfo.OpenId));
            paramList.Add(new DiscuzOAuthParameter("oauth_consumer_key", config.Connectappid));
            paramList.Add(new DiscuzOAuthParameter("u_id", connectInfo.Uid.ToString()));
            paramList.Add(new DiscuzOAuthParameter("username", userName));
            paramList.Add(new DiscuzOAuthParameter("birthday", birthday));
            string sex = "unknown";
            sex = gender == 1 ? "male" : sex;
            sex = gender == 2 ? "female" : sex;

            paramList.Add(new DiscuzOAuthParameter("sex", sex));
            paramList.Add(new DiscuzOAuthParameter("email", email));
            paramList.Add(new DiscuzOAuthParameter("is_public_email", isPublicEmail.ToString()));
            paramList.Add(new DiscuzOAuthParameter("is_use_qq_avatar", isUsedQQAvatar.ToString()));
            paramList.Add(new DiscuzOAuthParameter("statreferer", "forum"));
            paramList.Add(new DiscuzOAuthParameter("avatar_input", "234"));
            paramList.Add(new DiscuzOAuthParameter("avatar_agent", "23432"));
            paramList.Add(new DiscuzOAuthParameter("type", type));
            paramList.Add(new DiscuzOAuthParameter("site_ucenter_id", config.Sitekey));

            string queryString = "";
            string sig = GenerateNotifySignature(paramList, config.Connectappid + "|" + config.Connectappkey, out queryString);
            return CONNECT_URL + "notify/user/bind?" + queryString + "sig=" + sig;
        }

        /// <summary>
        /// 构造云平台通知接口的签名
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="secret"></param>
        /// <param name="queryStr">返回postdata</param>
        /// <returns></returns>
        private static string GenerateNotifySignature(List<DiscuzOAuthParameter> parms, string secret, out string queryStr)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder queryString = new StringBuilder();
            parms.Sort(new ParameterComparer());

            foreach (DiscuzOAuthParameter parm in parms)
            {
                queryString.AppendFormat("{0}={1}&", parm.Name, Utils.PHPUrlEncode(parm.Value));
                sb.AppendFormat("{0}={1}", parm.Name, parm.Value);
            }
            sb.Append(secret);
            queryStr = queryString.ToString();
            return Utils.MD5(sb.ToString());
        }

        /// <summary>
        /// 解除用户QQ绑定
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static int UnbindUserConnectInfo(string openId)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            UserConnectInfo userConnectInfo = DiscuzCloud.GetUserConnectInfo(openId);

            if (userConnectInfo == null)
                return -1;

            List<DiscuzOAuthParameter> paramList = new List<DiscuzOAuthParameter>();
            paramList.Add(new DiscuzOAuthParameter("client_ip", DNTRequest.GetIP()));

            DiscuzOAuth oauth = new DiscuzOAuth();
            string queryStr = "";
            string unbindUrl = oauth.GetOAuthUrl(UNBIND_URL, "POST", config.Connectappid, config.Connectappkey, userConnectInfo.Token, userConnectInfo.Secret,
                "", "", paramList, out queryStr);

            string response = Utils.GetHttpWebResponse(unbindUrl, queryStr);
            DeleteUserConnectInfo(openId);
            Utils.WriteCookie("bindconnect", "");
            return 1;
        }

        /// <summary>
        /// 创建用户的互联信息
        /// </summary>
        /// <param name="userConnectInfo"></param>
        /// <returns></returns>
        public static int CreateUserConnectInfo(UserConnectInfo userConnectInfo)
        {
            return Data.DiscuzCloud.CreateUserConnectInfo(userConnectInfo);
        }

        /// <summary>
        /// 更新用户的互联信息
        /// </summary>
        /// <param name="userConnectInfo"></param>
        /// <returns></returns>
        public static int UpdateUserConnectInfo(UserConnectInfo userConnectInfo)
        {
            if (userConnectInfo.OpenId.Length < 32 || userConnectInfo.Token.Length < 16 || userConnectInfo.Secret.Length < 16)
                return -1;

            return Data.DiscuzCloud.UpdateUserConnectInfo(userConnectInfo);
        }

        /// <summary>
        /// 根据openid获取用户的互联信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static UserConnectInfo GetUserConnectInfo(string openId)
        {
            if (openId.Length < 32)
                return null;

            return Data.DiscuzCloud.GetUserConnectInfo(openId);
        }

        /// <summary>
        /// 根据uid获取用户的互联信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserConnectInfo GetUserConnectInfo(int userId)
        {
            if (userId < 0)
                return null;

            return Data.DiscuzCloud.GetUserConnectInfo(userId);
        }

        /// <summary>
        /// 删除指定openid的用户互联信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static int DeleteUserConnectInfo(string openId)
        {
            if (openId.Length < 32)
                return -1;
            return Data.DiscuzCloud.DeleteUserConnectInfo(-1, openId, "openid");
        }

        /// <summary>
        /// 删除指定用户的互联信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int DeleteUserConnectInfo(int userId)
        {
            if (userId < 1)
                return -1;

            return Data.DiscuzCloud.DeleteUserConnectInfo(userId, "", "uid");
        }

        /// <summary>
        /// 返回当前用户是否绑定了QQ互联
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool OnlineUserIsBindConnect(int userId)
        {
            string status = Utils.GetCookie("bindconnect");
            if (string.IsNullOrEmpty(status) && userId > 0)
            {
                status = IsBindConnect(userId).ToString().ToLower();
                Utils.WriteCookie("bindconnect", status);
            }
            if (userId < 1 && !string.IsNullOrEmpty(status))
            {
                status = "";
                Utils.WriteCookie("bindconnect", status);
            }
            return status == "true";
        }

        /// <summary>
        /// 返回指定用户是否绑定了QQ互联
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool IsBindConnect(int userId)
        {
            if (DiscuzCloudConfigs.GetConfig().Connectenabled == 0 || userId < 1)
                return false;

            return GetUserConnectInfo(userId) != null;
        }

        /// <summary>
        /// 创建主题pushfeed到云平台的日志
        /// </summary>
        /// <param name="feedInfo"></param>
        /// <returns></returns>
        public static int CreateTopicPushFeedLog(TopicPushFeedInfo feedInfo)
        {
            if (feedInfo == null || feedInfo.TopicId < 0 || feedInfo.Uid < 0 || feedInfo.AuthorToken.Length < 16 || feedInfo.AuthorSecret.Length < 16)
                return -1;

            return Data.DiscuzCloud.CreateTopicPushFeedLog(feedInfo);
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
            if (tid < 0)
                return null;
            return Data.DiscuzCloud.GetTopicPushFeedLog(tid);
        }

        /// <summary>
        /// 删除指定主题id的pushfeed日志信息
        /// (当删除某feed到用户空间和微博的主题时,需要调用云平台接口来移除之前的feed信息,操作之后,该日志中的信息也可以删除了)
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static int DeleteTopicPushFeedLog(int tid)
        {
            if (tid < 0)
                return -1;
            return Data.DiscuzCloud.DeleteTopicPushFeedLog(tid);
        }

        /// <summary>
        /// 发送feed请求到云平台
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="post"></param>
        /// <param name="attachments"></param>
        /// <param name="connectInfo"></param>
        /// <returns></returns>
        public static bool PushFeedToDiscuzCloud(TopicInfo topic, PostInfo post, AttachmentInfo[] attachments, UserConnectInfo connectInfo, string ip, string rootUrl)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            List<DiscuzOAuthParameter> parmlist = new List<DiscuzOAuthParameter>();
            parmlist.Add(new DiscuzOAuthParameter("client_ip", ip));
            parmlist.Add(new DiscuzOAuthParameter("thread_id", topic.Tid.ToString()));
            parmlist.Add(new DiscuzOAuthParameter("author_id", topic.Posterid.ToString()));
            parmlist.Add(new DiscuzOAuthParameter("author", topic.Poster));
            parmlist.Add(new DiscuzOAuthParameter("forum_id", topic.Fid.ToString()));
            parmlist.Add(new DiscuzOAuthParameter("p_id", post.Pid.ToString()));
            parmlist.Add(new DiscuzOAuthParameter("subject", topic.Title));

            #region 构造postparmsinfo

            GeneralConfigInfo generalConfig = GeneralConfigs.GetConfig();
            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Sdetail = post.Message;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Bbcodemode = generalConfig.Bbcodemode;
            postpramsInfo.Parseurloff = post.Parseurloff;
            postpramsInfo.Bbcodeoff = post.Bbcodeoff;
            postpramsInfo.Signature = 0;
            postpramsInfo.Allowhtml = post.Htmlon;
            postpramsInfo.Pid = post.Pid;
            postpramsInfo.Showimages = 1 - post.Smileyoff;
            postpramsInfo.Smileyoff = post.Smileyoff;
            postpramsInfo.Smiliesmax = generalConfig.Smiliesmax;
            //判断是否为回复可见帖, hide=0为不解析[hide]标签, hide>0解析为回复可见字样, hide=-1解析为以下内容回复可见字样显示真实内容
            //将逻辑判断放入取列表的循环中处理,此处只做是否为回复人的判断，主题作者也该可见
            postpramsInfo.Hide = 0;

            #endregion
            parmlist.Add(new DiscuzOAuthParameter("html_content", UBB.UBBToHTML(postpramsInfo)));
            parmlist.Add(new DiscuzOAuthParameter("bbcode_content", post.Message));
            parmlist.Add(new DiscuzOAuthParameter("read_permission", "0"));
            parmlist.Add(new DiscuzOAuthParameter("u_id", topic.Posterid.ToString()));
            parmlist.Add(new DiscuzOAuthParameter("f_type", connectInfo.AllowPushFeed.ToString()));

            StringBuilder attachUrlList = new StringBuilder();
            int attachCount = 0;
            if (attachments != null)
            {
                foreach (AttachmentInfo info in attachments)
                {
                    if (attachCount < 3 && info.Filetype.IndexOf("image") > -1 && info.Attachprice <= 0)
                    {
                        attachUrlList.AppendFormat("|{0}upload/{1}", rootUrl, info.Filename.Replace("\\", "/"));
                        attachCount++;
                    }
                }
            }

            parmlist.Add(new DiscuzOAuthParameter("attach_images", attachUrlList.ToString().TrimStart('|')));

            DiscuzOAuth oAuth = new DiscuzOAuth();
            string queryStr = "";
            string feedUrl = oAuth.GetOAuthUrl(API_CONNECT_URL + "connect/feed/new", "POST",
                config.Connectappid, config.Connectappkey, connectInfo.Token, connectInfo.Secret, "", "", parmlist, out queryStr);

            Utils.GetHttpWebResponse(feedUrl, queryStr);
            return true;
        }

        /// <summary>
        /// 发送删除已发送feed的请求到云平台接口
        /// </summary>
        /// <param name="feedInfo"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool DeletePushedFeedInDiscuzCloud(TopicPushFeedInfo feedInfo, string ip)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            List<DiscuzOAuthParameter> parmlist = new List<DiscuzOAuthParameter>();
            parmlist.Add(new DiscuzOAuthParameter("client_ip", ip));
            parmlist.Add(new DiscuzOAuthParameter("thread_id", feedInfo.TopicId.ToString()));

            DiscuzOAuth oAuth = new DiscuzOAuth();
            string queryStr = "";
            string deleteFeedUrl = oAuth.GetOAuthUrl(API_CONNECT_URL + "connect/feed/remove", "POST", config.Connectappid, config.Connectappkey,
                feedInfo.AuthorToken, feedInfo.AuthorSecret, "", "", parmlist, out queryStr);

            Utils.GetHttpWebResponse(deleteFeedUrl, queryStr);
            return true;
        }

        /// <summary>
        /// 获取当前用户发送feed到云平台的设置(0不发送,1发送到Qzone,2发送到微博,3都发送)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetOnlineUserCloudFeedStatus(int userId)
        {
            string status = Utils.GetCookie("cloud_feed_status");
            if (!string.IsNullOrEmpty(status))
            {
                string[] statusArray = status.Split('|');
                if (statusArray.Length == 2 && userId == Utils.StrToInt(statusArray[0], -1))
                    return Utils.StrToInt(statusArray[1], 0);
            }
            UserConnectInfo connectInfo = DiscuzCloud.GetUserConnectInfo(userId);
            if (connectInfo != null)
            {
                Utils.WriteCookie("cloud_feed_status", string.Format("{0}|{1}", userId, connectInfo.AllowPushFeed));
                return connectInfo.AllowPushFeed;
            }
            return 0;
        }

        /// <summary>
        /// 创建用户绑定QQ的记录
        /// </summary>
        /// <param name="bindLog"></param>
        /// <returns></returns>
        public static int CreateUserConnectBindLog(UserBindConnectLog bindLog)
        {
            return Data.DiscuzCloud.CreateUserConnectBindLog(bindLog);
        }

        /// <summary>
        /// 更新用户绑定QQ的记录
        /// </summary>
        /// <param name="bindLog"></param>
        /// <returns></returns>
        public static int UpdateUserConnectBindLog(UserBindConnectLog bindLog)
        {
            return Data.DiscuzCloud.UpdateUserConnectBindLog(bindLog);
        }

        /// <summary>
        /// 获取用户绑定QQ的记录
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static UserBindConnectLog GetUserConnectBindLog(string openId)
        {
            return Data.DiscuzCloud.GetUserConnectBindLog(openId);
        }

        #region private method

        /// <summary>
        /// 从云平台调用指定方法并返回指定类型(T)信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="mParams"></param>
        /// <returns></returns>
        private static BaseCloudResponse<T> GetCloudResponse<T>(string method, DiscuzCloudMethodParameter mParams)
        {
            string timeStamp = mParams.Find("sTimestamp");
            timeStamp = string.IsNullOrEmpty(timeStamp) ? Utils.ConvertToUnixTimestamp(DateTime.Now).ToString() : timeStamp;

            string postData = string.Format("format={0}&method={1}&sId={2}&sig={3}&ts={4}{5}"
                                , FORMAT, method, DiscuzCloudConfigs.GetConfig().Cloudsiteid,
                                GetCloudMethodSignature(method, timeStamp, mParams), timeStamp, mParams.GetPostData());

            string response = Utils.GetHttpWebResponse(CLOUD_URL, postData);
            try
            {
                return JavaScriptConvert.DeserializeObject<BaseCloudResponse<T>>(response);
            }
            catch
            {
                BaseCloudResponse<string> err = JavaScriptConvert.DeserializeObject<BaseCloudResponse<string>>(response);
                BaseCloudResponse<T> errObj = new BaseCloudResponse<T>();
                errObj.ErrCode = err.ErrCode;
                errObj.ErrMessage = err.ErrMessage;
                return errObj;
            }
        }

        /// <summary>
        /// 获取云平台方法签名
        /// </summary>
        /// <param name="method"></param>
        /// <param name="timeStamp"></param>
        /// <param name="mParams"></param>
        /// <returns></returns>
        private static string GetCloudMethodSignature(string method, string timeStamp, DiscuzCloudMethodParameter mParams)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();

            return Utils.MD5(string.Format("format={0}&method={1}&sId={2}{3}|{4}|{5}",
                FORMAT, method, config.Cloudsiteid, mParams.GetPostData(), config.Cloudsitekey, timeStamp));
        }

        /// <summary>
        /// 获取云平台后台调用签名(产品后台iframe链接和跳转链接的附带签名)
        /// </summary>
        /// <param name="mParams"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private static string GetCloudIframeSignature(DiscuzCloudMethodParameter mParams, string timeStamp)
        {
            return Utils.MD5(string.Format("{0}|{1}|{2}", mParams.GetPostData().TrimStart('&'), DiscuzCloudConfigs.GetConfig().Cloudsitekey, timeStamp));
        }

        /// <summary>
        /// 获取云平台后台调用链接(产品后台iframe链接和跳转链接)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mParams"></param>
        /// <returns></returns>
        private static string GetCloudCpUrl(string target, DiscuzCloudMethodParameter mParams)
        {
            string timeStamp = Utils.ConvertToUnixTimestamp(DateTime.Now).ToString();
            return CLOUD_CP_URL + target + "?ts=" + timeStamp + mParams.GetPostData() + "&sig=" + GetCloudIframeSignature(mParams, timeStamp);
        }

        #endregion
    }

    /// <summary>
    /// 读取并保存用户QZone头像的异步操作类
    /// </summary>
    public class QZoneAvatar
    {
        private delegate bool GetUserQZoneAvatar(UserConnectInfo userConnectInfo);

        private GetUserQZoneAvatar getAvatar_asyncCallback;

        public void AsyncGetAvatar(UserConnectInfo userConnectInfo)
        {
            getAvatar_asyncCallback = new GetUserQZoneAvatar(SaveUserAvatar);
            getAvatar_asyncCallback.BeginInvoke(userConnectInfo, null, null);
        }

        private bool SaveUserAvatar(UserConnectInfo userConnectInfo)
        {
            string formatUid = Avatars.FormatUid(userConnectInfo.Uid.ToString());
            string avatarFileName = string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_",
               BaseConfigs.GetForumPath, formatUid.Substring(0, 3), formatUid.Substring(3, 2), formatUid.Substring(5, 2), formatUid.Substring(7, 2));

            avatarFileName = Utils.GetMapPath(avatarFileName);
            string url = string.Format("http://avatar.connect.discuz.qq.com/{0}/{1}", DiscuzCloudConfigs.GetConfig().Connectappid, userConnectInfo.OpenId);

            if (!Directory.Exists(avatarFileName))
                Utils.CreateDir(avatarFileName);

            if (!Thumbnail.MakeRemoteThumbnailImage(url, avatarFileName + "large.jpg", 200, 200))
                return false;

            Image tmpImage = Image.FromFile(avatarFileName + "large.jpg");
            if (tmpImage.Width * 0.8 <= 130)
            {
                Thumbnail.MakeThumbnailImage(avatarFileName + "large.jpg", avatarFileName + "medium.jpg",
                          (int)(tmpImage.Width * 0.8),
                          (int)(tmpImage.Height * 0.8));
                Thumbnail.MakeThumbnailImage(avatarFileName + "large.jpg", avatarFileName + "small.jpg",
                          (int)(tmpImage.Width * 0.6),
                          (int)(tmpImage.Height * 0.6));
            }
            else
            {
                Thumbnail.MakeThumbnailImage(avatarFileName + "large.jpg", avatarFileName + "medium.jpg",
                         (int)(tmpImage.Width * 0.5),
                         (int)(tmpImage.Height * 0.5));
                Thumbnail.MakeThumbnailImage(avatarFileName + "large.jpg", avatarFileName + "small.jpg",
                         (int)(tmpImage.Width * 0.3),
                         (int)(tmpImage.Height * 0.3));
            }
            try
            {
                tmpImage.Dispose();
            }
            catch { }

            //当支持FTP上传头像时,使用FTP上传远程头像
            if (FTPs.GetForumAvatarInfo.Allowupload == 1)
            {
                FTPs ftps = new FTPs();
                string ftpAvatarFileName = string.Format("/avatars/upload/{0}/{1}/{2}/",
                       formatUid.Substring(0, 3), formatUid.Substring(3, 2), formatUid.Substring(5, 2));
                ftps.UpLoadFile(ftpAvatarFileName, avatarFileName + "large.jpg", FTPs.FTPUploadEnum.ForumAvatar);
                ftps.UpLoadFile(ftpAvatarFileName, avatarFileName + "medium.jpg", FTPs.FTPUploadEnum.ForumAvatar);
                ftps.UpLoadFile(ftpAvatarFileName, avatarFileName + "small.jpg", FTPs.FTPUploadEnum.ForumAvatar);
            }
            return true;
        }
    }

    /// <summary>
    /// 推送主题信息到云平台的异步操作类
    /// </summary>
    public class PushFeed
    {
        private delegate bool PushFeedToCloud(TopicInfo topic, PostInfo post, AttachmentInfo[] attachments, int feedStatus);
        private PushFeedToCloud pushFeed_asyncCallback;
        private string rootUrl = Utils.GetRootUrl(BaseConfigs.GetForumPath);
        private string ip = DNTRequest.GetIP();

        public void TopicPushFeed(TopicInfo topic, PostInfo post, AttachmentInfo[] attachments, int feedStatus)
        {
            pushFeed_asyncCallback = new PushFeedToCloud(PushFeedToDiscuzCloud);
            pushFeed_asyncCallback.BeginInvoke(topic, post, attachments, feedStatus, null, null);
        }

        private bool PushFeedToDiscuzCloud(TopicInfo topic, PostInfo post, AttachmentInfo[] attachments, int feedStatus)
        {
            //如果传入数据不合法，以及主题需要审核或者是回复可见的，需要付费的，就不推送到云平台
            if (topic == null || post == null || topic.Tid < 0 || topic.Posterid < 0 || post.Tid != topic.Tid || post.Pid < 0 ||
                topic.Displayorder < 0 || topic.Hide == 1 || topic.Price > 0 || post.Invisible != 0 || feedStatus < 0 || feedStatus > 3)
                return false;

            UserConnectInfo userConnectInfo = DiscuzCloud.GetUserConnectInfo(topic.Posterid);
            if (userConnectInfo == null || feedStatus == 0)
                return false;
            //设置用户自选的操作状态
            userConnectInfo.AllowPushFeed = feedStatus;

            if (DiscuzCloud.PushFeedToDiscuzCloud(topic, post, attachments, userConnectInfo, ip, rootUrl))
            {
                TopicPushFeedInfo feedInfo = new TopicPushFeedInfo();
                feedInfo.TopicId = topic.Tid;
                feedInfo.Uid = topic.Posterid;
                feedInfo.AuthorToken = userConnectInfo.Token;
                feedInfo.AuthorSecret = userConnectInfo.Secret;
                DiscuzCloud.CreateTopicPushFeedLog(feedInfo);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 删除云平台已发送feed的异步操作类
    /// </summary>
    public class DeleteFeed
    {
        private delegate bool DeletePushedFeed(int tid);
        private DeletePushedFeed deleteFeed_asyncCallback;
        private string ip = DNTRequest.GetIP();

        public void DeleteTopicPushedFeed(int tid)
        {
            deleteFeed_asyncCallback = new DeletePushedFeed(DeletePushedFeedInCloud);
            deleteFeed_asyncCallback.BeginInvoke(tid, null, null);
        }

        private bool DeletePushedFeedInCloud(int tid)
        {
            if (tid < 0)
                return false;

            TopicPushFeedInfo feedInfo = DiscuzCloud.GetTopicPushFeedLog(tid);
            if (feedInfo == null)
                return false;

            bool result = DiscuzCloud.DeletePushedFeedInDiscuzCloud(feedInfo, ip);
            if (result)
                DiscuzCloud.DeleteTopicPushFeedLog(tid);
            return true;
        }
    }
}
