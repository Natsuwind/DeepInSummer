using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLibrary
{

    /// <summary>
    /// ��Ϣ����,������
    /// </summary>
    [Serializable]
    public abstract class MessageBase
    {
        //��Ϣ����
    }


    #region �ͻ��˷��͵�����������Ϣ

    /// <summary>
    /// �ͻ��˷��͵�����������Ϣ����
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
    /// �û���¼��Ϣ
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
    /// �û��ǳ���Ϣ
    /// </summary>
    [Serializable]
    public class C2S_LogoutMessage : C2S_MessageBase
    {

        public C2S_LogoutMessage(string userName)
            : base(userName)
        { }
    }

    /// <summary>
    /// �����û��б���Ϣ
    /// </summary>
    [Serializable]
    public class C2S_GetUsersMessage : C2S_MessageBase
    {
        public C2S_GetUsersMessage(string userName)
            : base(userName)
        { }
    }

    /// <summary>
    /// ����Purch Hole��Ϣ
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

    #region ��Ե���Ϣ

    /// <summary>
    /// ��Ե���Ϣ����
    /// </summary>
    [Serializable]
    public abstract class P2P_MessageBase : MessageBase
    {
        //
    }

    /// <summary>
    /// ������Ϣ
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
    /// UDP�򶴲�����Ϣ
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
    /// �յ���Ϣ�Ļظ�ȷ��
    /// ��A��B�뽨��ͨ��ͨ����Щ������B����ȷ�ϴ򶴳ɹ�
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

    #region ���������͵��ͻ�����Ϣ

    /// <summary>
    /// ���������͵��ͻ�����Ϣ����
    /// </summary>
    [Serializable]
    public abstract class S2C_MessageBase : MessageBase
    {
    }

    /// <summary>
    /// �����û��б�Ӧ����Ϣ
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
    /// ת������Purch Hole��Ϣ
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
    /// ������֪ͨ���������û���
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
    /// �û�����
    /// </summary>
    public enum UserAction
    {
        Login,
        Logout
    }
}
