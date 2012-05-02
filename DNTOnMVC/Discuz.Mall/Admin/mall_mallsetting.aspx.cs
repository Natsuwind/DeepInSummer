using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Mall.Data;
using Discuz.Web.Admin;

namespace Discuz.Mall.Admin
{
    /// <summary>
    /// ��������ʾ��ʽ����
    /// </summary>
    
#if NET1
    public class global_mallsetting : AdminPage
#else
    public partial class mall_mallsetting : AdminPage
#endif
    {

#if NET1
        #region �ؼ�����
        protected Discuz.Control.DropDownList templateid;
        protected Discuz.Control.RadioButtonList stylejump;
        protected Discuz.Control.RadioButtonList browsecreatetemplate;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button SaveInfo;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            #region ����������Ϣ

            GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
            EnableMall.SelectedValue = __configinfo.Enablemall.ToString();
            gpp.Text = __configinfo.Gpp.ToString();
            
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region ����������Ϣ

            if (this.CheckCookie())
            {
                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));

                __configinfo.Enablemall = Convert.ToInt16(EnableMall.SelectedValue);
                __configinfo.Gpp = Convert.ToInt16(gpp.Text);

                GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));

                base.RegisterStartupScript( "PAGE",  "window.location.href='mall_mallsetting.aspx';");
            }

            #endregion
        }

        #region Web ������������ɵĴ���

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

    }
}