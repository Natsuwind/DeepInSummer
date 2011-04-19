using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Natsuhime.Data
{
    public class DbConfigs
    {
        private static object lockHelper = new object();

        private static DbConfigInfo m_configinfo;

        private static string configpath = System.Web.HttpContext.Current.Server.MapPath("~/data/config/NSWindBase.config");
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


    public class SerializationHelper
    {
        /// <summary>
        /// XML 序列化
        /// </summary>
        /// <param name="o"></param>
        /// <param name="t"></param>
        /// <param name="fullXmlPath"></param>
        public static void SaveXml(object o, string fullXmlPath)
        {
            //FileStream fs = null;
            XmlTextWriter xw = null;
            try
            {
                //fs = new FileStream(fullXmlPath, FileMode.OpenOrCreate);
                xw = new XmlTextWriter(fullXmlPath, Encoding.GetEncoding("gb2312"));
                XmlSerializer xs = new XmlSerializer(o.GetType());


                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                xs.Serialize(xw, o, ns);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //if (fs != null)
                //    fs.Close();
                if (xw != null)
                {
                    xw.Close();
                }
            }
        }
        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fullXmlPath"></param>
        /// <returns></returns>
        public static object LoadXml(Type t, string fullXmlPath)
        {
            FileStream fs = null;
            object o = null;

            try
            {
                fs = new FileStream(fullXmlPath, FileMode.Open);
                o = XmlDeSerialize(t, fs);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return o;
        }
        /// <summary>
        /// XML反序列化(重载读入流,记得手动关闭流)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object XmlDeSerialize(Type t, Stream s)
        {
            XmlSerializer xs = new XmlSerializer(t);
            return xs.Deserialize(s);
        }


        public static void SaveBinary(object o, string fullFilePath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fullFilePath, FileMode.OpenOrCreate);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, o);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        public static object LoadBinary(string fullFilePath)
        {
            FileStream fs = null;
            object o = null;

            try
            {
                fs = new FileStream(fullFilePath, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                o = formatter.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return o;
        }


        //public static void SaveJson(object o, string fullFilePath)
        //{
        //    string json = JavaScriptConvert.SerializeObject(o);
        //    File.WriteAllText(fullFilePath, json, new UTF8Encoding(true, true));
        //}
        //public static object LoadJson(string fullFilePath, Type t)
        //{
        //    string oldJson = File.ReadAllText(fullFilePath, new UTF8Encoding(true, true));
        //    if (oldJson.Trim() != string.Empty)
        //    {
        //        return JavaScriptConvert.DeserializeObject(oldJson, t);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
