using Microsoft.Extensions.Configuration;
using RabbitMQSender;
using System;
using System.IO;
using System.Text.Json;

namespace RabbitMQApp
{
    class Program
    {
        static IConfigurationRoot configuration;
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var hostName = configuration.GetSection("RabbitMQ:HostName").Value;

            string queueName = GetQueueName();

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

                    var messageObject = new MessageFormat
                    {
                        Type = "Text",
                        Content = message,
                        From = "User1",
                        To="",
                        Status=""
                    };

                    string serializedMessage = JsonSerializer.Serialize(messageObject);

                    Console.WriteLine("Sending message...");
                    sender.SendMessage(queueName, serializedMessage);
                    Console.WriteLine("Message sent successfully...");


                    Console.Write("Do you want to change the queue name? (yes/no): ");
                    string changeQueueName = Console.ReadLine().ToLower();

                    if (changeQueueName == "yes")
                    {
                        queueName = GetQueueName();
                    }
                }
            }
        
        }

        static string GetQueueName()
        {

            Console.Write("Enter the queue name: ");
            string queueName = Console.ReadLine();


            if (string.IsNullOrEmpty(queueName))
            {
                 queueName = configuration.GetSection("RabbitMQ:queueName").Value;

            }

            return queueName;
        }

        public class MessageFormat
        {
            public string Type { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string Status { get; set; }
            public string Content { get; set; }
        }

    }
}
