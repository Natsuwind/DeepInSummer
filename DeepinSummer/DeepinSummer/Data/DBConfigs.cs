using System;
using Natsuhime.Common;
using System.Configuration;

namespace Natsuhime.Data
{
    public class DbConfigs
    {
        private static object lockHelper = new object();

        private static DbConfigInfo m_configinfo;

        private static string configpath = Utils.GetMapPath("~/NSWindBase.config");
        /// <summary>
        /// 静态构造函数初始化相应实例和定时器
        /// </summary>
        static DbConfigs()
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
        public static DbConfigInfo GetConfig()
        {
            return m_configinfo;
        }


        /// <summary>
        /// 序列化配置信息为XML
        /// </summary>
        /// <param name="configinfo">配置信息</param>
        /// <param name="configFilePath">配置文件完整路径</param>
        public static void Save(DbConfigInfo configinfo)
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
        public static DbConfigInfo Load()
        {
            return (DbConfigInfo)SerializationHelper.LoadXml(typeof(DbConfigInfo), configpath);
        }

        #region 过时代码
        [Obsolete("过时的")]
        /// <summary>
        /// 得到数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetDBConnectString()
        {
            //return string.Format("Data Source={0}\\config\\db.config", System.Web.HttpContext.Current.Server.MapPath("~/"));
            //return @"Data Source=D:\database\sqlite\aspx163.config";
            return ConfigurationSettings.AppSettings["dbconnstring"];
        }
        [Obsolete("过时的")]
        public static string GetDbType()
        {
            //return "Sqlite";
            return ConfigurationSettings.AppSettings["dbtype"];
        }
        #endregion
    }
}
