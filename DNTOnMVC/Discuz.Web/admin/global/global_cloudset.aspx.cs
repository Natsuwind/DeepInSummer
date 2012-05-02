using System;
using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 用户权限设置
    /// </summary>
    public partial class cloudset : AdminPage
    {
        //public string iframeUrl = "";

        public DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (DNTRequest.GetString("action"))
            {
                case "reg":
                    AjaxResponse(DiscuzCloud.RegisterSite());
                    break;
                case "bind":
                    AjaxResponse(string.Format("{{url:\"{0}\"}}", DiscuzCloud.GetCloudBindUrl(userid)));
                    break;
                case "sync":
                    AjaxResponse(DiscuzCloud.SyncSite());
                    break;
                case "resetkey":
                    AjaxResponse(DiscuzCloud.ResetSiteKey());
                    break;
                    
            }
        }

        public void AjaxResponse(string content)
        {
            Response.Write(content);
            Response.End();
        }

        public string SecritySiteKey(string oriKey)
        {
            string headStr = oriKey.Substring(0,4);
            string footStr = oriKey.Substring(oriKey.Length - 3, 2);

            return headStr + "****" + footStr;
        }
    }
}