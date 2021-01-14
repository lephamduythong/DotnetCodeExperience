using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            bool done = false;
            int listenPort = 55600;
            using (UdpClient listener = new UdpClient(listenPort))
            {
                IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
                while (!done)
                {
                    byte[] receivedData = listener.Receive(ref listenEndPoint);

                    Console.WriteLine("Received broadcast message from client {0}", listenEndPoint.ToString());

                    Console.WriteLine("Decoded data is:");
                    Console.WriteLine(Encoding.ASCII.GetString(receivedData)); //should be "Hello World" sent from above client
                }
            }
        }
    }
}
