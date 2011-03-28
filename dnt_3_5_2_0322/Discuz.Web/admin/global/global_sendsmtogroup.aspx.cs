using System;
using System.Data;
using System.Threading;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Forum;
using Button = Discuz.Control.Button;
using CheckBoxList = Discuz.Control.CheckBoxList;
using DropDownList = Discuz.Control.DropDownList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ���鷢�Ͷ���Ϣ
    /// </summary>
    public partial class sendsmtogroup : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.username != "")
                {
                    msgfrom.Text = this.username;
                    postdatetime.Text = DateTime.Now.ToShortDateString();
                }
            }
        }

        private void BatchSendSM_Click(object sender, EventArgs e)
        {
            #region ��������Ϣ����

            if (this.CheckCookie())
            {
                string groupidlist = Usergroups.GetSelectString(",");

                if (groupidlist == "")
                {
                    base.RegisterStartupScript("", "<script>alert('������ѡȡ��ص��û���,�ٵ���ύ��ť');</script>");
                    return;
                }

#if EntLib
                if (RabbitMQConfigs.GetConfig() != null && RabbitMQConfigs.GetConfig().SendShortMsg.Enable)//������errlog������־��¼����ʱ
                {
                    PrivateMessageInfo pm = new PrivateMessageInfo()
                    {
                        Msgfrom = username.Replace("'", "''"),
                        Msgfromid = userid,
                        Folder = int.Parse(folder.SelectedValue),
                        Subject = subject.Text,
                        Postdatetime = Discuz.Common.Utils.GetDateTime(),//��ȡ������Ϣ��ϵͳʱ��
                        Message = message.Text,
                        New = 1//���Ϊδ��
                    };
                    Discuz.EntLib.ServiceBus.SendShortMsgClientHelper.GetSendShortMsgClient().AsyncSendShortMsgByUserGroup(groupidlist, pm);
                }
#else

                int percount = Discuz.Common.Utils.StrToInt(postcountpercircle.Text, 100); //ÿ���ټ�¼Ϊһ�εȴ�
                int count = 0; //��ǰ��¼��

                //foreach (DataRow dr in DbHelper.ExecuteDataset("SELECT [uid] ,[username]  From [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid] IN(" + groupidlist + ")").Tables[0].Rows)
                foreach (DataRow dr in Users.GetUserListByGroupidList(groupidlist).Rows)
                {
                    //DbHelper.ExecuteNonQuery("INSERT INTO [" + BaseConfigs.GetTablePrefix + "pms] (msgfrom,msgfromid,msgto,msgtoid,folder,new,subject,postdatetime,message) VALUES ('" + this.username.Replace("'", "''") + "','" + this.userid.ToString() + "','" + dr["username"].ToString().Replace("'", "''") + "','" + dr["uid"].ToString() + "','" + folder.SelectedValue + "','1','" + subject.Text + "','" + postdatetime.Text + "','" + message.Text + "')");
                    //DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=[newpmcount]+1  WHERE [uid] =" + dr["uid"].ToString());
                    //Discuz.Data.DatabaseProvider.GetInstance().SendPMToUser(username.Replace("'", "''"), userid, dr["username"].ToString().Replace("'", "''"), Convert.ToInt32(dr["uid"].ToString()), int.Parse(folder.SelectedValue), subject.Text, Convert.ToDateTime(postdatetime.Text), message.Text);
                    PrivateMessageInfo pm = new PrivateMessageInfo();
                    pm.Msgfrom = username.Replace("'", "''");
                    pm.Msgfromid = userid;
                    pm.Msgto = dr["username"].ToString().Replace("'", "''");
                    pm.Msgtoid = Convert.ToInt32(dr["uid"].ToString());
                    pm.Folder = int.Parse(folder.SelectedValue);
                    pm.Subject = subject.Text;
                    pm.Postdatetime = postdatetime.Text;
                    pm.Message = message.Text;
                    pm.New = 1;//���Ϊδ��
                    PrivateMessages.CreatePrivateMessage(pm, 0);
                    if (count >= percount)
                    {
                        Thread.Sleep(500);
                        count = 0;
                    }
                    count++;
                }                        
#endif
                base.RegisterStartupScript("PAGE", "window.location.href='global_sendSMtogroup.aspx';"); 
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
            this.BatchSendSM.Click += new EventHandler(this.BatchSendSM_Click);
            message.is_replace = true;

            DataTable dt = UserGroups.GetUserGroupWithOutGuestTitle();
            foreach (DataRow dr in dt.Rows)
            {
                dr["grouptitle"] = "<img src=../images/usergroup.gif border=0  style=\"position:relative;top:2 ;height:18 \">" + dr["grouptitle"];
            }
            Usergroups.AddTableData(dt);
        }

        #endregion
    }
}