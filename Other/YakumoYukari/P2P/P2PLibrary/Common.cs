using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Collections;

namespace P2PLibrary
{
    /// <summary>
    /// ��ʾ������Ϣ���¼�ί��
    /// </summary>
    public delegate void WriteLogHandle(string msg);

    /// <summary>
    /// ˢ�������û����¼�ί��
    /// </summary>    
    public delegate void UserChangedHandle(UserCollection users);

    public class Globals
    {
        /// <summary>
        /// �����������˿�
        /// </summary>
        public const int SERVER_PORT = 21134;

        /// <summary>
        /// ���������˿�
        /// </summary>
        public const int LOCAL_PORT = 19786;
    }

    /// <summary>
    /// User ��ժҪ˵����
    /// </summary>
    [Serializable]
    public class User
    {
        protected string _userName;
        protected IPEndPoint _netPoint;
        protected bool _conntected;

        public User(string UserName, IPEndPoint NetPoint)
        {
            this._userName = UserName;
            this._netPoint = NetPoint;
        }

        public string UserName
        {
            get { return _userName; }
        }

        public IPEndPoint NetPoint
        {
            get { return _netPoint; }
            set { _netPoint = value; }
        }

        public bool IsConnected //�򶴱��
        {
            get { return _conntected; }
            set { _conntected = value; }
        }

        public string FullName { get { return this.ToString(); } }

        public override string ToString()
        {
            return _userName + " - [" + _netPoint.Address.ToString() + ":" + _netPoint.Port.ToString() + "] " + (_conntected ? "Y" : "N");
        }
    }

    /// <summary>
    /// �����û��б�
    /// </summary>
    [Serializable]
    public class UserCollection : CollectionBase
    {
        public void Add(User user)
        {
            InnerList.Add(user);
        }

        public void Remove(User user)
        {
            InnerList.Remove(user);
        }

        public User this[int index]
        {
            get { return (User)InnerList[index]; }
        }

        public User Find(string userName)
        {
            foreach (User user in this)
            {
                if (string.Compare(userName, user.UserName, true) == 0)
                {
                    return user;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// ���л������л�����
    /// </summary>
    public class ObjectSerializer
    {
        public static byte[] Serialize(object obj)
        {
            BinaryFormatter binaryF = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(1024 * 10);
            binaryF.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[(int)ms.Length];
            ms.Read(buffer, 0, buffer.Length);
            ms.Close();
            return buffer;
        }

        public static object Deserialize(byte[] buffer)
        {
            BinaryFormatter binaryF = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length, false);
            object obj = binaryF.Deserialize(ms);
            ms.Close();
            return obj;
        }
    }


}
