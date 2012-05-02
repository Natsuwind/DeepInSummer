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

    public partial class mall_goodsmanage : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string mod = DNTRequest.GetString("mod");
                DataGrid1.DataKeyField = "goodsid";
                if (mod == "audit")
                {
                    DataGrid1.TableHeaderName = "��Ʒ��˹���";
                    resume.Visible = false;
                    DataGrid1.BindData(DbProvider.GetInstance().GetAllAuditGoods());
                }
                else
                {
                    DataGrid1.TableHeaderName = "��Ʒ����վ����";
                    pass.Visible = false;
                    DataGrid1.BindData(DbProvider.GetInstance().GetAllRecyclebinGoods());
                } 
            }
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        protected void resume_Click(object sender, EventArgs e)
        {
            string goodsid = DNTRequest.GetString("goodsid");
            DbProvider.GetInstance().ResetRecyclebinGoods(goodsid);
            Response.Redirect("mall_goodsmanage.aspx?mod" + DNTRequest.GetString("mod"));
        }

        /// <summary>
        /// ͨ�������Ʒ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void pass_Click(object sender, EventArgs e)
        {
            string goodsid = DNTRequest.GetString("goodsid");
            DbProvider.GetInstance().PassAuditGoods(goodsid);
            Response.Redirect("mall_goodsmanage.aspx?mod" + DNTRequest.GetString("mod"));
        }

        /// <summary>
        /// ɾ����Ʒ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void delete_Click(object sender, EventArgs e)
        {
            string goodsid = DNTRequest.GetString("goodsid");
            //DbProvider.GetInstance().ResetRecyclebinGoods(goodsid);
            Goods.DeleteGoods(goodsid, 0, false);
            Response.Redirect("mall_goodsmanage.aspx?mod" + DNTRequest.GetString("mod"));
        }
        public string GetProductInfo(string goodsid,string productname)
        {
            return string.Format("<a href=\"javascript:;\" onclick=\"ShowGoodsInfo({0})\">{1}</a>",goodsid,productname);
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

        protected string GetAccount(string email)
        {
            if (email.Trim() == "")
                return "";
            return string.Format("<a href='https://www.alipay.com/trade/i_credit.do?email={0}' target='_blank'>{1}</a>", email, email);
        }

        protected string GetTransport(string transporttype)
        {
            string result = "";
            switch (transporttype)
            {
                case "0":
                    result = "������Ʒ�������ʵ�";
                    break;
                case "1":
                    result = "���ҳе��˷�";
                    break;
                case "2":
                    result = "��ҳе��˷�";
                    break;
                case "3":
                    result = "֧����������˾";
                    break;
            }
            return result;
        }

        protected string GetStatus(string status)
        {
            // 0Ϊ���� <0����ʾ   -1Ϊ����վ   -2�����  -3δ�ϼ�(���ɼ�)
            string result = "";
            switch(status)
            {
                case "0":
                    result = "����";
                    break;
                case "-1":
                    result = "����վ";
                    break;
                case "-2":
                    result = "�����";
                    break;
                case "-3":
                    result = "δ�ϼ�";
                    break;
            }
            return result;
        }
    }
}
