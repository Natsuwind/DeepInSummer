using System;
using System.Collections.Generic;
using System.Web;

namespace Wysky.Discuz.Plugin.QZoneLogin.BLL.Config
{
    public class QZoneLoginConfigs
    {
        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static QZoneLoginConfigInfo GetConfig()
        {
            return QZoneLoginConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <param name="qzloginConfigInfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(QZoneLoginConfigInfo qzloginConfigInfo)
        {
            QZoneLoginConfigFileManager qzlfm = new QZoneLoginConfigFileManager();
            QZoneLoginConfigFileManager.ConfigInfo = qzloginConfigInfo;
            return qzlfm.SaveConfig();
        }
    }
}