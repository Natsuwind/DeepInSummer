
namespace Discuz.Entity
{
    /// <summary>
    /// 用户的QQ互联信息
    /// </summary>
    public class UserConnectInfo
    {
        private string openId = "";

        /// <summary>
        /// 云平台开放的用户唯一ID
        /// </summary>
        public string OpenId
        {
            get { return openId; }
            set { openId = value; }
        }
        private int uid = -1;

        /// <summary>
        /// 用户论坛ID
        /// </summary>
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        private string token = "";

        /// <summary>
        /// 云平台提供的用户授权token
        /// </summary>
        public string Token
        {
            get { return token; }
            set { token = value; }
        }
        private string secret = "";

        /// <summary>
        /// 云平台提供的用户授权secret
        /// </summary>
        public string Secret
        {
            get { return secret; }
            set { secret = value; }
        }
        private int allowVisitQQUserInfo = 0;

        /// <summary>
        /// 用户是否授权论坛访问QQ用户资料
        /// </summary>
        public int AllowVisitQQUserInfo
        {
            get { return allowVisitQQUserInfo; }
            set { allowVisitQQUserInfo = value; }
        }
        private int allowPushFeed = 0;
        
        /// <summary>
        /// 用户是否授权论坛发帖后同步到用户的QQ空间和微博等
        /// </summary>
        public int AllowPushFeed
        {
            get { return allowPushFeed; }
            set { allowPushFeed = value; }
        }

        private int isSetPassword = 0;

        /// <summary>
        /// 是否设置过密码
        /// </summary>
        public int IsSetPassword
        {
            get { return isSetPassword; }
            set { isSetPassword = value; }
        }

        private string callbackInfo = "";

        /// <summary>
        /// 云平台初次callback用户信息
        /// </summary>
        public string CallbackInfo
        {
            get { return callbackInfo; }
            set { callbackInfo = value; }
        }
    }
    /// <summary>
    /// 用户绑定QQ记录
    /// </summary>
    public class UserBindConnectLog
    {
        private string openId;

        public string OpenId
        {
            get { return openId; }
            set { openId = value; }
        }

        private int uid;

        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private int type;
        /// <summary>
        /// 1:绑定中；2:解绑状态
        /// </summary>
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private int bindCount;

        public int BindCount
        {
            get { return bindCount; }
            set { bindCount = value; }
        }
    }
}
