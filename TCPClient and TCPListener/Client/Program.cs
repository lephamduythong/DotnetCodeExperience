using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 55600;
            TcpClient client = new TcpClient("192.168.1.9", port);
            System.Console.WriteLine("Connected");
            NetworkStream stream = client.GetStream();
            stream.Write(Encoding.UTF8.GetBytes("Hello wtf"));
            System.Console.WriteLine("Sent the message");
            stream.Close();
            client.Close();
        }
    }
}
