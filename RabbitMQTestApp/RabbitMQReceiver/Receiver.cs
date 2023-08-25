using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQSender
{
    public class Receiver
    {
        private string hostName;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        public Receiver(string hostName)
        {
            this.hostName = hostName;
            this.factory = new ConnectionFactory() { HostName = hostName };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }

        public void StartReceiving(string queueName)
        {
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    
                    var receivedMessage = System.Text.Encoding.UTF8.GetString(body);

                    var receivedObject = JsonSerializer.Deserialize<MessageFormat>(receivedMessage);

                    Console.WriteLine($"Message Received, From : {receivedObject.From}, Type : {receivedObject.Type}, Content : {receivedObject.Content}");
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
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
