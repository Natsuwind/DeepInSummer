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
        /// ��̬���캯����ʼ����Ӧʵ���Ͷ�ʱ��
        /// </summary>
        static DbConfigs()
        {
            if (m_configinfo == null)
            {
                m_configinfo = Load();
            }
        }


        /// <summary>
        /// ����������ʵ��
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = Load();
        }
        /// <summary>
        /// ȡ�þ�̬������Ϣ
        /// </summary>
        /// <returns></returns>
        public static DbConfigInfo GetConfig()
        {
            return m_configinfo;
        }


        /// <summary>
        /// ���л�������ϢΪXML
        /// </summary>
        /// <param name="configinfo">������Ϣ</param>
        /// <param name="configFilePath">�����ļ�����·��</param>
        public static void Save(DbConfigInfo configinfo)
        {
            lock (lockHelper)
            {
                SerializationHelper.SaveXml(configinfo, configpath);
            }
        }

        /// <summary>
        /// ��XML����������Ϣ
        /// </summary>
        /// <returns></returns>
        public static DbConfigInfo Load()
        {
            return (DbConfigInfo)SerializationHelper.LoadXml(typeof(DbConfigInfo), configpath);
        }

        #region ��ʱ����
        [Obsolete("��ʱ��")]
        /// <summary>
        /// �õ����ݿ������ַ���
        /// </summary>
        /// <returns></returns>
        public static string GetDBConnectString()
        {
            //return string.Format("Data Source={0}\\config\\db.config", System.Web.HttpContext.Current.Server.MapPath("~/"));
            //return @"Data Source=D:\database\sqlite\aspx163.config";
            return ConfigurationSettings.AppSettings["dbconnstring"];
        }
        [Obsolete("��ʱ��")]
        public static string GetDbType()
        {
            //return "Sqlite";
            return ConfigurationSettings.AppSettings["dbtype"];
        }
        #endregion
    }
}
