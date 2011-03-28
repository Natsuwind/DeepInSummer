using System;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ����Ϣ����
    /// </summary>
    public partial class searchsm : AdminPage
    {
        private void SaveSearchInfo_Click(object sender, EventArgs e)
        {
            #region ��ָ���������ж���Ϣ����

            if (this.CheckCookie())
            {
                string sqlstring = PrivateMessages.GetDeletePrivateMessagesCondition(isnew.Checked, postdatetime.Text, msgfromlist.Text,
                    lowerupper.Checked, subject.Text, message.Text, isupdateusernewpm.Checked);

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "�����������Ϣ", "ɾ��������:" + sqlstring);

                base.RegisterStartupScript( "PAGE", "window.location.href='forum_searchsm.aspx';");

            }

            #endregion
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveSearchInfo.Click += new EventHandler(this.SaveSearchInfo_Click);
        }

        #endregion
    }
}