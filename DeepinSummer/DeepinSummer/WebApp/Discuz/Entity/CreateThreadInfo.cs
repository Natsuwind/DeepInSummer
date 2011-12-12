using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.WebApp.Discuz.Entity
{
    public class CreateThreadInfo
    {
        public string Subject { get; set; }         
        public string Message { get; set; } 
        public string DoUsername { get; set; }
        public string DoStatus { get; set; }
        public string PdUsername { get; set; }
        public string RdUsername { get; set; }
        public string UedUsername { get; set; }
        public string QaUsername { get; set; }

        public int tid { get; set; }
        public int fid { get; set; }
    }
}
