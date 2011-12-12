using System;
using System.Data;
using System.Web;
using Yuwen.Tools.Data;

namespace iTCA.Yuwen.Web
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
        #endregion

        protected BasePage()
        {
            //ҳ��ͳ�ƿ�ʼ
            sw = System.Diagnostics.Stopwatch.StartNew();
            DbHelper.QueryCount = 0;
            DbHelper.QueryDetail = "";

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
    }
}
