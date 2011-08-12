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

            RelayListener relay = new RelayListener();
            relay.Port = 89;
            relay.DestIP = IPAddress.Parse("10.0.1.101");
            relay.DestPort = 3389;
            relay.AddAllowIP("127.0.0.1");

            string strRead;
            do
            {
                Console.Write(">");
                strRead = Console.ReadLine();

                if (strRead == "start")
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
