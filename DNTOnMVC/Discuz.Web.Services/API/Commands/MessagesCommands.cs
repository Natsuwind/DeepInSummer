using System;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;

namespace Discuz.Web.Services.API.Commands
{
    /// <summary>
    /// 发送论坛短消息
    /// </summary>
    public sealed class SendMessageCommand : Command
    {
        public SendMessageCommand()
            : base("messages.send")
        {
        }

        /*
         * 每个用户UID 30秒内只能调用一次该接口
         */
        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果是桌面程序则需要验证用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("subject,message,to_ids"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string ids = commandParam.GetDNTParam("to_ids").ToString();
            if (!Utils.IsNumericList(ids))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string[] idArray = ids.Split(',');
            if (idArray.Length > 10)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PM_TOID_OVERFLOW, commandParam.ParamList);
                return false;
            }
            //桌面应用程序用户强制使用session_info.uid
            int fromId = commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP ?
                commandParam.LocalUid : commandParam.GetIntParam("from_id", commandParam.LocalUid);
            ShortUserInfo fromUserInfo = Discuz.Forum.Users.GetShortUserInfo(fromId);
            if (fromUserInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PM_FROMID_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            //如果发送用户不是管理员,且在30秒内调用了该接口
            if (fromUserInfo.Adminid != 1 && !CommandCacheQueue<SendMessageItem>.EnQueue(new SendMessageItem(fromUserInfo.Uid, DateTime.Now.Ticks)))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PM_VISIT_TOOFAST, commandParam.ParamList);
                return false;
            }

            string message = UBB.ParseUrl(Utils.EncodeHtml(commandParam.GetDNTParam("message").ToString()));
            string successfulIds = string.Empty;
            foreach (string id in ids.Split(','))
            {
                int toUid = TypeConverter.StrToInt(id);
                if (toUid < 1 || toUid == fromId)
                    continue;
                ShortUserInfo toUserInfo = Discuz.Forum.Users.GetShortUserInfo(toUid);
                if (toUserInfo == null)
                    continue;

                PrivateMessageInfo pm = new PrivateMessageInfo();
                pm.Folder = 0;
                pm.Message = message;
                pm.Msgfrom = fromUserInfo.Username;
                pm.Msgfromid = fromId;
                pm.Msgto = toUserInfo.Username;
                pm.Msgtoid = TypeConverter.StrToInt(id);
                pm.New = 1;
                pm.Postdatetime = Utils.GetDateTime();
                pm.Subject = commandParam.GetDNTParam("subject").ToString();

                successfulIds += (PrivateMessages.CreatePrivateMessage(pm, 0) > 0) ? (id + ",") : "";
            }
            successfulIds = successfulIds.Length > 0 ? successfulIds.Remove(successfulIds.Length - 1) : successfulIds;

            if (commandParam.Format == FormatType.JSON)
                result = string.Format("\"{0}\"", successfulIds);
            else
            {
                MessageSendResponse nsr = new MessageSendResponse();
                nsr.Result = successfulIds;
                result = SerializationHelper.Serialize(nsr);
            }
            return true;
        }
    }

    /// <summary>
    /// 获取论坛短消息
    /// </summary>
    public sealed class GetMessagesCommand : Command
    {
        public GetMessagesCommand()
            : base("messages.get")
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
            }

            if (!commandParam.CheckRequiredParams("uid,page_size,page_index"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            int uid = commandParam.GetIntParam("uid");
            int pageSize = commandParam.GetIntParam("page_size", 10);
            int pageIndex = commandParam.GetIntParam("page_index", 1);
            pageSize = pageSize < 1 ? 10 : pageSize;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;

            List<PrivateMessageInfo> list = PrivateMessages.GetPrivateMessageCollection(uid, 0, pageSize, pageIndex, 1);

            List<Message> newList = new List<Message>();
            foreach (PrivateMessageInfo pm in list)
            {
                Message m = new Message();
                m.MessageId = pm.Pmid;
                m.From = pm.Msgfrom;
                m.FromId = pm.Msgfromid;
                m.MessageContent = pm.Message;
                m.PostDateTime = pm.Postdatetime;
                m.Subject = pm.Subject;

                newList.Add(m);
            }

            MessageGetResponse mgr = new MessageGetResponse();
            mgr.Count = PrivateMessages.GetPrivateMessageCount(uid, 0, 1);
            mgr.List = true;
            mgr.Messages = newList.ToArray();

            result = commandParam.Format == FormatType.JSON ?
                JavaScriptConvert.SerializeObject(mgr) : Util.AddMessageCDATA(SerializationHelper.Serialize(mgr));

            return true;
        }
    }
}
