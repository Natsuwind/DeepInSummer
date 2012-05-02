using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Common;

namespace Discuz.Config
{
    class DiscuzCloudConfigFileManager : DefaultConfigFileManager
    {
        private static DiscuzCloudConfigInfo m_configInfo;

        private static DateTime m_fileOldChange;


        static DiscuzCloudConfigFileManager()
        {
            if (!Utils.FileExists(ConfigFilePath))
                SerializationHelper.Save(DiscuzCloudConfigInfo.CreateInstance(), ConfigFilePath);

            m_fileOldChange = System.IO.File.GetLastWriteTime(ConfigFilePath);
            m_configInfo = (DiscuzCloudConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(DiscuzCloudConfigInfo));
        }

        public new static IConfigInfo ConfigInfo
        {
            get { return m_configInfo; }
            set { m_configInfo = (DiscuzCloudConfigInfo)value; }
        }

        /// <summary>
        /// 配置文件所在路径
        /// </summary>
        public static string fileName = null;

        /// <summary>
        /// 获取配置文件所在路径
        /// </summary>
        public new static string ConfigFilePath
        {
            get
            {
                if (fileName == null)
                    fileName = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/dzcloud.config");
                return fileName;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static DiscuzCloudConfigInfo LoadConfig()
        {
            ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileOldChange, ConfigFilePath, ConfigInfo);
            return ConfigInfo as DiscuzCloudConfigInfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }

    }
}
