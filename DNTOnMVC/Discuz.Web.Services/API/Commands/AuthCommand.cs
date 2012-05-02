using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Plugin.PasswordMode;
using System.Data;

namespace Discuz.Web.Services.API.Commands
{
    /// <summary>
    /// 为客户端创建令牌,仅用于桌面应用程序,web不可用
    /// </summary>
    public sealed class CreateTokenCommand : Command
    {
        public CreateTokenCommand()
            : base("auth.createtoken", false)
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.WEB)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                return false;
            }
            TokenInfo token = new TokenInfo();

            if (System.Web.HttpContext.Current.Request.Cookies["dnt"] == null || System.Web.HttpContext.Current.Request.Cookies["dnt"]["expires"] == null)
            {
                token.Token = "";
                result = commandParam.Format == FormatType.JSON ? string.Empty : SerializationHelper.Serialize(token);
                return true;
            }

            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(commandParam.GeneralConfig.Passwordkey, commandParam.GeneralConfig.Onlinetimeout);
            int olid = oluserinfo.Olid;

            string expires = string.Empty;
            DateTime expireUTCTime;

            expires = System.Web.HttpContext.Current.Request.Cookies["dnt"]["expires"].ToString();
            ShortUserInfo userinfo = Discuz.Forum.Users.GetShortUserInfo(oluserinfo.Userid);
            expireUTCTime = DateTime.Parse(userinfo.Lastvisit).ToUniversalTime().AddSeconds(Convert.ToDouble(expires));
            expires = Utils.ConvertToUnixTimestamp(expireUTCTime).ToString();

            string time = string.Empty;
            if (oluserinfo == null)
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            else
                time = DateTime.Parse(oluserinfo.Lastupdatetime).ToString("yyyy-MM-dd HH:mm:ss");

            string authToken = Common.DES.Encode(string.Format("{0},{1},{2}", olid.ToString(), time, expires), commandParam.AppInfo.Secret.Substring(0, 10)).Replace("+", "[");
            token.Token = authToken;
            result = commandParam.Format == FormatType.JSON ? authToken : SerializationHelper.Serialize(token);
            return true;
        }
    }

    /// <summary>
    /// 获得会话
    /// </summary>
    public sealed class GetSessionCommand : Command
    {

        public GetSessionCommand()
            : base("auth.getsession", false)
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.GetDNTParam("auth_token") == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string authToken = commandParam.GetDNTParam("auth_token").ToString().Replace("[", "+");
            string a = Discuz.Common.DES.Decode(authToken, commandParam.AppInfo.Secret.Substring(0, 10));
            string[] userstr = a.Split(',');
            if (userstr.Length != 3)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            int olid = Utils.StrToInt(userstr[0], -1);
            OnlineUserInfo oluser = OnlineUsers.GetOnlineUser(olid);
            if (oluser == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }
            string time = DateTime.Parse(oluser.Lastupdatetime).ToString("yyyy-MM-dd HH:mm:ss");
            if (time != userstr[1])
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }
            byte[] md5_result = System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(olid.ToString() + commandParam.AppInfo.Secret));

            StringBuilder sessionkey_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sessionkey_builder.Append(b.ToString("x2"));

            string sessionkey = string.Format("{0}-{1}", sessionkey_builder.ToString(), oluser.Userid.ToString());
            SessionInfo session = new SessionInfo();
            session.SessionKey = sessionkey;
            session.UId = oluser.Userid;
            session.UserName = oluser.Username;
            session.Expires = Utils.StrToInt(userstr[2], 0);

            if (commandParam.Format == FormatType.JSON)
                result = string.Format(@"{{""session_key"":""{0}"",""uid"":{1},""user_name"":""{2}"",""expires"":{3}}}", sessionkey, commandParam.LocalUid, session.UserName, session.Expires);
            else
                result = SerializationHelper.Serialize(session);

            OnlineUsers.UpdateAction(olid, UserAction.Login.ActionID, 0, GeneralConfigs.GetConfig().Onlinetimeout);
            return true;
        }
    }

    /// <summary>
    /// 注册新用户
    /// </summary>
    public sealed class UserRegisterCommand : Command
    {
        public UserRegisterCommand()
            : base("auth.register")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (!commandParam.CheckRequiredParams("user_name,password,email"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)//如果是桌面程序则不允许此方法
            {
                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }
                ShortUserInfo shortUserInfo = Users.GetShortUserInfo(commandParam.LocalUid);
                if (shortUserInfo == null || shortUserInfo.Adminid != 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }
            }
            else if (commandParam.LocalUid > 0)//已经登录的用户不能再注册
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_USER_ONLINE, commandParam.ParamList);
                return false;
            }

            string username = commandParam.GetDNTParam("user_name").ToString();
            string password = commandParam.GetDNTParam("password").ToString();
            string email = commandParam.GetDNTParam("email").ToString();

            bool isMD5Passwd = commandParam.GetDNTParam("password_format") != null && commandParam.GetDNTParam("password_format").ToString() == "md5" ? true : false;

            //用户名不符合规范
            if (!AuthCommandUtils.CheckUsername(username))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_USERNAME_ILLEGAL, commandParam.ParamList);
                return false;
            }

            if (Discuz.Forum.Users.GetUserId(username) != 0)//如果用户名符合注册规则, 则判断是否已存在
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_USER_ALREADY_EXIST, commandParam.ParamList);
                return false;
            }

            if (!isMD5Passwd && password.Length < 6)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (!AuthCommandUtils.CheckEmail(email, commandParam.GeneralConfig))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_EMAIL, commandParam.ParamList);
                return false;
            }

            #region Create New UserInfo

            UserInfo userInfo = new UserInfo();
            userInfo.Username = username;
            userInfo.Nickname = string.Empty;
            userInfo.Password = isMD5Passwd ? password : Utils.MD5(password);
            userInfo.Secques = string.Empty;
            userInfo.Gender = 0;
            userInfo.Adminid = 0;
            userInfo.Groupexpiry = 0;
            userInfo.Extgroupids = "";
            userInfo.Regip = DNTRequest.GetIP();
            userInfo.Joindate = Utils.GetDateTime();
            userInfo.Lastip = DNTRequest.GetIP();
            userInfo.Lastvisit = Utils.GetDateTime();
            userInfo.Lastactivity = Utils.GetDateTime();
            userInfo.Lastpost = Utils.GetDateTime();
            userInfo.Lastpostid = 0;
            userInfo.Lastposttitle = "";
            userInfo.Posts = 0;
            userInfo.Digestposts = 0;
            userInfo.Oltime = 0;
            userInfo.Pageviews = 0;
            userInfo.Credits = 0;
            userInfo.Extcredits1 = Scoresets.GetScoreSet(1).Init;
            userInfo.Extcredits2 = Scoresets.GetScoreSet(2).Init;
            userInfo.Extcredits3 = Scoresets.GetScoreSet(3).Init;
            userInfo.Extcredits4 = Scoresets.GetScoreSet(4).Init;
            userInfo.Extcredits5 = Scoresets.GetScoreSet(5).Init;
            userInfo.Extcredits6 = Scoresets.GetScoreSet(6).Init;
            userInfo.Extcredits7 = Scoresets.GetScoreSet(7).Init;
            userInfo.Extcredits8 = Scoresets.GetScoreSet(8).Init;
            userInfo.Email = email;
            userInfo.Bday = string.Empty;
            userInfo.Sigstatus = 0;

            userInfo.Tpp = 0;
            userInfo.Ppp = 0;
            userInfo.Templateid = 0;
            userInfo.Pmsound = 0;
            userInfo.Showemail = 0;
            userInfo.Salt = "0";
            int receivepmsetting = commandParam.GeneralConfig.Regadvance == 0 ? 7 : 1;
            userInfo.Newsletter = (ReceivePMSettingType)receivepmsetting;
            userInfo.Invisible = 0;
            userInfo.Newpm = commandParam.GeneralConfig.Welcomemsg == 1 ? 1 : 0;
            userInfo.Medals = "";
            userInfo.Accessmasks = 0;
            userInfo.Website = string.Empty;
            userInfo.Icq = string.Empty;
            userInfo.Qq = string.Empty;
            userInfo.Yahoo = string.Empty;
            userInfo.Msn = string.Empty;
            userInfo.Skype = string.Empty;
            userInfo.Location = string.Empty;
            userInfo.Customstatus = string.Empty;
            userInfo.Bio = string.Empty;
            userInfo.Signature = string.Empty;
            userInfo.Sightml = string.Empty;
            userInfo.Authtime = Utils.GetDateTime();

            //邮箱激活链接验证
            if (commandParam.GeneralConfig.Regverify == 1)
            {
                userInfo.Authstr = ForumUtils.CreateAuthStr(20);
                userInfo.Authflag = 1;
                userInfo.Groupid = 8;
                Emails.DiscuzSmtpMail(username, email, string.Empty, userInfo.Authstr);
            }
            //系统管理员进行后台验证
            else if (commandParam.GeneralConfig.Regverify == 2)
            {
                userInfo.Authstr = string.Empty;
                userInfo.Groupid = 8;
                userInfo.Authflag = 1;
            }
            else
            {
                userInfo.Authstr = "";
                userInfo.Authflag = 0;
                userInfo.Groupid = CreditsFacade.GetCreditsUserGroupId(0).Groupid;
            }
            userInfo.Realname = string.Empty;
            userInfo.Idcard = string.Empty;
            userInfo.Mobile = string.Empty;
            userInfo.Phone = string.Empty;

            if (commandParam.GeneralConfig.Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
            {
                userInfo.Uid = PasswordModeProvider.GetInstance().CreateUserInfo(userInfo);
            }
            else
            {
                userInfo.Uid = Discuz.Forum.Users.CreateUser(userInfo);
            }

            #endregion

            if (commandParam.GeneralConfig.Welcomemsg == 1)
            {
                PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();
                // 收件箱
                privatemessageinfo.Message = commandParam.GeneralConfig.Welcomemsgtxt;
                privatemessageinfo.Subject = "欢迎您的加入! (请勿回复本信息)";
                privatemessageinfo.Msgto = userInfo.Username;
                privatemessageinfo.Msgtoid = userInfo.Uid;
                privatemessageinfo.Msgfrom = PrivateMessages.SystemUserName;
                privatemessageinfo.Msgfromid = 0;
                privatemessageinfo.New = 1;
                privatemessageinfo.Postdatetime = Utils.GetDateTime();
                privatemessageinfo.Folder = 0;
                PrivateMessages.CreatePrivateMessage(privatemessageinfo, 0);
            }
            Statistics.ReSetStatisticsCache();

            //信息同步通知不会发向当前请求接口的应用程序，所以此处应保留，以支持论坛向其他关联应用程序发送通知
            Sync.UserRegister(userInfo.Uid, userInfo.Username, userInfo.Password, commandParam.AppInfo.APIKey);

            CreditsFacade.UpdateUserCredits(userInfo.Uid);

            if (commandParam.Format == FormatType.JSON)
                result = string.Format("\"{0}\"", userInfo.Uid);
            else
            {
                RegisterResponse rr = new RegisterResponse();
                rr.Uid = userInfo.Uid;
                result = SerializationHelper.Serialize(rr);
            }
            return true;
        }
    }

    /// <summary>
    /// 生成用户cookie密码密文
    /// </summary>
    public sealed class EncodePasswordCommand : Command
    {
        public EncodePasswordCommand()
            : base("auth.encodepassword")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("password"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string password = commandParam.GetDNTParam("password").ToString();
            bool isMD5Passwd = commandParam.GetDNTParam("password_format") != null &&
                commandParam.GetDNTParam("password_format").ToString() == "md5" ? true : false;

            EncodePasswordResponse epr = new EncodePasswordResponse();
            epr.Password = Utils.UrlEncode(ForumUtils.SetCookiePassword(isMD5Passwd ? password : Utils.MD5(password), commandParam.GeneralConfig.Passwordkey));

            result = commandParam.Format == FormatType.JSON ? string.Format("\"{0}\"", epr.Password) : SerializationHelper.Serialize(epr);
            return true;
        }
    }

    /// <summary>
    /// 终端应用程序登录接口,WEB不可用
    /// </summary>
    public sealed class LoginCommand : Command
    {
        public LoginCommand()
            : base("auth.login", false)
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.WEB)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                return false;
            }

            if (commandParam.LocalUid > 0)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_USER_ONLINE, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("user_name,password"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), false) >= 5)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_MORE_LOGIN_FAILED, commandParam.ParamList);
                return false;
            }

            string loginName = commandParam.GetDNTParam("user_name").ToString();
            string password = commandParam.GetDNTParam("password").ToString();
            string passwordFormat = commandParam.CheckRequiredParams("password_format") ? commandParam.GetDNTParam("password_format").ToString() : "";
            int expires = commandParam.GetIntParam("expires");
            expires = expires > 0 ? expires : 999;

            int userId = -1;
            ShortUserInfo userInfo = new ShortUserInfo();

            if (commandParam.GeneralConfig.Emaillogin == 1 && Utils.IsValidEmail(loginName))
            {
                DataTable dt = Users.GetUserInfoByEmail(loginName);
                if (dt.Rows.Count == 0)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_USER_NOT_EXIST, commandParam.ParamList);
                    return false;
                }
                if (dt.Rows.Count > 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SAME_USER_EMAIL, commandParam.ParamList);
                    return false;
                }
                loginName = dt.Rows[0]["username"].ToString();
                userId = TypeConverter.ObjectToInt(dt.Rows[0]["uid"]);
                userInfo.Uid = userId;
                userInfo.Username = loginName;
                userInfo.Groupid = TypeConverter.ObjectToInt(dt.Rows[0]["groupid"]);
                userInfo.Groupexpiry = TypeConverter.ObjectToInt(dt.Rows[0]["groupexpiry"]);
                userInfo.Credits = TypeConverter.ObjectToInt(dt.Rows[0]["credits"]);
                userInfo.Email = dt.Rows[0]["email"].ToString();
                userInfo.Password = dt.Rows[0]["password"].ToString();
            }
            else
            {
                userId = Users.GetUserId(loginName);
                if (userId < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_USER_NOT_EXIST, commandParam.ParamList);
                    return false;
                }
                userInfo = Users.GetShortUserInfo(userId);
            }

            int uid = -1;
            if (passwordFormat == "")
            {
                switch (commandParam.GeneralConfig.Passwordmode)
                {
                    case 0://默认模式
                        {
                            uid = Users.CheckPassword(loginName, password, true);
                            break;
                        }
                    case 1://动网兼容模式
                        {
                            uid = Users.CheckDvBbsPassword(loginName, password);
                            break;
                        }
                }
            }
            else
            {
                uid = userInfo.Password == password ? userInfo.Uid : -1;
            }

            if (uid != userInfo.Uid)
            {
                LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), true);
                result = Util.CreateErrorMessage(ErrorType.API_EC_WRONG_PASSWORD, commandParam.ParamList);
                return false;
            }

            #region 当前用户所在用户组为"禁止访问"或"等待激活"时

            if ((userInfo.Groupid == 4 || userInfo.Groupid == 5) && userInfo.Groupexpiry != 0 && userInfo.Groupexpiry <= Utils.StrToInt(DateTime.Now.ToString("yyyyMMdd"), 0))
            {
                //根据当前用户的积分获取对应积分用户组
                UserGroupInfo groupInfo = CreditsFacade.GetCreditsUserGroupId(userInfo.Credits);
                Users.UpdateUserGroup(userInfo.Uid, userInfo.Groupid);
            }

            #endregion

            if (userInfo.Groupid == 5 || userInfo.Groupid == 8)// 5-禁止访问或者需要激活帐号的用户
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_BANNED_USERGROUP, commandParam.ParamList);
                return false;
            }

            #region 无延迟更新在线信息和相关用户信息
            ForumUtils.WriteUserCookie(userInfo.Uid, expires, commandParam.GeneralConfig.Passwordkey, 0, -1);
            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(commandParam.GeneralConfig.Passwordkey, commandParam.GeneralConfig.Onlinetimeout, userInfo.Uid, "");
            OnlineUsers.UpdateAction(oluserinfo.Olid, UserAction.Login.ActionID, 0);
            LoginLogs.DeleteLoginLog(DNTRequest.GetIP());
            Users.UpdateUserCreditsAndVisit(userInfo.Uid, DNTRequest.GetIP());
            #endregion

            result = "success";
            result = commandParam.Format == FormatType.JSON ? string.Format("\"{0}\"", result) : SerializationHelper.Serialize(result);

            return true;
        }
    }

    public class AuthCommandUtils
    {
        public static bool CheckUsername(string username)
        {
            if (username.Equals(""))
                return false;
            if (username.Length > 20)
                //如果用户名超过20...
                return false;
            if (Utils.GetStringLength(username) < 3)
                return false;
            if (username.IndexOf(" ") != -1)
                //如果用户名符合注册规则, 则判断是否已存在
                return false;
            if (username.IndexOf("　") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1 || username.IndexOf("") != -1)
                //如果用户名符合注册规则, 则判断是否已存在                
                return false;
            if (username.IndexOf(":") != -1)
                //如果用户名符合注册规则, 则判断是否已存在
                return false;
            if ((!Utils.IsSafeSqlString(username)) || (!Utils.IsSafeUserInfoString(username)))
                return false;
            // 如果用户名属于禁止名单, 或者与负责发送新用户注册欢迎信件的用户名称相同...
            if (username.Trim() == PrivateMessages.SystemUserName || ForumUtils.IsBanUsername(username, GeneralConfigs.GetConfig().Censoruser))
                return false;

            return true;
        }

        public static bool CheckEmail(string email, GeneralConfigInfo config)
        {
            if (!Utils.IsValidEmail(email) || !Discuz.Forum.Users.ValidateEmail(email))
                return false;

            string emailhost = Utils.GetEmailHostName(email);
            // 允许名单规则优先于禁止名单规则
            if (config.Accessemail.Trim() != "" && !Utils.InArray(emailhost, config.Accessemail.Replace("\r\n", "\n"), "\n"))
                return false;

            if (config.Censoremail.Trim() != "" && Utils.InArray(email, config.Censoremail.Replace("\r\n", "\n"), "\n"))
                return false;
            return true;
        }
    }
}
