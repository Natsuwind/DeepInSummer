using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discuz.Common;
using Discuz.Entity;
using System.Data;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Services.API.Commands
{
    /// <summary>
    /// 创建论坛版块
    /// </summary>
    public sealed class CreateForumCommand : Command
    {
        public CreateForumCommand()
            : base("forums.create")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (commandParam.LocalUid < 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
                    return false;
                }

                ShortUserInfo userInfo = Users.GetShortUserInfo(commandParam.LocalUid);
                if (userInfo == null || userInfo.Adminid != 1)
                {
                    result = Util.CreateErrorMessage(ErrorType.API_EC_PERMISSION_DENIED, commandParam.ParamList);
                    return false;
                }
            }

            if (!commandParam.CheckRequiredParams("forum_info"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            Forum forum;
            try
            {
                forum = JavaScriptConvert.DeserializeObject<Forum>(commandParam.GetDNTParam("forum_info").ToString());
            }
            catch
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (forum == null || string.IsNullOrEmpty(forum.Name))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            if (!Utils.StrIsNullOrEmpty(forum.RewriteName) && Discuz.Forum.Forums.CheckRewriteNameInvalid(forum.RewriteName))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_REWRITENAME, commandParam.ParamList);
                return false;
            }

            int fid;
            if (forum.ParentId > 0)
            {
                #region 添加与当前论坛同级的论坛

                //添加与当前论坛同级的论坛
                ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(forum.ParentId);

                //找出当前要插入的记录所用的FID
                string parentidlist = null;
                parentidlist = forumInfo.Parentidlist == "0" ? forumInfo.Fid.ToString() : forumInfo.Parentidlist + "," + forumInfo.Fid;

                DataTable dt = AdminForums.GetMaxDisplayOrder(forum.ParentId);
                int maxdisplayorder = (dt.Rows.Count > 0) && (dt.Rows[0][0].ToString() != "") ? Convert.ToInt32(dt.Rows[0][0]) : forumInfo.Displayorder;

                AdminForums.UpdateForumsDisplayOrder(maxdisplayorder);
                fid = ForumsCommandUtils.InsertForum(forum, forumInfo.Layer + 1, parentidlist, 0, maxdisplayorder + 1);

                AdminForums.SetSubForumCount(forumInfo.Fid);
                #endregion
            }
            else
            {
                #region 按根论坛插入

                int maxdisplayorder = AdminForums.GetMaxDisplayOrder();
                fid = ForumsCommandUtils.InsertForum(forum, 0, "0", 0, maxdisplayorder);

                #endregion
            }
            ForumCreateResponse fcr = new ForumCreateResponse();
            fcr.Fid = fid;
            fcr.Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + Urls.ShowForumAspxRewrite(fid, 1, forum.RewriteName);
            result = commandParam.Format == FormatType.JSON ? JavaScriptConvert.SerializeObject(fcr) : SerializationHelper.Serialize(fcr);
            return true;
        }
    }

    /// <summary>
    /// 获取指定版块
    /// </summary>
    public sealed class GetForumInfoCommand : Command
    {
        public GetForumInfoCommand()
            : base("forums.get")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            //如果是桌面程序则需要验证用户身份
            //if (commandParam.AppInfo.ApplicationType == (int)ApplicationType.DESKTOP)
            //{
            //    if (commandParam.LocalUid < 1)
            //    {
            //        result = Util.CreateErrorMessage(ErrorType.API_EC_SESSIONKEY, commandParam.ParamList);
            //        return false;
            //    }
            //}

            if (!commandParam.CheckRequiredParams("fid"))
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            int fid = Utils.StrToInt(commandParam.GetDNTParam("fid"), 0);
            if (fid < 1)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_PARAM, commandParam.ParamList);
                return false;
            }

            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(fid);
            if (forumInfo == null)
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_FORUM_NOT_EXIST, commandParam.ParamList);
                return false;
            }

            ForumGetResponse fgr = new ForumGetResponse();
            fgr.Fid = fid;
            fgr.Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + Urls.ShowForumAspxRewrite(fid, 1, forumInfo.Rewritename);
            fgr.CurTopics = forumInfo.CurrentTopics;
            fgr.Description = forumInfo.Description;
            fgr.Icon = forumInfo.Icon;
            fgr.LastPost = forumInfo.Lastpost;
            fgr.LastPoster = forumInfo.Lastposter.Trim();
            fgr.LastPosterId = forumInfo.Lastposterid;
            fgr.LastTid = forumInfo.Lasttid;
            fgr.LastTitle = forumInfo.Lasttitle.Trim();
            fgr.Moderators = forumInfo.Moderators;
            fgr.Name = forumInfo.Name;
            fgr.ParentId = forumInfo.Parentid;
            fgr.ParentIdList = forumInfo.Parentidlist.Trim();
            fgr.PathList = forumInfo.Pathlist.Trim();
            fgr.Posts = forumInfo.Posts;
            fgr.Rules = forumInfo.Rules;
            fgr.Status = forumInfo.Status;
            fgr.SubForumCount = forumInfo.Subforumcount;
            fgr.TodayPosts = forumInfo.Todayposts;
            fgr.Topics = forumInfo.Topics;

            result = commandParam.Format == FormatType.JSON ? JavaScriptConvert.SerializeObject(fgr) : SerializationHelper.Serialize(fgr);
            return true;
        }
    }

    /// <summary>
    /// 获取首页版块列表
    /// </summary>
    public sealed class GetIndexForumListCommand : Command
    {
        public GetIndexForumListCommand()
            : base("forums.getindexlist")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            int userGroupId = commandParam.LocalUid > 0 ? Discuz.Forum.Users.GetShortUserInfo(commandParam.LocalUid).Groupid : 7;
            int topicCount, postCount, todayCount;
            List<IndexPageForumInfo> list = Discuz.Forum.Forums.GetForumIndexCollection(1, userGroupId, 0, out topicCount, out postCount, out todayCount);

            List<IndexForum> newList = new List<IndexForum>();

            foreach (IndexPageForumInfo f in list)
            {
                IndexForum newf = new IndexForum();
                newf.Fid = f.Fid;
                newf.Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + Urls.ShowForumAspxRewrite(f.Fid, 1, f.Rewritename);
                newf.CurTopics = f.CurrentTopics;
                newf.Description = f.Description;
                newf.Icon = f.Icon;
                newf.LastPost = f.Lastpost;
                newf.LastPoster = f.Lastposter.Trim();
                newf.LastPosterId = f.Lastposterid;
                newf.LastTid = f.Lasttid;
                newf.LastTitle = f.Lasttitle.Trim();
                newf.Moderators = f.Moderators;
                newf.Name = f.Name;
                newf.ParentId = f.Parentid;
                newf.ParentIdList = f.Parentidlist.Trim();
                newf.PathList = f.Pathlist.Trim();
                newf.Posts = f.Posts;
                newf.Rules = f.Rules;
                newf.Status = f.Status;
                newf.SubForumCount = f.Subforumcount;
                newf.TodayPosts = f.Todayposts;
                newf.Topics = f.Topics;

                newList.Add(newf);
            }

            ForumGetIndexListResponse fgilr = new ForumGetIndexListResponse();
            fgilr.Forums = newList.ToArray();
            fgilr.List = true;

            result = commandParam.Format == FormatType.JSON ? JavaScriptConvert.SerializeObject(fgilr) : SerializationHelper.Serialize(fgilr);
            return true;
        }
    }


    public class ForumsCommandUtils
    {
        public static int InsertForum(Forum forum, int layer, string parentidlist, int subforumcount, int systemdisplayorder)
        {
            #region 添加新论坛
            ForumInfo foruminfo = new ForumInfo();

            foruminfo.Parentid = forum.ParentId;
            foruminfo.Layer = layer;
            foruminfo.Parentidlist = parentidlist;
            foruminfo.Subforumcount = subforumcount;
            foruminfo.Name = forum.Name.Trim();

            foruminfo.Status = forum.Status == null ? 1 : Convert.ToInt32(forum.Status);

            foruminfo.Displayorder = systemdisplayorder;

            foruminfo.Templateid = forum.TemplateId;
            foruminfo.Allowsmilies = forum.AllowSmilies;
            foruminfo.Allowrss = forum.AllowRss;
            foruminfo.Allowhtml = 1;
            foruminfo.Allowbbcode = forum.AllowBbcode;
            foruminfo.Allowimgcode = forum.AllowImgcode;
            foruminfo.Allowblog = 0;
            foruminfo.Istrade = 0;
            foruminfo.Alloweditrules = forum.AllowEditRules;
            foruminfo.Recyclebin = forum.RecycleBin;
            foruminfo.Modnewposts = forum.ModNewPosts;
            foruminfo.Modnewtopics = forum.ModNewTopics;
            foruminfo.Jammer = forum.Jammer;
            foruminfo.Disablewatermark = forum.DisableWatermark;
            foruminfo.Inheritedmod = forum.InheritedMod;
            foruminfo.Allowthumbnail = forum.AllowThumbnail;
            foruminfo.Allowtag = forum.AllowTag;

            foruminfo.Allowpostspecial = 0;
            foruminfo.Allowspecialonly = 0;

            foruminfo.Autoclose = forum.AutoClose;

            foruminfo.Description = forum.Description == null ? string.Empty : forum.Description;
            foruminfo.Password = string.Empty;
            foruminfo.Icon = forum.Icon == null ? string.Empty : forum.Icon;
            foruminfo.Postcredits = "";
            foruminfo.Replycredits = "";
            foruminfo.Redirect = string.Empty;
            foruminfo.Attachextensions = string.Empty;
            foruminfo.Moderators = forum.Moderators == null ? string.Empty : forum.Moderators;
            foruminfo.Rules = forum.Rules == null ? string.Empty : forum.Rules;
            foruminfo.Seokeywords = forum.SeoKeywords == null ? string.Empty : forum.SeoKeywords;
            foruminfo.Seodescription = forum.SeoDescription == null ? string.Empty : forum.SeoDescription;
            foruminfo.Rewritename = forum.RewriteName == null ? string.Empty : forum.RewriteName;
            foruminfo.Topictypes = string.Empty;
            foruminfo.Colcount = 1;
            foruminfo.Viewperm = string.Empty;
            foruminfo.Postperm = string.Empty;
            foruminfo.Replyperm = string.Empty;
            foruminfo.Getattachperm = string.Empty;
            foruminfo.Postattachperm = string.Empty;

            return Discuz.Forum.AdminForums.CreateForums(foruminfo);


            #endregion
        }
    }
}
