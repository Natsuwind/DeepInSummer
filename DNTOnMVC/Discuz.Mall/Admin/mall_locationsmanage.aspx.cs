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
using Discuz.Entity;
using Discuz.Mall;
using Discuz.Web.Admin;

namespace Discuz.Mall.Admin
{
    /// <summary>
    /// 诚信规则
    /// </summary>

    public partial class mall_locationsmanage : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string locations = "var locations = " + Utils.DataTableToJSON(DbProvider.GetInstance().GetLocationsTable());
            this.RegisterStartupScript("locations", "<script type='text/javascript'>" + locations + "</script>");
            if (!IsPostBack)
            {
                DataGrid1.AllowCustomPaging = false;
                DataGrid1.TableHeaderName = "区域管理";
                DataGrid1.DataKeyField = "lid";
                DataGrid1.BindData(DbProvider.GetInstance().GetLocationsTableSql());
                country_select.Items.Add(new ListItem("　　　　　　　　　　　　　　", ""));
                foreach (DataRow dr in DbProvider.GetInstance().GetCountriesTable().Rows)
                {
                    country_select.Items.Add(new ListItem(dr["country"].ToString()));
                }
                state_select.Items.Add(new ListItem("　　　　　　　　　　　　　　", ""));
            }
        }

        protected void SaveLocation_Click(object sender, EventArgs e)
        {
            #region 保存区域修改
            int row = 0;
            DataTable dt = DbProvider.GetInstance().GetLocationsTable();
            string errorinfo = "";
            bool iserror = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                string country = DataGrid1.GetControlValue(row, "country").Trim();
                string state = DataGrid1.GetControlValue(row, "state").Trim();
                string city = DataGrid1.GetControlValue(row, "city").Trim();
                string zipcode = DataGrid1.GetControlValue(row, "zipcode").Trim();
                row++;
                DataRow oldrow = null;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["lid"].ToString() == id.ToString())
                    {
                        oldrow = dr;
                        break;
                    }
                }
                if (country == "" || state == "" || city == "")
                {
                    iserror = true;
                    errorinfo += "原<b>" + oldrow["city"].ToString() + "</b>行修改后信息不完整!<br>";
                    continue;
                }
                bool isreply = false;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["lid"].ToString() != id.ToString() && dr["country"].ToString() == country && dr["state"].ToString() == state && dr["city"].ToString() == city)
                    {
                        iserror = true;
                        errorinfo += "原<b>" + oldrow["city"].ToString() + "</b>修改后与其它记录重复!<br>";
                        isreply = true;
                        break;
                    }
                }
                if (isreply)
                    continue;
                Locationinfo local = new Locationinfo();
                local.Lid = id;
                local.Country = country;
                local.State = state;
                local.City = city;
                local.Zipcode = zipcode;
                DbProvider.GetInstance().UpdateLocations(local);
            }
            Locations.GetInstance.WriteJsonFile();
            if (iserror)
            {
                RegisterMessage(errorinfo + "<a href=\\'mall_locationsmanage.aspx\\'>返回</a>");
            }
            else
            {
                RegisterStartupScript("PAGE", "window.location.href='mall_locationsmanage.aspx';");
            }
            #endregion
        }

        protected void AddNewRec_Click(object sender, EventArgs e)
        {
            string country = DNTRequest.GetString("combox_country_select");
            string state = DNTRequest.GetString("combox_state_select");
            string city = DNTRequest.GetString("city");
            string zipcode = DNTRequest.GetString("zipcode");
            if (country == "" || state == "" || city == "")
            {
                RegisterMessage("国家、省份或城市信息没有填写!<br><a href=\\'mall_locationsmanage.aspx\\'>返回</a>");
                return;
            }
            DataTable dt = DbProvider.GetInstance().GetLocationsTable();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["country"].ToString() == country && dr["state"].ToString() == state && dr["city"].ToString() == city)
                {
                    RegisterMessage("新建记录与其它记录重复!<br><a href=\\'mall_locationsmanage.aspx\\'>返回</a>");
                    return;
                }
            }
            Locationinfo local = new Locationinfo();
            local.Country = country;
            local.State = state;
            local.City = city;
            local.Zipcode = zipcode;
            DbProvider.GetInstance().CreateLocations(local);
            Locations.GetInstance.WriteJsonFile();
            RegisterStartupScript("PAGE", "window.location.href='mall_locationsmanage.aspx';");
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            string idlist = DNTRequest.GetString("delid");
            if (idlist == "")
            {
                RegisterMessage("未选择任何要删除的记录!<br><a href=\\'mall_locationsmanage.aspx\\'>返回</a>");
                return;
            }
            DbProvider.GetInstance().DeleteLocations(idlist);
            Locations.GetInstance.WriteJsonFile();
            RegisterStartupScript("PAGE", "window.location.href='mall_locationsmanage.aspx';");
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }
    }
}
