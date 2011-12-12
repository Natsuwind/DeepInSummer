using System;
using System.Collections.Generic;
using System.Text;

namespace Jyi.Entity
{
    class BaseConfigInfo
    {
        private List<ProxyInfo> m_ProxyList;
        public List<ProxyInfo> ProxyList
        {
            get { return m_ProxyList; }
            set { m_ProxyList = value; }
        }
    }
}
