using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// ��������� 
    /// </summary>

    public partial class auditnewtopic : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            #region ���������
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "��������б�";
            DataGrid1.DataKeyField = "tid";
            DataGrid1.BindData(AdminTopics.GetUnauditNewTopic());
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
            BindData();
        }

        private void SelectPass_Click(object sender, EventArgs e)
        {
            #region ��ѡ�е���������Ϊͨ�����

            if (this.CheckCookie())
            {
                string tidlist = DNTRequest.GetString("tid");
                if (tidlist != "")
                {
                    //UpdateUserCredits(tidlist);
                    Topics.PassAuditNewTopic(tidlist);
                    base.RegisterStartupScript("", "<script>window.location='forum_auditnewtopic.aspx';</script>");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('��δѡ���κ�ѡ��');window.location='forum_auditnewtopic.aspx';</script>");
                }
            }

            #endregion
        }

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <param name="tidlist">ͨ����������Tid�б�</param>
        //private void UpdateUserCredits(string tidlist)
        //{
        //    string[] tidarray = tidlist.Split(',');
        //    float[] values = null;
        //    ForumInfo forum = null;
        //    TopicInfo topic = null;
        //    int fid = -1;
        //    foreach(string tid in tidarray)
        //    {
        //        topic = Discuz.Forum.Topics.GetTopicInfo(int.Parse(tid));    //��ȡ������Ϣ
        //        if(fid != topic.Fid)    //����һ���͵�ǰ���ⲻ��һ�������ʱ�����¶�ȡ���Ļ�������
        //        {
        //            fid = topic.Fid;
        //            forum = Discuz.Forum.Forums.GetForumInfo(fid);
        //            if (!forum.Postcredits.Equals(""))
        //            {
        //                int index = 0;
        //                float tempval = 0;
        //                values = new float[8];
        //                foreach (string ext in Utils.SplitString(forum.Postcredits, ","))
        //                {

        //                    if (index == 0)
        //                    {
        //                        if (!ext.Equals("True"))
        //                        {
        //                            values = null;
        //                            break;
        //                        }
        //                        index++;
        //                        continue;
        //                    }
        //                    tempval = Utils.StrToFloat(ext, 0);
        //                    values[index - 1] = tempval;
        //                    index++;
        //                    if (index > 8)
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        if (values != null)
        //        {
        //            ///ʹ�ð���ڻ���
        //            Discuz.Forum.UserCredits.UpdateUserCreditsByPostTopic(topic.Posterid, values);
        //        }
        //        else
        //        {
        //            ///ʹ��Ĭ�ϻ���
        //            Discuz.Forum.UserCredits.UpdateUserCreditsByPostTopic(topic.Posterid);
        //        }
        //    }
        //}

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            #region ��ѡ�е��������ɾ��

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("tid") != "")
                {
                    //TopicAdmins.DeleteTopics(DNTRequest.GetString("tid"), 0, false);
                    Discuz.Forum.TopicAdmins.DeleteTopicsWithoutChangingCredits(DNTRequest.GetString("tid"), false);
                    base.RegisterStartupScript("",  "<script>window.location='forum_auditnewtopic.aspx';</script>");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('��δѡ���κ�ѡ��');window.location='forum_auditnewtopic.aspx';</script>");
                }
            }

            #endregion
        }

        public string GetTopicType(string topicType)
        {
            switch(topicType)
            {
                case "0":
                    return "��ͨ����";
                case "1":
                    return "ͶƱ��";
                case "2":
                case "3":
                    return "������";
                case "4":
                    return "������";
                default:
                    return topicType;
            }
        }

        protected string GetTopicStatus(string displayOrder)
        {
            //>0Ϊ�ö�,<0����ʾ,==0����   -1Ϊ����վ   -2����� -3Ϊ������
            int order = int.Parse(displayOrder);
            if (order > 0)
                return "�ö�";
            if (order == 0)
                return "����";
            if (order == -1)
                return "����վ";
            if (order == -2)
                return "�����";
            if (order == -3)
                return "������";
            return displayOrder;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SelectPass.Click += new EventHandler(this.SelectPass_Click);
            this.SelectDelete.Click += new EventHandler(this.SelectDelete_Click);

            DataGrid1.DataKeyField = "tid";
            DataGrid1.TableHeaderName = "��������б�";
            DataGrid1.ColumnSpan = 10;
        }

        #endregion
    }
}