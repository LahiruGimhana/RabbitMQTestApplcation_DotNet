using Microsoft.Extensions.Configuration;
using RabbitMQSender;
using System;

namespace RabbitMQApp
{
    class Program
    {
        static void Main(string[] args)
        {

            IConfiguration configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();

            var hostName = configuration.GetSection("RabbitMQ:HostName").Value;
            string queueName = configuration.GetSection("RabbitMQ:queueName").Value;


            if (hostName == null)
            {
                Console.WriteLine("RabbitMQ HostName is missing or empty in configuration.");
            }
            else
            {
                var receiver = new Receiver(hostName);

                Console.WriteLine("Receiving messages...");
                receiver.StartReceiving(queueName);
                Console.WriteLine("Message receive successfully...");
            }
        }
    }
}
