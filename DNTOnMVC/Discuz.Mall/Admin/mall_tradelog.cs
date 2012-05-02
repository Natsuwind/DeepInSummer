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
    /// ��������
    /// </summary>

    public partial class mall_tradelog : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataGrid1.TableHeaderName = "��������";
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
                return "<span style=\"color:green\">���½���</span>";
            else
                return "<span style=\"color:red\">���Ͻ���</span>";
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
                    result = "δ��Ч�Ľ���";
                    break;
                case "1":
                    result = "�ȴ���Ҹ���";
                    break;
                case "2":
                    result = "�����Ѵ���,�ȴ�����ȷ��";
                    break;
                case "3":
                    result = "ȷ����Ҹ����У����𷢻�";
                    break;
                case "4":
                    result = "����Ѹ���(��֧�����յ���Ҹ���),�����ҷ���";
                    break;
                case "5":
                    result = "�����ѷ��������ȷ����";
                    break;
                case "6":
                    result = "���ȷ���յ������ȴ�֧������������";
                    break;
                case "7":
                    result = "<span style=\"color:green\">���׳ɹ�����</span>";
                    break;
                case "8":
                    result = "������;�ر�(δ���)";
                    break;
                case "10":
                    result = "�ȴ�����ͬ���˿�";
                    break;
                case "11":
                    result = "���Ҿܾ�����������ȴ�����޸�����";
                    break;
                case "12":
                    result = "����ͬ���˿�ȴ�����˻�";
                    break;
                case "13":
                    result = "�ȴ������ջ�";
                    break;
                case "14":
                    result = "˫���Ѿ�һ�£��ȴ�֧�����˿�";
                    break;
                case "15":
                    result = "֧����������";
                    break;
                case "16":
                    result = "�������˿�";
                    break;
                case "17":
                    result = "�˿�ɹ�";
                    break;
                case "18":
                    result = "�˿�ر�";
                    break;
            }
            return string.Format("<a href=\"../../{0}.aspx?goodstradelogid={1}\" target=\"_blank\">{2}<br />{3}</a>", (offline == "1" ? "offlinetrade" : "onlinetrade"), id, result,lastupdate);
        }
    }
}
