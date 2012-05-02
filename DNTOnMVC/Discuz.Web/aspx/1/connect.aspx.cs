using System;
using System.Text;
using System.Data;
using System.Web;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;

using Newtonsoft.Json;
using Discuz.Plugin.PasswordMode;

namespace Discuz.Web
{
    public class connect : PageBase
    {
        /// <summary>
        /// 页面操作类型
        /// </summary>
        public string action = Utils.HtmlEncode(DNTRequest.GetString("action"));
        /// <summary>
        /// 当前QQ用户曾用论坛用户名列表
        /// </summary>
        public string usedusernames = DNTRequest.GetString("con_x_usernames");
        /// <summary>
        /// 当前QQ用户生日
        /// </summary>
        public string birthday = Utils.HtmlEncode(DNTRequest.GetString("con_x_birthday"));
        /// <summary>
        /// 当前QQ用户性别
        /// </summary>
        public int gender = DNTRequest.GetInt("con_x_sex", 0);
        /// <summary>
        /// 当前QQ用户曾用email
        /// </summary>
        public string email = Utils.HtmlEncode(DNTRequest.GetString("con_x_email"));
        /// <summary>
        /// 当前QQ用户曾用昵称
        /// </summary>
        //public string nickname = DNTRequest.GetString("con_x_nick");
        /// <summary>
        /// 当前QQ用户的qzone头像地址
        /// </summary>
        public string avatarurl = "";
        /// <summary>
        /// 当前QQ用户的云平台OPENID
        /// </summary>
        public string openid = Utils.HtmlEncode(DNTRequest.GetString("openid"));
        /// <summary>
        /// 是否允许注册新用户
        /// </summary>
        public bool allowreg = false;
        /// <summary>
        /// 绑定操作页面的tab类型
        /// </summary>
        public int connectswitch = 0;
        /// <summary>
        /// 当前QQ用户是否超过了最大的注册次数
        /// </summary>
        public bool isbindoverflow = false;
        /// <summary>
        /// 用户提交的登录用户名
        /// </summary>
        public string postusername = Utils.HtmlEncode(DNTRequest.GetString("loginusername"));
        /// <summary>
        /// 用户提交的登录密码
        /// </summary>
        public string postpassword = Utils.HtmlEncode(DNTRequest.GetString("password"));
        /// <summary>
        /// 绑定成功后的通知脚本
        /// </summary>
        public string notifyscript = "";
        /// <summary>
        /// 用户Connect信息
        /// </summary>
        public UserConnectInfo userconnectinfo;
        /// <summary>
        /// 云平台配置类
        /// </summary>
        public DiscuzCloudConfigInfo cloudconfig = DiscuzCloudConfigs.GetConfig();

        protected override void ShowPage()
        {
            if (!DiscuzCloud.GetCloudServiceEnableStatus("connect"))
            {
                AddErrLine("QQ登录功能已关闭");
                return;
            }

            switch (action)
            {
                case "access":
                    if (!CheckCallbackSignature(DNTRequest.GetString("con_sig")))
                    {
                        AddErrLine("非法请求");
                        return;
                    }

                    OAuthAccessTokenInfo tokenInfo = DiscuzCloud.GetConnectAccessTokenInfo();
                    if (tokenInfo == null)
                    {
                        AddErrLine("QQ登录过程中出现异常,请尝试再次登录");
                        return;
                    }

                    userconnectinfo = DiscuzCloud.GetUserConnectInfo(tokenInfo.Openid);
                    if (userconnectinfo == null)
                    {
                        userconnectinfo = new UserConnectInfo();
                        userconnectinfo.OpenId = tokenInfo.Openid;
                        userconnectinfo.Token = tokenInfo.Token;
                        userconnectinfo.Secret = tokenInfo.Secret;
                        userconnectinfo.AllowVisitQQUserInfo = DNTRequest.GetInt("con_is_user_info", 0);
                        userconnectinfo.AllowPushFeed = DNTRequest.GetInt("con_is_feed", 0) == 1 ? 3 : 0;
                        userconnectinfo.CallbackInfo = usedusernames + "&" + birthday + "&" + gender + "&" + email;
                        DiscuzCloud.CreateUserConnectInfo(userconnectinfo);
                    }
                    else if (userconnectinfo.Uid > 0)
                    {
                        if (userid > 0)
                        {
                            SetBackLink("index.aspx");
                            AddErrLine(userconnectinfo.Uid != userid ? "该QQ已经绑定了其他帐号" : "该QQ用户已登录");
                            return;
                        }

                        ShortUserInfo userInfo = Users.GetShortUserInfo(userconnectinfo.Uid);
                        string redirectUrl = "";
                        //如果userInfo==null，可能是管理员后台删除了这个帐号,则用户的ConnnectInfo也需要被解绑重置
                        if (userInfo == null)
                        {
                            DiscuzCloud.UnbindUserConnectInfo(userconnectinfo.OpenId);
                            redirectUrl = HttpContext.Current.Request.RawUrl;
                        }
                        else
                        {
                            redirectUrl = forumpath + "index.aspx";
                            //如果云端的token和Secret发生改变,则更新本地保存的token和Secret
                            if (tokenInfo.Token != userconnectinfo.Token || tokenInfo.Secret != userconnectinfo.Secret)
                            {
                                userconnectinfo.Token = tokenInfo.Token;
                                userconnectinfo.Secret = tokenInfo.Secret;
                                DiscuzCloud.UpdateUserConnectInfo(userconnectinfo);
                            }
                            LoginUser(userInfo);
                        }
                        HttpContext.Current.Response.Redirect(redirectUrl);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        string[] callbackInfo = userconnectinfo.CallbackInfo.Split('&');
                        if (callbackInfo.Length == 4)
                        {
                            usedusernames = string.IsNullOrEmpty(usedusernames) ? callbackInfo[0] : usedusernames;
                            birthday = string.IsNullOrEmpty(birthday) ? callbackInfo[1] : birthday;
                            gender = gender == 0 ? Utils.StrToInt(callbackInfo[2], 0) : gender;
                            email = string.IsNullOrEmpty(email) ? callbackInfo[3] : email;
                        }
                    }
                    UserBindConnectLog userBindLog = DiscuzCloud.GetUserConnectBindLog(userconnectinfo.OpenId);
                    isbindoverflow = userBindLog != null && cloudconfig.Maxuserbindcount > 0 && userBindLog.BindCount >= cloudconfig.Maxuserbindcount;

                    allowreg = config.Regstatus != 0 && cloudconfig.Allowconnectregister == 1 && !isbindoverflow;
                    connectswitch = allowreg && userid < 0 ? 1 : 2;

                    #region convert used username list
                    byte[] bt = Convert.FromBase64String(usedusernames);
                    usedusernames = System.Text.Encoding.Default.GetString(bt);

                    #endregion
                    avatarurl = string.Format("http://avatar.connect.discuz.qq.com/{0}/{1}", DiscuzCloudConfigs.GetConfig().Connectappid, userconnectinfo.OpenId);
                    openid = userconnectinfo.OpenId;
                    break;
                case "bind":
                    if (ispost)
                    {
                        if (DNTRequest.GetString("bind_type") == "new")
                            RegisterAndBind();
                        else
                        {
                            if (userid < 0)
                                BindForumExistedUser();
                            else
                                BindLoginedUser();
                        }
                    }
                    break;
                case "unbind":
                    if (userid < 1)
                    {
                        AddErrLine("未登录用户无法进行该操作");
                        return;
                    }
                    userconnectinfo = DiscuzCloud.GetUserConnectInfo(userid);
                    if (userconnectinfo == null)
                    {
                        AddErrLine("您并没有绑定过QQ,不需要执行该操作");
                        return;
                    }
                    if (ispost)
                    {
                        if (userconnectinfo.IsSetPassword == 0)
                        {
                            string passwd = DNTRequest.GetString("newpasswd");
                            if (string.IsNullOrEmpty(passwd))
                            {
                                AddErrLine("您必须为帐号设置新密码才能解除绑定");
                                return;
                            }
                            if (passwd.Length < 6)
                            {
                                AddErrLine("密码不得少于6个字符");
                                return;
                            }

                            if (passwd != DNTRequest.GetString("confirmpasswd"))
                            {
                                AddErrLine("两次输入的新密码不一致");
                                return;
                            }
                            UserInfo userInfo = Users.GetUserInfo(userid);
                            userInfo.Password = passwd;
                            Users.ResetPassword(userInfo);
                            //同步其他应用密码
                            Sync.UpdatePassword(userInfo.Username, userInfo.Password, "");

                            if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("changesecques")))
                                Users.UpdateUserSecques(userid, DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"));

                            ForumUtils.WriteCookie("password", ForumUtils.SetCookiePassword(userInfo.Password, config.Passwordkey));
                            OnlineUsers.UpdatePassword(olid, userInfo.Password);
                        }

                        DiscuzCloud.UnbindUserConnectInfo(userconnectinfo.OpenId);
                        UserBindConnectLog bindLog = DiscuzCloud.GetUserConnectBindLog(userconnectinfo.OpenId);
                        if (bindLog != null)
                        {
                            bindLog.Type = 2;
                            DiscuzCloud.UpdateUserConnectBindLog(bindLog);
                        }

                        AddMsgLine("解绑成功");
                        string reurl = Utils.UrlDecode(ForumUtils.GetReUrl());
                        SetUrl(reurl.IndexOf("register.aspx") < 0 ? reurl : forumpath + "index.aspx");
                        SetMetaRefresh();
                    }
                    break;
                default:
                    if (isbindconnect)
                    {
                        AddErrLine("用户已登录");
                        return;
                    }
                    HttpContext.Current.Response.Redirect(DiscuzCloud.GetConnectLoginPageUrl(userid));
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    break;
            }
        }

        /// <summary>
        /// 绑定当前在线用户
        /// </summary>
        private void BindLoginedUser()
        {
            userconnectinfo = DiscuzCloud.GetUserConnectInfo(openid);
            if (userconnectinfo == null || userconnectinfo.Uid > 0)
            {
                AddErrLine("Connect信息异常,登录失败,请尝试再次登录");
                return;
            }
            if (DiscuzCloud.IsBindConnect(userid))
            {
                AddErrLine("该用户已经绑定了QQ,无法再次绑定");
                return;
            }
            userconnectinfo.Uid = userid;
            userconnectinfo.IsSetPassword = 1;
            DiscuzCloud.UpdateUserConnectInfo(userconnectinfo);
            UserBindConnectLog bindLog = DiscuzCloud.GetUserConnectBindLog(userconnectinfo.OpenId);
            if (bindLog == null)
            {
                bindLog = new UserBindConnectLog();
                bindLog.OpenId = userconnectinfo.OpenId;
                bindLog.Uid = userconnectinfo.Uid;
                bindLog.Type = 1;
                bindLog.BindCount = 1;
                DiscuzCloud.CreateUserConnectBindLog(bindLog);
            }
            else
            {
                bindLog.Uid = userconnectinfo.Uid;
                bindLog.Type = 1;
                DiscuzCloud.UpdateUserConnectBindLog(bindLog);
            }

            SetUrl("index.aspx");
            SetMetaRefresh();
            SetShowBackLink(false);
            AddMsgLine("QQ绑定成功,继续浏览");
            Utils.WriteCookie("bindconnect", "true");//将当前登录用户是否绑定QQ互联的状态设置为true
            ShortUserInfo userInfo = Users.GetShortUserInfo(userid);
            notifyscript = GetNotifyScript(userconnectinfo, userInfo.Username, userInfo.Bday, userInfo.Gender,
                                            userInfo.Email, userInfo.Showemail, DNTRequest.GetInt("useqqavatar", 2), "loginbind");
        }

        /// <summary>
        /// 绑定论坛已存在的用户
        /// </summary>
        private void BindForumExistedUser()
        {
            if (LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), false) >= 5)
            {
                AddErrLine("您已经多次输入密码错误, 请15分钟后再登录");
                return;
            }

            if (config.Emaillogin == 1 && Utils.IsValidEmail(postusername))
            {
                DataTable dt = Users.GetUserInfoByEmail(postusername);
                if (dt.Rows.Count == 0)
                {
                    AddErrLine("用户不存在");
                    return;
                }
                if (dt.Rows.Count > 1)
                {
                    AddErrLine("您所使用Email不唯一，请使用用户名登陆");
                    return;
                }
                if (dt.Rows.Count == 1)
                {
                    postusername = dt.Rows[0]["username"].ToString();
                }
            }

            if (config.Emaillogin == 0)
            {
                if ((Users.GetUserId(postusername) == 0))
                    AddErrLine("用户不存在");
            }

            if (string.IsNullOrEmpty(postpassword))
                AddErrLine("密码不能为空");

            if (IsErr()) return;

            ShortUserInfo userInfo = GetShortUserInfo();

            if (userInfo != null)
            {
                #region 当前用户所在用户组为"禁止访问"或"等待激活"时
                if ((userInfo.Groupid == 4 || userInfo.Groupid == 5) && userInfo.Groupexpiry != 0 && userInfo.Groupexpiry <= Utils.StrToInt(DateTime.Now.ToString("yyyyMMdd"), 0))
                {
                    //根据当前用户的积分获取对应积分用户组
                    UserGroupInfo groupInfo = CreditsFacade.GetCreditsUserGroupId(userInfo.Credits);
                    usergroupid = groupInfo.Groupid != 0 ? groupInfo.Groupid : usergroupid;
                    userInfo.Groupid = usergroupid;
                    Users.UpdateUserGroup(userInfo.Uid, usergroupid);
                }

                if (userInfo.Groupid == 5)// 5-禁止访问
                {
                    AddErrLine("该用户已经被禁止访问,无法绑定");
                    return;
                }
                #endregion


                //读取当前用户的OPENID信息
                userconnectinfo = DiscuzCloud.GetUserConnectInfo(openid);
                if (userconnectinfo == null || userconnectinfo.Uid > 0)
                {
                    AddErrLine("Connect信息异常,登录失败,请尝试再次登录");
                    return;
                }
                if (DiscuzCloud.IsBindConnect(userInfo.Uid))
                {
                    AddErrLine("该用户已经绑定了QQ,无法再次绑定");
                    return;
                }
                userconnectinfo.Uid = userInfo.Uid;
                userconnectinfo.IsSetPassword = 1;
                DiscuzCloud.UpdateUserConnectInfo(userconnectinfo);

                UserBindConnectLog bindLog = DiscuzCloud.GetUserConnectBindLog(userconnectinfo.OpenId);
                if (bindLog == null)
                {
                    bindLog = new UserBindConnectLog();
                    bindLog.OpenId = userconnectinfo.OpenId;
                    bindLog.Uid = userconnectinfo.Uid;
                    bindLog.Type = 1;
                    bindLog.BindCount = 1;
                    DiscuzCloud.CreateUserConnectBindLog(bindLog);
                }
                else
                {
                    bindLog.Uid = userconnectinfo.Uid;
                    bindLog.Type = 1;
                    DiscuzCloud.UpdateUserConnectBindLog(bindLog);
                }

                if (userInfo.Groupid != 8)
                {
                    LoginUser(userInfo);
                    AddMsgLine("QQ登录成功,继续浏览");
                }
                else
                {
                    AddMsgLine("帐号绑定成功,但需要管理员审核通过才能登录");
                }
                SetUrl("index.aspx");
                SetMetaRefresh();
                SetShowBackLink(false);
                notifyscript = GetNotifyScript(userconnectinfo, userInfo.Username, userInfo.Bday, userInfo.Gender,
                                            userInfo.Email, userInfo.Showemail, DNTRequest.GetInt("useqqavatar", 2), "registerbind");
                return;
            }
            else
            {
                int errcount = LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), true);
                if (errcount > 5)
                    AddErrLine("您已经输入密码5次错误, 请15分钟后再试");
                else
                    AddErrLine(string.Format("密码或安全提问第{0}次错误, 您最多有5次机会重试", errcount));
            }
            if (IsErr()) return;
        }

        /// <summary>
        /// 在论坛注册一个新用户并绑定
        /// </summary>
        private void RegisterAndBind()
        {
            if (userid > 0)
            {
                AddErrLine("当前已有用户登录,无法注册");
                return;
            }

            if (config.Regstatus < 1 || cloudconfig.Allowconnectregister == 0)
            {
                AddErrLine("论坛当前禁止新的QQ会员登录");
                return;
            }

            string tmpUserName = DNTRequest.GetString(config.Antispamregisterusername);
            string email = DNTRequest.GetString(config.Antispamregisteremail).Trim().ToLower();
            string tmpBday = DNTRequest.GetString("bday").Trim();

            string errorMessage = "";
            if (!Users.PageValidateUserName(tmpUserName, out errorMessage) || !Users.PageValidateEmail(email, false, out errorMessage))
            {
                AddErrLine(errorMessage);
                return;
            }

            //用户注册模板中,生日可以单独用一个名为bday的文本框, 也可以分别用bday_y bday_m bday_d三个文本框, 用户可不填写
            if (!Utils.IsDateString(tmpBday) && !string.IsNullOrEmpty(tmpBday))
            {
                AddErrLine("生日格式错误, 如果不想填写生日请置空");
                return;
            }

            //如果用户名符合注册规则, 则判断是否已存在
            if (Users.GetUserId(tmpUserName) > 0)
            {
                AddErrLine("请不要重复提交！");
                return;
            }

            //读取当前用户的OPENID信息
            userconnectinfo = DiscuzCloud.GetUserConnectInfo(openid);
            if (userconnectinfo == null || userconnectinfo.Uid > 0)
            {
                AddErrLine("Connect信息异常,登录失败,请尝试再次登录");
                return;
            }

            UserBindConnectLog bindLog = DiscuzCloud.GetUserConnectBindLog(userconnectinfo.OpenId);
            if (cloudconfig.Maxuserbindcount != 0 && bindLog != null && (bindLog.Type != 1 && bindLog.BindCount >= cloudconfig.Maxuserbindcount))
            {
                AddErrLine("当前QQ用户解绑次数过多,无法绑定新注册的用户");
                return;
            }

            UserInfo userInfo = CreateUser(tmpUserName, email, tmpBday);

            userconnectinfo.Uid = userInfo.Uid;
            DiscuzCloud.UpdateUserConnectInfo(userconnectinfo);

            if (bindLog == null)
            {
                bindLog = new UserBindConnectLog();
                bindLog.OpenId = userconnectinfo.OpenId;
                bindLog.Uid = userconnectinfo.Uid;
                bindLog.Type = 1;
                bindLog.BindCount = 1;
                DiscuzCloud.CreateUserConnectBindLog(bindLog);
            }
            else
            {
                bindLog.BindCount++;
                bindLog.Uid = userconnectinfo.Uid;
                bindLog.Type = 1;
                DiscuzCloud.UpdateUserConnectBindLog(bindLog);
            }

            #region 发送欢迎信息
            if (config.Welcomemsg == 1)
            {
                // 收件箱
                PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();
                privatemessageinfo.Message = config.Welcomemsgtxt;
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
            #endregion

            //发送同步数据给应用程序
            Sync.UserRegister(userInfo.Uid, userInfo.Username, userInfo.Password, "");

            //如果用户选择使用QZone头像
            if (cloudconfig.Allowuseqzavater == 1 && DNTRequest.GetString("use_qzone_avatar") == "1")
            {
                QZoneAvatar qz = new QZoneAvatar();
                qz.AsyncGetAvatar(userconnectinfo);
            }

            SetUrl("index.aspx");
            SetShowBackLink(false);
            //如果不是需要管理员审核的注册,页面延时刷新为2秒,否则是5秒
            SetMetaRefresh(config.Regverify != 2 ? 2 : 5);
            Statistics.ReSetStatisticsCache();

            if (config.Regverify != 2)
            {
                CreditsFacade.UpdateUserCredits(userInfo.Uid);
                ForumUtils.WriteUserCookie(userInfo, -1, config.Passwordkey);
                Utils.WriteCookie("bindconnect", "true");//将当前登录用户是否绑定QQ互联的状态设置为true
                OnlineUsers.UpdateAction(olid, UserAction.Register.ActionID, 0, config.Onlinetimeout);
                AddMsgLine("QQ登录成功,继续浏览");
            }
            else
            {
                AddMsgLine("QQ数据绑定完成, 但需要系统管理员审核您的帐户后才可登录使用");
            }
            notifyscript = GetNotifyScript(userconnectinfo, userInfo.Username, userInfo.Bday, userInfo.Gender,
                userInfo.Email, userInfo.Showemail, DNTRequest.GetInt("useqqavatar", 2), "register");
        }

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="birthday"></param>
        /// <returns></returns>
        private UserInfo CreateUser(string userName, string email, string birthday)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.Username = userName;
            userInfo.Email = email;
            userInfo.Bday = birthday;
            userInfo.Nickname = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("nickname")));
            userInfo.Password = Utils.MD5(ForumUtils.CreateAuthStr(16));
            userInfo.Secques = "";
            userInfo.Gender = DNTRequest.GetInt("gender", 0);
            userInfo.Adminid = 0;
            userInfo.Groupexpiry = 0;
            userInfo.Extgroupids = "";
            userInfo.Regip = userInfo.Lastip = DNTRequest.GetIP();
            userInfo.Joindate = userInfo.Lastvisit = userInfo.Lastactivity = userInfo.Lastpost = Utils.GetDateTime();
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

            userInfo.Sigstatus = DNTRequest.GetInt("sigstatus", 1) != 0 ? 1 : 0;
            userInfo.Tpp = DNTRequest.GetInt("tpp", 0);
            userInfo.Ppp = DNTRequest.GetInt("ppp", 0);
            userInfo.Templateid = DNTRequest.GetInt("templateid", 0);
            userInfo.Pmsound = DNTRequest.GetInt("pmsound", 0);
            userInfo.Showemail = DNTRequest.GetInt("showemail", 0);
            userInfo.Salt = "";

            int receivepmsetting = config.Regadvance == 0 ? 3 : DNTRequest.GetInt("receivesetting", 3);//关于短信息枚举值的设置看ReceivePMSettingType类型注释，此处不禁止用户接受系统短信息
            userInfo.Newsletter = (ReceivePMSettingType)receivepmsetting;
            userInfo.Invisible = DNTRequest.GetInt("invisible", 0);
            userInfo.Newpm = config.Welcomemsg == 1 ? 1 : 0;
            userInfo.Medals = "";
            userInfo.Accessmasks = DNTRequest.GetInt("accessmasks", 0);
            userInfo.Website = DNTRequest.GetHtmlEncodeString("website");
            userInfo.Icq = DNTRequest.GetHtmlEncodeString("icq");
            userInfo.Qq = DNTRequest.GetHtmlEncodeString("qq");
            userInfo.Yahoo = DNTRequest.GetHtmlEncodeString("yahoo");
            userInfo.Msn = DNTRequest.GetHtmlEncodeString("msn");
            userInfo.Skype = DNTRequest.GetHtmlEncodeString("skype");
            userInfo.Location = DNTRequest.GetHtmlEncodeString("location");
            userInfo.Customstatus = (usergroupinfo.Allowcstatus == 1) ? DNTRequest.GetHtmlEncodeString("customstatus") : "";
            userInfo.Bio = ForumUtils.BanWordFilter(DNTRequest.GetString("bio"));
            userInfo.Signature = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("signature")));

            PostpramsInfo postpramsinfo = new PostpramsInfo();
            postpramsinfo.Usergroupid = usergroupid;
            postpramsinfo.Attachimgpost = config.Attachimgpost;
            postpramsinfo.Showattachmentpath = config.Showattachmentpath;
            postpramsinfo.Hide = 0;
            postpramsinfo.Price = 0;
            postpramsinfo.Sdetail = userInfo.Signature;
            postpramsinfo.Smileyoff = 1;
            postpramsinfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
            postpramsinfo.Parseurloff = 1;
            postpramsinfo.Showimages = usergroupinfo.Allowsigimgcode;
            postpramsinfo.Allowhtml = 0;
            postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsinfo.Smiliesmax = config.Smiliesmax;
            userInfo.Sightml = UBB.UBBToHTML(postpramsinfo);
            userInfo.Authtime = Utils.GetDateTime();
            userInfo.Realname = DNTRequest.GetString("realname");
            userInfo.Idcard = DNTRequest.GetString("idcard");
            userInfo.Mobile = DNTRequest.GetString("mobile");
            userInfo.Phone = DNTRequest.GetString("phone");

            //系统管理员进行后台验证
            if (config.Regverify == 2)
            {
                userInfo.Authstr = DNTRequest.GetString("website");
                userInfo.Groupid = 8;
                userInfo.Authflag = 1;
            }
            else
            {
                userInfo.Authstr = "";
                userInfo.Authflag = 0;
                userInfo.Groupid = CreditsFacade.GetCreditsUserGroupId(0).Groupid;
            }
            userInfo.Uid = Users.CreateUser(userInfo);
            return userInfo;
        }

        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="userInfo"></param>
        private void LoginUser(ShortUserInfo userInfo)
        {
            #region 无延迟更新在线信息和相关用户信息
            ForumUtils.WriteUserCookie(userInfo.Uid, TypeConverter.StrToInt(DNTRequest.GetString("expires"), -1),
                    config.Passwordkey, DNTRequest.GetInt("templateid", 0), DNTRequest.GetInt("loginmode", -1));
            oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout, userInfo.Uid, "");
            olid = oluserinfo.Olid;
            username = userInfo.Username;
            userid = userInfo.Uid;
            usergroupinfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);
            useradminid = usergroupinfo.Radminid; // 根据用户组得到相关联的管理组id
            Utils.WriteCookie("bindconnect", "true");//将当前登录用户是否绑定QQ互联的状态设置为true
            OnlineUsers.UpdateAction(olid, UserAction.Login.ActionID, 0);
            LoginLogs.DeleteLoginLog(DNTRequest.GetIP());
            Users.UpdateUserCreditsAndVisit(userInfo.Uid, DNTRequest.GetIP());
            #endregion
        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <returns></returns>
        private ShortUserInfo GetShortUserInfo()
        {
            int uid = -1;
            switch (config.Passwordmode)
            {
                case 1://动网兼容模式
                    {
                        if (config.Secques == 1)
                            uid = Users.CheckDvBbsPasswordAndSecques(postusername, postpassword, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
                        else
                            uid = Users.CheckDvBbsPassword(postusername, postpassword);
                        break;
                    }
                case 0://默认模式
                    {
                        if (config.Secques == 1)
                            uid = Users.CheckPasswordAndSecques(postusername, postpassword, true, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
                        else
                            uid = Users.CheckPassword(postusername, postpassword, true);
                        break;
                    }
                default: //第三方加密验证模式
                    {
                        return (ShortUserInfo)Users.CheckThirdPartPassword(postusername, postpassword, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
                    }
            }
            if (uid != -1)
                Users.UpdateTrendStat(TrendType.Login);
            return uid > 0 ? Users.GetShortUserInfo(uid) : null;
        }

        /// <summary>
        /// 生成通知云平台的js脚本
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
        private string GetNotifyScript(UserConnectInfo connectInfo, string userName, string birthday, int gender,
                string email, int isPublicEmail, int isUsedQQAvatar, string type)
        {
            return string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>", DiscuzCloud.GetBindUserNotifyUrl(connectInfo, userName, birthday
                , gender, email, isPublicEmail == 1 ? 1 : 2, isUsedQQAvatar, type));
        }

        /// <summary>
        /// 检查云平台Callback的签名,用来防范伪造请求
        /// </summary>
        /// <param name="sig"></param>
        /// <returns></returns>
        private bool CheckCallbackSignature(string sig)
        {
            StringBuilder sb = new StringBuilder();
            List<DiscuzOAuthParameter> parms = new List<DiscuzOAuthParameter>();

            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (key.Substring(0, 4) == "con_" && key != "con_sig")
                    parms.Add(new DiscuzOAuthParameter(key, DNTRequest.GetString(key)));
            }
            parms.Sort(new ParameterComparer());
            foreach (DiscuzOAuthParameter parm in parms)
            {
                if (!string.IsNullOrEmpty(parm.Value))
                    sb.AppendFormat("{0}={1}&", parm.Name, parm.Value);
            }
            sb.Append(DiscuzCloudConfigs.GetConfig().Connectappkey);
            return sig == Utils.MD5(sb.ToString());

        }
    }
}