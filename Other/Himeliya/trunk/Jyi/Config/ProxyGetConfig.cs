using System;
using System.Collections.Generic;
using System.Text;
using Jyi.Entity;
using System.Xml;

namespace Jyi.Config
{
    class ProxyGetConfig
    {
        public static List<ProxyGetInfo> GetProxyGetPageUrlList()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(MainForm.configFilePath);
            XmlNodeList xnlProxyGet = xmlDoc.SelectSingleNode("/JyiConfig/ProxyGet").ChildNodes;

            List<ProxyGetInfo> objProxyGetList = new List<ProxyGetInfo>();
            foreach (XmlNode xnf in xnlProxyGet)
            {
                ProxyGetInfo objProxyGetInfo = new ProxyGetInfo();
                XmlElement xe = (XmlElement)xnf;
                objProxyGetInfo.PageUrl = xnf.InnerText;
                objProxyGetInfo.Regex = xnf.Attributes["Regex"].Value;
                objProxyGetInfo.Charset = xnf.Attributes["Charset"].Value;
                objProxyGetList.Add(objProxyGetInfo);
            }
            return objProxyGetList;
        }
    }
}
