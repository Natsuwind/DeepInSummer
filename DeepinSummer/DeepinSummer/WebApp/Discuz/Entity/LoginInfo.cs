using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.WebApp.Discuz.Entity
{
    public class LoginInfo
    {
        public string LoginUrl { get; set; }
        public string LoginName { get; set; }
        public LoginNameType LoginType { get; set; }
        public string Password { get; set; }
        public string VCode { get; set; }
        public string Questionid { get; set; }
        public string Answer { get; set; }
    }
}
