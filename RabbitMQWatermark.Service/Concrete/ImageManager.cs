using Microsoft.AspNetCore.Http;
using RabbitMQWatermark.Service.Abstract;


namespace RabbitMQWatermark.Service.Concrete
{
   public class ImageManager : IImageService
   {
      private readonly IRabbitMQConsumerService _consumerService;

      public ImageManager(IRabbitMQConsumerService consumerService)
      {
         _consumerService = consumerService;
      }

      public async Task<string> CreateImagePath(IFormFile formFile)
      {
         if (formFile is { Length: > 0 })
         {
            var randomImageName = Guid.NewGuid() + Path.GetExtension(formFile.FileName);
            var path = Directory.GetCurrentDirectory() + "/wwwroot/Images";
            var fullPath = Path.Combine(path, randomImageName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
               await formFile.CopyToAsync(stream);

               _consumerService.Publish(new ProductImageCreatedEvent() { ImageName = randomImageName });
            }
            return randomImageName;
         }
         return "";
      }
   }
}