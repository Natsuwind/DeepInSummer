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
    /// 发送论坛通知
    /// </summary>
    public sealed class SendNoticeCommand : Command
    {
        public SendNoticeCommand()
            : base("notifications.send")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果是桌面程序则需要验证用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                    return false;
                }
                //如果当前用户不是管理员
                if (Discuz.Forum.UserGroups.GetUserGroupInfo(Discuz.Forum.Users.GetShortUserInfo(commandParam.LocalUid).Groupid).Radminid != 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }
            }

            if (!commandParam.CheckRequiredParams("notification"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            //给当前登录用户发送通知可以将to_ids设置为空
            if (commandParam.LocalUid < 1 && (!commandParam.CheckRequiredParams("to_ids") || !Utils.IsNumericList(commandParam.GetDNTParam("to_ids").ToString())))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string ids = commandParam.GetDNTParam("to_ids").ToString();

            string notification = commandParam.GetDNTParam("notification").ToString();

            string[] to_ids;
            if (ids == string.Empty)
            {
                to_ids = new string[1];
                to_ids[0] = commandParam.LocalUid.ToString();
            }
            else
            {
                to_ids = commandParam.GetDNTParam("to_ids").ToString().Split(',');
            }

            string successfulIds = string.Empty;
            ShortUserInfo shortUserInfo = null;
            if (commandParam.LocalUid > 0)
                shortUserInfo = Discuz.Forum.Users.GetShortUserInfo(commandParam.LocalUid);

            foreach (string id in to_ids)
            {
                if (Utils.StrToInt(id, 0) < 1)
                    continue;

                NoticeInfo noticeinfo = new NoticeInfo();
                noticeinfo.Uid = Utils.StrToInt(id, 0);
                noticeinfo.New = 1;
                noticeinfo.Postdatetime = Utils.GetDateTime();

                //如果应用程序没有指定来源id,则会将当前应用程序id的hash值作为来源ID,若不指定来源id,用户的通知列表中只存在一条最新的应用程序通知
                noticeinfo.Fromid = commandParam.GetIntParam("from_id", Utils.BKDEHash(commandParam.AppInfo.APIKey, 113));
                //如果应用程序指定了来源id,则通知类型为“应用程序自定义通知”,否则是“应用程序通知”
                noticeinfo.Type = commandParam.CheckRequiredParams("from_id") ? NoticeType.ApplicationCustomNotice : NoticeType.ApplicationNotice;

                if (commandParam.LocalUid > 0)
                {
                    noticeinfo.Poster = shortUserInfo == null ? "" : shortUserInfo.Username;
                    noticeinfo.Posterid = commandParam.LocalUid;
                }
                else
                {
                    noticeinfo.Poster = "";
                    noticeinfo.Posterid = 0;
                }
                noticeinfo.Note = Utils.EncodeHtml(notification);//需要做ubb标签转换

                if (Notices.CreateNoticeInfo(noticeinfo) > 0)
                    successfulIds += (id + ",");
            }

            if (successfulIds.Length > 0)
                successfulIds = successfulIds.Remove(successfulIds.Length - 1);
            if (commandParam.Format == FormatType.JSON)
            {
                result = string.Format("\"{0}\"", successfulIds);
            }
            else
            {
                NotificationSendResponse nsr = new NotificationSendResponse();
                nsr.Result = successfulIds;
                result = SerializationHelper.Serialize(nsr);
            }
            return true;
        }
    }

    /// <summary>
    /// 发送Email
    /// </summary>
    public sealed class SendEmailCommand : Command
    {
        public SendEmailCommand()
            : base("notifications.sendemail")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果是桌面程序则需要验证用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                    return false;
                }
                //如果当前用户不是管理员
                if (Discuz.Forum.UserGroups.GetUserGroupInfo(Discuz.Forum.Users.GetShortUserInfo(commandParam.LocalUid).Groupid).Radminid != 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }
            }

            //	 recipients subject 
            if (!commandParam.CheckRequiredParams("recipients,subject,text"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string recipients = commandParam.GetDNTParam("recipients").ToString();

            if (!Utils.IsNumericList(recipients))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            //需要过滤部分html标签，待开发
            //得到了 用逗号分隔的ids 和 subject，先通过ids得到所有人的用户名
            string uids = Discuz.Forum.Emails.SendMailToUsers(recipients, commandParam.GetDNTParam("subject").ToString(), commandParam.GetDNTParam("text").ToString());

            if (commandParam.Format == FormatType.JSON)
                result = string.Format("\"{0}\"", uids);
            else
            {
                NotificationSendEmailResponse nser = new NotificationSendEmailResponse();
                nser.Recipients = uids;
                result = SerializationHelper.Serialize(nser);
            }
            return true;
        }
    }

    /// <summary>
    /// 获取论坛通知
    /// </summary>
    public sealed class GetNoticesCommand : Command
    {
        public GetNoticesCommand()
            : base("notifications.get")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            //get unread and mostrecent message/notification count
            NotificationGetResponse notification = new NotificationGetResponse();
            notification.Message = new Notification();
            notification.Message.Unread = Discuz.Forum.PrivateMessages.GetPrivateMessageCount(commandParam.LocalUid, 0, 1);
            notification.Message.MostRecent = Discuz.Forum.PrivateMessages.GetLatestPMID(commandParam.LocalUid);


            notification.Notification = new Notification();
            notification.Notification.Unread = Discuz.Forum.Notices.GetNoticeCount(commandParam.LocalUid, 1);
            notification.Notification.MostRecent = Discuz.Forum.Notices.GetLatestNoticeID(commandParam.LocalUid);

            result = commandParam.Format == FormatType.JSON ?
                JavaScriptConvert.SerializeObject(notification) : SerializationHelper.Serialize(notification);

            return true;
        }
    }
}
