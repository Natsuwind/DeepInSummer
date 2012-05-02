using System;
using System.Text;
using System.Web;
using System.IO;

using Discuz.Common;
using System.Xml.Serialization;
using System.Xml;


namespace Discuz.Config
{
    /// <summary>
    /// 论坛基本设置管理类
    /// </summary>
    class GeneralConfigFileManager : Discuz.Config.DefaultConfigFileManager
    {
        private static GeneralConfigInfo m_configinfo;

      
        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;


        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static GeneralConfigFileManager()
        {
            m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);

            try
            {
                m_configinfo = (GeneralConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(GeneralConfigInfo));
            }
            catch
            {
                if (File.Exists(ConfigFilePath))
                {
                    //ReviseConfig();
                    m_configinfo = (GeneralConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(GeneralConfigInfo));
                }
            }
        }

        /// <summary>
        /// 此函数仅为升级需要
        /// </summary>
        //private static void ReviseConfig()
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(ConfigFilePath);

        //    //1.0正式版升级到2.0
        //    if (doc.DocumentElement.Name != typeof(GeneralConfigInfo).Name)
        //    {
        //        XmlDocument newdoc = new XmlDocument();
        //        XmlNode declarenode = newdoc.CreateXmlDeclaration("1.0", null, null);

        //        newdoc.AppendChild(declarenode);
        //        XmlNode rootnode = newdoc.CreateElement(typeof(GeneralConfigInfo).Name);

        //        newdoc.AppendChild(rootnode);
        //        XmlNodeList xnl = doc.DocumentElement.ChildNodes;

        //        foreach (XmlNode node in xnl)
        //        {
        //            if (node.Name != "Maxavatarpixel")
        //            {
        //                XmlNode newnode = newdoc.CreateElement(node.Name);
        //                newnode.InnerXml = node.InnerXml;
        //                newdoc.DocumentElement.AppendChild(newnode);
        //            }

        //            if (node.Name == "Forumurl")
        //            {
        //                XmlNode newnode = newdoc.CreateElement(node.Name);
        //                newnode.InnerXml = "forumindex.aspx";
        //                newdoc.DocumentElement.AppendChild(newnode);
        //            }
        //        }
        //        newdoc.Save(ConfigFilePath);
        //    }
        //    else //升级2.0开源之后的版本
        //    {
        //        XmlNodeList xnl = doc.DocumentElement.ChildNodes;
        //        foreach (XmlNode node in xnl)
        //        {
        //            if (node.Name == "Specifytemplate")
        //            {
        //                if (node.InnerXml.ToLower() == "true")
        //                {
        //                    node.InnerXml = "1";
        //                }
        //                else if (node.InnerXml.ToLower() == "false")
        //                {
        //                    node.InnerXml = "0";
        //                }
        //            }
        //        }
        //        doc.Save(ConfigFilePath);
        //    }
        //}

        /// <summary>
        /// 当前配置类的实例
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (GeneralConfigInfo) value; }
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
                    filename = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/general.config");
                }

                return filename;
            }

        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static GeneralConfigInfo LoadConfig()
        {
            try
            {
                IConfigInfo configInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, true);
                //如为空则表示出现异常，则再次尝试
                if (configInfo == null)
                {
                    configInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo, true);
                    if (configInfo == null)
                    {   //还为空，且ConfigInfo同时也为空，为确保程序继续运行，这里重新生成序列化类实例
                        ConfigInfo = (ConfigInfo == null) ? new GeneralConfigInfo() : ConfigInfo;
                        try
                        {
                            SerializationHelper.Save(ConfigInfo, ConfigFilePath);
                        }
                        catch (Exception ex)
                        {
                            //如果序列化也失败则直接抛出异常
                            throw ex;
                        }
                    }
                }
            }
            catch
            {
                ;
            }
            
            return  ConfigInfo as GeneralConfigInfo;
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

