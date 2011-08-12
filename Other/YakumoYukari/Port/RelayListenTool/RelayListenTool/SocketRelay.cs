using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace RelayListenTool
{
    class SocketRelay
    {
        public SocketRelay(Socket act, Socket pass)
        {
            m_sktActive = act;
            m_sktPassive = pass;
        }

        public void Run()
        {
            // 把从Passive收到的,发送给Active
            RecvSend(m_sktPassive, m_sktActive);
            // 把从Active收到的,发送给Passive
            RecvSend(m_sktActive, m_sktPassive);
        }

        public Action<SocketRelay> OnFinished;

        private void RecvSend(Socket sktRecv, Socket sktSend)
        {
            RecvPack pack = new RecvPack();
            pack.sktRecv = sktRecv;
            pack.sktSend = sktSend;
            AsyncCallback ac = RecvCallback;
            sktRecv.BeginReceive(pack.buf, 0, pack.buf.Length, SocketFlags.None, ac, pack);
        }

        private void RecvCallback(IAsyncResult ar)
        {
            try
            {
                // 1.接收
                RecvPack pack = (RecvPack)ar.AsyncState;
                int nRecv = pack.sktRecv.EndReceive(ar);
                if (nRecv > 0)
                {
                    // 2.复制
                    RecvPack packSend = new RecvPack();
                    packSend.sktSend = pack.sktSend;
                    Array.Copy(pack.buf, packSend.buf, nRecv);
                    // 3.发送
                    AsyncCallback ac = SendCallback;
                    pack.sktSend.BeginSend(packSend.buf, 0, nRecv, SocketFlags.None, ac, packSend);
                    // 4.再接收
                    AsyncCallback acRecv = RecvCallback;
                    pack.sktRecv.BeginReceive(pack.buf, 0, pack.buf.Length, SocketFlags.None, acRecv, pack);
                }
                else
                {
                    // 5.关闭连接
                    pack.sktSend.Shutdown(SocketShutdown.Both);
                    pack.sktSend.Close();
                    pack.sktRecv.Shutdown(SocketShutdown.Both);
                    pack.sktRecv.Close();
                    if (OnFinished != null) OnFinished(this);
                }
            }
            catch (SocketException)
            {
                // Cancelled 对端关闭了连接
                if (OnFinished != null) OnFinished(this);
            }
            catch (ObjectDisposedException)
            {
                // Cancelled
                if (OnFinished != null) OnFinished(this);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                RecvPack pack = (RecvPack)ar.AsyncState;
                int nSent = pack.sktSend.EndSend(ar);
            }
            catch (SocketException)
            {
                // Cancelled 对端关闭了连接
                if (OnFinished != null) OnFinished(this);
            }
            catch (ObjectDisposedException)
            {
                // Cancelled
                if (OnFinished != null) OnFinished(this);
            }
        }

        // 主动端,即主动发起连接的一端
        private Socket m_sktActive;
        // 被动端
        private Socket m_sktPassive;
    }

    class RecvPack
    {
        public RecvPack()
        {
            buf = new byte[1024 * 128];	// 128k
        }

        public Socket sktRecv;
        public Socket sktSend;
        public byte[] buf;
    }
}
