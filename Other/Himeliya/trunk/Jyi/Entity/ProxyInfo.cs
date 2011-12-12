using System;

namespace Jyi.Entity
{
    class ProxyInfo : Natsuhime.Proxy.ProxyInfo
    {
        private bool m_Checking = false;

        public bool HaveGet
        {
            get { return m_Checking; }
            set { m_Checking = value; }
        }
    }
}
