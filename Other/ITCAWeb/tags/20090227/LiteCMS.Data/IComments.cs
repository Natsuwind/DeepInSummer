using System;
using System.Data;
using System.Data.Common;
using LiteCMS.Entity;

namespace LiteCMS.Data
{
    public partial interface IDataProvider
    {
        #region 取得列表
        IDataReader GetComments(int articleid, int pagesize, int currentpage);
        int GetCommentsPageCount(int articleid, int pagesize);

        IDataReader GetUserComments(int uid, int pagesize, int currentpage);
        int GetUserCommentsPageCount(int uid, int pagesize);

        IDataReader GetMostGradComments(int pagesize, int currentpage);
        int GetMostGradCommentsPageCount(int pagesize);
        #endregion

        IDataReader GetCommentInfo(int commentid);

        void CreateComment(CommentInfo commentinfo);
        void DeleteComment(int commentid);
        void GradeComment(int commentid, int type);
    }
}
