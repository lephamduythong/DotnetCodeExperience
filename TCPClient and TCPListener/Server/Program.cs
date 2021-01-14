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
            TcpListener server = new TcpListener(IPAddress.Parse("192.168.1.9"),listenPort);
            server.Start();
            while (!done) 
            {
                Console.WriteLine("Listening");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                NetworkStream stream = client.GetStream();
                string message = String.Empty;
                int i = -1;
                while ((i = stream.ReadByte()) != -1)
                {
                    message += (char)i;
                }
                System.Console.WriteLine("Result: " + message);
                stream.Close();
            }
        }
    }
}
