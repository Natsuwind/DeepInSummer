using System;

namespace Jyi.Entity
{
    class ProxyGetInfo
    {
        private string m_PageUrl;
        public string PageUrl
        {
            get { return m_PageUrl; }
            set { m_PageUrl = value; }
        }

        private string m_Regex;
        public string Regex
        {
            get { return m_Regex; }
            set { m_Regex = value; }
        }

        private string m_Charset;
        public string Charset
        {
            get { return m_Charset; }
            set { m_Charset = value; }
        }
    }
}
