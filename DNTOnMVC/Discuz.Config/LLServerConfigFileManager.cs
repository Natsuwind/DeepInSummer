using System;

using Discuz.Common;

namespace Discuz.Config
{

    /// <summary>
    /// LLServer配置管理类
    /// </summary>
    public class LLServerConfigFileManager : DefaultConfigFileManager
    {
        private static LLServerConfigInfo m_configinfo;

        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;


        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static LLServerConfigFileManager()
        {
            if (Utils.FileExists(ConfigFilePath))
            {
                m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
                m_configinfo = (LLServerConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(LLServerConfigInfo));
            }
        }

        /// <summary>
        /// 当前的配置类实例
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (LLServerConfigInfo)value; }
        }

        /// <summary>
        /// 配置文件所在路径
        /// </summary>
        public static string filename = null;


        /// <summary>
        /// 获取配置文件所在路径
        /// </summary>
        public new static string ConfigFilePath
        {
            get
            {
                if (filename == null)
                {
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/llserver.config");
                }

                return filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static LLServerConfigInfo LoadConfig()
        {
            if (Utils.FileExists(ConfigFilePath))
            {
                ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
                return ConfigInfo as LLServerConfigInfo;
            }
            else
                return null;
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
