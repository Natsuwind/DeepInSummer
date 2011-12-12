using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Yuwen.Tools.Data
{
    public class DBConfigs
    {
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

        public static string GetDbType()
        {
            //return "Sqlite";
            return ConfigurationSettings.AppSettings["dbtype"];
        }
    }
}
