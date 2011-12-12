using System;
using System.Collections.Generic;
using System.Text;
using Jyi.Entity;
using System.Xml;
using System.IO;

namespace Jyi.Config
{
    class BaseConfig
    {
        public static BaseConfigInfo GetBaseConfig()
        {
            BaseConfigInfo objBaseConfig = new BaseConfigInfo();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(MainForm.configFilePath);
            XmlNode xnBaseConfig = xmlDoc.SelectSingleNode("/JyiConfig");
            #region Proxy
            List<Natsuhime.Proxy.ProxyInfo> list = Natsuhime.Proxy.ProxyUtility.GetProxyList_FromConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProxyList.dat"));
            List<ProxyInfo> objProxyList = new List<ProxyInfo>();
            foreach (Natsuhime.Proxy.ProxyInfo info in list)
            {
                ProxyInfo pi = new ProxyInfo();
                pi.Name = info.Name;
                pi.Address = info.Address;
                pi.Port = info.Port;
                objProxyList.Add(pi);
            }
            //XmlNodeList xnlProxys = xnBaseConfig.SelectSingleNode("Proxy").ChildNodes;

            //List<ProxyInfo> objProxyList = new List<ProxyInfo>();
            //foreach (XmlNode xnf in xnlProxys)
            //{
            //    ProxyInfo objProxy = new ProxyInfo();
            //    XmlElement xe = (XmlElement)xnf;
            //    objProxy.Name = xnf.InnerText;
            //    objProxy.Address = xnf.Attributes["Address"].Value;
            //    objProxy.Port = int.Parse(xnf.Attributes["Port"].Value);
            //    objProxyList.Add(objProxy);
            //}
            objBaseConfig.ProxyList = objProxyList;
            #endregion

            return objBaseConfig;
        }
    }
}
