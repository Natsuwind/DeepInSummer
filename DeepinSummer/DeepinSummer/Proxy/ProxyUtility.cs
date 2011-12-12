using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Natsuhime.Proxy
{
    public class ProxyUtility
    {
        public static string GetCurrentIP_RegexPage(string pagesource, string regexString)
        {
            return RegexUtility.GetMatch(pagesource, regexString);
        }
        public static List<ProxyInfo> GetProxyList_FromConfig(string configFilePath)
        {
            string json = File.ReadAllText(configFilePath, new UTF8Encoding(true, true));
            if (json.Trim() != string.Empty)
            {
                return (List<ProxyInfo>)JavaScriptConvert.DeserializeObject(json, typeof(List<ProxyInfo>));
            }
            return null;
        }
        public static void SaveProxyList_ToConfig(List<ProxyInfo> list, string configFilePath)
        {
            string json = JavaScriptConvert.SerializeObject(list);
            File.WriteAllText(configFilePath, json, new UTF8Encoding(true, true));
        }
    }
}
