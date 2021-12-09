using System.Text;
using System.Text.Json;
using RabbitMQWatermark.Service.Abstract;

namespace RabbitMQWatermark.Service.Concrete
{
   public class RabbitMQPublisherManager : IRabbitMQConsumerService
   {
      private readonly RabbitMQClientService _clientService;

      public RabbitMQPublisherManager(RabbitMQClientService clientService)
      {
         _clientService = clientService;
      }

      public void Publish(ProductImageCreatedEvent productImageCreatedEvent)
      {
         var channel = _clientService.Connect();

         var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);

         var bodyByte = Encoding.UTF8.GetBytes(bodyString);

         var property = channel.CreateBasicProperties();

         property.Persistent = true;

         channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingWaterMark,
         mandatory: false,
         basicProperties: property,
         body: bodyByte);
      }

   }
}