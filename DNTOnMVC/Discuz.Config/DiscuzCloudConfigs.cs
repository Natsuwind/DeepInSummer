using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Config
{
    public class DiscuzCloudConfigs
    {
        private static object lockHelper = new object();

        private static System.Timers.Timer cloudConfigTimer = new System.Timers.Timer(15000);

        private static DiscuzCloudConfigInfo m_configInfo;

        static DiscuzCloudConfigs()
        {
            m_configInfo = DiscuzCloudConfigFileManager.LoadConfig();

            cloudConfigTimer.AutoReset = true;
            cloudConfigTimer.Enabled = true;
            cloudConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Time_Elapsed);
            cloudConfigTimer.Start();

        }

        private static void Time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetConfig();
        }

        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static DiscuzCloudConfigInfo GetConfig()
        {
            return m_configInfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(DiscuzCloudConfigInfo discuzCloudConfigInfo)
        {
            DiscuzCloudConfigFileManager dcfm = new DiscuzCloudConfigFileManager();
            DiscuzCloudConfigFileManager.ConfigInfo = discuzCloudConfigInfo;
            return dcfm.SaveConfig();
        }

        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configInfo = DiscuzCloudConfigFileManager.LoadConfig();
        }
    }
}
