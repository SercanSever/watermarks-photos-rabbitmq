using Microsoft.AspNetCore.Http;

namespace RabbitMQWatermark.Service.Abstract
{
   public interface IImageService
   {
      Task<string> CreateImagePath(IFormFile formFile);
   }
}