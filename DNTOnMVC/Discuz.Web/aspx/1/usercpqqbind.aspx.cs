using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using System.Text;

namespace Discuz.Web
{
    public class usercpqqbind : UserCpPage
    {
        /// <summary>
        /// 是否默认发送动态到qq空间
        /// </summary>
        public bool ispublishfeed = false;

        /// <summary>
        /// 是否默认发送动态到微博
        /// </summary>
        public bool ispublisht = false;

        protected override void ShowPage()
        {
            pagetitle = "QQ绑定";

            if (!IsLogin())
                return;

            if (!isbindconnect)
            {
                AddErrLine("您未绑定QQ");
                return;
            }

            UserConnectInfo userConnectInfo = DiscuzCloud.GetUserConnectInfo(userid);
            if (userConnectInfo == null)
            {
                //修正Cookie值状态
                Utils.WriteCookie("bindconnect", "false");
                AddErrLine("您未绑定QQ");
                return;
            }

            if (ispost)
            {
                userConnectInfo.AllowPushFeed = DNTRequest.GetInt("ispublishfeed", 0) + DNTRequest.GetInt("ispublisht", 0);
                DiscuzCloud.UpdateUserConnectInfo(userConnectInfo);
                Utils.WriteCookie("cloud_feed_status", string.Format("{0}|{1}", userid, userConnectInfo.AllowPushFeed));
                SetUrl("usercpqqbind.aspx");
                SetMetaRefresh();
                SetShowBackLink(true);
                AddMsgLine("绑定设置修改完毕");
                return;
            }
            else
            {
                ispublishfeed = (userConnectInfo.AllowPushFeed & 1) == 1;
                ispublisht = (userConnectInfo.AllowPushFeed & 2) == 2;
            }
        }
    }
}