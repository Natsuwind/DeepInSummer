using System;
using System.Data;
using System.Threading;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// �û����ʼ�����. 
    /// </summary>
     
    public partial class usergroupsendemail : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EmailConfigInfo __emailinfo = EmailConfigs.GetConfig();
                GeneralConfigInfo configinfo = GeneralConfigs.GetConfig();

                string strbody = __emailinfo.Emailcontent.Replace("{forumtitle}", configinfo.Forumtitle);
                strbody = strbody.Replace("{forumurl}", "<a href=" + configinfo.Forumurl + ">" + configinfo.Forumurl + "</a>");
                strbody = strbody.Replace("{webtitle}", configinfo.Webtitle);
                strbody = strbody.Replace("{weburl}", "<a href=" + configinfo.Forumurl + ">" + configinfo.Weburl + "</a>");
                body.Text = strbody;

            }
            if (DNTRequest.GetString("flag") == "1")
            {
                this.ExportUserEmails();
            }
        }


        private void BatchSendEmail_Click(object sender, EventArgs e)
        {
            #region ���������ʼ�

            if (this.CheckCookie())
            {
                string groupidlist = Usergroups.GetSelectString(",");

                if ((groupidlist == "") && (usernamelist.Text.Trim() == ""))
                {
                    base.RegisterStartupScript( "", "<script>alert('����Ҫ��������ʼ��û����ƻ�ѡȡ��ص��û���,����ʼ��޷�����');</script>");
                    return;
                }

                int percount = 5; //ÿ���ټ�¼Ϊһ�εȴ�

                //�����û��б��ʼ�
                if (usernamelist.Text.Trim() != "")
                {
                    //string strwhere = " WHERE [Email] Is Not null AND (";
                    //foreach (string username in usernamelist.Text.Split(','))
                    //{
                    //    if (username.Trim() != "")
                    //        strwhere += " [username] like '%" + username.Trim() + "%' OR ";
                    //}
                    //strwhere = strwhere.Substring(0, strwhere.Length - 3) + ")";

                    //DataTable dt = DbHelper.ExecuteDataset("SELECT [username],[Email]  From [" + BaseConfigs.GetTablePrefix + "users] " + strwhere).Tables[0];
                    DataTable dt = Users.GetEmailListByUserNameList(usernamelist.Text);
                    if (dt.Rows.Count <= 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('������Ľ����ʼ��û���δ�ܲ��ҵ�����û�,����ʼ��޷�����');</script>");
                        return;
                    }
                    base.LoadRegisterStartupScript("PAGE", "window.location.href='global_usergroupsendemail.aspx';");

                    Thread[] lThreads = new Thread[dt.Rows.Count];
                    int count = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        EmailMultiThread emt = new EmailMultiThread(dr["UserName"].ToString(), dr["Email"].ToString(), subject.Text, body.Text);
                        lThreads[count] = new Thread(new ThreadStart(emt.Send));
                        lThreads[count].Start();

                        if (count >= percount)
                        {
                            Thread.Sleep(5000);
                            count = 0;
                        }
                        count++;

                        //ThreadPool.QueueUserWorkItem(new WaitCallback(SendMail),string.Format("http://bbs.ent.tom.com/forum/view_thread.php?forumid=1&threadid={0}&backurl=http%3A%2F%2Fbbs.ent.tom.com%2Fforum%2Flist_thread.php%3Fforumid%3D1%26page%3D1%26sort%3D0",PageNumber.ToString()));
                    }

                }

                //�����û����ʼ�
                if (groupidlist != "")
                {
                    //DataTable dt = DbHelper.ExecuteDataset("SELECT [username],[Email]  From [" + BaseConfigs.GetTablePrefix + "users] WHERE [Email] Is Not null AND [Email]<>'' AND [groupid] IN(" + groupidlist + ")").Tables[0];
                    DataTable dt = Users.GetEmailListByGroupidList(groupidlist);
                    Thread[] lThreads = new Thread[dt.Rows.Count];
                    int count = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        EmailMultiThread emt = new EmailMultiThread(dr["UserName"].ToString(), dr["Email"].ToString(), subject.Text, body.Text);
                        lThreads[count] = new Thread(new ThreadStart(emt.Send));
                        lThreads[count].Start();

                        if (count >= percount)
                        {
                            Thread.Sleep(5000);
                            count = 0;
                        }
                        count++;

                        //ThreadPool.QueueUserWorkItem(new WaitCallback(SendMail),string.Format("http://bbs.ent.tom.com/forum/view_thread.php?forumid=1&threadid={0}&backurl=http%3A%2F%2Fbbs.ent.tom.com%2Fforum%2Flist_thread.php%3Fforumid%3D1%26page%3D1%26sort%3D0",PageNumber.ToString()));
                    }
                }
                base.RegisterStartupScript( "PAGE", "window.location.href='global_usergroupsendemail.aspx';");
            }

            #endregion
        }

        private void ExportUserEmails()
        {
            string groupidlist="";

            if (this.CheckCookie())
            {
                groupidlist = Usergroups.GetSelectString(",");
            }


            if (groupidlist == "")
            {
                return;
            }

            DataTable dt = Users.GetEmailListByGroupidList(groupidlist);
            
            string words = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    words += dt.Rows[i][1].ToString().Trim() + "; ";
                }
            }

            string filename = "Useremail.txt";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(filename));
            HttpContext.Current.Response.ContentType = "text/plain";
            this.EnableViewState = false;
            HttpContext.Current.Response.Write(words);
            HttpContext.Current.Response.End();
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.BatchSendEmail.Click += new EventHandler(this.BatchSendEmail_Click);
            DataTable dt = UserGroups.GetUserGroupWithOutGuestTitle();
            foreach (DataRow dr in dt.Rows)
            {
                dr["grouptitle"] = "<img src=../images/usergroup.GIF border=0  style=\"position:relative;top:2 ;height:18 ;\">" + dr["grouptitle"];
            }
            Usergroups.AddTableData(dt);
        }

        #endregion
    }
}