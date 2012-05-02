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
                    DataGrid1.TableHeaderName = "商品审核管理";
                    resume.Visible = false;
                    DataGrid1.BindData(DbProvider.GetInstance().GetAllAuditGoods());
                }
                else
                {
                    DataGrid1.TableHeaderName = "商品回收站管理";
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
        /// 通过审核商品
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
        /// 删除商品
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
                    result = "虚拟物品或无需邮递";
                    break;
                case "1":
                    result = "卖家承担运费";
                    break;
                case "2":
                    result = "买家承担运费";
                    break;
                case "3":
                    result = "支付给物流公司";
                    break;
            }
            return result;
        }

        protected string GetStatus(string status)
        {
            // 0为正常 <0不显示   -1为回收站   -2待审核  -3未上架(不可见)
            string result = "";
            switch(status)
            {
                case "0":
                    result = "正常";
                    break;
                case "-1":
                    result = "回收站";
                    break;
                case "-2":
                    result = "待审核";
                    break;
                case "-3":
                    result = "未上架";
                    break;
            }
            return result;
        }
    }
}
