using System;
using System.Collections.Generic;
using System.Text;

namespace LiteCMS.Entity
{
    public class AdminInfo
    {
        public int Adminid { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Uid { get; set; }
        public string Allowip { get; set; }
        public string Lastlogindate { get; set; }
        public string Lastloginip { get; set; }
    }
}
