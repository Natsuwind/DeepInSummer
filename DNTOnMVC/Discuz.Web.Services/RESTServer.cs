using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

using Discuz.Common;
using Discuz.Config;
using Discuz.Web.Services.API;
using Discuz.Web.Services.API.Commands;

namespace Discuz.Web.Services
{
    public class RESTServer : Page
    {
        /// <summary>
        /// 返回信息文本格式(xml or json)
        /// </summary>
        public string format = DNTRequest.GetString("format").Trim().ToLower();

        public RESTServer()
        {
            this.Load += new EventHandler(RESTServer_Load);
        }

        void RESTServer_Load(object sender, EventArgs e)
        {
            List<DNTParam> parameters = GetParamsFromRequest(HttpContext.Current.Request);
            APIConfigInfo apiInfo = APIConfigs.GetConfig();
            if (!apiInfo.Enable)
            {
                RESTServerResponse(Util.CreateErrorMessage(ErrorType.API_EC_SERVICE, parameters));
                return;
            }

            //查找匹配客户端配置信息
            ApplicationInfo appInfo = null;
            ApplicationInfoCollection appcollection = apiInfo.AppCollection;
            foreach (ApplicationInfo newapp in appcollection)
            {
                if (newapp.APIKey == DNTRequest.GetString("api_key"))
                {
                    appInfo = newapp;
                    break;
                }
            }

            if (appInfo == null)
            {
                RESTServerResponse(Util.CreateErrorMessage(ErrorType.API_EC_APPLICATION, parameters));
                return;
            }

            //check request ip
            string ip = DNTRequest.GetIP();
            if (appInfo.IPAddresses != null && appInfo.IPAddresses.Trim() != string.Empty && !Utils.InIPArray(ip, appInfo.IPAddresses.Split(',')))
            {
                RESTServerResponse(Util.CreateErrorMessage(ErrorType.API_EC_BAD_IP, parameters));
                return;
            }

            string sig = GetSignature(parameters, appInfo.Secret);
            if (sig != DNTRequest.GetString("sig"))
            {
                //输出签名错误
                RESTServerResponse(Util.CreateErrorMessage(ErrorType.API_EC_SIGNATURE, parameters));
                return;
            }

            string method = DNTRequest.GetString("method").Trim().ToLower();
            //如果客户端未指定方法名称
            if (string.IsNullOrEmpty(method))
            {
                RESTServerResponse(Util.CreateErrorMessage(ErrorType.API_EC_METHOD, parameters));
                return;
            }

            RESTServerResponse(CommandManager.Run(new CommandParameter(method, parameters, appInfo)));
        }

        /// <summary>
        /// 输出服务端执行结果
        /// </summary>
        /// <param name="content"></param>
        void RESTServerResponse(string content)
        {
            //获取客户端设置的javascript回调函数
            string jsCallback = DNTRequest.GetString("callback");

            Response.Clear();
            Response.ContentType = (format == "json" || !string.IsNullOrEmpty(jsCallback)) ? "text/html" : "text/xml";

            //如果设置了js回调函数
            if (!string.IsNullOrEmpty(jsCallback))
                content = string.Format("{0}({1});", jsCallback, format == "json" ? content : "\"" + content.Replace("\"", "\\\"") + "\"");

            Response.Write(content);
            Response.End();
        }

        /// <summary>
        /// 根据参数和密码生成签名字符串
        /// </summary>
        /// <param name="parameters">API参数</param>
        /// <param name="secret">密码</param>
        /// <returns>签名字符串</returns>
        private string GetSignature(List<DNTParam> parameters, string secret)
        {
            StringBuilder values = new StringBuilder();
            foreach (DNTParam param in parameters)
            {
                if (param.Name == "sig" || string.IsNullOrEmpty(param.Value))
                    continue;
                values.Append(param.ToString());
            }
            values.Append(secret);
            return Utils.MD5(values.ToString());
        }

        private List<DNTParam> GetParamsFromRequest(HttpRequest request)
        {
            List<DNTParam> list = new List<DNTParam>();
            foreach (string key in request.QueryString.AllKeys)
            {
                list.Add(DNTParam.Create(key, request.QueryString[key]));
            }
            foreach (string key in request.Form.AllKeys)
            {
                list.Add(DNTParam.Create(key, request.Form[key]));
            }
            list.Sort();
            return list;
        }
    }
}
