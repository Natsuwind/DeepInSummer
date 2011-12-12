using System;
using System.Data;
using System.Web;
using Natsuhime.TinyData;

namespace iTCA.Yuwen.GuestBook
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
        /// ���ݲ�ѯ����
        /// </summary>
        protected DBHelper dbhelper;
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
            //dbhelper = new DBHelper();
            ////ҳ��ͳ�ƿ�ʼ
            //sw = System.Diagnostics.Stopwatch.StartNew();
            //dbhelper.Querycount = 0;

            ////ҳ��ִ��
            //Page_Show();

            ////ҳ��ͳ�ƽ���
            //querycount = dbhelper.QueryCount;
            //processtime = sw.Elapsed.TotalSeconds.ToString("F6");
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
