using System;
using System.Collections.Generic;
using LiteCMS.Core;
using LiteCMS.Entity;
using Natsuhime.Web;

namespace LiteCMS.Web
{
    public partial class showmessage : BasePage
    {
        protected bool isautoredirect;
        protected string messageheader;
        protected string messagefooter;
        protected string messagebody;
        protected string redirecturl;
        protected override void Page_Show()
        {
            string type = YRequest.GetString("type").ToLower();
            messageheader = Utils.HtmlEncode(YRequest.GetString("header"));
            messagefooter = Utils.HtmlEncode(YRequest.GetString("footer"));
            messagebody = Utils.HtmlEncode(YRequest.GetString("body"));
            redirecturl = YRequest.GetString("redirecturl") == string.Empty ? "javascript:history.back(-1);" : Utils.HtmlEncode(YRequest.GetString("redirecturl"));

            isautoredirect = type.ToLower() == "error" ? false : true;

            pagetitle = "系统提示";
        }
    }
}
