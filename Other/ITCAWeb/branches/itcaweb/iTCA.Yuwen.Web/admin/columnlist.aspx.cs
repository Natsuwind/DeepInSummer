using System;
using System.Web;
using iTCA.Yuwen.Core;
using iTCA.Yuwen.Entity;
using System.Web.UI.WebControls;
using Yuwen.Tools.TinyCache;

namespace iTCA.Yuwen.Web.Admin
{
    public partial class columnlist : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            BindddlColumns();
            BindgvColumnList();
        }
        private void BindddlColumns()
        {
            dddlParentid.DataTextField = "columnname";
            dddlParentid.DataValueField = "columnid";
            dddlParentid.DataSource = Articles.GetColumnCollection();
            dddlParentid.DataBind();
        }
        private void BindgvColumnList()
        {
            gvColumnList.DataSource = Articles.GetColumnCollection();
            gvColumnList.DataBind();
        }



        protected void btnAddNewColumn_Click(object sender, EventArgs e)
        {
            ColumnInfo columninfo = new ColumnInfo();
            columninfo.Columnname = tbxColumnname.Text.Trim();
            columninfo.Parentid = Convert.ToInt32(dddlParentid.SelectedValue);
            Articles.CreateColumn(columninfo);
            lbMessage.Text = "��ӳɹ�!";
            RemoveColumnListCache();
            BindData();
        }

        private static void RemoveColumnListCache()
        {
            TinyCache cache = new TinyCache();
            cache.RemoveObject("ColumnList");
        }


        protected void gvColumnList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvColumnList.EditIndex = e.NewEditIndex;
            this.BindData();
        }

        protected void gvColumnList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvColumnList.EditIndex = -1;
            this.BindData();
        }

        protected void gvColumnList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int columnid = Convert.ToInt32(gvColumnList.Rows[e.RowIndex].Cells[0].Text);
            if (columnid > 5)//id<6��ΪϵͳĬ�Ϸ��಻����ɾ��
            {
                Articles.DeleteColumn(columnid);
                lbMessage.Text = "ɾ���ɹ�!";
                RemoveColumnListCache();
                this.BindData();
            }
            else
            {
                lbMessage.Text = "ϵͳĬ�Ϸ��಻����ɾ��!";
            }
            
        }

        protected void gvColumnList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ColumnInfo columninfo = new ColumnInfo();
            columninfo.Columnid = Convert.ToInt32(gvColumnList.Rows[e.RowIndex].Cells[0].Text);
            columninfo.Columnname = ((TextBox)(gvColumnList.Rows[e.RowIndex].Cells[1].Controls[0])).Text.Trim();
            columninfo.Parentid = Convert.ToInt32((((TextBox)gvColumnList.Rows[e.RowIndex].Cells[2].Controls[0])).Text.Trim());
            Articles.EditColumn(columninfo);
            lbMessage.Text = "���³ɹ�!";
            gvColumnList.EditIndex = -1;
            RemoveColumnListCache();
            BindData();
        }
    }
}
