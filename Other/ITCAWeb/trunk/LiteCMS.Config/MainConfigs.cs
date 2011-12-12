using System;
using System.IO;
using Natsuhime.Common;

namespace LiteCMS.Config
{
    public class MainConfigs
    {
        private static object lockHelper = new object();

        private static MainConfigInfo m_configinfo;

        private static string configpath = Path.Combine(Utils.GetMapPath("~/"), "config" + Path.DirectorySeparatorChar + "main.config");
        /// <summary>
        /// 静态构造函数初始化相应实例和定时器
        /// </summary>
        static MainConfigs()
        {
            if (m_configinfo == null)
            {
                m_configinfo = Load();
            }
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = Load();
        }
        /// <summary>
        /// 取得静态配置信息
        /// </summary>
        /// <returns></returns>
        public static MainConfigInfo GetConfig()
        {
            return m_configinfo;
        }

        
        /// <summary>
        /// 序列化配置信息为XML
        /// </summary>
        /// <param name="configinfo">配置信息</param>
        /// <param name="configFilePath">配置文件完整路径</param>
        public static void Save(MainConfigInfo configinfo)
        {
            lock (lockHelper)
            {
                SerializationHelper.SaveXml(configinfo, configpath);
            }
        }

        /// <summary>
        /// 从XML加载配置信息
        /// </summary>
        /// <returns></returns>
        public static MainConfigInfo Load()
        {
            return (MainConfigInfo)SerializationHelper.LoadXml(typeof(MainConfigInfo), configpath);
        }
    }
}
