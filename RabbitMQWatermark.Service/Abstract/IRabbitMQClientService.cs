using RabbitMQ.Client;

namespace RabbitMQWatermark.Service.Abstract
{
   public interface IRabbitMQClientService
   {
      IModel Connect();
   }
}