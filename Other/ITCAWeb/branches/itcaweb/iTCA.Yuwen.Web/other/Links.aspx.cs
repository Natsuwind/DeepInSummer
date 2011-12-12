using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace itca.WebPages.other
{
    public partial class Links : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string ToPage = Request.QueryString["ToPage"];
                if (ToPage == null)
                {
                    ToPage = "1";
                }
                /*          if (!StrRegExp.IsID(ToPage)) //StrRegExp.IsID是原来代码里用来检查ToPage是否为规定的3位0-9的数字
                            {
                                ToPage = "1";
                            }
                */
                this.Bind_rptList(Convert.ToInt32(ToPage));
            }
        }

        private void Bind_rptList(int ToPage)
        {
            int CurrentPage = ToPage;
            int PageSize = 9;
            int PageCount;
            int RecordCount;
            string PageSQL;
            string DataTable = " i_links ";
            string DataFiled = " l_id ";

            //string DataFileds = "";
            string DataOrders = " l_id ";
            OleDbConnection objConn = db.CreateConnection();
            objConn.Open();

            //* 取得记录总数，计算总页数
            string countSql = "select count(" + DataFiled + ") from " + DataTable;
            OleDbCommand cmd = new OleDbCommand(countSql, objConn);
            RecordCount = Convert.ToInt32(cmd.ExecuteScalar());//执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
            if ((RecordCount % PageSize) != 0)
            {
                PageCount = RecordCount / PageSize + 1;
            }
            else
            {
                PageCount = RecordCount / PageSize;
            }
            if (ToPage > PageCount)
            {
                CurrentPage = PageCount;
            }
            if (CurrentPage <= 1)
            {
                PageSQL = "Select Top " + PageSize + " * From " + DataTable + " Order By " + DataOrders;
            }
            else
            {
                PageSQL = "Select Top " + PageSize + " * From " + DataTable + " Where " + DataFiled + " Not In ( Select Top " + PageSize * (CurrentPage - 1) + " " + DataFiled + " From " + DataTable + "  Order By " + DataOrders + " ) Order By " + DataOrders;

            }
            OleDbCommand cmd1 = new OleDbCommand(PageSQL, objConn);
            OleDbDataReader drPage = cmd1.ExecuteReader();
            this.lbTotalPage.Text = Convert.ToString(PageCount);
            this.hlkFirstPage.NavigateUrl = "?ToPage=1";
            this.hlkLastPage.NavigateUrl = "?ToPage=" + PageCount;
            this.lbCurrentPage.Text = Convert.ToString(CurrentPage);
            if (CurrentPage <= 1)
            {
                this.hlkPrevPage.Enabled = false;
                CurrentPage = 1;
            }
            else
            {
                this.hlkPrevPage.Enabled = true;
                this.hlkPrevPage.NavigateUrl = "?ToPage=" + (ToPage - 1);
            }
            if (CurrentPage >= PageCount)
            {
                this.hlkNextPage.Enabled = false;
                CurrentPage = PageCount;
            }
            else
            {
                this.hlkNextPage.Enabled = true;
                this.hlkNextPage.NavigateUrl = "?ToPage=" + (ToPage + 1);
            }
            rptList.DataSource = drPage;
            rptList.DataBind();
            objConn.Close();

        }
    }
}
