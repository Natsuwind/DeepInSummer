using System;
using System.Collections.Generic;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class comment : BasePage
    {
        protected override void Page_Show()
        {
            UserInfo userinfo = GetUserInfo();
            if (userinfo == null)
            {
                ShowError("评论信息", "请登录后再留言评论.", "", "login.aspx");
            }
            string action = YRequest.GetQueryString("action");
            if (action == string.Empty)
            {
                currentcontext.Response.End();
            }
            if (action == "postcomment")
            {
                string content = YRequest.GetFormString("commentcontent");
                int articleid = YRequest.GetQueryInt("articleid", 0);
                if (content != string.Empty && articleid > 0)
                {
                    if (content != string.Empty)
                    {
                        CommentInfo info = new CommentInfo();
                        info.Articleid = articleid;
                        info.Uid = userinfo.Uid;
                        info.Username = userinfo.Username;
                        info.Postdate = DateTime.Now.ToString();
                        info.Del = 0;
                        info.Content = Utils.RemoveUnsafeHtml(content);
                        info.Goodcount = 0;
                        info.Badcount = 0;
                        info.Articletitle = Articles.GetArticleInfo(articleid).Title;
                        Comments.CreateComment(info);
                        Articles.ChangeCommentCount(articleid, 1, 1);
                        Articles.RemoveArtilceCache();
                        currentcontext.Response.Redirect(YRequest.GetUrlReferrer());
                    }
                }
                else
                {
                    currentcontext.Response.Write("参数为空.");
                    currentcontext.Response.End();
                    return;
                }
            }
            else if (action == "grade")
            {
                int commentid = YRequest.GetQueryInt("commentid", 0);
                if (commentid > 0)
                {
                    int type = YRequest.GetQueryInt("type", 0);
                    Comments.GradeComment(commentid, type);
                    Articles.RemoveArtilceCache();
                    currentcontext.Response.Redirect(YRequest.GetUrlReferrer());
                }
                else
                {
                    ShowError("评论信息", "参数为空,请检查输入!", "", "");
                }
            }
            else if (action == "del")
            {
                int commentid = YRequest.GetQueryInt("commentid", 0);
                if (commentid > 0)
                {
                    CommentInfo info = Comments.GetCommentInfo(commentid);
                    Comments.DeleteComment(info.Commentid);
                    Articles.ChangeCommentCount(info.Articleid, 1, -1);
                    Articles.RemoveArtilceCache();
                    currentcontext.Response.Redirect(YRequest.GetUrlReferrer());
                }
                else
                {
                    ShowError("评论信息", "参数为空,请检查输入!", "", "");
                }
            }
            else
            {
                ShowError("评论信息", "非法的参数!", "", "");
            }
        }
    }
}
