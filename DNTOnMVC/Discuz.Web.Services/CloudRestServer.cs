using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

using Discuz.Common;
using Discuz.Config;
using Discuz.Web.Services.DiscuzCloud;
using Discuz.Web.Services.DiscuzCloud.Commands;

namespace Discuz.Web.Services
{
    public class CloudRestServer : Page
    {
        public static RegexOptions options = RegexOptions.None;

        private static Regex regex1 = new Regex(@",""params"":({([\s\S]+?)})},{""module", options);//匹配除最后一个任务params的所有params项目

        private static Regex regex2 = new Regex(@",""params"":({([\s\S]+?)})}]$", options);//匹配最后一个任务params项目

        public CloudRestServer()
        {
            this.Load += new EventHandler(CloudRest_Load);
        }

        void CloudRest_Load(object sender, EventArgs e)
        {
            Response.Clear();

            string className = DNTRequest.GetString("module");
            string method = DNTRequest.GetString("method");
            string myParams = DNTRequest.GetString("params");
            string sign = DNTRequest.GetString("sign");

            if (!CheckSignature(sign, className, method, myParams))//校验请求签名合法性
                return;

            string result = CommandManager.Run(new CommandParameter(className + "." + method, myParams));

            Response.Write(result);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private bool CheckSignature(string sign, string className, string method, string myParams)
        {
            return sign == Utils.MD5(string.Format("{0}|{1}|{2}|{3}", className, method, myParams, DiscuzCloudConfigs.GetConfig().Cloudsitekey));
        }
    }
}
