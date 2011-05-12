using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmiForum.Models.Entity
{
    public class ShortUserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// 用户名（昵称）
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        public string Email { get; set; }
        public string RegIp { get; set; }
        public DateTime RegDate { get; set; }
        public string LastLoginIp { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string Salt { get; set; }
        public string SecQues { get; set; }
        public string QqOpenId { get; set; }
    }
}