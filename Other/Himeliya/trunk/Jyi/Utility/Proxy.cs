using System;
using System.Collections.Generic;
using System.Text;
using Jyi.Entity;

namespace Jyi.Utility
{
    class Proxy
    {
        public static ProxyInfo GetNextProxy(List<ProxyInfo> proxyInfoList)
        {
            foreach (ProxyInfo objProxyInfo in proxyInfoList)
            {
                if (!objProxyInfo.HaveGet)
                {
                    objProxyInfo.HaveGet = true;
                    return objProxyInfo;//如果有没有被使用过的代理,就返回当前的代理,并设置其属性为已经使用
                }
            }

            //如果循环完毕都还没有符合条件的代理返回,则返回一个空
            return null;
        }        
    }
}
