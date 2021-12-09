using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQWatermark.Service.Abstract;

namespace RabbitMQWatermark.Service.Concrete
{
   public class RabbitMQClientService : IDisposable
   {
      public static string ExchangeName = "ImageDirectExchange";
      public static string QueueName = "queue-watermark-image";
      public static string RoutingWaterMark = "watermark-route-image";
      private readonly ConnectionFactory _connectionFactory;
      private IConnection _connection;
      private IModel _channel;
      private readonly ILogger<RabbitMQClientService> _logger;
      public RabbitMQClientService(ConnectionFactory connectionFactory,
                                   ILogger<RabbitMQClientService> logger)
      {
         _connectionFactory = connectionFactory;
         _logger = logger;
      }
      public IModel Connect()
      {
         _connection = _connectionFactory.CreateConnection();
         if (_channel is { IsOpen: true })
         {
            return _channel;
         }
         _channel = _connection.CreateModel();

         _channel.ExchangeDeclare(exchange: ExchangeName, type: "direct", durable: true, autoDelete: false, arguments: null);

         _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

         _channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: RoutingWaterMark, arguments: null);

         _logger.LogInformation("Message Send");

         return _channel;
      }

      public void Dispose()
      {
         _channel?.Close();
         _channel?.Dispose();
         _channel = default;
         _connection?.Close();
         _connection?.Dispose();
         _logger.LogInformation("Lost connection with rabbitmq...");
      }
   }
}