using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmiForum.Models.Entity
{
    public class PostInfo
    {
        public int Pid { get; set; }
        public string Poster { get; set; }
        public int PosterId { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string Ip { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}