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
    /// 订单管理
    /// </summary>

    public partial class mall_tradelog : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataGrid1.TableHeaderName = "订单管理";
            DataGrid1.DataKeyField = "id";
            DataGrid1.BindData(DbProvider.GetInstance().GetAllGoodstradelogs(drpstatus.SelectedValue));   
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        public string GetOrderNo(string offline)
        {
            if (offline == "1")
                return "<span style=\"color:green\">线下交易</span>";
            else
                return "<span style=\"color:red\">线上交易</span>";
        }

        public string GetProductInfo(string goodsid,string productname)
        {
            string result = "";
            if(config.Aspxrewrite == 1)
                result = string.Format("<a href=\"../../showgoods-{0}.aspx\" target=\"_blank\">{1}</a>",goodsid,productname);
            else
                result = string.Format("<a href=\"../../showgoods.aspx?goodsid={0}\" target=\"_blank\">{1}</a>", goodsid, productname);

            return result;
        }

        public string GetUserInfo(string userid, string username)
        {
            string result = "";
            if (config.Aspxrewrite == 1)
                result = string.Format("<a href=\"../../userinfo-{0}.aspx\" target=\"_blank\">{1}</a>", userid, username);
            else
                result = string.Format("<a href=\"../../userinfo.aspx?userid={0}\" target=\"_blank\">{1}</a>", userid, username);

            return result;
        }

        public string GetOrderStatus(string offline,string id,string status,string lastupdate)
        {
            string result = "";
            switch (status)
            {
                case "0":
                    result = "未生效的交易";
                    break;
                case "1":
                    result = "等待买家付款";
                    break;
                case "2":
                    result = "交易已创建,等待卖家确认";
                    break;
                case "3":
                    result = "确认买家付款中，暂勿发货";
                    break;
                case "4":
                    result = "买家已付款(或支付宝收到买家付款),请卖家发货";
                    break;
                case "5":
                    result = "卖家已发货，买家确认中";
                    break;
                case "6":
                    result = "买家确认收到货，等待支付宝打款给卖家";
                    break;
                case "7":
                    result = "<span style=\"color:green\">交易成功结束</span>";
                    break;
                case "8":
                    result = "交易中途关闭(未完成)";
                    break;
                case "10":
                    result = "等待卖家同意退款";
                    break;
                case "11":
                    result = "卖家拒绝买家条件，等待买家修改条件";
                    break;
                case "12":
                    result = "卖家同意退款，等待买家退货";
                    break;
                case "13":
                    result = "等待卖家收货";
                    break;
                case "14":
                    result = "双方已经一致，等待支付宝退款";
                    break;
                case "15":
                    result = "支付宝处理中";
                    break;
                case "16":
                    result = "结束的退款";
                    break;
                case "17":
                    result = "退款成功";
                    break;
                case "18":
                    result = "退款关闭";
                    break;
            }
            return string.Format("<a href=\"../../{0}.aspx?goodstradelogid={1}\" target=\"_blank\">{2}<br />{3}</a>", (offline == "1" ? "offlinetrade" : "onlinetrade"), id, result,lastupdate);
        }
    }
}
