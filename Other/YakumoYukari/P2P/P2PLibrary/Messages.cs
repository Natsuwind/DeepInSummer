using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLibrary
{

    /// <summary>
    /// 消息基类,抽象类
    /// </summary>
    [Serializable]
    public abstract class MessageBase
    {
        //消息基类
    }


    #region 客户端发送到服务器的消息

    /// <summary>
    /// 客户端发送到服务器的消息基类
    /// </summary>
    [Serializable]
    public abstract class C2S_MessageBase : MessageBase
    {
        private string _fromUserName;

        protected C2S_MessageBase(string fromUserName)
        {
            _fromUserName = fromUserName;
        }

        public string FromUserName
        {
            get { return _fromUserName; }
        }
    }

    /// <summary>
    /// 用户登录消息
    /// </summary>
    [Serializable]
    public class C2S_LoginMessage : C2S_MessageBase
    {
        private string _password;

        public C2S_LoginMessage(string userName, string password)
            : base(userName)
        {
            this._password = password;
        }

        public string Password
        {
            get { return _password; }
        }
    }

    /// <summary>
    /// 用户登出消息
    /// </summary>
    [Serializable]
    public class C2S_LogoutMessage : C2S_MessageBase
    {

        public C2S_LogoutMessage(string userName)
            : base(userName)
        { }
    }

    /// <summary>
    /// 请求用户列表消息
    /// </summary>
    [Serializable]
    public class C2S_GetUsersMessage : C2S_MessageBase
    {
        public C2S_GetUsersMessage(string userName)
            : base(userName)
        { }
    }

    /// <summary>
    /// 请求Purch Hole消息
    /// </summary>
    [Serializable]
    public class C2S_HolePunchingRequestMessage : C2S_MessageBase
    {
        protected string toUserName;

        public C2S_HolePunchingRequestMessage(string fromUserName, string toUserName)
            : base(fromUserName)
        {
            this.toUserName = toUserName;
        }

        public string ToUserName
        {
            get { return this.toUserName; }
        }
    }

    #endregion

    #region 点对点消息

    /// <summary>
    /// 点对点消息基类
    /// </summary>
    [Serializable]
    public abstract class P2P_MessageBase : MessageBase
    {
        //
    }

    /// <summary>
    /// 聊天消息
    /// </summary>
    [Serializable]
    public class P2P_TalkMessage : P2P_MessageBase
    {
        private string message;

        public P2P_TalkMessage(string msg)
        {
            message = msg;
        }

        public string Message
        {
            get { return message; }
        }
    }

    /// <summary>
    /// UDP打洞测试消息
    /// </summary>
    [Serializable]
    public class P2P_HolePunchingTestMessage : P2P_MessageBase
    {
        private string _UserName;

        public P2P_HolePunchingTestMessage(string userName)
        {
            _UserName = userName;
        }

        public string UserName
        {
            get { return _UserName; }
        }
    }

    /// <summary>
    /// 收到消息的回复确认
    /// 如A与B想建立通话通道，些命令由B发出确认打洞成功
    /// </summary>
    [Serializable]
    public class P2P_HolePunchingResponse : P2P_MessageBase
    {
        private string _UserName;

        public P2P_HolePunchingResponse(string userName)
        {
            _UserName = userName;
        }

        public string UserName
        {
            get { return _UserName; }
        }
    }

    #endregion

    #region 服务器发送到客户端消息

    /// <summary>
    /// 服务器发送到客户端消息基类
    /// </summary>
    [Serializable]
    public abstract class S2C_MessageBase : MessageBase
    {
    }

    /// <summary>
    /// 请求用户列表应答消息
    /// </summary>
    [Serializable]
    public class S2C_UserListMessage : S2C_MessageBase
    {
        private UserCollection userList;

        public S2C_UserListMessage(UserCollection users)
        {
            this.userList = users;
        }

        public UserCollection UserList
        {
            get { return userList; }
        }
    }

    /// <summary>
    /// 转发请求Purch Hole消息
    /// </summary>
    [Serializable]
    public class S2C_HolePunchingMessage : S2C_MessageBase
    {
        protected System.Net.IPEndPoint _remotePoint;

        public S2C_HolePunchingMessage(System.Net.IPEndPoint point)
        {
            this._remotePoint = point;
        }

        public System.Net.IPEndPoint RemotePoint
        {
            get { return _remotePoint; }
        }
    }

    /// <summary>
    /// 服务器通知所有在线用户，
    /// </summary>
    [Serializable]
    public class S2C_UserAction : S2C_MessageBase
    {
        protected User _User;
        protected UserAction _Action;

        public S2C_UserAction(User user, UserAction action)
        {
            _User = user;
            _Action = action;
        }

        public User User
        {
            get { return _User; }
        }

        public UserAction Action
        {
            get { return _Action; }
        }
    }

    #endregion

    /// <summary>
    /// 用户动作
    /// </summary>
    public enum UserAction
    {
        Login,
        Logout
    }
}
