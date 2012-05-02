using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Web.Services.DiscuzCloud
{
    public class CommandParameter
    {
        private string _method = "";

        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }

        private string _cloudParams = "";

        public string CloudParams
        {
            get { return _cloudParams; }
            set { _cloudParams = value; }
        }

        public CommandParameter(string method, string cloudParams)
        {
            _method = method;
            _cloudParams = cloudParams;
        }
    }
}
