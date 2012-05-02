using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;

namespace Discuz.Web.Services.API.Commands
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    public sealed class GetUserInfoCommand : Command
    {
        public GetUserInfoCommand()
            : base("users.getinfo")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            ShortUserInfo localUserInfo = null;

            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                    return false;
                }

                localUserInfo = Users.GetShortUserInfo(commandParam.LocalUid);
                if (localUserInfo == null)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_USER_NOT_EXIST, commandParam.ParamList);
                    return false;
                }
            }

            if (!commandParam.CheckRequiredParams("uids,fields"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string[] uIds = commandParam.GetDNTParam("uids").ToString().Split(',');

            //单次最多接受查询100个用户
            if (!Utils.IsNumericArray(uIds) || Utils.StrToInt(uIds[0], -1) < 1 || uIds.Length > 100)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            List<User> userList = new List<User>();
            UserInfo userInfo;
            for (int i = 0; i < uIds.Length; i++)
            {
                int userid = Utils.StrToInt(uIds[i], -1);
                if (userid < 1)
                    continue;
                userInfo = Discuz.Forum.Users.GetUserInfo(userid);
                if (userInfo == null)
                    continue;

                bool loadAuthAttr = true;
                if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
                    loadAuthAttr = userInfo.Uid == localUserInfo.Uid || localUserInfo.Adminid == 1;

                userList.Add(UserCommandUtils.LoadSingleUser(userInfo, commandParam.GetDNTParam("fields").ToString(), loadAuthAttr));
            }

            UserInfoResponse uir = new UserInfoResponse();
            uir.user_array = userList.ToArray();
            uir.List = true;

            if (commandParam.Format == FormatType.JSON)
            {
                result = Util.RemoveJsonNull(JavaScriptConvert.SerializeObject(userList.ToArray()));
            }
            else
            {
                //如果userList长度不大于1,则移除空节点会导致客户端反序列化错误
                //result = userList.Count > 1 ? Util.RemoveEmptyNodes(SerializationHelper.Serialize(uir), commandParam.GetDNTParam("fields").ToString()) :
                //SerializationHelper.Serialize(uir);

                result = Util.RemoveEmptyNodes(SerializationHelper.Serialize(uir), commandParam.GetDNTParam("fields").ToString());
            }
            return true;
        }
    }

    /// <summary>
    /// 获得当前登录用户
    /// </summary>
    public sealed class GetLoggenInUserCommand : Command
    {
        public GetLoggenInUserCommand()
            : base("users.getloggedinuser")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (commandParam.Format == FormatType.JSON)
                result = string.Format("\"{0}\"", commandParam.LocalUid);
            else
            {
                LoggedInUserResponse loggeduser = new LoggedInUserResponse();
                loggeduser.Uid = commandParam.LocalUid;
                result = SerializationHelper.Serialize(loggeduser);
            }
            return true;
        }
    }

    /// <summary>
    /// 设置用户资料
    /// </summary>
    public sealed class SetUserInfoCommand : Command
    {
        public SetUserInfoCommand()
            : base("users.setinfo")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("user_info"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            UserForEditing ufe;
            try
            {
                ufe = JavaScriptConvert.DeserializeObject<UserForEditing>(commandParam.GetDNTParam("user_info").ToString());
            }
            catch
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            #region 用户信息读取及权限校验
            int uid = commandParam.GetIntParam("uid");
            uid = uid > 0 ? uid : commandParam.LocalUid;
            if (uid <= 0)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            UserInfo localUserInfo = null;
            //终端应用程序需要校验当前用户权限,不是管理员则只能修改自己的资料
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                localUserInfo = Users.GetUserInfo(commandParam.LocalUid);
                if (localUserInfo == null || (localUserInfo.Uid != uid && localUserInfo.Adminid != 1))
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }
            }

            UserInfo userInfo = localUserInfo != null && localUserInfo.Uid == uid ? localUserInfo : Users.GetUserInfo(uid);
            if (userInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_USER_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            #endregion

            if (!string.IsNullOrEmpty(ufe.Email))
            {
                if (!UserCommandUtils.CheckEmail(ufe.Email, commandParam.GeneralConfig.Accessemail))
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_EMAIL, commandParam.ParamList);
                    return false;
                }
                userInfo.Email = ufe.Email;
            }

            if (!string.IsNullOrEmpty(ufe.Password))
                userInfo.Password = ufe.Password;

            if (!string.IsNullOrEmpty(ufe.Bio))
                userInfo.Bio = ufe.Bio;

            if (!string.IsNullOrEmpty(ufe.Birthday))
                userInfo.Bday = ufe.Birthday;

            if (!string.IsNullOrEmpty(ufe.ExtCredits1))
                userInfo.Extcredits1 = Utils.StrToFloat(ufe.ExtCredits1, 0);

            if (!string.IsNullOrEmpty(ufe.ExtCredits2))
                userInfo.Extcredits2 = Utils.StrToFloat(ufe.ExtCredits2, 0);

            if (!string.IsNullOrEmpty(ufe.ExtCredits3))
                userInfo.Extcredits3 = Utils.StrToFloat(ufe.ExtCredits3, 0);

            if (!string.IsNullOrEmpty(ufe.ExtCredits4))
                userInfo.Extcredits4 = Utils.StrToFloat(ufe.ExtCredits4, 0);

            if (!string.IsNullOrEmpty(ufe.ExtCredits5))
                userInfo.Extcredits5 = Utils.StrToFloat(ufe.ExtCredits5, 0);

            if (!string.IsNullOrEmpty(ufe.ExtCredits6))
                userInfo.Extcredits6 = Utils.StrToFloat(ufe.ExtCredits6, 0);

            if (!string.IsNullOrEmpty(ufe.ExtCredits7))
                userInfo.Extcredits7 = Utils.StrToFloat(ufe.ExtCredits7, 0);

            if (!string.IsNullOrEmpty(ufe.ExtCredits8))
                userInfo.Extcredits8 = Utils.StrToFloat(ufe.ExtCredits8, 0);

            if (!string.IsNullOrEmpty(ufe.Gender))
                userInfo.Gender = Utils.StrToInt(ufe.Gender, 0);

            if (!string.IsNullOrEmpty(ufe.Icq))
                userInfo.Icq = ufe.Icq;

            if (!string.IsNullOrEmpty(ufe.IdCard))
                userInfo.Idcard = ufe.IdCard;

            if (!string.IsNullOrEmpty(ufe.Location))
                userInfo.Location = ufe.Location;

            if (!string.IsNullOrEmpty(ufe.Mobile))
                userInfo.Mobile = ufe.Mobile;

            if (!string.IsNullOrEmpty(ufe.Msn))
                userInfo.Msn = ufe.Msn;

            if (!string.IsNullOrEmpty(ufe.NickName))
                userInfo.Nickname = ufe.NickName;

            if (!string.IsNullOrEmpty(ufe.Phone))
                userInfo.Phone = ufe.Phone;

            if (!string.IsNullOrEmpty(ufe.Qq))
                userInfo.Qq = ufe.Qq;

            if (!string.IsNullOrEmpty(ufe.RealName))
                userInfo.Realname = ufe.RealName;

            if (!string.IsNullOrEmpty(ufe.Skype))
                userInfo.Skype = ufe.Skype;

            if (!string.IsNullOrEmpty(ufe.SpaceId))
                userInfo.Spaceid = Utils.StrToInt(ufe.SpaceId, 0);

            if (!string.IsNullOrEmpty(ufe.WebSite))
                userInfo.Website = ufe.WebSite;

            if (!string.IsNullOrEmpty(ufe.Yahoo))
                userInfo.Yahoo = ufe.Yahoo;

            try
            {
                Users.UpdateUser(userInfo);
            }
            catch
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_UNKNOWN, commandParam.ParamList);
                return false;
            }

            if (commandParam.Format == FormatType.JSON)
                result = "true";
            else
            {
                SetInfoResponse sir = new SetInfoResponse();
                sir.Successfull = 1;
                result = SerializationHelper.Serialize(sir);
            }
            return true;
        }
    }

    /// <summary>
    /// 设置用户积分
    /// </summary>
    public sealed class SetUserExtCreditsCommand : Command
    {
        public SetUserExtCreditsCommand()
            : base("users.setextcredits")
        {
        }

        /*
         * Description:
         *      每个用户UID 15秒内只能调用一次该接口,否则无法更新成功
         */
        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                    return false;
                }

                if (Discuz.Forum.Users.GetShortUserInfo(commandParam.LocalUid).Adminid != 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }
            }

            if (!commandParam.CheckRequiredParams("uids,additional_values"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string[] values = commandParam.GetDNTParam("additional_values").ToString().Split(',');
            string[] uids = commandParam.GetDNTParam("uids").ToString().Split(',');

            if (!Utils.IsNumericArray(uids) || !Utils.IsNumericArray(values) || uids.Length > 100)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (values.Length != 8)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            List<float> list = new List<float>();
            for (int i = 0; i < values.Length; i++)
            {
                list.Add(Utils.StrToFloat(values[i], 0));
            }

            foreach (string uId in uids)
            {
                int id = TypeConverter.StrToInt(uId);
                if (id == 0)
                    continue;

                if (!CommandCacheQueue<SetExtCreditItem>.EnQueue(new SetExtCreditItem(id, DateTime.Now.Ticks)))
                    continue;

                CreditsFacade.UpdateUserExtCredits(id, list.ToArray(), true);
                CreditsFacade.UpdateUserCredits(id);

                //向第三方应用同步积分
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] != 0.0)
                        Sync.UpdateCredits(TypeConverter.StrToInt(uId), i + 1, list[i].ToString(), commandParam.AppInfo.APIKey);
                }
            }

            if (commandParam.Format == FormatType.JSON)
                result = "true";
            else
            {
                SetExtCreditsResponse secr = new SetExtCreditsResponse();
                secr.Successfull = 1;
                result = SerializationHelper.Serialize(secr);
            }
            return true;
        }
    }

    /// <summary>
    /// 根据用户名获得用户ID
    /// </summary>
    public sealed class GetUserIDCommand : Command
    {
        public GetUserIDCommand()
            : base("users.getid")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("user_name"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            int uid = Users.GetUserId(commandParam.GetDNTParam("user_name").ToString());

            if (commandParam.Format == FormatType.JSON)
                result = string.Format("\"{0}\"", uid);
            else
            {
                GetIDResponse gir = new GetIDResponse();
                gir.UId = uid;
                result = SerializationHelper.Serialize(gir);
            }
            return true;
        }
    }

    /// <summary>
    /// 更改用户密码的快速接口,需提供被修改用户的原有密码明文
    /// </summary>
    public sealed class ChangeUserPasswordCommand : Command
    {
        public ChangeUserPasswordCommand()
            : base("users.changepassword")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            int uid = commandParam.GetIntParam("uid");

            //如果是桌面程序则需要验证用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                    return false;
                }

                if (commandParam.LocalUid != uid)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }
            }

            if (!commandParam.CheckRequiredParams("uid,original_password,new_password,confirm_new_password"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string originalPassword = commandParam.GetDNTParam("original_password").ToString();
            string newPassword = commandParam.GetDNTParam("new_password").ToString();
            string confirmNewPassword = commandParam.GetDNTParam("confirm_new_password").ToString();

            if (newPassword != confirmNewPassword)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            bool isMD5Passwd = commandParam.GetDNTParam("password_format") != null && commandParam.GetDNTParam("password_format").ToString().ToLower() == "md5";

            ShortUserInfo user = Discuz.Forum.Users.GetShortUserInfo(uid);
            if (!isMD5Passwd)
                originalPassword = Utils.MD5(originalPassword);

            if (user.Password != originalPassword)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_ORI_PASSWORD_EQUAL_FALSE, commandParam.ParamList);
                return false;
            }

            bool updateSuccess = Discuz.Forum.Users.UpdateUserPassword(uid, newPassword, !isMD5Passwd);

            if (commandParam.Format == FormatType.JSON)
                result = string.Format("\"{0}\"", updateSuccess.ToString().ToLower());
            else
            {
                ChangePasswordResponse cpr = new ChangePasswordResponse();
                cpr.Successfull = updateSuccess ? 1 : 0;
                result = SerializationHelper.Serialize(cpr);
            }
            return true;
        }
    }

    /// <summary>
    /// 根据Email获取用户信息
    /// </summary>
    public sealed class GetUserInfoByEmailCommand : Command
    {
        public GetUserInfoByEmailCommand()
            : base("users.getinfobyemail")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            ShortUserInfo localUserInfo = null;

            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                    return false;
                }

                localUserInfo = Users.GetShortUserInfo(commandParam.LocalUid);
                if (localUserInfo == null)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_USER_NOT_EXIST, commandParam.ParamList);
                    return false;
                }
            }

            if (!commandParam.CheckRequiredParams("email,fields"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            List<UserInfo> userList = new List<UserInfo>();
            List<User> userListResult = new List<User>();

            userList = Discuz.Forum.Users.GetUserListByEmail(commandParam.GetDNTParam("email").ToString().Trim());
            string fields = commandParam.GetDNTParam("fields").ToString();

            foreach (UserInfo userInfo in userList)
            {
                bool loadAuthAttr = true;
                if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
                    loadAuthAttr = userInfo.Uid == localUserInfo.Uid || localUserInfo.Adminid == 1;
                userListResult.Add(UserCommandUtils.LoadSingleUser(userInfo, fields, loadAuthAttr));
            }

            UserInfoResponse uir = new UserInfoResponse();
            uir.user_array = userListResult.ToArray();
            uir.List = true;

            if (commandParam.Format == FormatType.JSON)
            {
                result = Util.RemoveJsonNull(JavaScriptConvert.SerializeObject(userListResult.ToArray()));
            }
            else
            {
                //如果userList长度不大于1,则移除空节点会导致客户端反序列化错误
                //result = userListResult.Count > 1 ? Util.RemoveEmptyNodes(SerializationHelper.Serialize(uir), commandParam.GetDNTParam("fields").ToString()) :
                //SerializationHelper.Serialize(uir);

                result = Util.RemoveEmptyNodes(SerializationHelper.Serialize(uir), commandParam.GetDNTParam("fields").ToString());
            }
            return true;
        }
    }

    public class UserCommandUtils
    {

        /// <summary>
        /// 从UserInfo对象中将属性值导入API接口专用User对象
        /// </summary>
        /// <param name="userInfo">UserInfo对象</param>
        /// <param name="fields">应用程序设置读取的字段</param>
        /// <param name="loadAuthAttribute">是否读取需要权限范围的字段</param>
        /// <returns></returns>
        public static User LoadSingleUser(UserInfo userInfo, string fields, bool loadAuthAttribute)
        {
            List<string> fieldlist = new List<string>(fields.Split(','));
            User user = new User();

            #region normal security

            user.Uid = fieldlist.Contains("uid") ? (int?)userInfo.Uid : null;
            user.UserName = fieldlist.Contains("user_name") ? userInfo.Username : null;
            user.Avatar = fieldlist.Contains("avatar") ? Avatars.GetAvatarUrl(userInfo.Uid).TrimStart('/') : null;
            user.Credits = fieldlist.Contains("credits") ? (int?)userInfo.Credits : null;
            user.Birthday = fieldlist.Contains("birthday") ? userInfo.Bday.Trim() : null;
            user.DigestPosts = fieldlist.Contains("digest_post_count") ? (int?)userInfo.Digestposts : null;
            user.ExtCredits1 = fieldlist.Contains("ext_credits_1") ? (int?)userInfo.Extcredits1 : null;
            user.ExtCredits2 = fieldlist.Contains("ext_credits_2") ? (int?)userInfo.Extcredits2 : null;
            user.ExtCredits3 = fieldlist.Contains("ext_credits_3") ? (int?)userInfo.Extcredits3 : null;
            user.ExtCredits4 = fieldlist.Contains("ext_credits_4") ? (int?)userInfo.Extcredits4 : null;
            user.ExtCredits5 = fieldlist.Contains("ext_credits_5") ? (int?)userInfo.Extcredits5 : null;
            user.ExtCredits6 = fieldlist.Contains("ext_credits_6") ? (int?)userInfo.Extcredits6 : null;
            user.ExtCredits7 = fieldlist.Contains("ext_credits_7") ? (int?)userInfo.Extcredits7 : null;
            user.ExtCredits8 = fieldlist.Contains("ext_credits_8") ? (int?)userInfo.Extcredits8 : null;
            user.ExtGroupids = fieldlist.Contains("ext_groupids") ? userInfo.Extgroupids.Trim() : null;
            user.Gender = fieldlist.Contains("gender") ? (int?)userInfo.Gender : null;
            user.AdminId = fieldlist.Contains("admin_id") ? (int?)userInfo.Adminid : null;
            user.GroupExpiry = fieldlist.Contains("group_expiry") ? (int?)userInfo.Groupexpiry : null;
            user.GroupId = fieldlist.Contains("group_id") ? (int?)userInfo.Groupid : null;
            user.JoinDate = fieldlist.Contains("join_date") ? userInfo.Joindate : null;
            user.LastActivity = fieldlist.Contains("last_activity") ? userInfo.Lastactivity : null;
            user.LastIp = fieldlist.Contains("last_ip") ? userInfo.Lastip.Trim() : null;
            user.LastPost = fieldlist.Contains("last_post") ? userInfo.Lastpost : null;
            user.LastPostid = fieldlist.Contains("last_post_id") ? (int?)userInfo.Lastpostid : null;
            user.LastPostTitle = fieldlist.Contains("last_post_title") ? userInfo.Lastposttitle : null;
            user.LastVisit = fieldlist.Contains("last_visit") ? userInfo.Lastvisit : null;
            user.NickName = fieldlist.Contains("nick_name") ? userInfo.Nickname : null;
            user.OnlineState = fieldlist.Contains("online_state") ? (int?)userInfo.Onlinestate : null;
            user.OnlineTime = fieldlist.Contains("online_time") ? (int?)userInfo.Oltime : null;
            user.PageViews = fieldlist.Contains("page_view_count") ? (int?)userInfo.Pageviews : null;
            user.Posts = fieldlist.Contains("post_count") ? (int?)userInfo.Posts : null;
            user.SpaceId = fieldlist.Contains("space_id") ? (int?)userInfo.Spaceid : null;
            user.CustomStatus = fieldlist.Contains("custom_status") ? userInfo.Customstatus : null;	//自定义头衔
            user.Medals = fieldlist.Contains("medals") ? userInfo.Medals : null; //勋章列表
            user.WebSite = fieldlist.Contains("web_site") ? userInfo.Website : null;	//网站
            user.Icq = fieldlist.Contains("icq") ? userInfo.Icq : null;	//icq号码
            user.Qq = fieldlist.Contains("qq") ? userInfo.Qq : null;	//qq号码
            user.Yahoo = fieldlist.Contains("yahoo") ? userInfo.Yahoo : null;//yahoo messenger帐号
            user.Msn = fieldlist.Contains("msn") ? userInfo.Msn : null;	//msn messenger帐号
            user.Skype = fieldlist.Contains("skype") ? userInfo.Skype : null;	//skype帐号
            user.Location = fieldlist.Contains("location") ? userInfo.Location : null;	//来自
            user.Bio = fieldlist.Contains("about_me") ? userInfo.Bio : null;	//自我介绍
            user.Sightml = fieldlist.Contains("signhtml") ? userInfo.Sightml : null;	//签名Html(自动转换得到)
            user.RealName = fieldlist.Contains("real_name") ? userInfo.Realname : null;  //用户实名
            user.IdCard = fieldlist.Contains("id_card") ? userInfo.Idcard : null;    //用户身份证件号
            user.Mobile = fieldlist.Contains("mobile") ? userInfo.Mobile : null;    //用户移动电话
            user.Phone = fieldlist.Contains("telephone") ? userInfo.Phone : null;     //用户固定电话

            #endregion

            if (loadAuthAttribute)
            {
                #region high security

                user.Password = fieldlist.Contains("password") ? userInfo.Password : null;
                user.ShowEmail = fieldlist.Contains("show_email") ? (int?)userInfo.Showemail : null;
                user.Email = fieldlist.Contains("email") ? userInfo.Email.Trim() : null;
                user.NewPm = fieldlist.Contains("has_new_pm") ? (int?)userInfo.Newpm : null;
                user.NewPmCount = fieldlist.Contains("new_pm_count") ? (int?)userInfo.Newpmcount : null;
                user.AccessMasks = fieldlist.Contains("access_masks") ? (int?)userInfo.Accessmasks : null;
                user.Invisible = fieldlist.Contains("invisible") ? (int?)userInfo.Invisible : null;
                user.PmSound = fieldlist.Contains("pm_sound") ? (int?)userInfo.Pmsound : null;
                user.Ppp = fieldlist.Contains("ppp") ? (int?)userInfo.Ppp : null;
                user.RegIp = fieldlist.Contains("reg_ip") ? userInfo.Regip : null;
                user.Secques = fieldlist.Contains("secques") ? userInfo.Secques : null;
                user.Templateid = fieldlist.Contains("template_id") ? (int?)userInfo.Templateid : null;
                user.Tpp = fieldlist.Contains("tpp") ? (int?)userInfo.Tpp : null;

                #endregion
            }
            return user;
        }

        /// <summary>
        /// 检测用户邮箱是否合法
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool CheckEmail(string email, string accessEmail)
        {
            if (!Utils.IsValidEmail(email) || !Discuz.Forum.Users.ValidateEmail(email))
                return false;

            string emailhost = Utils.GetEmailHostName(email);
            // 允许名单规则优先于禁止名单规则
            if (accessEmail != "" && !Utils.InArray(emailhost, accessEmail.Replace("\r\n", "\n"), "\n"))
                return false;

            if (accessEmail != "" && Utils.InArray(email, accessEmail.Replace("\r\n", "\n"), "\n"))
                return false;

            return true;
        }
    }
}
