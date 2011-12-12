using System;
using System.Data;
using System.Data.Common;
using LiteCMS.Entity;

namespace LiteCMS.Data
{
    public partial interface IDataProvider
    {
        #region 取得列表
        IDataReader GetArticles(int cid, int pagesize, int currentpage);
        int GetArticleCollectionPageCount(int cid, int pagesize);

        IDataReader GetUserArticles(int uid, int pagesize, int currentpage);
        int GetUserArticleCollectionPageCount(int uid, int pagesize);

        IDataReader GetArticles(string cids, int pagesize, int currentpage);
        int GetArticleCollectionPageCount(string cids, int pagesize);

        IDataReader GetRecommendArticles(int pagesize, int currentpage);
        int GetRecommendArticleCollectionPageCount(int pagesize);

        IDataReader GetHotArticles(int pagesize, int currentpage);
        int GetHotArticleCollectionPageCount(int pagesize);

        IDataReader GetSearchArticles(string searchkey, int pagesize, int currentpage);
        int GetSearchArticleCollectionPageCount(string searchkey, int pagesize);
        #endregion

        IDataReader GetArticleInfo(int articleid);
        void CreateArticle(ArticleInfo articleinfo);
        void EditArticle(ArticleInfo articleinfo);
        void DeleteArticle(int articleid);

        void ChangeCommentCount(int articleid, int changevalue, int type);
    }
}
