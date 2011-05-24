using System;
using System.Collections.Generic;
using System.Web;
using Discuz.Config;
using Discuz.Common;

namespace Wysky.Discuz.Plugin.QZoneLogin.BLL.Config
{
    class QZoneLoginConfigFileManager : DefaultConfigFileManager
    {        
         private static QZoneLoginConfigInfo m_configinfo ;
        
        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;

        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static QZoneLoginConfigFileManager()
        {
            if (!System.IO.File.Exists(ConfigFilePath))
            {
                QZoneLoginConfigInfo qzlci = new QZoneLoginConfigInfo();
                SerializationHelper.Save(qzlci, ConfigFilePath);
            }
            m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
            m_configinfo = (QZoneLoginConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(QZoneLoginConfigInfo));
        }

        /// <summary>
        /// 当前的配置实例
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (QZoneLoginConfigInfo)value; }
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
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/QZoneLogin.config");
                }

                return filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static QZoneLoginConfigInfo LoadConfig()
        {
            ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
            return ConfigInfo as QZoneLoginConfigInfo;
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