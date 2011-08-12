using System;
using System.Collections.Generic;
using System.Text;
using P2PLibrary;

namespace P2PServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server _server;
            _server = new Server();
            _server.OnWriteLog += new WriteLogHandle(server_OnWriteLog);
            _server.OnUserChanged += new UserChangedHandle(OnUserChanged);
            try
            {
                _server.Start();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[ERROR]{0}", ex.Message);
            }
        }

        //刷新用户列表
        private static void OnUserChanged(UserCollection users)
        {
            System.Console.WriteLine("======= Online User Count:{0} ======", users.Count);
            foreach (User u in users)
            {
                System.Console.WriteLine(
                    "User:{0},IP:{1}:{2},IsHoled:{3}",
                    u.UserName, 
                    u.NetPoint.Address, 
                    u.NetPoint.Port, 
                    u.IsConnected
                    );
            }
            System.Console.WriteLine("====================================", users.Count);
        }

        //显示跟踪消息
        public static void server_OnWriteLog(string msg)
        {
            System.Console.WriteLine(msg);
        }
    }
}
