using System;
using System.Data;
using System.Collections.Generic;

using LiteCMS.Data;
using LiteCMS.Entity;
using Natsuhime.Common;
using Natsuhime;

namespace LiteCMS.Core
{
    public class Articles
    {
        /// <summary>
        /// ��DataReader��Articleת��ΪArticleInfo�����б�
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static ArticleInfo DataReader2ArticleInfo(IDataReader reader)
        {
            ArticleInfo info = new ArticleInfo();
            info.Articleid = Convert.ToInt32(reader["articleid"]);
            info.Columnid = Convert.ToInt32(reader["columnid"]);
            info.Title = reader["title"].ToString();
            info.Badcount = Convert.ToInt32(reader["badcount"]);
            info.Goodcount = Convert.ToInt32(reader["goodcount"]);
            info.Commentcount = Convert.ToInt32(reader["commentcount"]);
            info.Viewcount = Convert.ToInt32(reader["viewcount"]);
            info.Sort = Convert.ToInt32(reader["sort"]);
            info.Highlight = reader["highlight"].ToString();
            info.Summary = reader["summary"].ToString();
            info.Content = reader["content"].ToString();
            info.Image = reader["image"].ToString();
            info.Uid = Convert.ToInt32(reader["uid"]);
            info.Username = reader["username"].ToString();
            info.Ip = reader["ip"].ToString();
            info.Postdate = Convert.ToDateTime(reader["postdate"]).ToString("yyyy-MM-dd");
            info.Del = Convert.ToInt32(reader["del"]);
            info.Recommend = Convert.ToInt32(reader["recommend"]);
            //��������
            if (info.Highlight != "")
            {
                info.Title = string.Format("<span style=\"{0}\">{1}</span>", info.Highlight, info.Title);
            }
            //���µ���Ŀǰ׺
            if (info.Columnid > 0)
            {
                info.Columnname = Columns.GetColumnName(info.Columnid);
            }
            else
            {
                info.Columnname = "";
            }
            return info;
        }
        #region ȡ���б�

        /// <summary>
        /// ͨ��cidȡ�������б�.
        /// </summary>
        /// <param name="cid">��Ŀid(���Ϊ0��ʾȡ��������Ŀ)</param>
        /// <param name="pagesize">��ҳ��С</param>
        /// <param name="currentpage">��ǰҳ</param>
        /// <returns>�б�</returns>
        public static List<ArticleInfo> GetArticleCollection(int cid, int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<ArticleInfo> coll = new List<ArticleInfo>();

            IDataReader reader = DatabaseProvider.GetInstance().GetArticles(cid, pagesize, currentpage);

            while (reader.Read())
            {
                coll.Add(DataReader2ArticleInfo(reader));
            }
            reader.Close();
            return coll;
        }
        /// <summary>
        /// ȡ�÷�ҳ��Ŀ
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static int GetArticleCollectionPageCount(int cid, int pagesize)
        {
            return DatabaseProvider.GetInstance().GetArticleCollectionPageCount(cid, pagesize);
        }

        /// <summary>
        /// ͨ��cidsȡ�������б�.
        /// </summary>
        /// <param name="cids">��Ŀid�ַ���(,�ָ�)Ϊ���ַ�����ʾ������Ŀ</param>
        /// <param name="pagesize">��ҳ��С</param>
        /// <param name="currentpage">��ǰҳ</param>
        /// <returns>�б�</returns>
        public static List<ArticleInfo> GetArticleCollection(string cids, int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<ArticleInfo> coll = new List<ArticleInfo>();
            if (TypeParse.IsNumericString(cids))
            {
                IDataReader reader = DatabaseProvider.GetInstance().GetArticles(cids, pagesize, currentpage);

                while (reader.Read())
                {
                    //DataReader2ArticleCollection(coll, reader);
                    coll.Add(DataReader2ArticleInfo(reader));
                }
                reader.Close();
            }
            return coll;
        }
        /// <summary>
        /// ȡ�÷�ҳ��Ŀ
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static int GetArticleCollectionPageCount(string cids, int pagesize)
        {
            if (TypeParse.IsNumericString(cids))
            {
                return DatabaseProvider.GetInstance().GetArticleCollectionPageCount(cids, pagesize);
            }
            else
            {
                return 0;
            }
        }

        public static List<ArticleInfo> GetUserArticleCollection(int uid, int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<ArticleInfo> coll = new List<ArticleInfo>();

            IDataReader reader = DatabaseProvider.GetInstance().GetUserArticles(uid, pagesize, currentpage);

            while (reader.Read())
            {
                coll.Add(DataReader2ArticleInfo(reader));
            }
            reader.Close();
            return coll;
        }
        public static int GetUserArticleCollectionPageCount(int uid, int pagesize)
        {
            return DatabaseProvider.GetInstance().GetUserArticleCollectionPageCount(uid, pagesize);
        }

        public static List<ArticleInfo> GetRecommendArticles(int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<ArticleInfo> coll = new List<ArticleInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetRecommendArticles(pagesize, currentpage);
            while (reader.Read())
            {
                coll.Add(DataReader2ArticleInfo(reader));
            }
            reader.Close();
            return coll;
        }
        public static int GetRecommendArticleCollectionPageCount(int pagesize)
        {
            return DatabaseProvider.GetInstance().GetRecommendArticleCollectionPageCount(pagesize);
        }

        public static List<ArticleInfo> GetHotArticles(int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }
            List<ArticleInfo> coll = new List<ArticleInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetHotArticles(pagesize, currentpage);
            while (reader.Read())
            {
                coll.Add(DataReader2ArticleInfo(reader));
            }
            reader.Close();
            return coll;
        }
        public static int GetHotArticleCollectionPageCount(int pagesize)
        {
            return DatabaseProvider.GetInstance().GetHotArticleCollectionPageCount(pagesize);
        }

        public static List<ArticleInfo> GetSearchArticles(string searchkey, int pagesize, int currentpage)
        {
            if (currentpage <= 0)
            {
                currentpage = 1;
            }

            List<ArticleInfo> coll = new List<ArticleInfo>();
            searchkey = searchkey.Trim();
            if (searchkey != string.Empty)
            {
                IDataReader reader = DatabaseProvider.GetInstance().GetSearchArticles(searchkey, pagesize, currentpage);
                while (reader.Read())
                {
                    coll.Add(DataReader2ArticleInfo(reader));
                }
                reader.Close();
            }
            return coll;
        }
        public static int GetSearchArticleCollectionPageCount(string searchkey, int pagesize)
        {
            searchkey = searchkey.Trim();
            if (searchkey != string.Empty)
            {
                return DatabaseProvider.GetInstance().GetSearchArticleCollectionPageCount(searchkey, pagesize);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// ȡ����������
        /// </summary>
        /// <param name="articleid">����id</param>
        /// <returns>AritlceInfo</returns>
        public static ArticleInfo GetArticleInfo(int articleid)
        {
            ArticleInfo info = new ArticleInfo();
            IDataReader reader = DatabaseProvider.GetInstance().GetArticleInfo(articleid);
            if (reader.Read())
            {
                info = DataReader2ArticleInfo(reader);
            }
            else
            {
                info = null;
            }
            reader.Close();
            return info;
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="articleinfo"></param>
        public static void CreateArticle(ArticleInfo articleinfo)
        {
            DatabaseProvider.GetInstance().CreateArticle(articleinfo);
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="articleinfo"></param>
        public static void EditArticle(ArticleInfo articleinfo)
        {
            DatabaseProvider.GetInstance().EditArticle(articleinfo);
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="articleid"></param>
        public static void DeleteArticle(int articleid)
        {
            DatabaseProvider.GetInstance().DeleteArticle(articleid);
        }
        /// <summary>
        /// �ı����µ�������
        /// </summary>
        /// <param name="articleid"></param>
        /// <param name="changevalue"></param>
        /// <param name="type">1Ϊ����,����Ϊ����</param>
        public static void ChangeCommentCount(int articleid, int changevalue, int type)
        {
            DatabaseProvider.GetInstance().ChangeCommentCount(articleid, changevalue, type);
        }

        public static void RemoveArtilceCache()
        {
            TinyCache cache = new TinyCache();
            cache.RemoveObject("articlelist_indexmain");
            cache.RemoveObject("articlelistdictionary_allcolumn");
            cache.RemoveObject("commentlist_mostgrade");
            cache.RemoveObject("commentlist_latest");
            cache.RemoveObject("articlelist_hot");
        }
    }
}
