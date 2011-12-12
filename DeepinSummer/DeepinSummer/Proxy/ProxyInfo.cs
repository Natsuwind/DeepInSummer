using System;

namespace Natsuhime.Proxy
{
    public class ProxyInfo
    {
        private string m_Name;
        private string m_Address;
        private int m_Port;
        private int m_SuccessTime;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }
        public int Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }
        public int SuccessCount
        {
            get { return m_SuccessTime; }
            set { m_SuccessTime = value; }
        }
    }
}
