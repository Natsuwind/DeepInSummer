using System;
using System.Collections.Generic;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using Newtonsoft.Json;

namespace Discuz.Web.Services.DiscuzCloud.Commands
{
    public class CloudOpenCommand : Command
    {
        public CloudOpenCommand()
            : base("cloud.openCloud")
        {

        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            config.Cloudenabled = 1;
            DiscuzCloudConfigs.SaveConfig(config);
            DiscuzCloudConfigs.ResetConfig();

            result = CloudUtils.GetCloudResponse<bool>(Utils.UrlDecode(commandParam.CloudParams) == "[]");
            return true;
        }
    }

    public class SetAppsCommand : Command
    {
        public SetAppsCommand()
            : base("cloud.setApps")
        {

        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            string[] actionList = { "connect", "qqgroup" };
            string action = "";

            foreach (string act in actionList)
            {
                if (commandParam.CloudParams.IndexOf(string.Format("\"{0}\"", act)) != -1)
                {
                    action = act;
                    break;
                }
            }
            int enable = commandParam.CloudParams.IndexOf(string.Format("\"{0}\"", "normal")) != -1 ? 1 : 0;

            DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
            bool changed = false;
            switch (action)
            {
                case "connect": config.Connectenabled = enable; changed = true; break;
            }
            if (changed)
            {
                DiscuzCloudConfigs.SaveConfig(config);
                DiscuzCloudConfigs.ResetConfig();
            }
            result = CloudUtils.GetCloudResponse<bool>(true);
            return true;
        }
    }

    public class GetAppsCommand : Command
    {
        public GetAppsCommand()
            : base("cloud.getApps")
        {

        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            result = CloudUtils.GetCloudResponse<bool>(true);
            return true;
        } 
    }

    public class SetConnectConfig : Command
    {
        public SetConnectConfig()
            : base("connect.setConfig")
        {
        }

        public override bool Run(CommandParameter commandParam, ref string result)
        {
            try
            {
                QQConnectAuthInfo authInfo = JavaScriptConvert.DeserializeObject<List<QQConnectAuthInfo>>(commandParam.CloudParams)[0];
                if (authInfo != null)
                {
                    DiscuzCloudConfigInfo config = DiscuzCloudConfigs.GetConfig();
                    config.Connectappid = authInfo.AppId;
                    config.Connectappkey = authInfo.AppKey;

                    DiscuzCloudConfigs.SaveConfig(config);
                    DiscuzCloudConfigs.ResetConfig();
                    result = CloudUtils.GetCloudResponse<bool>(true);
                }
                else
                {
                    result = CloudUtils.GetCloudResponse<bool>(false);
                }
            }
            catch
            {
                result = CloudUtils.GetCloudResponse<bool>(false);
            }
            return true;
        }
    }
}
