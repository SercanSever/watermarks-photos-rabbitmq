using RabbitMQWatermark.Service.Concrete;

namespace RabbitMQWatermark.Service.Abstract
{
   public interface IRabbitMQConsumerService
   {
      void Publish(ProductImageCreatedEvent productImageCreatedEvent);
   }
}