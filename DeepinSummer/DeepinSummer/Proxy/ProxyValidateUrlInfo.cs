using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Proxy
{
    [Serializable]
    public class ProxyValidateUrlInfo
    {
        public string UrlName { get; set; }
        public string Url { get; set; }
        public string Charset { get; set; }
        public string RegexString { get; set; }
    }
}
