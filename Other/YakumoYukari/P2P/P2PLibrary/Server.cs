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
    /// 服务器端业务类
    /// </summary>
    public class Server
    {
        private UdpClient _server; //服务器端消息监听器
        private UserCollection _userList; //在线用户列表
        private Thread _serverThread;
        private IPEndPoint _remotePoint; //远程用户请求的IP地址及端口

        private WriteLogHandle _WriteLogHandle = null;
        private UserChangedHandle _UserChangedHandle = null;

        /// <summary>
        /// 显示跟踪消息
        /// </summary>
        public WriteLogHandle OnWriteLog
        {
            get { return _WriteLogHandle; }
            set { _WriteLogHandle = value; }
        }

        /// <summary>
        /// 当用户登入/登出时触发此事件
        /// </summary>
        public UserChangedHandle OnUserChanged
        {
            get { return _UserChangedHandle; }
            set { _UserChangedHandle = value; }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public Server()
        {
            _userList = new UserCollection();
            _remotePoint = new IPEndPoint(IPAddress.Any, 0);
            _serverThread = new Thread(new ThreadStart(Run));
        }

        /// <summary>
        ///显示跟踪记录 
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
        /// 刷新用户列表
        /// </summary>
        /// <param name="list">用户列表</param>
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
        /// 开始启动线程
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
        /// 停止线程
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

        //线程主方法
        private void Run()
        {
            byte[] msgBuffer = null;

            while (true)
            {
                msgBuffer = _server.Receive(ref _remotePoint); //接受消息
                try
                {
                    //将消息转换为对象
                    object msgObject = ObjectSerializer.Deserialize(msgBuffer);
                    if (msgObject == null) continue;

                    Type msgType = msgObject.GetType();
                    DoWriteLog("========Recevied:======");
                    DoWriteLog("Message:" + msgType.ToString());
                    DoWriteLog("From:" + _remotePoint.ToString());
                    DoWriteLog("=======================");

                    //新用户登录
                    if (msgType == typeof(C2S_LoginMessage))
                    {
                        C2S_LoginMessage lginMsg = (C2S_LoginMessage)msgObject;
                        DoWriteLog(string.Format("User:'{0}' Logined!", lginMsg.FromUserName));

                        // 添加用户到列表
                        IPEndPoint userEndPoint = new IPEndPoint(_remotePoint.Address, _remotePoint.Port);
                        User user = new User(lginMsg.FromUserName, userEndPoint);
                        _userList.Add(user);

                        this.DoUserChanged(_userList);

                        //通知所有人，有新用户登录
                        S2C_UserAction msgNewUser = new S2C_UserAction(user, UserAction.Login);
                        foreach (User u in _userList)
                        {
                            if (u.UserName == user.UserName) //如果是自己，发送所有在线用户列表
                                this.SendMessage(new S2C_UserListMessage(_userList), u.NetPoint);
                            else
                                this.SendMessage(msgNewUser, u.NetPoint);
                        }
                    }
                    else if (msgType == typeof(C2S_LogoutMessage))
                    {
                        C2S_LogoutMessage lgoutMsg = (C2S_LogoutMessage)msgObject;
                        DoWriteLog(string.Format("User:'{0}' Logoff!", lgoutMsg.FromUserName));

                        // 从列表中删除用户
                        User logoutUser = _userList.Find(lgoutMsg.FromUserName);
                        if (logoutUser != null) _userList.Remove(logoutUser);

                        this.DoUserChanged(_userList);

                        //通知所有人，有用户登出
                        S2C_UserAction msgNewUser = new S2C_UserAction(logoutUser, UserAction.Logout);
                        foreach (User u in _userList)
                            this.SendMessage(msgNewUser, u.NetPoint);
                    }

                    else if (msgType == typeof(C2S_HolePunchingRequestMessage))
                    {
                        //接收到A给B打洞的消息,打洞请求，由客户端发送给服务器端
                        C2S_HolePunchingRequestMessage msgHoleReq = (C2S_HolePunchingRequestMessage)msgObject;

                        User userA = _userList.Find(msgHoleReq.FromUserName);
                        User userB = _userList.Find(msgHoleReq.ToUserName);

                        // 发送打洞(Punching Hole)消息
                        DoWriteLog(string.Format("User:[{0} IP:{1}] want to create session tunnel with [{2} IP:{3}].",
                          userA.UserName, userA.NetPoint.ToString(),
                          userB.UserName, userB.NetPoint.ToString()));

                        //由Server发送消息给B,将A的IP的IP地址信息告诉B,然后由B发送一个测试消息给A.
                        S2C_HolePunchingMessage msgHolePunching = new S2C_HolePunchingMessage(_remotePoint);
                        this.SendMessage(msgHolePunching, userB.NetPoint); //Server->B
                    }
                    else if (msgType == typeof(C2S_GetUsersMessage))
                    {
                        // 发送当前用户信息
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
        /// 发送消息
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


