using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using Discuz.Mall.Data;
using Discuz.Common;
using Discuz.Web.Admin;

namespace Discuz.Mall.Admin
{
    /// <summary>
    /// ���Ź���
    /// </summary>
    
    public partial class mall_creditrule : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("lowerlimit");
                dt.Columns.Add("upperlimit");
                dt.Columns.Add("sellericon");
                dt.Columns.Add("buyericon");
                IDataReader ida = DbProvider.GetInstance().GetGoodsCreditRules();
                while (ida.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["id"] = ida[0];
                    dr["lowerlimit"] = ida[1];
                    dr["upperlimit"] = ida[2];
                    dr["sellericon"] = ida[3];
                    dr["buyericon"] = ida[4];
                    dt.Rows.Add(dr);
                }
                ida.Close();
                DataGrid1.AllowCustomPaging = false;
                DataGrid1.TableHeaderName = "���Ź���";
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
            }
        }

        protected void SaveCreditRule_Click(object sender, EventArgs e)
        {
            #region �������õȼ��޸�
            int row = 0;
            int nextlowerlimit = -1;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                string strlowerlimit = DataGrid1.GetControlValue(row, "lowerlimit").Trim();
                string strupperlimit = DataGrid1.GetControlValue(row, "upperlimit").Trim();
                if (!Utils.IsNumeric(strlowerlimit) || !Utils.IsNumeric(strupperlimit))
                {
                    RegisterMessage("���õȼ� " + id + " ȡֵ�Ƿ�!<br><a href=\\'mall_creditrule.aspx\\'>����</a>");
                    return;
                }
                int lowerlimit = int.Parse(strlowerlimit);
                int upperlimit = int.Parse(strupperlimit);
                if (id != 1 && (lowerlimit != nextlowerlimit))
                {
                    RegisterMessage("���õȼ� " + id + " ȡֵ�Ƿ�!<br><a href=\\'mall_creditrule.aspx\\'>����</a>");
                    return;
                }
                nextlowerlimit = upperlimit + 1;
                row++;
            }
            row = 0;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                int lowerlimit = int.Parse(DataGrid1.GetControlValue(row, "lowerlimit").Trim());
                int upperlimit = int.Parse(DataGrid1.GetControlValue(row, "upperlimit").Trim());
                DbProvider.GetInstance().UpdateCreditRules(id, lowerlimit, upperlimit);
                row++;
            }
            RegisterStartupScript("PAGE", "window.location.href='mall_creditrule.aspx';");
            #endregion
        }
    }
}
