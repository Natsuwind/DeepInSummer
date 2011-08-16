using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace UDPClientTest
{
    class Program
    {
        static UdpClient client;
        static void Main(string[] args)
        {

            string[] strRead;
            do
            {
                Console.Write(">");
                strRead = Console.ReadLine().ToLower().Split('|');

                if (strRead[0] == ":rs")
                {
                    int port = int.Parse(strRead[1]);
                    if (port > 0)
                    {
                        client = new UdpClient(port);
                        Thread thread = new Thread(new ThreadStart(Listen));
                        thread.IsBackground = true;
                        thread.Start();

                        Console.WriteLine("Start Listenning...");
                    }
                    else
                    {
                        Console.WriteLine(":rs|[port]");
                    }
                }
                else if (strRead[0] == ":es")
                {
                    client.Close();
                    Console.WriteLine("Listener has stopped.");
                }
                else if (strRead[0] == ":send")
                {
                    if (strRead.Length == 4)
                    {
                        string ip = strRead[1];
                        int port = int.Parse(strRead[2]);
                        string msg = strRead[3];
                        byte[] bytes = Encoding.UTF8.GetBytes(msg);
                        client.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse(ip), port));
                    }
                    else
                    {
                        Console.WriteLine(":send|[ip]|[port]|[msg]");
                    }
                }

            } while (strRead[0] != ":q");
        }

        static void Listen()
        {
            IPEndPoint iep = null;
            while (true)
            {
                string msg = Encoding.UTF8.GetString(client.Receive(ref iep));
                System.Console.WriteLine(msg);
            }
        }

    }
}
