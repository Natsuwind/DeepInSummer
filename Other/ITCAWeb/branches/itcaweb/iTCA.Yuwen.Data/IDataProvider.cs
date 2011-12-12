using System;
using System.Data;
using System.Data.Common;
using iTCA.Yuwen.Entity;

namespace iTCA.Yuwen.Data
{
    public interface IDataProvider
    {
        IDataReader GetArticles(int cid, int pagesize, int currentpage);
        IDataReader GetArticles(string cids, int pagesize, int currentpage);

        IDataReader GetArticleColumnList();

        IDataReader GetArticleInfo(int articleid);

        void CreateArticle(ArticleInfo articleinfo);

        void EditArticle(ArticleInfo articleinfo);

        void DeleteArticle(int articleid);

        void CreateColumn(ColumnInfo columninfo);

        void DeleteColumn(int columnid);

        void EditColumn(ColumnInfo columninfo);

        int GetArticleCollectionPageCount(int cid, int pagesize);
        int GetArticleCollectionPageCount(string cids, int pagesize);
    }
}
