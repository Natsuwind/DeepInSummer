using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace RelayListenTool
{
    public class UDPRelayListener
    {
        public UDPRelayListener()
        {
            m_evCanExit = new EventWaitHandle(false, EventResetMode.ManualReset);
            m_actListen = Listen;
            m_epDest = new IPEndPoint(IPAddress.None, 0);
            //m_confirm = new SecurityConfirm();
            m_listSR = new List<SocketRelay>();
        }

        // 中继侦听端口
        public int Port { get; set; }

        // 中继目标IP
        public IPAddress DestIP
        {
            get { return m_epDest.Address; }
            set { m_epDest.Address = value; }
        }

        // 中继目标端口
        public int DestPort
        {
            get { return m_epDest.Port; }
            set { m_epDest.Port = value; }
        }

        public void AddAllowIP(string strIP)
        {
            //m_confirm.AddAllowIP(strIP);
        }

        public void Start()
        {
            lock (m_actListen)
            {
                // 已存在一个监听,避免重复开启
                if ((m_arListen != null) && (!m_arListen.IsCompleted)) return;
                m_evCanExit.Reset();
                m_arListen = m_actListen.BeginInvoke(null, null);
            }
        }

        public void Stop()
        {
            lock (m_actListen)
            {
                // 没有活动的监听
                if (m_arListen == null || m_arListen.IsCompleted) return;
                m_evCanExit.Set();
                m_actListen.EndInvoke(m_arListen);
                lock (m_listSR) m_listSR.Clear();
            }
        }

        private void Listen()
        {
            EventWaitHandle evAccept = new EventWaitHandle(false, EventResetMode.ManualReset);

            // IPv4 TCP
            using (Socket socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, Port);
                socketListener.Bind(ep);
                socketListener.Listen(5);

                WaitHandle[] waits = new WaitHandle[]
				{
					m_evCanExit,
					evAccept
				};

                int nWait;	// 等待结果
                do
                {
                    evAccept.Reset();
                    AsyncCallback ac = AcceptCallback;
                    Tuple<Socket, EventWaitHandle> state = new Tuple<Socket, EventWaitHandle>(socketListener, evAccept);
                    socketListener.BeginAccept(ac, state);
                    nWait = WaitHandle.WaitAny(waits);
                }
                while (nWait != 0);

                socketListener.Close();
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket sktActive = null;
            try
            {
                var state = (Tuple<Socket, EventWaitHandle>)ar.AsyncState;
                state.Item2.Set();
                sktActive = state.Item1.EndAccept(ar);
            }
            catch (ObjectDisposedException)
            {
                // Accept has been cancelled.
                return;
            }

            try
            {
                // 安全验证
                //if (!m_confirm.Confirm(sktActive))
                //{
                //	sktActive.Close();
                //	return;
                //}

                Socket sktPassive = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sktPassive.Connect(m_epDest);

                SocketRelay relay = new SocketRelay(sktActive, sktPassive);
                relay.OnFinished += OnSocketRelayFinished;
                relay.Run();
                lock (m_listSR) m_listSR.Add(relay);
            }
            catch (SocketException ex)
            {
                string strbuf = String.Format("{0}({1})", ex.Message, ex.ErrorCode);
                byte[] buf = Encoding.Default.GetBytes(strbuf);
                sktActive.Send(buf);

                sktActive.Shutdown(SocketShutdown.Both);
                sktActive.Close();
            }
        }

        private void OnSocketRelayFinished(SocketRelay item)
        {
            lock (m_listSR)
            {
                if (m_listSR.Contains(item))
                    m_listSR.Remove(item);
            }
        }

        // 侦听Action
        private Action m_actListen;
        // 侦听异步结果
        private IAsyncResult m_arListen;
        // 退出事件
        private EventWaitHandle m_evCanExit;
        // 中继目的
        private IPEndPoint m_epDest;
        // 安全验证
        //private SecurityConfirm m_confirm;
        // 保持对活动连接的引用
        private List<SocketRelay> m_listSR;
    }
}
