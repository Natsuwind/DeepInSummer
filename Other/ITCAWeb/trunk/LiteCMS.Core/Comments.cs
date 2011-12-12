using System;
using System.Data;
using System.Collections.Generic;

using LiteCMS.Data;
using LiteCMS.Entity;
using Natsuhime.Common;
using Natsuhime;

namespace LiteCMS.Core
{
    public class Comments
    {
        static CommentInfo DataReader2CommentInfo(IDataReader reader)
        {
            CommentInfo info = new CommentInfo();
            info.Commentid = Convert.ToInt32(reader["commentid"]);
            info.Articleid = Convert.ToInt32(reader["articleid"]);
            info.Articletitle = reader["articletitle"].ToString();
            info.Uid = Convert.ToInt32(reader["uid"]);
            info.Username = reader["username"].ToString();
            info.Postdate = Convert.ToDateTime(reader["postdate"]).ToString("yyyy-MM-dd");
            info.Del = Convert.ToInt32(reader["del"]);
            info.Content = reader["content"].ToString();
            info.Goodcount = Convert.ToInt32(reader["goodcount"]);
            info.Badcount = Convert.ToInt32(reader["badcount"]);
            return info;
        }

        #region 取得列表
        public static List<CommentInfo> GetCommentCollection(int articleid, int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<CommentInfo> coll = new List<CommentInfo>();

            IDataReader reader = DatabaseProvider.GetInstance().GetComments(articleid, pagesize, currentpage);

            while (reader.Read())
            {
                coll.Add(DataReader2CommentInfo(reader));
            }
            reader.Close();
            return coll;
        }
        public static int GetCommentCollectionPageCount(int articleid, int pagesize)
        {
            return DatabaseProvider.GetInstance().GetCommentsPageCount(articleid, pagesize);
        }
        public static List<CommentInfo> GetUserCommentCollection(int uid, int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<CommentInfo> coll = new List<CommentInfo>();

            IDataReader reader = DatabaseProvider.GetInstance().GetUserComments(uid, pagesize, currentpage);

            while (reader.Read())
            {
                coll.Add(DataReader2CommentInfo(reader));
            }
            reader.Close();
            return coll;
        }
        public static int GetUserCommentCollectionPageCount(int uid, int pagesize)
        {
            return DatabaseProvider.GetInstance().GetUserCommentsPageCount(uid, pagesize);
        }

        public static List<CommentInfo> GetMostGradComments(int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<CommentInfo> coll = new List<CommentInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetMostGradComments(pagesize, currentpage);
            while (reader.Read())
            {
                coll.Add(DataReader2CommentInfo(reader));
            }
            reader.Close();
            return coll;
        }
        public static int GetMostGradCommentsPageCount(int pagesize)
        {
            return DatabaseProvider.GetInstance().GetMostGradCommentsPageCount(pagesize);
        }
        #endregion
        public static CommentInfo GetCommentInfo(int commentid)
        {
            CommentInfo info;
            IDataReader reader = DatabaseProvider.GetInstance().GetCommentInfo(commentid);
            if (reader.Read())
            {
                info = DataReader2CommentInfo(reader);
            }
            else
            {
                info = null;
            }
            reader.Close();
            return info;
        }
        public static void CreateComment(CommentInfo info)
        {
            if (info != null)
            {
                DatabaseProvider.GetInstance().CreateComment(info);
            }
        }
        public static void DeleteComment(int commentid)
        {
            DatabaseProvider.GetInstance().DeleteComment(commentid);
        }
        public static void GradeComment(int commentid, int type)
        {
            DatabaseProvider.GetInstance().GradeComment(commentid, type);
        }
    }
}
