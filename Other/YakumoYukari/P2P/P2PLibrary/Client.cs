using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace P2PLibrary
{
    /// <summary>
    /// �ͻ���ҵ����
    /// </summary>
    public class Client : IDisposable
    {
        //private const int MAX_RETRY_SEND_MSG = 1; //��ʱ���Ӵ���,���������һ�ξ��ܳɹ�

        private UdpClient _client;//�ͻ��˼�����
        private IPEndPoint _hostPoint; //����IP
        private IPEndPoint _remotePoint; //�����κ�Զ�̻���������
        private UserCollection _userList;//�����û��б�
        private Thread _listenThread; //�����߳�
        private string _LocalUserName; //�����û���
        //private bool _HoleAccepted = false; //A->B,���յ�B�û���ȷ����Ϣ

        private WriteLogHandle _OnWriteMessage;
        public WriteLogHandle OnWriteMessage
        {
            get { return _OnWriteMessage; }
            set { _OnWriteMessage = value; }
        }

        private UserChangedHandle _UserChangedHandle = null;
        public UserChangedHandle OnUserChanged
        {
            get { return _UserChangedHandle; }
            set { _UserChangedHandle = value; }
        }

        /// <summary>
        ///��ʾ���ټ�¼ 
        /// </summary>
        /// <param name="log"></param>
        private void DoWriteLog(string log)
        {
            if (_OnWriteMessage != null)
                (_OnWriteMessage.Target as Control).Invoke(_OnWriteMessage, log);
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="serverIP"></param>
        public Client()
        {
            string serverIP = this.GetServerIP();
            _remotePoint = new IPEndPoint(IPAddress.Any, 0); //�κ��뱾�����ӵ��û�IP��ַ��
            _hostPoint = new IPEndPoint(IPAddress.Parse(serverIP), Globals.SERVER_PORT); //��������ַ
            _client = new UdpClient();//��ָ���˿ڣ�ϵͳ�Զ�����
            _userList = new UserCollection();
            _listenThread = new Thread(new ThreadStart(Run));
        }

        /// <summary>
        /// ��ȡ������IP��INI�ļ�������
        /// </summary>
        /// <returns></returns>
        private string GetServerIP()
        {
            string file = Application.StartupPath + "\\ip.ini";
            string ip = File.ReadAllText(file);
            return ip.Trim();
        }

        /// <summary>
        /// �����ͻ���
        /// </summary>
        public void Start()
        {
            if (this._listenThread.ThreadState == ThreadState.Unstarted)
            {
                this._listenThread.Start();
            }
        }

        /// <summary>
        /// �ͻ���¼
        /// </summary>
        public void Login(string userName, string password)
        {
            _LocalUserName = userName;

            // ���͵�¼��Ϣ��������
            C2S_LoginMessage loginMsg = new C2S_LoginMessage(userName, password);
            this.SendMessage(loginMsg, _hostPoint);
        }

        /// <summary>
        /// �ǳ�
        /// </summary>
        public void Logout()
        {
            C2S_LogoutMessage lgoutMsg = new C2S_LogoutMessage(_LocalUserName);
            this.SendMessage(lgoutMsg, _hostPoint);

            this.Dispose();
            System.Environment.Exit(0);
        }

        /// <summary>
        /// ���������ȡ�û��б�
        /// </summary>
        public void DownloadUserList()
        {
            C2S_GetUsersMessage getUserMsg = new C2S_GetUsersMessage(_LocalUserName);
            this.SendMessage(getUserMsg, _hostPoint);
        }

        /// <summary>
        /// ��ʾ�����û�
        /// </summary>
        /// <param name="users"></param>
        private void DisplayUsers(UserCollection users)
        {
            if (_UserChangedHandle != null)
            {
                Control c = (_UserChangedHandle.Target as Control);
                if (c != null)
                {
                    c.Invoke(_UserChangedHandle, users);
                }
                else
                {
                    _UserChangedHandle(users);
                }
            }
        }

        //�����߳�
        private void Run()
        {
            try
            {
                byte[] buffer;//����������
                while (true)
                {
                    buffer = _client.Receive(ref _remotePoint);//_remotePoint�������ص�ǰ���ӵ��û�IP��ַ

                    object msgObj = ObjectSerializer.Deserialize(buffer);
                    Type msgType = msgObj.GetType();
                    DoWriteLog("���յ���Ϣ:" + msgType.ToString() + " From:" + _remotePoint.ToString());

                    if (msgType == typeof(S2C_UserListMessage))
                    {
                        // �����û��б�
                        S2C_UserListMessage usersMsg = (S2C_UserListMessage)msgObj;
                        _userList.Clear();

                        foreach (User user in usersMsg.UserList)
                            _userList.Add(user);

                        this.DisplayUsers(_userList);
                    }
                    else if (msgType == typeof(S2C_UserAction))
                    {
                        //�û����������û���¼/�û��ǳ�
                        S2C_UserAction msgAction = (S2C_UserAction)msgObj;
                        if (msgAction.Action == UserAction.Login)
                        {
                            _userList.Add(msgAction.User);
                            this.DisplayUsers(_userList);
                        }
                        else if (msgAction.Action == UserAction.Logout)
                        {
                            User user = _userList.Find(msgAction.User.UserName);
                            if (user != null) _userList.Remove(user);
                            this.DisplayUsers(_userList);
                        }
                    }
                    else if (msgType == typeof(S2C_HolePunchingMessage))
                    {
                        //���ܵ��������Ĵ�����
                        S2C_HolePunchingMessage msgHolePunching = (S2C_HolePunchingMessage)msgObj;

                        //NAT-B���û���NAT-A���û�������Ϣ,��ʱUDP���϶��ᱻNAT-A������
                        //��ΪNAT-A�ϲ�û��A->NAT-B�ĺϷ�Session, ��������NAT-B�Ͼͽ�������B->NAT-A�ĺϷ�session��!                        
                        P2P_HolePunchingTestMessage msgTest = new P2P_HolePunchingTestMessage(_LocalUserName);
                        this.SendMessage(msgTest, msgHolePunching.RemotePoint);
                    }
                    else if (msgType == typeof(P2P_HolePunchingTestMessage))
                    {
                        //UDP�򶴲�����Ϣ
                        //_HoleAccepted = true;
                        P2P_HolePunchingTestMessage msgTest = (P2P_HolePunchingTestMessage)msgObj;
                        UpdateConnection(msgTest.UserName, _remotePoint);

                        //����ȷ����Ϣ
                        P2P_HolePunchingResponse response = new P2P_HolePunchingResponse(_LocalUserName);
                        this.SendMessage(response, _remotePoint);
                    }
                    else if (msgType == typeof(P2P_HolePunchingResponse))
                    {
                        //_HoleAccepted = true;//�򶴳ɹ�
                        P2P_HolePunchingResponse msg = msgObj as P2P_HolePunchingResponse;
                        UpdateConnection(msg.UserName, _remotePoint);

                    }
                    else if (msgType == typeof(P2P_TalkMessage))
                    {
                        //�û���Ի���Ϣ
                        P2P_TalkMessage workMsg = (P2P_TalkMessage)msgObj;
                        DoWriteLog(workMsg.Message);
                    }
                    else
                    {
                        DoWriteLog("�յ�δ֪��Ϣ!");
                    }
                }
            }
            catch (Exception ex) { DoWriteLog(ex.Message); }
        }

        private void UpdateConnection(string user, IPEndPoint ep)
        {
            User remoteUser = _userList.Find(user);
            if (remoteUser != null)
            {
                remoteUser.NetPoint = ep;//����˴����ӵ�IP���˿�
                remoteUser.IsConnected = true;
                DoWriteLog(string.Format("���Ѿ���{0}����ͨ��ͨ��,IP:{1}!",
                    remoteUser.UserName, remoteUser.NetPoint.ToString()));
                this.DisplayUsers(_userList);
            }
        }

        #region IDisposable ��Ա

        public void Dispose()
        {
            try
            {
                this._listenThread.Abort();
                this._client.Close();
            }
            catch
            {

            }
        }

        #endregion

        public void SendMessage(MessageBase msg, User user)
        {
            this.SendMessage(msg, user.NetPoint);
        }

        public void SendMessage(MessageBase msg, IPEndPoint remoteIP)
        {
            if (msg == null) return;
            DoWriteLog("���ڷ�����Ϣ��->" + remoteIP.ToString() + ",����:" + msg.ToString());
            byte[] buffer = ObjectSerializer.Serialize(msg);
            _client.Send(buffer, buffer.Length, remoteIP);
            DoWriteLog("��Ϣ�ѷ���.");
        }

        /// <summary>
        /// UDP�򶴹���
        /// ����A������B.����A���ʹ���Ϣ��Server,��Server����B���������㽨��ͨ��ͨ��,Server��A��IP��Ϣת����B
        /// B�յ��������A��һ��UDP��,��ʱB��NAT�Ὠ��һ����AͨѶ��Session. Ȼ��A�ٴ���B����UDP��B�����յ���
        /// </summary>
        public void HolePunching(User user)
        {
            //A:�Լ�; B:����user
            //A���ʹ���Ϣ��������,������B��
            C2S_HolePunchingRequestMessage msg = new C2S_HolePunchingRequestMessage(_LocalUserName, user.UserName);
            this.SendMessage(msg, _hostPoint);

            Thread.Sleep(2000);//�ȴ��Է�����UDP��������Session

            //����Է�����ȷ����Ϣ������Է��յ��ᷢ��һ��P2P_HolePunchingResponseȷ����Ϣ����ʱ�򶴳ɹ�
            P2P_HolePunchingTestMessage confirmMessage = new P2P_HolePunchingTestMessage(_LocalUserName);
            this.SendMessage(confirmMessage, user);
        }
    }

}


