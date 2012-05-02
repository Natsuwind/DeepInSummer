using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// 论坛云平台设置描述类
    /// </summary>
    [Serializable]
    public class DiscuzCloudConfigInfo : IConfigInfo
    {
        private int m_cloudenabled = 0;//是否开启Discuz云平台
        private string m_cloudsiteid = "";//云平台站点ID
        private string m_cloudsitekey = "";//云平台站点KEY
        private string m_sitekey = "";//本地站点key

        private int m_connectenabled = 0;//是否开启QQConnect
        private string m_connectappid = "";//QQConnect Appid
        private string m_connectappkey = "";//QQConnect Appkey
        private int m_allowconnectregister = 1;//是否允许QQConnect注册新用户
        private int m_allowuseqzavater = 1;//是否允许论坛抓取用户的Qzone头像
        private int m_maxuserbindcount = 0;//QQ用户最多可以注册的论坛帐号个数,0为不限制



        /// <summary>
        /// 云平台是否开启
        /// </summary>
        public int Cloudenabled
        {
            get { return m_cloudenabled; }
            set { m_cloudenabled = value; }
        }

        /// <summary>
        /// 云平台站点ID
        /// </summary>
        public string Cloudsiteid
        {
            get { return m_cloudsiteid; }
            set { m_cloudsiteid = value; }
        }

        /// <summary>
        /// 云平台站点key
        /// </summary>
        public string Cloudsitekey
        {
            get { return m_cloudsitekey; }
            set { m_cloudsitekey = value; }
        }

        /// <summary>
        /// 站点key
        /// </summary>
        public string Sitekey
        {
            get
            {
                if (m_sitekey == "")
                {
                    string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    Random rnd = new Random();
                    for (int i = 1; i <= 16; i++)
                    {
                        m_sitekey += chars.Substring(rnd.Next(chars.Length), 1);
                    }
                }
                return m_sitekey;
            }
            set { m_sitekey = value; }
        }

        /// <summary>
        /// 是否开启QQConnect
        /// </summary>
        public int Connectenabled
        {
            get { return m_connectenabled; }
            set { m_connectenabled = value; }
        }

        /// <summary>
        /// QQConnect Appid
        /// </summary>
        public string Connectappid
        {
            get { return m_connectappid; }
            set { m_connectappid = value; }
        }

        /// <summary>
        /// QQConnect Appkey
        /// </summary>
        public string Connectappkey
        {
            get { return m_connectappkey; }
            set { m_connectappkey = value; }
        }

        /// <summary>
        /// 是否允许QQConnect注册新用户
        /// </summary>
        public int Allowconnectregister
        {
            get { return m_allowconnectregister; }
            set { m_allowconnectregister = value; }
        }

        /// <summary>
        /// 是否允许论坛抓取用户Qzone头像
        /// </summary>
        public int Allowuseqzavater
        {
            get { return m_allowuseqzavater; }
            set { m_allowuseqzavater = value; }
        }

        /// <summary>
        /// QQ用户最多可以注册的论坛帐号个数,0为不限制
        /// </summary>
        public int Maxuserbindcount
        {
            get { return m_maxuserbindcount; }
            set { m_maxuserbindcount = value; }
        }

        public static DiscuzCloudConfigInfo CreateInstance()
        {
            return new DiscuzCloudConfigInfo();
        }
    }
}
