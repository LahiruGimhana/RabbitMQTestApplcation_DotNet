using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;


namespace RabbitMQSender
{
    public class Sender
    {
        private string hostName;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        public Sender(string hostName)
        {
            this.hostName = hostName;
            this.factory = new ConnectionFactory() { HostName = hostName };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }

        public void SendMessage(string queueName, string message)
        {
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
