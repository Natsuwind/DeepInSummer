using System;
using System.Data;
using System.Text;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Mall.Data;
using Discuz.Config;
using Discuz.Mall;

namespace Discuz.Mall.Pages
{
    /// <summary>
    /// 买卖信用
    /// </summary>
    public class eccredit : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo userinfo;
        /// <summary>
        /// 要显示的信用记录
        /// </summary>
        public StringBuilder sb_usercredit = new StringBuilder();
        /// <summary>
        /// 用户注册日期
        /// </summary>
        public string joindate = "";
        /// <summary>
        /// 获取诚信规则列表GetCreditRulesJsonData
        /// </summary>
        public string creditrulesjsondata = "";
        #endregion

        protected override void ShowPage()
        {
            if (config.Enablemall == 0) //未启用交易服务
            {
                AddErrLine("系统未开启交易服务, 当前页面暂时无法访问!");
                return;
            }

            int uid = DNTRequest.GetInt("uid", 0);
            if (uid <= 0)
            {
                AddErrLine("无效的用户id.");
                return;
            }

            userinfo = Users.GetUserInfo(uid);
            if (userinfo != null)
                joindate = Convert.ToDateTime(userinfo.Joindate).ToString("yyyy-MM-dd");
            
            sb_usercredit = GoodsUserCredits.GetUserCreditJsonData(uid);
            creditrulesjsondata = GoodsUserCredits.GetCreditRulesJsonData().ToString();          
        }
    }
}
