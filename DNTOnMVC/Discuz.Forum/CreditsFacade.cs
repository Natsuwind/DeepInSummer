using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Config;
using System.Data;
using System.Collections;
using System.Web;

namespace Discuz.Forum
{
    /// <summary>
    /// 用户积分操作类
    /// </summary>
    public class CreditsFacade
    {
        #region 短消息
        /// <summary>
        /// 是否能充足的积分发短消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public static bool IsEnoughCreditsPM(int userId)
        {
            return CheckUserCreditsIsEnough(userId, 1, CreditsOperationType.SendMessage, -1);
        }

        /// <summary>
        /// 用户发送短消息时更新用户的积分
        /// </summary>
        /// <param name="userId">用户id</param>
        public static int SendPM(int userId)
        {
            if (userId > 0)
            {
                return UpdateUserExtCredits(userId, 1, CreditsOperationType.SendMessage, 1, false);
                //UserCredits.UpdateUserCredits(userId);
                //return result;
            }
            else
                return -1;
        }
        #endregion

        #region 主题
        /// <summary>
        /// 发主题时更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="forumInfo">版块信息</param>
        /// <param name="isNeedAnimation">是否需要积分动画</param>
        public static void PostTopic(int userId, ForumInfo forumInfo, bool isNeedAnimation)
        {
            if (userId == -1)
                return;
            float[] values = Forums.GetValues(forumInfo.Postcredits);
            if (values != null) ///使用版块内积分
            {
                UpdateUserExtCredits(userId, values, false);
            }
            else ///使用默认积分                
            {
                UpdateUserExtCredits(userId, 1, CreditsOperationType.PostTopic, 1, false);
            }
            if(isNeedAnimation)
                WriteUpdateUserExtCreditsCookies(values != null ? values : Scoresets.GetUserExtCredits(CreditsOperationType.PostTopic));
        }

        /// <summary>
        /// 发主题时更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="forumInfo">版块信息</param>
        public static void PostTopic(int userId, ForumInfo forumInfo)
        {
            PostTopic(userId, forumInfo, false);
        }

        /// <summary>
        /// 删除主题帖(layer=0)或是回复时更新用户积分
        /// </summary>
        /// <param name="postInfo">帖子信息</param>
        /// <param name="forumInfo">版块信息</param>
        /// <param name="reserveAttach">是否保留附件</param>
        public static void DeletePost(PostInfo postInfo, ForumInfo forumInfo, bool reserveAttach)
        {
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            if (configInfo.Losslessdel == 0 || Utils.StrDateDiffHours(postInfo.Postdatetime, configInfo.Losslessdel * 24) < 0)
            {
                CreditsOperationType creditsOperationType = postInfo.Layer == 0 ? CreditsOperationType.PostTopic : CreditsOperationType.PostReply;
                //获取版块积分规则
                float[] creditsValue = Forums.GetValues(
                    creditsOperationType == CreditsOperationType.PostTopic ?
                    forumInfo.Postcredits :
                    forumInfo.Replycredits
                    );

                //如果未定义版块积分规则
                if (creditsValue == null)
                    creditsValue = Scoresets.GetUserExtCredits(creditsOperationType);
                UpdateUserExtCredits(postInfo.Posterid, creditsValue, 1, creditsOperationType, -1, true);
                //当不保留附件时，对附件进行相应的减分操作
                if (!reserveAttach)
                {
                    int attCount = Attachments.GetAttachmentCountByPid(postInfo.Pid);
                    if (attCount != 0)
                        DeleteAttachments(postInfo.Posterid, attCount);
                }
            }
        }
        #endregion

        #region 回复
        /// <summary>
        /// 发回复时更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="forumInfo">版块信息</param>
        /// <param name="isNeedAnimation">是否需要积分动画</param>
        public static void PostReply(int userId, ForumInfo forumInfo, bool isNeedAnimation)
        {
            if (userId == -1)
                return;
            float[] values = Forums.GetValues(forumInfo.Replycredits);
            if (values != null)
            {
                UpdateUserExtCredits(userId, values, false);//使用版块内积分
            }
            else
            {
                UpdateUserExtCredits(userId, 1, CreditsOperationType.PostReply, 1, false);
            }
            if(isNeedAnimation)
                WriteUpdateUserExtCreditsCookies(values != null ? values : Scoresets.GetUserExtCredits(CreditsOperationType.PostReply));
        }

        /// <summary>
        /// 发回复时更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="forumInfo">版块信息</param>
        public static void PostReply(int userId, ForumInfo forumInfo)
        {
            PostReply(userId, forumInfo, false);
        }
        #endregion

        #region 附件
        /// <summary>
        /// 上传附件更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="attachmentCount">上传的附件个数</param>
        public static int UploadAttachments(int userId, int attachmentCount)
        {
            //UserCredits.UpdateUserExtCreditsByUploadAttachment(userId, attachmentCount);
            if (userId > 0 && attachmentCount > 0)
                return UpdateUserExtCredits(userId, attachmentCount, CreditsOperationType.UploadAttachment, 1, false);
            else
                return 0;
        }
        /// <summary>
        /// 删除附件更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="attachmentCount">删除的附件个数</param>
        /// <returns></returns>
        public static int DeleteAttachments(int userId, int attachmentCount)
        {
            if (userId > 0 && attachmentCount > 0)
                return UpdateUserExtCredits(userId, attachmentCount, CreditsOperationType.UploadAttachment, -1, true);
            else
                return 0;
        }
        /// <summary>
        /// 下载附件更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="attachmentCount">下载的附件个数</param>
        /// <returns></returns>
        public static int DowlnLoadAttachments(int userId, int attachmentCount)
        {
            if (userId > 0 && attachmentCount > 0)
                return UpdateUserExtCredits(userId, attachmentCount, CreditsOperationType.DownloadAttachment, 1, false);
            else
                return -1;
        }
        /// <summary>
        /// 是否能充足的积分发下载附件
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="attachmentCount">下载附件的个数</param>
        /// <returns></returns>
        public static bool IsEnoughCreditsDownloadAttachment(int userId, int attachmentCount)
        {
            return CheckUserCreditsIsEnough(userId, attachmentCount, CreditsOperationType.DownloadAttachment, -1);
        }
        #endregion

        #region 精华
        /// <summary>
        /// 设置精华更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        public static void SetDigest(int userId)
        {
            if(userId > 0)
                UpdateUserExtCredits(userId, 1, CreditsOperationType.Digest, 1, false);
        }
        /// <summary>
        /// 取消精华更新用户积分
        /// </summary>
        /// <param name="userId">用户id</param>
        public static void UnDigest(int userId)
        {
            if (userId > 0)
                UpdateUserExtCredits(userId, 1, CreditsOperationType.Digest, -1, false);
        }
        #endregion

        #region 搜索
        /// <summary>
        /// 用户搜索时更新用户的积分
        /// </summary>
        /// <param name="userId">用户id</param>
        public static int Search(int userId)
        {
            return UpdateUserExtCredits(userId, 1, CreditsOperationType.Search, 1, false);
        }
        #endregion

        #region 交易
        /// <summary>
        /// 用户交易成功时更新用户的积分
        /// </summary>
        /// <param name="userId">用户id</param>
        public static int UpdateUserCreditsByTradefinished(int userId)
        {
            if (userId > 0)
                return UpdateUserExtCredits(userId, 1, CreditsOperationType.TradeSucceed, 1, false);
            else
                return 0;
        }
        #endregion

        #region 投票
        /// <summary>
        /// 用户参与投票时更新用户的积分
        /// </summary>
        /// <param name="userId">用户id</param>
        public static void Vote(int userId)
        {
            if (userId > 0)
                UpdateUserExtCredits(userId, 1, CreditsOperationType.Vote, 1, false);
        }
        #endregion

        #region 邀请
        /// <summary>
        /// 用户邀请注册更新用户的积分
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="mount">更新数</param>
        public static void Invite(int userId, int mount)
        {
            if (userId > 0)
                UpdateUserExtCredits(userId, mount, CreditsOperationType.Invite, 1, false);
        }
        #endregion

        #region 用户组
        /// <summary>
        /// 根据积分获得积分用户组所应该匹配的用户组描述 (如果没有匹配项或用户非积分用户组则返回null)
        /// </summary>
        /// <param name="credits">积分</param>
        /// <returns>用户组描述</returns>
        public static UserGroupInfo GetCreditsUserGroupId(float credits)
        {
            List<UserGroupInfo> userGroupInfoList = UserGroups.GetUserGroupList();
            UserGroupInfo tempUserGroupInfo = null;

            UserGroupInfo maxCreditGroup = null;
            foreach (UserGroupInfo userGroupInfo in userGroupInfoList)
            {
                // 积分用户组的特征是radminid等于0
                if (userGroupInfo.Radminid == 0 && userGroupInfo.System == 0 && (credits >= userGroupInfo.Creditshigher && credits <= userGroupInfo.Creditslower))
                {
                    if (tempUserGroupInfo == null || userGroupInfo.Creditshigher > tempUserGroupInfo.Creditshigher)
                        tempUserGroupInfo = userGroupInfo;
                }
                //更新积分上线最高的用户组
                if (maxCreditGroup == null || maxCreditGroup.Creditshigher < userGroupInfo.Creditshigher)
                    maxCreditGroup = userGroupInfo;
            }

            if (maxCreditGroup != null && maxCreditGroup.Creditshigher < credits)
                tempUserGroupInfo = maxCreditGroup;

            return tempUserGroupInfo == null ? new UserGroupInfo() : tempUserGroupInfo;
        }

        /// <summary>
        /// 根据用户信息重新计算用户积分
        /// </summary>
        /// <param name="shortUserInfo">用户信息</param>
        /// <returns>用户积分</returns>
        public static int GetUserCreditsByUserInfo(ShortUserInfo shortUserInfo)
        {
            string ArithmeticStr = Scoresets.GetScoreCalFormula();

            if (Utils.StrIsNullOrEmpty(ArithmeticStr))
                return 0;

            ArithmeticStr = ArithmeticStr.Replace("digestposts", shortUserInfo.Digestposts.ToString());
            ArithmeticStr = ArithmeticStr.Replace("posts", shortUserInfo.Posts.ToString());
            ArithmeticStr = ArithmeticStr.Replace("oltime", shortUserInfo.Oltime.ToString());
            ArithmeticStr = ArithmeticStr.Replace("pageviews", shortUserInfo.Pageviews.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits1", shortUserInfo.Extcredits1.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits2", shortUserInfo.Extcredits2.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits3", shortUserInfo.Extcredits3.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits4", shortUserInfo.Extcredits4.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits5", shortUserInfo.Extcredits5.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits6", shortUserInfo.Extcredits6.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits7", shortUserInfo.Extcredits7.ToString());
            ArithmeticStr = ArithmeticStr.Replace("extcredits8", shortUserInfo.Extcredits8.ToString());

            object expression = Arithmetic.ComputeExpression(ArithmeticStr);
            return Utils.StrToInt(Math.Floor(Utils.StrToFloat(expression, 0)), 0);
        }

        /// <summary>
        /// 根据积分公式更新用户积分,并且受分数变动影响有可能会更改用户所属的用户组
        /// <param name="userId">用户ID</param>
        /// </summary>
        public static int UpdateUserCredits(int userId)
        {
            Data.UserCredits.UpdateUserCredits(userId);
            ShortUserInfo userInfo = userId > 0 ? Users.GetShortUserInfo(userId) : null;
            if (userInfo == null)
                return 0;

            UserGroupInfo tmpUserGroupInfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);

            if (tmpUserGroupInfo != null && UserGroups.IsCreditUserGroup(tmpUserGroupInfo))//当用户组为积分用户组或者组ID为游客(ID=7)
            {
                tmpUserGroupInfo = GetCreditsUserGroupId(userInfo.Credits);
                if (tmpUserGroupInfo.Groupid != userInfo.Groupid)//当用户所属组发生变化时
                {
                    Data.Users.UpdateUserGroup(userInfo.Uid.ToString(), tmpUserGroupInfo.Groupid);
                    Data.OnlineUsers.UpdateGroupid(userInfo.Uid, tmpUserGroupInfo.Groupid);
                }
            }
            //判断操作用户是否是当前用户，如果是则更新dntusertips的cookie
            HttpCookie cookie = HttpContext.Current.Request.Cookies["dnt"];
            if (cookie != null && cookie["userid"] == userId.ToString())
                ForumUtils.WriteUserCreditsCookie(userInfo, tmpUserGroupInfo.Grouptitle);
            return 1;
        }

        /// <summary>
        /// 通过指定值更新用户积分
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="values">积分变动值,应保证是一个长度为8的数组,对应8种扩展积分的变动值</param>
        /// <param name="allowMinus">是否允许被扣成负分,true允许,false不允许并且不进行扣分返回-1</param>
        /// <returns></returns>
        public static int UpdateUserExtCredits(int uid, float[] values, bool allowMinus)
        {
            if (uid < 1 || Discuz.Data.Users.GetUserInfo(uid) == null)
                return 0;

            if (values.Length < 8)
                return -1;

            if (!allowMinus)//不允许扣成负分时要进行判断积分是否足够被减
            {
                // 如果要减扩展积分, 首先判断扩展积分是否足够被减
                if (!Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, values))
                    return -1;
            }

            Discuz.Data.UserCredits.UpdateUserExtCredits(uid, values);

            CreditsFacade.UpdateUserCredits(uid);

            //向应用同步扩展积分
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != 0.0)
                {
                    Sync.UpdateCredits(uid, i + 1, values[i].ToString(), "");
                }
            }
            ///更新用户积分
            return 1;
        }

        /// <summary>
        /// 根据用户列表,一次更新多个用户的积分
        /// </summary>
        /// <param name="uidlist">用户ID列表</param>
        /// <param name="values">扩展积分值</param>
        public static int UpdateUserExtCredits(string uidlist, float[] values)
        {
            int reval = -1;
            if (Utils.IsNumericList(uidlist))
            {
                reval = 0;
                ///根据公式计算用户的总积分,并更新	
                foreach (string uid in Utils.SplitString(uidlist, ","))
                {
                    if (TypeConverter.StrToInt(uid, 0) > 0)
                        reval = reval + UpdateUserExtCredits(TypeConverter.StrToInt(uid, 0), values, true);
                }
            }
            return reval;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 更新用户积分(适用于单用户,单个或多个操作)
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <param name="allowMinus">是否允许被扣成负分,true允许,false不允许并且不进行扣分返回-1</param>
        /// <returns></returns>
        private static int UpdateUserExtCredits(int uid, int mount, CreditsOperationType creditsOperationType, int pos, bool allowMinus)
        {
            return UpdateUserExtCredits(uid, Scoresets.GetUserExtCredits(creditsOperationType), mount, creditsOperationType, pos, allowMinus);
        }
        /// <summary>
        /// 创建积分动画的COOKIE
        /// </summary>
        /// <param name="values">积分列表</param>
        private static void WriteUpdateUserExtCreditsCookies(float[] values)
        {
            StringBuilder creditsValue = new StringBuilder("");
            creditsValue.Append("0,");
            foreach (float s in values)
            {
                creditsValue.Append(s.ToString());
                creditsValue.Append(",");
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies["discuz_creditnotice"];
            if (cookie == null)
            {
                cookie = new HttpCookie("discuz_creditnotice");
            }
            cookie.Value = creditsValue.ToString().TrimEnd(',');
            cookie.Expires = DateTime.Now.AddMinutes(36000);
            cookie.Path = BaseConfigs.GetForumPath;
            HttpContext.Current.Response.AppendCookie(cookie);
            //Utils.WriteCookie("discuz_creditnotice", creditsValue.ToString().TrimEnd(','), 36000);
        }

        /// <summary>
        /// 检查用户积分是否足够被减(适用于单用户, 单个或多个积分)
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <returns></returns>
        private static bool CheckUserCreditsIsEnough(int uid, int mount, CreditsOperationType creditsOperationType, int pos)
        {
            DataTable dt = Scoresets.GetScoreSet();

            dt.PrimaryKey = new DataColumn[1] { dt.Columns["id"] };

            float[] extCredits = new float[8];
            for (int i = 0; i < 8; i++)
            {
                extCredits[i] = TypeConverter.ObjectToFloat(dt.Rows[(int)creditsOperationType]["extcredits" + (i + 1)]);
            }

            if (pos < 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (Utils.StrToFloat(extCredits[i], 0) < 0)//只要任何一项要求减分,就去数据库检查
                        return Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, extCredits, pos, mount);
                }
            }
            return true;
        }

        /// <summary>
        /// 更新用户积分(适用于单用户,单个或多个操作)
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="extCredits">使用的积分规则</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <param name="allowMinus">是否允许被扣成负分,true允许,false不允许并且不进行扣分返回-1</param>
        /// <returns></returns>
        private static int UpdateUserExtCredits(int uid, float[] extCredits, int mount, CreditsOperationType creditsOperationType, int pos, bool allowMinus)
        {
            if (uid == -1)//如果当前用户为游客，则直接返回-1
                return -1;

            //float[] extCredits = Scoresets.GetUserExtCredits(creditsOperationType);
            float extCredit = 0;

            foreach (float e in extCredits)//此循环用于校验当前积分操作是否需要更新用户积分
            {
                if (e != 0)
                {
                    extCredit = e;
                    break;
                }
            }

            if (extCredit == 0)//如果搜索积分设置中全部为0，即不操作积分，则直接返回1
                return 1;

            // 如果要减扩展积分, 首先判断扩展积分是否足够被减
            if (pos < 0)
            {
                //当不是删除主题或回复时
                if (creditsOperationType != CreditsOperationType.PostTopic && creditsOperationType != CreditsOperationType.PostReply)
                {
                    if (!allowMinus && !Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, extCredits, pos, mount))
                        return -1;
                }
            }
            else
            {
                if (creditsOperationType == CreditsOperationType.DownloadAttachment || creditsOperationType == CreditsOperationType.Search)//临时性解决用户搜索扣分可以为负数的问题，当积分系统被重新开发时，该判断代码可根据实际情况拿掉
                {
                    if (!allowMinus && !Discuz.Data.UserCredits.CheckUserCreditsIsEnough(uid, extCredits, -1, mount))
                        return -1;
                }
            }

            Discuz.Data.UserCredits.UpdateUserExtCredits(uid, extCredits, pos, mount);

            for (int i = 0; i < extCredits.Length; i++)
            {
                if (extCredits[i] != 0.0)
                {
                    Sync.UpdateCredits(uid, i + 1, extCredits[i].ToString(), "");
                }
            }
            //根据积分公式更新用户积分,并且受分数变动影响有可能会更改用户所属的用户组
            CreditsFacade.UpdateUserCredits(uid);
            ///更新用户积分
            return 1;
        }
        #endregion
    }
}
