using System;
using System.Text;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using Newtonsoft.Json;

namespace Discuz.Web.Services.API.Commands
{

    /// <summary>
    /// 发布主题
    /// </summary>
    public sealed class PostTopicCommand : Command
    {
        public PostTopicCommand()
            : base("topics.create")
        {
        }
        /*
         * Description:
         * 桌面程序强制validate=true,且必须是在线用户
         */
        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //桌面程序因为安全需要,游客不允许操作
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            //如果validate为true,则校验数据的合法性,包括广告强力屏蔽,是否含有需审核的，以及非法内容.和当前用户的发帖权限,桌面程序强制验证
            bool validate = commandParam.GetIntParam("validate") == 1 || commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP;

            if (!commandParam.CheckRequiredParams("topic_info"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }
            Topic topic;
            try
            {
                topic = JavaScriptConvert.DeserializeObject<Topic>(commandParam.GetDNTParam("topic_info").ToString());
            }
            catch
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }
            if (topic == null || Util.AreParamsNullOrZeroOrEmptyString(topic.Fid, topic.Title, topic.Message))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }
            //文档中应说明title长度范围和内容范围
            if (topic.Title.Length > 60)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_TITLE_INVALID, commandParam.ParamList);
                return false;
            }

            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(topic.Fid ?? 0);
            if (forumInfo == null || forumInfo.Layer == 0)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_FORUM_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            //如果validate为true,则强制读取当前用户
            ShortUserInfo userInfo = Discuz.Forum.Users.GetShortUserInfo(validate || (topic.UId == null) ? commandParam.LocalUid : (int)topic.UId);
            userInfo = userInfo == null ? TopicsCommandUtils.GetGuestUserInfo() : userInfo;
            UserGroupInfo userGroupInfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);
            AdminGroupInfo adminInfo = AdminGroups.GetAdminGroupInfo(userGroupInfo.Groupid);

            //是否受审核、过滤、灌水等限制权限
            int disablePost = adminInfo != null ? adminInfo.Disablepostctrl : userGroupInfo.Disableperiodctrl;
            bool hasAudit = false;

            if (validate)
            {
                ErrorType et = TopicsCommandUtils.GeneralValidate(topic.Title, topic.Message, userInfo, userGroupInfo, forumInfo, commandParam, disablePost);
                if (et != ErrorType.API_EC_NONE)
                {
                    result = Util.CreateErrorMessage(et, commandParam.ParamList);
                    return false;
                }
                string str = "";
                //是否允许发主题
                if (!UserAuthority.PostAuthority(forumInfo, userGroupInfo, userInfo.Uid, ref str))
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_POST_PERM, commandParam.ParamList);
                    return false;
                }

                if (disablePost != 1)
                {
                    et = TopicsCommandUtils.PostTimeAndRepostMessageValidate(userInfo, topic.Title + topic.Message);
                    if (et != ErrorType.API_EC_NONE)
                    {
                        result = Util.CreateErrorMessage(et, commandParam.ParamList);
                        return false;
                    }
                    //内容中是否含有需审核的词汇
                    if (ForumUtils.HasAuditWord(topic.Title + topic.Message))
                        hasAudit = true;
                    //过滤非法词汇
                    topic.Title = ForumUtils.BanWordFilter(topic.Title);
                    topic.Message = ForumUtils.BanWordFilter(topic.Message);
                }
            }

            //主题图标id
            int iconId = topic.Iconid ?? 0;
            //图标id值域仅为0-15
            iconId = (iconId > 15 || iconId < 0) ? 0 : iconId;

            TopicInfo topicInfo = new TopicInfo();
            topicInfo.Fid = forumInfo.Fid;
            topicInfo.Iconid = iconId;
            topicInfo.Title = Utils.HtmlEncode(topic.Title);

            bool htmlon = topic.Message.Length != Utils.RemoveHtml(topic.Message).Length && userGroupInfo.Allowhtml == 1;
            //支持html标签?
            if (!htmlon)
                topic.Message = Utils.HtmlEncode(topic.Message);

            string curDateTime = Utils.GetDateTime();

            //发帖主题分类校验和绑定
            topicInfo.Typeid = 0;
            if (forumInfo.Applytopictype == 1)
            {
                if (Discuz.Forum.Forums.IsCurrentForumTopicType(topic.Typeid.ToString(), forumInfo.Topictypes))
                    topicInfo.Typeid = (int)topic.Typeid;
                else if (forumInfo.Postbytopictype == 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                    return false;
                }
            }
            topicInfo.Readperm = 0;
            topicInfo.Price = 0;
            topicInfo.Poster = userInfo.Username;
            topicInfo.Posterid = userInfo.Uid;
            topicInfo.Postdatetime = curDateTime;
            topicInfo.Lastpost = curDateTime;
            topicInfo.Lastposter = userInfo.Username;
            topicInfo.Views = 0;
            topicInfo.Replies = 0;

            topicInfo.Displayorder = (forumInfo.Modnewtopics == 1) ? -2 : 0;
            if (topicInfo.Displayorder != -2 && (hasAudit || Scoresets.BetweenTime(commandParam.GeneralConfig.Postmodperiods)))
                topicInfo.Displayorder = -2;

            topicInfo.Highlight = "";
            topicInfo.Digest = 0;
            topicInfo.Rate = 0;
            topicInfo.Hide = 0;
            topicInfo.Attachment = 0;
            topicInfo.Moderated = 0;
            topicInfo.Closed = 0;

            string tags = string.Empty;
            string[] tagArray = null;

            //是否使用tag
            bool enableTag = (commandParam.GeneralConfig.Enabletag & forumInfo.Allowtag) == 1;
            if (!string.IsNullOrEmpty(topic.Tags))
            {
                //标签(Tag)操作                
                tags = topic.Tags.Trim();
                tagArray = Utils.SplitString(tags, ",", true, 2, 10);
                if (enableTag)
                {
                    if (topicInfo.Magic == 0)
                        topicInfo.Magic = 10000;
                    topicInfo.Magic = Utils.StrToInt(topicInfo.Magic.ToString() + "1", 0);
                }
            }

            int topicId = Discuz.Forum.Topics.CreateTopic(topicInfo);

            if (enableTag && tagArray != null && tagArray.Length > 0)
            {
                //若当前用户不受过滤,审核约束
                if (!validate || disablePost == 1 || !ForumUtils.HasBannedWord(tags))
                    ForumTags.CreateTopicTags(tagArray, topicId, userInfo.Uid, curDateTime);
            }

            PostInfo postInfo = new PostInfo();
            postInfo.Fid = forumInfo.Fid;
            postInfo.Tid = topicId;
            postInfo.Parentid = 0;
            postInfo.Layer = 0;
            postInfo.Poster = userInfo.Username;
            postInfo.Posterid = userInfo.Uid;
            postInfo.Title = topicInfo.Title;
            postInfo.Postdatetime = curDateTime;
            postInfo.Message = topic.Message;
            postInfo.Ip = DNTRequest.GetIP();
            postInfo.Lastedit = "";
            postInfo.Invisible = topicInfo.Displayorder == -2 ? 1 : 0;
            postInfo.Usesig = 0;
            postInfo.Htmlon = htmlon ? 1 : 0;
            postInfo.Smileyoff = 1 - forumInfo.Allowsmilies;
            postInfo.Bbcodeoff = 1;

            if (userGroupInfo.Allowcusbbcode == 1 && forumInfo.Allowbbcode == 1)
                postInfo.Bbcodeoff = 0;

            postInfo.Parseurloff = 0;
            postInfo.Attachment = 0;
            postInfo.Rate = 0;
            postInfo.Ratetimes = 0;
            postInfo.Topictitle = topicInfo.Title;

            int postid = 0;
            try
            {
                postid = Posts.CreatePost(postInfo);
            }
            catch
            {
                TopicAdmins.DeleteTopics(topicId.ToString(), false);
                result = Util.CreateErrorMessage(ErrorType.API_EC_UNKNOWN, commandParam.ParamList);
                return false;
            }

            Discuz.Forum.Topics.AddParentForumTopics(forumInfo.Parentidlist.Trim(), 1);

            TopicCreateResponse tcr = new TopicCreateResponse();

            tcr.TopicId = topicId;
            tcr.Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + Discuz.Forum.Urls.ShowTopicAspxRewrite(topicId, 0);
            tcr.NeedAudit = topicInfo.Displayorder == -2;

            #region 更新积分

            //设置用户的积分
            ///首先读取版块内自定义积分
            ///版设置了自定义积分则使用，否则使用论坛默认积分
            //float[] values = null;
            //if (!string.IsNullOrEmpty(forumInfo.Postcredits))
            //{
            //    int index = 0;
            //    float tempval = 0;
            //    values = new float[8];
            //    foreach (string ext in Utils.SplitString(forumInfo.Postcredits, ","))
            //    {
            //        if (index == 0)
            //        {
            //            if (!ext.Equals("True"))
            //            {
            //                values = null;
            //                break;
            //            }
            //            index++;
            //            continue;
            //        }
            //        tempval = Utils.StrToFloat(ext, 0);
            //        values[index - 1] = tempval;
            //        index++;
            //        if (index > 8)
            //            break;
            //    }
            //}
            if (userInfo.Adminid == 1 || !tcr.NeedAudit)
                CreditsFacade.PostTopic(userInfo.Uid, forumInfo);
                //TopicsCommandUtils.UpdateScore(userInfo.Uid, values);

            #endregion

            //同步到其他应用程序
            Sync.NewTopic(topicId.ToString(), topicInfo.Title, topicInfo.Poster, topicInfo.Posterid.ToString(), topicInfo.Fid.ToString(), commandParam.AppInfo.APIKey);

            result = commandParam.Format == FormatType.JSON ? JavaScriptConvert.SerializeObject(tcr) : SerializationHelper.Serialize(tcr);
            return true;
        }
    }

    /// <summary>
    /// 回复主题
    /// </summary>
    public sealed class PostReplyCommand : Command
    {
        public PostReplyCommand()
            : base("topics.reply")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果validate为true,则校验数据的合法性,包括广告强力屏蔽,是否含有需审核的，以及非法内容.和当前用户的发帖权限
            bool validate = commandParam.GetIntParam("validate") == 1 || commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP;

            //桌面程序因为安全需要,游客不允许操作
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("reply_info"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            Reply reply;
            try
            {
                reply = JavaScriptConvert.DeserializeObject<Reply>(commandParam.GetDNTParam("reply_info").ToString());
            }
            catch
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (reply == null || Util.AreParamsNullOrZeroOrEmptyString(reply.Tid, reply.Fid, reply.Message))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (reply.Title == null)
                reply.Title = string.Empty;

            if (reply.Title.IndexOf("　") != -1 || reply.Title.Length > 60)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_TITLE_INVALID, commandParam.ParamList);
                return false;
            }

            if (reply.Message.Length < commandParam.GeneralConfig.Minpostsize ||
                reply.Message.Length > commandParam.GeneralConfig.Maxpostsize)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_MESSAGE_LENGTH, commandParam.ParamList);
                return false;
            }

            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(reply.Fid);
            if (forumInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_FORUM_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            TopicInfo topicInfo = Discuz.Forum.Topics.GetTopicInfo(reply.Tid);
            if (topicInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_TOPIC_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            //validate=true或未指定回帖uid时则默认读取当前用户uid,游客为-1
            ShortUserInfo userInfo = Discuz.Forum.Users.GetShortUserInfo(validate || reply.Uid == null ? commandParam.LocalUid : (int)reply.Uid);
            userInfo = userInfo == null ? TopicsCommandUtils.GetGuestUserInfo() : userInfo;
            UserGroupInfo userGroupInfo = Discuz.Forum.UserGroups.GetUserGroupInfo(userInfo.Groupid);
            AdminGroupInfo adminInfo = AdminGroups.GetAdminGroupInfo(userGroupInfo.Groupid);
            //是否受审核、过滤、灌水等限制权限
            int disablePost = adminInfo != null ? adminInfo.Disablepostctrl : userGroupInfo.Disableperiodctrl;
            bool hasAudit = false;

            if (validate)
            {
                ErrorType et = TopicsCommandUtils.GeneralValidate(reply.Title, reply.Message, userInfo, userGroupInfo, forumInfo, commandParam, disablePost);
                if (et != ErrorType.API_EC_NONE)
                {
                    result = Util.CreateErrorMessage(et, commandParam.ParamList);
                    return false;
                }
                //是否有回复的权限
                if (!UserAuthority.PostReply(forumInfo, commandParam.LocalUid, userGroupInfo, topicInfo))
                {
                    result = Util.CreateErrorMessage(topicInfo.Closed >= 1 ? ErrorType.API_EC_TOPIC_CLOSED : ErrorType.API_EC_REPLY_PERM, commandParam.ParamList);
                    return false;
                }

                if (disablePost != 1)
                {
                    et = TopicsCommandUtils.PostTimeAndRepostMessageValidate(userInfo, reply.Title + reply.Message);
                    if (et != ErrorType.API_EC_NONE)
                    {
                        result = Util.CreateErrorMessage(et, commandParam.ParamList);
                        return false;
                    }
                    //内容中是否含有需审核的词汇
                    if (ForumUtils.HasAuditWord(reply.Title + reply.Message))
                        hasAudit = true;

                    reply.Title = ForumUtils.BanWordFilter(reply.Title);
                    reply.Message = ForumUtils.BanWordFilter(reply.Message);

                }
            }
            PostInfo postInfo = TopicsCommandUtils.PostReply(reply, userGroupInfo, userInfo, forumInfo, topicInfo.Title, disablePost, hasAudit);
            if (topicInfo.Replies < (commandParam.GeneralConfig.Ppp + 9))
            {
                ForumUtils.DeleteTopicCacheFile(topicInfo.Tid);
            }

            TopicReplyResponse trr = new TopicReplyResponse();
            trr.PostId = postInfo.Pid;
            trr.Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + string.Format("showtopic.aspx?topicid={0}&postid={1}#{1}", reply.Tid, trr.PostId);
            trr.NeedAudit = postInfo.Invisible == 1;

            //同步到其他应用程序
            Sync.Reply(postInfo.Pid.ToString(), postInfo.Tid.ToString(), postInfo.Topictitle, postInfo.Poster, postInfo.Posterid.ToString(), postInfo.Fid.ToString(), commandParam.AppInfo.APIKey);

            result = commandParam.Format == FormatType.JSON ? JavaScriptConvert.SerializeObject(trr) : SerializationHelper.Serialize(trr);
            return true;
        }
    }

    /// <summary>
    /// 获取最新回复
    /// </summary>
    public sealed class GetRecentRepliesCommand : Command
    {
        public GetRecentRepliesCommand()
            : base("topics.getrecentreplies")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果是桌面程序则需要验证用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("fid,tid,page_size,page_index"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            int fid = commandParam.GetIntParam("fid");
            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(fid);
            if (forumInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_FORUM_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            int tid = commandParam.GetIntParam("tid");
            TopicInfo topicInfo = Discuz.Forum.Topics.GetTopicInfo(tid);
            if (topicInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_TOPIC_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            int pageSize = commandParam.GetIntParam("page_size", commandParam.GeneralConfig.Ppp);
            int pageIndex = commandParam.GetIntParam("page_index", 1);
            pageSize = pageSize < 1 ? commandParam.GeneralConfig.Ppp : pageSize;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;

            PostpramsInfo postPramsInfo = TopicsCommandUtils.GetPostParamInfo(commandParam.LocalUid, topicInfo, forumInfo, pageSize, pageIndex);
            System.Data.DataTable lastpostlist = Posts.GetPagedLastDataTable(postPramsInfo);

            List<Post> list = new List<Post>();
            foreach (System.Data.DataRow dr in lastpostlist.Rows)
            {
                Post post = new Post();
                post.AdIndex = Utils.StrToInt(dr["adindex"], 0);
                post.Invisible = Utils.StrToInt(dr["invisible"], 0);
                post.Layer = Utils.StrToInt(dr["layer"], 0);
                post.Message = dr["message"].ToString();
                post.Pid = Utils.StrToInt(dr["pid"], 0);
                post.PostDateTime = DateTime.Parse(dr["postdatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                post.PosterAvator = dr["avatar"].ToString().Replace("\\", "/");
                post.PosterAvatorWidth = Utils.StrToInt(dr["avatarwidth"], 0);
                post.PosterAvatorHeight = Utils.StrToInt(dr["avatarheight"], 0);
                post.PosterEmail = dr["email"].ToString().Trim();
                post.PosterId = Utils.StrToInt(dr["posterid"], 0);
                post.PosterLocation = dr["location"].ToString();
                post.PosterName = dr["poster"].ToString();
                post.PosterShowEmail = Utils.StrToInt(dr["showemail"], 0);
                post.PosterSignature = dr["signature"].ToString();
                post.Rate = Utils.StrToInt(dr["rate"], 0);
                post.RateTimes = Utils.StrToInt(dr["ratetimes"], 0);
                post.UseSignature = Utils.StrToInt(dr["usesig"], 0);

                list.Add(post);
            }

            TopicGetRencentRepliesResponse tgrrr = new TopicGetRencentRepliesResponse();
            tgrrr.List = true;
            tgrrr.Count = topicInfo.Replies;
            tgrrr.Posts = list.ToArray();

            result = commandParam.Format == FormatType.JSON ?
                JavaScriptConvert.SerializeObject(tgrrr) : Util.AddMessageCDATA(SerializationHelper.Serialize(tgrrr));
            return true;
        }
    }

    /// <summary>
    /// 获取主题列表
    /// </summary>
    public sealed class GetTopicListCommand : Command
    {
        public GetTopicListCommand()
            : base("topics.getlist")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果是桌面程序则需要验证用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("fid,page_size,page_index"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            int fid = commandParam.GetIntParam("fid");
            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(fid);
            if (forumInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_FORUM_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            int pageSize = commandParam.GetIntParam("page_size", commandParam.GeneralConfig.Tpp);
            int pageIndex = commandParam.GetIntParam("page_index", 1);
            pageSize = pageSize < 1 ? commandParam.GeneralConfig.Tpp : pageSize;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;

            //主题分类条件idlist
            string topicTypeIdList = commandParam.GetDNTParam("type_id_list").ToString();
            string condition = string.Empty;//查询主题的条件
            //如果条件不为空且是逗号分割的list，则添加condition条件
            if (!string.IsNullOrEmpty(topicTypeIdList) && Utils.IsNumericList(topicTypeIdList))
                condition = " AND [typeid] IN (" + topicTypeIdList + ") ";

            int count = Discuz.Forum.Topics.GetTopicCount(fid, true, string.Empty);
            List<TopicInfo> topicList = Discuz.Forum.Topics.GetTopicList(fid, pageSize, pageIndex,
                                                              0, 600, commandParam.GeneralConfig.Hottopic, forumInfo.Autoclose,
                                                              forumInfo.Topictypeprefix, condition);
            TopicGetListResponse tglr = new TopicGetListResponse();
            List<ForumTopic> list = new List<ForumTopic>();
            foreach (TopicInfo topicInfo in topicList)
            {
                ForumTopic topic = new ForumTopic();
                topic.Author = topicInfo.Poster;
                topic.AuthorId = topicInfo.Posterid;
                topic.LastPosterId = topicInfo.Lastposterid;
                topic.LastPostTime = DateTime.Parse(topicInfo.Lastpost).ToString("yyyy-MM-dd HH:mm:ss");
                topic.ReplyCount = topicInfo.Replies;
                topic.ViewCount = topicInfo.Views;
                topic.Title = topicInfo.Title;
                topic.TopicId = topicInfo.Tid;
                topic.Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + Discuz.Forum.Urls.ShowTopicAspxRewrite(topic.TopicId, 0);
                list.Add(topic);
            }

            tglr.Count = count;
            tglr.Topics = list.ToArray();
            tglr.List = true;

            result = commandParam.Format == FormatType.JSON ? JavaScriptConvert.SerializeObject(tglr) : SerializationHelper.Serialize(tglr);
            return true;
        }
    }

    /// <summary>
    /// 获取需要关注的主题
    /// </summary>
    public sealed class GetAttentionListCommand : Command
    {
        public GetAttentionListCommand()
            : base("topics.getattentionlist")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果是桌面程序则需要验证用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("fid,page_size,page_index"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            int fid = commandParam.GetIntParam("fid", 0);
            int pageSize = commandParam.GetIntParam("page_size", commandParam.GeneralConfig.Tpp);
            int pageIndex = commandParam.GetIntParam("page_index", 1);
            pageSize = pageSize < 1 ? commandParam.GeneralConfig.Tpp : pageSize;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;

            int count = Discuz.Forum.Topics.GetAttentionTopicCount(fid.ToString(), string.Empty);
            List<TopicInfo> topicList = Discuz.Forum.Topics.GetAttentionTopics(fid.ToString(), pageSize, pageIndex, string.Empty);

            TopicGetListResponse tglr = new TopicGetListResponse();
            List<ForumTopic> list = new List<ForumTopic>();

            foreach (TopicInfo topicInfo in topicList)
            {
                ForumTopic topic = new ForumTopic();
                topic.Author = topicInfo.Poster;
                topic.AuthorId = topicInfo.Posterid;
                topic.LastPosterId = topicInfo.Lastposterid;
                topic.LastPostTime = DateTime.Parse(topicInfo.Lastpost).ToString("yyyy-MM-dd HH:mm:ss");
                topic.ReplyCount = topicInfo.Replies;
                topic.ViewCount = topicInfo.Views;
                topic.Title = topicInfo.Title;
                topic.TopicId = topicInfo.Tid;
                topic.Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + Discuz.Forum.Urls.ShowTopicAspxRewrite(topic.TopicId, 0);
                list.Add(topic);
            }
            tglr.Count = count;
            tglr.Topics = list.ToArray();
            tglr.List = true;

            result = commandParam.Format == FormatType.JSON ? JavaScriptConvert.SerializeObject(tglr) : SerializationHelper.Serialize(tglr);
            return true;
        }
    }

    /// <summary>
    /// 删除主题
    /// </summary>
    public sealed class DeleteTopicCommand : Command
    {
        public DeleteTopicCommand()
            : base("topics.delete")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (!commandParam.CheckRequiredParams("topic_ids"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            string topicIds = commandParam.GetDNTParam("topic_ids").ToString();
            if (!Utils.IsNumericList(topicIds))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (topicIds.Split(',').Length > 20)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }
            int forumId = commandParam.GetIntParam("fid");

            //桌面程序需要验证当前登录用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (!commandParam.CheckRequiredParams("fid"))
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                    return false;
                }

                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                    return false;
                }
                ShortUserInfo user = Discuz.Forum.Users.GetShortUserInfo(commandParam.LocalUid);
                if (user == null || !Moderators.IsModer(user.Adminid, commandParam.LocalUid, forumId))
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }

                if (!Discuz.Forum.Topics.InSameForum(topicIds, forumId))
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                    return false;
                }
            }
            bool deleteResult = Discuz.Forum.TopicAdmins.DeleteTopics(topicIds, false) > 0;

            TopicDeleteResponse tdr = new TopicDeleteResponse();
            tdr.Successfull = deleteResult ? 1 : 0;
            result = commandParam.Format == FormatType.JSON ? string.Format("\"{0}\"", result.ToString().ToLower()) : SerializationHelper.Serialize(tdr);
            return true;
        }
    }

    /// <summary>
    /// 编辑主题
    /// </summary>
    public sealed class EditTopicCommand : Command
    {
        public EditTopicCommand()
            : base("topics.edit")
        {
        }

        /*
         * Description:
         * 该接口需要能关联到一个论坛用户,不允许游客操作,如果validate=true或者接口类型为桌面程序,则只获取session_info中的uid,若无则返回API_EC_SESSIONKEY
         */
        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果validate为true,则校验数据的合法性,包括广告强力屏蔽,是否含有需审核的，以及非法内容.和当前用户的发帖权限
            bool validate = commandParam.GetIntParam("validate") == 1 || commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP;

            //如果validate是true或者桌面程序则需要验证用户身份
            if (validate && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("topic_info,tid"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            Topic topic;
            try
            {
                topic = JavaScriptConvert.DeserializeObject<Topic>(commandParam.GetDNTParam("topic_info").ToString());
            }
            catch
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (topic == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            //文档中应说明title长度范围和内容范围
            if (!Util.AreParamsNullOrZeroOrEmptyString(topic.Title) && topic.Title.Length > 60)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_TITLE_INVALID, commandParam.ParamList);
                return false;
            }

            //编辑主题必须要能关联到一个用户
            ShortUserInfo userInfo = Users.GetShortUserInfo(validate || topic.UId == null ? commandParam.LocalUid : (int)topic.UId);
            if (userInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_EDIT_NOUSER, commandParam.ParamList);
                return false;
            }

            TopicInfo topicInfo = Discuz.Forum.Topics.GetTopicInfo(commandParam.GetIntParam("tid", 0));
            if (topicInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_TOPIC_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(topic.Fid ?? topicInfo.Fid);
            if (forumInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_FORUM_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            UserGroupInfo userGroupInfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);
            AdminGroupInfo adminInfo = AdminGroups.GetAdminGroupInfo(userGroupInfo.Groupid);
            //是否受审核、过滤、灌水等限制权限
            int disablePost = adminInfo != null ? adminInfo.Disablepostctrl : userGroupInfo.Disableperiodctrl;
            bool hasAudit = false;
            if (validate)
            {
                string title = topic.Title ?? "";
                string message = topic.Message ?? "";

                ErrorType et = TopicsCommandUtils.GeneralValidate(title, message, userInfo, userGroupInfo, forumInfo, commandParam, disablePost);
                if (et != ErrorType.API_EC_NONE)
                {
                    result = Util.CreateErrorMessage(et, commandParam.ParamList);
                    return false;
                }

                //如果主题作者与当前用户不一样且当前用户不是管理员
                if (topicInfo.Posterid != commandParam.LocalUid && userInfo.Adminid != 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_EDIT_PERM, commandParam.ParamList);
                    return false;
                }

                //如果当前用户是管理组成员,则跳过编辑时间限制校验
                if (!Moderators.IsModer(userInfo.Adminid, commandParam.LocalUid, forumInfo.Fid))
                {
                    if (commandParam.GeneralConfig.Edittimelimit == -1)
                    {
                        result = Util.CreateErrorMessage(ErrorType.API_EC_EDIT_PERM, commandParam.ParamList);
                        return false;
                    }
                    if (commandParam.GeneralConfig.Edittimelimit > 0 &&
                        Utils.StrDateDiffSeconds(topicInfo.Postdatetime, commandParam.GeneralConfig.Edittimelimit) > 0)
                    {
                        result = Util.CreateErrorMessage(ErrorType.API_EC_EDIT_PERM, commandParam.ParamList);
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(title + message))
                {
                    if (ForumUtils.HasAuditWord(title) || ForumUtils.HasAuditWord(message))
                        hasAudit = true;

                    if (disablePost != 1)
                    {
                        topic.Title = ForumUtils.BanWordFilter(topic.Title);
                        topic.Message = ForumUtils.BanWordFilter(topic.Message);
                    }
                }
            }

            topic.Iconid = topic.Iconid ?? 0;
            topic.Iconid = topic.Iconid > 15 || topic.Iconid < 0 ? 0 : topic.Iconid;

            topicInfo.Fid = topic.Fid ?? topicInfo.Fid;
            topicInfo.Iconid = (int)topic.Iconid;
            topicInfo.Title = topic.Title != null ? Utils.HtmlEncode(topic.Title) : topicInfo.Title;
            topicInfo.Displayorder = hasAudit ? -2 : topicInfo.Displayorder;

            if (topic.Message != null)
            {
                bool htmlon = topic.Message.Length != Utils.RemoveHtml(topic.Message).Length && userGroupInfo.Allowhtml == 1;
                topic.Message = htmlon ? Utils.HtmlDecode(topic.Message) : topic.Message;
            }

            bool enabletag = (commandParam.GeneralConfig.Enabletag & forumInfo.Allowtag) == 1;
            string tags = string.Empty;
            string[] tagArray = null;

            if (!string.IsNullOrEmpty(topic.Tags))
            {
                //标签(Tag)操作                
                tags = topic.Tags.Trim();
                tagArray = Utils.SplitString(tags, ",", true, 2, 10);
                if (enabletag)
                {
                    if (topicInfo.Magic == 0)
                        topicInfo.Magic = 10000;
                    topicInfo.Magic = Utils.StrToInt(topicInfo.Magic.ToString() + "1", 0);
                }
            }

            if (forumInfo.Applytopictype == 1)
            {
                if (Discuz.Forum.Forums.IsCurrentForumTopicType(topic.Typeid.ToString(), forumInfo.Topictypes))
                {
                    topicInfo.Typeid = (int)topic.Typeid;
                }
                else if (forumInfo.Postbytopictype == 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                    return false;
                }
            }

            int editResult = Discuz.Forum.Topics.UpdateTopic(topicInfo);

            if (enabletag && tagArray != null && tagArray.Length > 0)
            {
                if (disablePost == 1 || !ForumUtils.HasBannedWord(tags))
                    ForumTags.CreateTopicTags(tagArray, topicInfo.Tid, userInfo.Uid, topicInfo.Postdatetime);
            }

            PostInfo postInfo = Discuz.Forum.Posts.GetPostInfo(topicInfo.Tid, Discuz.Forum.Posts.GetFirstPostId(topicInfo.Tid));
            if (topic.Fid != null)
                postInfo.Fid = forumInfo.Fid;
            if (topic.Title != null)
            {
                postInfo.Title = topicInfo.Title;
                postInfo.Topictitle = topicInfo.Title;
            }
            postInfo.Message = topic.Message ?? postInfo.Message;

            editResult = Posts.UpdatePost(postInfo);

            TopicEditResponse ter = new TopicEditResponse();
            ter.Successfull = editResult;

            result = commandParam.Format == FormatType.JSON ? (editResult == 1).ToString().ToLower() : SerializationHelper.Serialize(ter);
            return true;
        }
    }

    /// <summary>
    /// 获取主题
    /// </summary>
    public sealed class GetTopicCommand : Command
    {
        public GetTopicCommand()
            : base("topics.get")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果是桌面程序则需要验证用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP && commandParam.LocalUid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                return false;
            }

            if (!commandParam.CheckRequiredParams("tid,page_size,page_index"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            int tid = commandParam.GetIntParam("tid");
            TopicInfo topicInfo = Discuz.Forum.Topics.GetTopicInfo(tid);
            if (topicInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_TOPIC_NOT_EXIST, commandParam.ParamList);
                return false;
            }
            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(topicInfo.Fid);

            int pageSize = commandParam.GetIntParam("page_size", commandParam.GeneralConfig.Tpp);
            int pageIndex = commandParam.GetIntParam("page_index", 1);
            pageSize = pageSize < 1 ? commandParam.GeneralConfig.Tpp : pageSize;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;

            PostpramsInfo postPramsInfo = TopicsCommandUtils.GetPostParamInfo(commandParam.LocalUid, topicInfo, forumInfo, pageSize, pageIndex);
            List<ShowtopicPageAttachmentInfo> attachmentList = new List<ShowtopicPageAttachmentInfo>();

            List<ShowtopicPagePostInfo> postList = Posts.GetPostList(postPramsInfo, out attachmentList, false);

            List<Post> list = new List<Post>();
            foreach (ShowtopicPagePostInfo postInfo in postList)
            {
                Post post = new Post();
                post.AdIndex = postInfo.Adindex;
                post.Invisible = postInfo.Invisible;
                post.Layer = postInfo.Layer;
                post.Message = postInfo.Message;
                post.Pid = postInfo.Pid;
                post.PostDateTime = postInfo.Postdatetime;
                post.PosterAvator = postInfo.Avatar;
                post.PosterAvatorWidth = postInfo.Avatarwidth;
                post.PosterAvatorHeight = postInfo.Avatarheight;
                post.PosterEmail = postInfo.Email;
                post.PosterId = postInfo.Posterid;
                post.PosterLocation = postInfo.Location;
                post.PosterName = postInfo.Poster;
                post.PosterShowEmail = postInfo.Showemail;
                post.PosterSignature = postInfo.Signature;
                post.Rate = postInfo.Rate;
                post.RateTimes = postInfo.Ratetimes;
                post.UseSignature = postInfo.Usesig;

                list.Add(post);
            }
            TopicGetResponse tgr = new TopicGetResponse();
            tgr.Author = topicInfo.Poster;
            tgr.AuthorId = topicInfo.Posterid;
            tgr.Fid = topicInfo.Fid;
            tgr.Iconid = topicInfo.Iconid;
            tgr.LastPosterId = topicInfo.Lastposterid;
            tgr.LastPostTime = topicInfo.Lastpost;
            tgr.List = list.Count > 1;
            tgr.ReplyCount = topicInfo.Replies;
            tgr.Tags = ForumTags.GetTagsByTopicId(topicInfo.Tid);
            tgr.Title = topicInfo.Title;
            tgr.TopicId = topicInfo.Tid;
            tgr.Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + Discuz.Forum.Urls.ShowTopicAspxRewrite(topicInfo.Tid, 0);
            tgr.ViewCount = topicInfo.Views;
            tgr.TypeId = topicInfo.Typeid;

            SortedList<int, string> topicTypeList = Caches.GetTopicTypeArray();
            topicTypeList.TryGetValue(topicInfo.Typeid, out tgr.TypeName);

            tgr.Posts = list.ToArray();
            tgr.Attachments = TopicsCommandUtils.ConvertAttachmentArray(attachmentList);

            result = commandParam.Format == FormatType.JSON ?
                JavaScriptConvert.SerializeObject(tgr) : Util.AddTitleCDATA(Util.AddMessageCDATA(SerializationHelper.Serialize(tgr)));

            return true;
        }
    }

    /// <summary>
    /// 删除主题回复
    /// </summary>
    public sealed class DeleteRepliesCommand : Command
    {
        public DeleteRepliesCommand()
            : base("topics.deletereplies")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (!commandParam.CheckRequiredParams("post_ids,tid"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }
            string successfulIds = string.Empty;

            int tid = commandParam.GetIntParam("tid");
            //如果是桌面程序则需要验证用户身份
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                    return false;
                }
                ShortUserInfo userInfo = Discuz.Forum.Users.GetShortUserInfo(commandParam.LocalUid);
                TopicInfo topicInfo = Discuz.Forum.Topics.GetTopicInfo(tid);
                if (!Discuz.Forum.Moderators.IsModer(userInfo.Adminid, commandParam.LocalUid, topicInfo.Fid))
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }
            }

            int i = 0;
            string postTableId = Discuz.Forum.Posts.GetPostTableId(tid);
            foreach (string s in commandParam.GetDNTParam("post_ids").ToString().Split(','))
            {
                int pid = TypeConverter.StrToInt(s);
                if (pid < 1)
                    continue;
                if (Discuz.Forum.Posts.DeletePost(postTableId, pid, false, true) > 0)
                    successfulIds += (pid + ",");
                if (++i >= 20)
                    break;
            }

            if (successfulIds.Length > 0)
                successfulIds = successfulIds.Remove(successfulIds.Length - 1);

            if (commandParam.Format == FormatType.JSON)
                result = string.Format("\"{0}\"", successfulIds);
            else
            {
                TopicDeleteRepliesResponse tdrr = new TopicDeleteRepliesResponse();
                tdrr.Result = successfulIds;
                result = SerializationHelper.Serialize(tdrr);
            }
            return true;
        }
    }


    public class TopicsCommandUtils
    {
        public static ShortUserInfo GetGuestUserInfo()
        {
            ShortUserInfo guest = new ShortUserInfo();
            guest.Uid = -1;
            guest.Username = "游客";
            guest.Groupid = 7;
            guest.Adminid = 0;
            guest.Posts = 0;
            guest.Joindate = Utils.GetDateTime();

            return guest;
        }

        //public static void UpdateScore(int userid, float[] values)
        //{
        //    //如果版块内积分策略为空则使用全局积分策略
        //    if (values != null)
        //        UserCredits.UpdateUserExtCredits(userid, values, false);
        //    else
        //        UserCredits.UpdateUserCreditsByPostTopic(userid);
        //}

        /// <summary>
        /// 将Entity内附件列表类型转换成API内附件列表类型
        /// </summary>
        /// <param name="attachmentList"></param>
        /// <returns></returns>
        public static Attachment[] ConvertAttachmentArray(List<ShowtopicPageAttachmentInfo> attachmentList)
        {
            List<Attachment> apiAttachmentList = new List<Attachment>();
            foreach (ShowtopicPageAttachmentInfo s in attachmentList)
            {
                Attachment a = new Attachment();
                a.AId = s.Aid;
                a.AllowRead = s.Allowread;
                a.IsImage = s.Attachimgpost;
                a.OriginalFileName = s.Attachment;
                a.AttachPrice = s.Attachprice;
                a.Description = s.Description;
                a.DownloadCount = s.Downloads;
                a.FileName = s.Filename;
                a.FileSize = s.Filesize;
                a.FileType = s.Filetype;
                a.DownloadPerm = s.Getattachperm;
                a.Inserted = s.Inserted;
                a.IsBought = s.Isbought;
                a.PId = s.Pid;
                a.PostDateTime = s.Postdatetime;
                a.Preview = s.Preview;
                a.ReadPerm = s.Readperm;
                a.TId = s.Tid;
                a.UId = s.Uid;
                apiAttachmentList.Add(a);
            }
            return apiAttachmentList.ToArray();
        }

        /// <summary>
        /// 发回复
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="reply"></param>
        /// <param name="usergroupinfo"></param>
        /// <param name="userinfo"></param>
        /// <param name="foruminfo"></param>
        /// <param name="topictitle"></param>
        /// <returns></returns>
        public static PostInfo PostReply(Reply reply, UserGroupInfo usergroupinfo, ShortUserInfo userinfo, ForumInfo foruminfo, string topictitle, int disablePost, bool hasAudit)
        {
            int hide = ForumUtils.IsHidePost(reply.Message) && usergroupinfo.Allowhidecode == 1 ? 1 : 0;
            string curdatetime = Utils.GetDateTime();
            if (reply.Title.Length >= 50)
                reply.Title = Utils.CutString(reply.Title, 0, 50) + "...";

            PostInfo postinfo = new PostInfo();
            postinfo.Fid = reply.Fid;
            postinfo.Tid = reply.Tid;
            postinfo.Parentid = 0;
            postinfo.Layer = 1;
            postinfo.Poster = (userinfo == null ? "游客" : userinfo.Username);
            postinfo.Posterid = userinfo.Uid;

            bool htmlon = reply.Message.Length != Utils.RemoveHtml(reply.Message).Length && usergroupinfo.Allowhtml == 1;
            reply.Message = !htmlon ? Utils.HtmlDecode(reply.Message) : reply.Message;
            postinfo.Title = Utils.HtmlEncode(reply.Title);
            postinfo.Message = reply.Message;
            postinfo.Postdatetime = curdatetime;

            postinfo.Ip = DNTRequest.GetIP();
            postinfo.Lastedit = "";
            postinfo.Debateopinion = 0;
            postinfo.Invisible = disablePost != 1 ? foruminfo.Modnewposts : 0;
            if (postinfo.Invisible != 1 && disablePost != 1)
                postinfo.Invisible = Scoresets.BetweenTime(GeneralConfigs.GetConfig().Postmodperiods) || hasAudit ? 1 : 0;

            postinfo.Usesig = 1;
            postinfo.Htmlon = htmlon ? 1 : 0;

            postinfo.Smileyoff = 1 - foruminfo.Allowsmilies;
            postinfo.Bbcodeoff = usergroupinfo.Allowcusbbcode == 1 && foruminfo.Allowbbcode == 1 ? 0 : 1;
            postinfo.Parseurloff = 0;
            postinfo.Attachment = 0;
            postinfo.Rate = 0;
            postinfo.Ratetimes = 0;
            postinfo.Topictitle = topictitle;

            // 产生新帖子
            int postid = Posts.CreatePost(postinfo);

            if (hide == 1)
                Discuz.Forum.Topics.UpdateTopicHide(reply.Tid);

            Discuz.Forum.Topics.UpdateTopicReplyCount(reply.Tid);
            //设置用户的积分
            ///首先读取版块内自定义积分
            ///版设置了自定义积分则使用，否则使用论坛默认积分
            if (postinfo.Invisible == 0 && userinfo != null)
                CreditsFacade.PostReply(userinfo.Uid, foruminfo);
            //float[] values = null;
            //if (!foruminfo.Replycredits.Equals(""))
            //{
            //    int index = 0;
            //    float tempval = 0;
            //    values = new float[8];
            //    foreach (string ext in Utils.SplitString(foruminfo.Replycredits, ","))
            //    {
            //        if (index == 0)
            //        {
            //            if (!ext.Equals("True"))
            //            {
            //                values = null;
            //                break;
            //            }
            //            index++;
            //            continue;
            //        }
            //        tempval = Utils.StrToFloat(ext, 0.0f);
            //        values[index - 1] = tempval;
            //        index++;
            //        if (index > 8)
            //        {
            //            break;
            //        }
            //    }
            //}

            //if (postinfo.Invisible != 1 && userinfo != null)
            //{
            //    if (values != null)
            //    {
            //        ///使用版块内积分
            //        UserCredits.UpdateUserExtCredits(userinfo.Uid, values, false);
            //    }
            //    else
            //    {
            //        ///使用默认积分
            //        UserCredits.UpdateUserCreditsByPosts(userinfo.Uid);
            //    }
            //}
            postinfo.Pid = postid;
            return postinfo;
        }

        /// <summary>
        /// 发帖,回帖,编辑帖子的共性权限校验
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        /// <param name="userInfo">用户资料</param>
        /// <param name="userGroupInfo">用户组</param>
        /// <param name="forumInfo">发帖版块</param>
        /// <param name="commandParam">任务参数对象</param>
        /// <param name="disablePost">是否忽略审核,灌水等验证</param>
        /// <returns></returns>
        public static ErrorType GeneralValidate(string title, string message, ShortUserInfo userInfo, UserGroupInfo userGroupInfo, ForumInfo forumInfo, CommandParameter commandParam, int disablePost)
        {
            GeneralConfigInfo config = commandParam.GeneralConfig;

            // 用户是否在新手见习期,如果游客允许发帖,就不能设置新手见习期
            if (Utils.StrDateDiffMinutes(userInfo.Joindate, config.Newbiespan) < 0)
                return ErrorType.API_EC_FRESH_USER;

            //版块设置了访问密码,则不允许API发帖回帖
            if (!string.IsNullOrEmpty(forumInfo.Password))
                return ErrorType.API_EC_FORUM_PASSWORD;

            //判断当前用户在当前版块浏览权限
            string str = "";
            if (!UserAuthority.VisitAuthority(forumInfo, userGroupInfo, commandParam.LocalUid, ref str))
                return ErrorType.API_EC_FORUM_PERM;

            //若当前用户无法忽略审核灌水过滤
            if (disablePost != 1)
            {
                if (ForumUtils.HasBannedWord(title + message))
                    return ErrorType.API_EC_SPAM;

                //如果开启新用户广告强力屏蔽检查或是游客
                if ((config.Disablepostad == 1) && userInfo.Adminid < 1)
                {
                    if ((config.Disablepostadpostcount != 0 && userInfo.Posts <= config.Disablepostadpostcount) || (config.Disablepostadregminute != 0 &&
                        DateTime.Now.AddMinutes(-config.Disablepostadregminute) <= Convert.ToDateTime(userInfo.Joindate)))
                    {
                        foreach (string regular in config.Disablepostadregular.Replace("\r", "").Split('\n'))
                        {
                            if (Posts.IsAD(regular, title, message))
                                return ErrorType.API_EC_SPAM;
                        }
                    }
                }
            }
            return ErrorType.API_EC_NONE;
        }

        /// <summary>
        /// 重复内容和发帖时间间隔校验
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ErrorType PostTimeAndRepostMessageValidate(ShortUserInfo userInfo, string context)
        {
            //是否未超过发布内容的时间间隔限制
            if (!CommandCacheQueue<PostTimeItem>.EnQueue(new PostTimeItem(userInfo.Uid, DateTime.Now.Ticks)))
                return ErrorType.API_EC_POST_TOOFAST;

            //是否发布了重复的内容
            if (!CommandCacheQueue<TopicItem>.EnQueue(new TopicItem(userInfo.Uid, Utils.BKDEHash(context, 131))))
                return ErrorType.API_EC_REPOST_MESSAGE;

            return ErrorType.API_EC_NONE;
        }


        /// <summary>
        /// 获取帖子参数
        /// </summary>
        /// <param name="topicInfo"></param>
        /// <param name="forumInfo"></param>
        /// <returns></returns>
        public static PostpramsInfo GetPostParamInfo(int localUid, TopicInfo topicInfo, ForumInfo forumInfo, int pageSize, int pageIndex)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            //判断是否为回复可见帖, hide=0为非回复可见(正常), hide > 0为回复可见, hide=-1为回复可见但当前用户已回复
            int hide = 0;
            if (topicInfo.Hide == 1)
            {
                hide = topicInfo.Hide;
                if (localUid > 0 && Posts.IsReplier(topicInfo.Tid, localUid))
                {
                    hide = -1;
                }
            }
            //判断是否为回复可见帖, price=0为非购买可见(正常), price > 0 为购买可见, price=-1为购买可见但当前用户已购买
            int price = 0;
            if (topicInfo.Price > 0)
            {
                price = topicInfo.Price;
                if (localUid > 0 && PaymentLogs.IsBuyer(topicInfo.Tid, localUid))//判断当前用户是否已经购买
                {
                    price = -1;
                }
            }

            ShortUserInfo userInfo = new ShortUserInfo();
            if (localUid > 0)
            {
                userInfo = Discuz.Forum.Users.GetShortUserInfo(localUid);
            }
            PostpramsInfo postpramsinfo = new PostpramsInfo();
            postpramsinfo.Fid = topicInfo.Fid;
            postpramsinfo.Tid = topicInfo.Tid;
            postpramsinfo.Jammer = forumInfo.Jammer;
            postpramsinfo.Pagesize = pageSize;
            postpramsinfo.Pageindex = pageIndex;
            postpramsinfo.Getattachperm = forumInfo.Getattachperm;
            postpramsinfo.Usergroupid = userInfo.Uid < 1 ? 7 : userInfo.Groupid;
            postpramsinfo.Attachimgpost = config.Attachimgpost;
            postpramsinfo.Showattachmentpath = config.Showattachmentpath;
            postpramsinfo.Hide = hide;
            postpramsinfo.Price = price;
            postpramsinfo.Ubbmode = false;

            postpramsinfo.Showimages = forumInfo.Allowimgcode;
            postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsinfo.Smiliesmax = config.Smiliesmax;
            postpramsinfo.Bbcodemode = config.Bbcodemode;
            postpramsinfo.CurrentUserGroup = Discuz.Forum.UserGroups.GetUserGroupInfo(postpramsinfo.Usergroupid);
            postpramsinfo.Usercredits = userInfo.Credits;
            return postpramsinfo;
        }
    }
}
