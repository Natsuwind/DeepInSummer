using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 邮箱验证注册请求类
    /// </summary>
    public class VerifyRegisterInfo
    {
        private int _regId = 0;
        /// <summary>
        /// 请求信息ID(数据主键)
        /// </summary>
        public int RegId
        {
            get { return _regId; }
            set { _regId = value; }
        }

        private string _ip = "";
        /// <summary>
        /// 请求注册用户的IP
        /// </summary>
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private string _email = "";
        /// <summary>
        /// 请求注册用户Email
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _createTime = "";
        /// <summary>
        /// 请求生成时间
        /// </summary>
        public string CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        private string _expireTime = "";
        /// <summary>
        /// 请求失效时间
        /// </summary>
        public string ExpireTime
        {
            get { return _expireTime; }
            set { _expireTime = value; }
        }

        private string _inviteCode = "";
        /// <summary>
        /// 请求注册时使用的邀请码(没有则留空)
        /// </summary>
        public string InviteCode
        {
            get { return _inviteCode.Trim(); }
            set { _inviteCode = value; }
        }

        private string _verifyCode = "";

        /// <summary>
        /// 自动生成的安全序列码,以链接参数形式发送到用户邮箱中
        /// </summary>
        public string VerifyCode
        {
            get { return _verifyCode; }
            set { _verifyCode = value; }
        }

    }
}
