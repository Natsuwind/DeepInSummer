using System;

namespace Natsuhime.Proxy
{
    [Serializable]
    public class ProxySourcePageInfo
    {
        public string Name { get; set; }

        private string _PageUrl;
        public string PageUrl
        {
            get { return _PageUrl; }
            set { _PageUrl = value; }
        }

        private string _RegexString;
        public string RegexString
        {
            get { return _RegexString; }
            set { _RegexString = value; }
        }

        private string _PageCharset;
        public string PageCharset
        {
            get { return _PageCharset; }
            set { _PageCharset = value; }
        }
    }
}
