using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQReceiver
{
    public class Receiver
    {
        private string hostName;

        public Receiver(string hostName)
        {
            this.hostName = hostName;
        }
        public void StartReceiving(string queueName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
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
                    Console.WriteLine($"Received: {receivedMessage}");
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
