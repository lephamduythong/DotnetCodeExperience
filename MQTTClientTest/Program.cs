using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace MQTTClientTest
{
    class Program
    {
        static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();

        static async void SetupMQTTConnectionAsync()
        {
            // Certificate based authentication
            List<X509Certificate2> certs = new List<X509Certificate2>
            {
                new X509Certificate2("server.pfx", "eNTimeNthEnD")
            };

            // Options, use secure TCP connection and username + password
            var options = new MqttClientOptionsBuilder()
                .WithClientId("Client_Netcore")
                .WithTcpServer("192.168.1.10", 8883)
                .WithTls(new MqttClientOptionsBuilderTlsParameters
                {
                    UseTls = true,
                    Certificates = certs
                })
                .WithCredentials("otheruser", "clgt12345")
                .Build();

            // When disconnected
            mqttClient.UseDisconnectedHandler(async e =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await mqttClient.ConnectAsync(options, CancellationToken.None);
                }
                catch
                {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            });

            // When connected
            mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to topics
                var sub1 = mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("esp/test").Build());
                var sub2 = mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("place/home/kitchen/sink").Build());
                
                await Task.WhenAll(sub1, sub2);
                Console.WriteLine("### SUBSCRIBED ###");
                
                // Publish test
                PublishTestAsync();
            });

            // When received message
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();

                // Task.Run(() => mqttClient.PublishAsync("esp/test"));
            });

            // Start connection
            await mqttClient.ConnectAsync(options);
        }

        static async void PublishTestAsync()
        {
            while (!mqttClient.IsConnected)
            {
                Thread.Sleep(100);
                continue;
            }

            while (true)
            {
                Console.WriteLine("Publish some dummies");

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("esp/test")
                    .WithPayload("YOLO YOLO")
                    // .WithAtMostOnceQoS() // QoS 0
                    // .WithAtLeastOnceQoS() // QoS 1
                    .WithExactlyOnceQoS() // QoS 2
                    // .WithRetainFlag()
                    .Build();

                var result = await mqttClient.PublishAsync(message, CancellationToken.None); // Since 3.0.5 with CancellationToken
                if (!string.IsNullOrEmpty(result.ReasonString))
                {
                    Console.WriteLine("Error: " + result.ReasonString);
                }
                
                Thread.Sleep(3000);
            }
        }

        static void Main(string[] args)
        {
            SetupMQTTConnectionAsync();
            Console.ReadLine();
        }
    }
}
