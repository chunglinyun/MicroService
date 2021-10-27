
using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory(){HostName = _configuration["RabbitMQHost"],Port = int.Parse(_configuration["RabbitMQPost"])};
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                
                Console.WriteLine("--> Connected To MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connection to the message bus : {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open , sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection Close, not sending");

            }
        }

        private void SendMessage(string msg)
        {
            var body = Encoding.UTF8.GetBytes(msg);
            _channel.BasicPublish(
                exchange: "trigger",
                routingKey:"",
                basicProperties:null,
                body:body);
            Console.WriteLine($"--> We have sent {msg}");
        }

        private void Dispose()
        {
            Console.WriteLine("MessageBug Dispose");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}