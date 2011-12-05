using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace RelayListenTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start : start test");
            Console.WriteLine("stop : stop test");
            Console.WriteLine("quit : quit this program");

            var relay;

            string strRead;
            do
            {
                Console.Write(">");
                strRead = Console.ReadLine();

                if (strRead == "udp")
                {
                    relay = new UDPRelayListener();
                    relay.Port = 89;
                    relay.DestIP = IPAddress.Parse("10.0.1.229");
                    relay.DestPort = 7788;
                    relay.AddAllowIP("127.0.0.1");
                    Console.WriteLine("init UDP");
                }
                else if (strRead == "tcp")
                {
                    relay = new RelayListener();
                    relay.Port = 89;
                    relay.DestIP = IPAddress.Parse("10.0.1.229");
                    relay.DestPort = 7788;
                    relay.AddAllowIP("127.0.0.1");
                    Console.WriteLine("init TCP");
                }
                else if (strRead == "start")
                {
                    relay.Start();
                    Console.WriteLine("Start at port {0} -> {1}:{2}", relay.Port, relay.DestIP, relay.DestPort);
                }
                else if (strRead == "stop")
                {
                    relay.Stop();
                    Console.WriteLine("Listener has stopped.");
                }

            } while (strRead != "quit");

        }
    }
}
