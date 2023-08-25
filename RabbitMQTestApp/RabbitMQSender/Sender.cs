using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;


namespace RabbitMQSender
{
    public class Sender
    {
        private string hostName;

        public Sender(string hostName)
        {
            this.hostName = hostName;
        }

        public void SendMessage(string queueName, string message)
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

                var body = System.Text.Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
