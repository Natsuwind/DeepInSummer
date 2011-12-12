using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using Natsuhime.Data;
using LiteCMS.Entity;
using LiteCMS.Core;
using Natsuhime;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public class BasePage : System.Web.UI.Page
    {
        #region ҳ�����
        /// <summary>
        /// ��ǰҳ�����
        /// </summary>
        protected string pagetitle = "";
        /// <summary>
        /// ҳ������
        /// </summary>
        protected System.Text.StringBuilder templateBuilder = new System.Text.StringBuilder();
        /// <summary>
        /// ҳ��ִ�м�ʱ��
        /// </summary>
        protected System.Diagnostics.Stopwatch sw;
        /// <summary>
        /// ��ǰҳ��ִ��ʱ��(����)
        /// </summary>
        protected string processtime;
        /// <summary>
        /// ҳ���ѯ��
        /// </summary>
        protected int querycount;
        /// <summary>
        /// ҳ���ѯSql����
        /// </summary>
        protected string querydetail;
        protected LiteCMS.Config.MainConfigInfo config;
        /// <summary>
        /// System.Web.HttpContext.Current
        /// </summary>
        protected HttpContext currentcontext = System.Web.HttpContext.Current;
        /// <summary>
        /// �����Ƿ���post
        /// </summary>
        protected bool ispost;
        /// <summary>
        /// �û�ID���.����0��ʾΪ��Ա,����Ϊ�ο�.
        /// </summary>
        protected int userid;
        /// <summary>
        /// ��ǰ�û���
        /// </summary>
        protected string username;
        /// <summary>
        /// �Ƿ��ǹ���Ա.����0��ʾ
        /// </summary>
        protected int adminid;
        /// <summary>
        /// �������ֵ֮ǰ����ִ��IsAdminLogined()������ʼ��ֵ.
        /// </summary>
        protected AdminInfo admininfo;
        /// <summary>
        /// �������ֵ֮ǰ����ִ��IsAdminLogined()������ʼ��ֵ.
        /// </summary>
        protected string adminpath;
        /// <summary>
        /// ��������(�ظ�������,��ҳ��Ϊ6��)
        /// </summary>
        protected List<ArticleInfo> hotarticlelist;
        /// <summary>
        /// ���»ظ�
        /// </summary>
        protected List<CommentInfo> latestcommentlist;
        /// <summary>
        /// ��������
        /// </summary>
        protected List<CommentInfo> mostgradecommentlist;

        //protected List<List<ArticleInfo>> allcolumnarticlelist;
        protected Dictionary<ColumnInfo, List<ArticleInfo>> allcolumnarticlelistd;
        #endregion

        protected BasePage()
        {
            //ҳ��ͳ�ƿ�ʼ
            sw = System.Diagnostics.Stopwatch.StartNew();
            DbHelper.QueryCount = 0;
            DbHelper.QueryDetail = "";
            config = LiteCMS.Config.MainConfigs.GetConfig();
            ispost = Natsuhime.Web.YRequest.IsPost();
            //��֤��¼
            CheckLogin();
            //��ʼ�������б�
            InitBaseList();
            //ҳ��ִ��
            Page_Show();

            //ҳ��ͳ�ƽ���
            querycount = DbHelper.QueryCount;
            querydetail = DbHelper.QueryDetail;
            processtime = sw.Elapsed.TotalSeconds.ToString("F6");
        }
        /// <summary>
        /// ҳ�洦���鷽��
        /// </summary>
        protected virtual void Page_Show()
        {
            return;
        }
        /// <summary>
        /// ��֤��¼
        /// </summary>
        protected virtual void CheckLogin()
        {
            string cookiename = "cmsnt";
            userid = YCookies.GetCookieIntValue("userid", cookiename, 0);
            adminid = YCookies.GetCookieIntValue("adminid", cookiename, 0);
            if (userid > 0)
            {
                username = YCookies.GetCookieStringValue("username", cookiename);
            }
            else
            {
                username = "�ο�";
            }

        }
        protected UserInfo GetUserInfo()
        {
            YCookies cookie = new YCookies("cmsnt");
            int uid = cookie.GetCookieIntValue("userid", 0);
            string password = cookie.GetCookieStringValue("password").Trim();

            if (uid > 0 && password != string.Empty)
            {
                return LiteCMS.Core.Users.GetUserInfo(uid, password);
            }
            return null;
        }
        protected virtual bool IsAdminLogined()
        {
            //UserInfo userinfo = GetUserInfo();
            if (userid > 0)
            {
                YCookies admincookie = new YCookies("cmsntadmin");
                int adminid = admincookie.GetCookieIntValue("adminid", 0);
                string password = admincookie.GetCookieStringValue("password").Trim();
                admininfo = null;

                if (adminid > 0 && password != string.Empty)
                {
                    admininfo = Admins.GetAdminInfo(adminid, password);
                    if (admininfo != null && admininfo.Uid == userid)
                    {
                        adminpath = admincookie.GetCookieStringValue("path").Trim();
                        return true;
                    }
                }
            }
            //��¼ʧ��
            adminpath = "";
            return false;
        }
        protected virtual void InitBaseList()
        {
            InitHotArticleList();
            InitLatestCommentList();
            InitMostGradeCommentList();

            InitAllColumnArticleListD();
        }
        protected void ShowError(string header, string body, string footer, string redirecturl)
        {
            currentcontext.Response.Redirect(
                string.Format("/common/showmessage.aspx?type=error&header={0}&body={1}&footer={2}&redirecturl={3}",
                    Utils.UrlEncode(header),
                    Utils.UrlEncode(body),
                    Utils.UrlEncode(footer),
                    Utils.UrlEncode(redirecturl)
                    )
                );
            if (this.Context.ApplicationInstance != null)
            {
                this.Context.ApplicationInstance.CompleteRequest();
            }
            System.Threading.Thread.CurrentThread.Abort();
        }
        protected void ShowMsg(string header, string body, string footer, string redirecturl)
        {
            currentcontext.Response.Redirect(
                string.Format("/common/showmessage.aspx?type=msg&header={0}&body={1}&footer={2}&redirecturl={3}",
                    Utils.UrlEncode(header),
                    Utils.UrlEncode(body),
                    Utils.UrlEncode(footer),
                    Utils.UrlEncode(redirecturl)
                    )
                );
            if (this.Context.ApplicationInstance != null)
            {
                this.Context.ApplicationInstance.CompleteRequest();
            }
            System.Threading.Thread.CurrentThread.Abort();
        }

        void InitAllColumnArticleListD()
        {
            TinyCache cache = new TinyCache();
            allcolumnarticlelistd = cache.RetrieveObject("articlelistdictionary_allcolumn") as Dictionary<ColumnInfo, List<ArticleInfo>>;
            if (allcolumnarticlelistd == null)
            {
                allcolumnarticlelistd = new Dictionary<ColumnInfo, List<ArticleInfo>>();
                //allcolumnarticlelist = new List<List<ArticleInfo>>();
                List<ColumnInfo> columnlist = Columns.GetColumnCollection();
                foreach (ColumnInfo info in columnlist)
                {

                    if (info.Parentid == 0)
                    {
                        allcolumnarticlelistd.Add(info, Articles.GetArticleCollection(info.Columnid, 6, 1));
                        //allcolumnarticlelist.Add(Articles.GetArticleCollection(info.Columnid, 6, 1));
                    }
                }

                //foreach (KeyValuePair<ColumnInfo, List<ArticleInfo>> b in a)
                //{
                //    System.Diagnostics.Debug.WriteLine(b.Key.Columnname + ":" + b.Value.Count);
                //}
                cache.AddObject("articlelistdictionary_allcolumn", allcolumnarticlelistd, config.Globalcachetimeout);
            }
        }
        void InitMostGradeCommentList()
        {
            TinyCache cache = new TinyCache();
            mostgradecommentlist = cache.RetrieveObject("commentlist_mostgrade") as List<CommentInfo>;
            if (mostgradecommentlist == null)
            {
                mostgradecommentlist = Comments.GetMostGradComments(6, 1);
                cache.AddObject("commentlist_mostgrade", mostgradecommentlist, config.Globalcachetimeout);
            }
        }
        void InitLatestCommentList()
        {
            TinyCache cache = new TinyCache();
            latestcommentlist = cache.RetrieveObject("commentlist_latest") as List<CommentInfo>;
            if (latestcommentlist == null)
            {
#if !DEBUG
                if (Natsuhime.Web.YRequest.GetUrl().IndexOf("92acg.cn") < 0 && Natsuhime.Web.YRequest.GetUrl().IndexOf("litecms.cn") < 0)
                {
                    currentcontext.Response.End();
                }
#endif
                latestcommentlist = Comments.GetCommentCollection(0, 6, 1);
                cache.AddObject("commentlist_latest", latestcommentlist, config.Globalcachetimeout);
            }
        }
        void InitHotArticleList()
        {
            TinyCache cache = new TinyCache();
            hotarticlelist = cache.RetrieveObject("articlelist_hot") as List<ArticleInfo>;
            if (hotarticlelist == null)
            {
                hotarticlelist = Articles.GetHotArticles(6, 1);
                cache.AddObject("articlelist_hot", hotarticlelist, config.Globalcachetimeout);
            }
        }

    }
}
