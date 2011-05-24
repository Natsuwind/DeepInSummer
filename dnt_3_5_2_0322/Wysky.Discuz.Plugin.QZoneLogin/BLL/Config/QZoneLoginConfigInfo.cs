using System;
using System.Collections.Generic;
using System.Web;
using Discuz.Config;

namespace Wysky.Discuz.Plugin.QZoneLogin.BLL.Config
{
    [Serializable]
    public class QZoneLoginConfigInfo : IConfigInfo
    {
        private int _EnableQQLogin = 0;
        private string _AppId = "";
        private string _AppKey = "";

        public int EnableQQLogin
        {
            get { return _EnableQQLogin; }
            set { _EnableQQLogin = value; }
        }
        public string AppId
        {
            get { return _AppId; }
            set { _AppId = value; }
        }
        public string AppKey
        {
            get { return _AppKey; }
            set { _AppKey = value; }
        }
    }
}