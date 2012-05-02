using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Cache;
using System.IO;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加鉴定
    /// </summary>
    public partial class addidentify : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                uploadfile.UpFilePath = uploadfilesmall.UpFilePath= Server.MapPath("../../images/identify/");
                
            }
        }

        private void AddIdentifyInfo_Click(object sender, EventArgs e)
        {
            #region 添加鉴定记录

            if (this.CheckCookie())
            {
                if (uploadfile.FileName.Trim() == "" || uploadfilesmall.FileName.Trim() == "")
                {
                    base.RegisterStartupScript("PAGE", "alert('没有选择鉴定图片');window.location.href='forum_addidentify.aspx';");
                    return;
                }
                string fileName = uploadfile.UpdateFile();
                string fileNameSmall = uploadfilesmall.UpdateFile();
                if (fileName.Trim() == "")
                {
                    base.RegisterStartupScript("PAGE", "alert('没有选择鉴定大图片');window.location.href='forum_addidentify.aspx';");
                    return;
                }
                if (fileNameSmall.Trim() == "")
                {
                    base.RegisterStartupScript("PAGE", "alert('没有选择鉴定小图片');window.location.href='forum_addidentify.aspx';");
                    return;
                }
                if (Identifys.AddIdentify(name.Text, fileName))
                {
                    string[] fileNameArray = fileName.Split('.');
                    string newFileNameSmall = string.Format("{0}_small.{1}",fileNameArray[0],fileNameArray[1]);
                    Directory.Move(Server.MapPath("../../images/identify/") + fileNameSmall, Server.MapPath("../../images/identify/") + newFileNameSmall);
                    DNTCache.GetCacheService().RemoveObject("/Forum/TopicIdentifys");
                    DNTCache.GetCacheService().RemoveObject("/Forum/TopicIndentifysJsArray");
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "鉴定文件添加", name.Text);
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_identifymanage.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("PAGE", "alert('插入失败，可能名称与原有的相同');window.location.href='forum_identifymanage.aspx';");
                }
            }

            #endregion
        }

        #region Web 窗体设计器生成的代码

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddIdentifyInfo.Click += new EventHandler(this.AddIdentifyInfo_Click);
        }

        #endregion


    }
} 