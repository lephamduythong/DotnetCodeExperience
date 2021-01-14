using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTPSClientByPass
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // HttpClientHandler clientHandler = new HttpClientHandler();
            // clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // using (var client = new HttpClient(clientHandler))
            // {
            //     var result = await client.GetStringAsync("https://192.168.1.9:3443");
            //     Console.WriteLine(result);
            // }


            var client = new HttpClient();
            var result = await client.GetStringAsync("https://192.168.1.9:3443");
            Console.WriteLine(result);
        }
    }
}
