using System;
using System.Collections.Generic;
using System.Text;

namespace LiteCMS.Entity
{
    public class UserInfo
    {
        public int Uid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Groupid { get; set; }
        public int Adminid { get; set; }
        public string Qq { get; set; }
        public string Email { get; set; }
        public string Secquestion { get; set; }
        public string Secanswer { get; set; }
        public string Msn { get; set; }
        public string Hi { get; set; }
        public string Nickname { get; set; }
        public string Realname { get; set; }
        public string Bdday { get; set; }
        public string Regip { get; set; }
        public string Regdate { get; set; }
        public string Lastlogip { get; set; }
        public string Lastlogdate { get; set; }
        public int Del { get; set; }
        public int Articlecount { get; set; }
        public int Topiccount { get; set; }
        public int Replycount { get; set; }
    }
}
