using Microsoft.Extensions.Configuration;
using RabbitMQSender;
using System;
using System.IO;

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

            if (hostName == null )
            {
                Console.WriteLine("RabbitMQ HostName is missing or empty in configuration.");
            }
            else
            {
                var sender = new Sender(hostName);

                while (true)
                {
                    Console.Write("Enter the message (or type 'exit' to quit): ");
                    string message = Console.ReadLine();

                    if (message.ToLower() == "exit")
                    {
                        break; // Exit the loop if the user types 'exit'
                    }

                    Console.WriteLine("Sending message...");
                    sender.SendMessage(queueName, message);
                    Console.WriteLine("Message sent successfully...");
                }
            }
        }
    }
}
