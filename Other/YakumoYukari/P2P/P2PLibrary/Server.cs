using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace P2PLibrary
{
    /// <summary>
    /// ��������ҵ����
    /// </summary>
    public class Server
    {
        private UdpClient _server; //����������Ϣ������
        private UserCollection _userList; //�����û��б�
        private Thread _serverThread;
        private IPEndPoint _remotePoint; //Զ���û������IP��ַ���˿�

        private WriteLogHandle _WriteLogHandle = null;
        private UserChangedHandle _UserChangedHandle = null;

        /// <summary>
        /// ��ʾ������Ϣ
        /// </summary>
        public WriteLogHandle OnWriteLog
        {
            get { return _WriteLogHandle; }
            set { _WriteLogHandle = value; }
        }

        /// <summary>
        /// ���û�����/�ǳ�ʱ�������¼�
        /// </summary>
        public UserChangedHandle OnUserChanged
        {
            get { return _UserChangedHandle; }
            set { _UserChangedHandle = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public Server()
        {
            _userList = new UserCollection();
            _remotePoint = new IPEndPoint(IPAddress.Any, 0);
            _serverThread = new Thread(new ThreadStart(Run));
        }

        /// <summary>
        ///��ʾ���ټ�¼ 
        /// </summary>
        /// <param name="log"></param>
        private void DoWriteLog(string log)
        {
            if (_WriteLogHandle != null)
            {
                System.Windows.Forms.Control c = (_WriteLogHandle.Target as System.Windows.Forms.Control);
                if (c != null)
                {
                    c.Invoke(_WriteLogHandle, log);
                }
                else
                {
                    _WriteLogHandle(log);
                }
            }
        }

        /// <summary>
        /// ˢ���û��б�
        /// </summary>
        /// <param name="list">�û��б�</param>
        private void DoUserChanged(UserCollection list)
        {
            if (_UserChangedHandle != null)
            {
                System.Windows.Forms.Control c = (_UserChangedHandle.Target as Control);
                if (c != null)
                {
                    c.Invoke(_UserChangedHandle, list);
                }
                else
                {
                    _UserChangedHandle(list);
                }
            }
        }

        /// <summary>
        /// ��ʼ�����߳�
        /// </summary>
        public void Start()
        {
            try
            {
                _server = new UdpClient(Globals.SERVER_PORT);
                _serverThread.Start();
                DoWriteLog("Server started,listening port:" + Globals.SERVER_PORT.ToString() + ",wait for client connect...");
            }
            catch (Exception ex)
            {
                DoWriteLog("[ERROR]Start Server Error:" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ֹͣ�߳�
        /// </summary>
        public void Stop()
        {
            DoWriteLog("Stop Server...");
            try
            {
                _serverThread.Abort();
                _server.Close();
                DoWriteLog("Server stopped.");
            }
            catch (Exception ex)
            {
                DoWriteLog("[ERROR]Stop Server Error: " + ex.Message);
                throw ex;
            }
        }

        //�߳�������
        private void Run()
        {
            byte[] msgBuffer = null;

            while (true)
            {
                msgBuffer = _server.Receive(ref _remotePoint); //������Ϣ
                try
                {
                    //����Ϣת��Ϊ����
                    object msgObject = ObjectSerializer.Deserialize(msgBuffer);
                    if (msgObject == null) continue;

                    Type msgType = msgObject.GetType();
                    DoWriteLog("========Recevied:======");
                    DoWriteLog("Message:" + msgType.ToString());
                    DoWriteLog("From:" + _remotePoint.ToString());
                    DoWriteLog("=======================");

                    //���û���¼
                    if (msgType == typeof(C2S_LoginMessage))
                    {
                        C2S_LoginMessage lginMsg = (C2S_LoginMessage)msgObject;
                        DoWriteLog(string.Format("User:'{0}' Logined!", lginMsg.FromUserName));

                        // ����û����б�
                        IPEndPoint userEndPoint = new IPEndPoint(_remotePoint.Address, _remotePoint.Port);
                        User user = new User(lginMsg.FromUserName, userEndPoint);
                        _userList.Add(user);

                        this.DoUserChanged(_userList);

                        //֪ͨ�����ˣ������û���¼
                        S2C_UserAction msgNewUser = new S2C_UserAction(user, UserAction.Login);
                        foreach (User u in _userList)
                        {
                            if (u.UserName == user.UserName) //������Լ����������������û��б�
                                this.SendMessage(new S2C_UserListMessage(_userList), u.NetPoint);
                            else
                                this.SendMessage(msgNewUser, u.NetPoint);
                        }
                    }
                    else if (msgType == typeof(C2S_LogoutMessage))
                    {
                        C2S_LogoutMessage lgoutMsg = (C2S_LogoutMessage)msgObject;
                        DoWriteLog(string.Format("User:'{0}' Logoff!", lgoutMsg.FromUserName));

                        // ���б���ɾ���û�
                        User logoutUser = _userList.Find(lgoutMsg.FromUserName);
                        if (logoutUser != null) _userList.Remove(logoutUser);

                        this.DoUserChanged(_userList);

                        //֪ͨ�����ˣ����û��ǳ�
                        S2C_UserAction msgNewUser = new S2C_UserAction(logoutUser, UserAction.Logout);
                        foreach (User u in _userList)
                            this.SendMessage(msgNewUser, u.NetPoint);
                    }

                    else if (msgType == typeof(C2S_HolePunchingRequestMessage))
                    {
                        //���յ�A��B�򶴵���Ϣ,�������ɿͻ��˷��͸���������
                        C2S_HolePunchingRequestMessage msgHoleReq = (C2S_HolePunchingRequestMessage)msgObject;

                        User userA = _userList.Find(msgHoleReq.FromUserName);
                        User userB = _userList.Find(msgHoleReq.ToUserName);

                        // ���ʹ�(Punching Hole)��Ϣ
                        DoWriteLog(string.Format("User:[{0} IP:{1}] want to create session tunnel with [{2} IP:{3}].",
                          userA.UserName, userA.NetPoint.ToString(),
                          userB.UserName, userB.NetPoint.ToString()));

                        //��Server������Ϣ��B,��A��IP��IP��ַ��Ϣ����B,Ȼ����B����һ��������Ϣ��A.
                        S2C_HolePunchingMessage msgHolePunching = new S2C_HolePunchingMessage(_remotePoint);
                        this.SendMessage(msgHolePunching, userB.NetPoint); //Server->B
                    }
                    else if (msgType == typeof(C2S_GetUsersMessage))
                    {
                        // ���͵�ǰ�û���Ϣ
                        S2C_UserListMessage srvResMsg = new S2C_UserListMessage(_userList);
                        this.SendMessage(srvResMsg, _remotePoint);
                    }
                }
                catch (Exception ex)
                {
                    DoWriteLog(ex.Message);
                }
            }
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public void SendMessage(MessageBase msg, IPEndPoint remoteIP)
        {
            DoWriteLog("====== Sendding Message:======");
            DoWriteLog(msg.ToString());
            if (msg == null) return;
            byte[] buffer = ObjectSerializer.Serialize(msg);
            _server.Send(buffer, buffer.Length, remoteIP);
            DoWriteLog("========== Sendded! ==========");
        }
    }
}


