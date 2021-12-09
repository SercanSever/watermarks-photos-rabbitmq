using Microsoft.AspNetCore.Mvc;
using RabbitMQWatermark.Service.Abstract;
using RabbitMQWatermark.Web.Models;
using RabbitMQWatermark.Web.Models.Context;

namespace RabbitMQWatermark.Web.Controllers
{
   public class ProductController : Controller
   {
      private readonly AppDbContext _context;
      private readonly IImageService _imageService;

      public ProductController(AppDbContext context,IImageService imageService)
      {
         _context = context;
         _imageService = imageService;
      }

      public IActionResult Index()
      {
         var products = _context.Products.ToList();
         return View(products);
      }
      public IActionResult CreateProduct()
      {
         return View();
      }
      [HttpPost]
      public async Task<IActionResult> CreateProduct(Product product, IFormFile imageFile)
      {
         // if (!ModelState.IsValid) return View(product);

         var imageName = await _imageService.CreateImagePath(imageFile);

         product.ImageName = imageName;
         _context.Add(product);
         await _context.SaveChangesAsync();

         return RedirectToAction("Index");
      }
      public IActionResult UpdateProduct(int id)
      {
         return View();
      }
      [HttpPost]
      public IActionResult UpdateProduct(Product product) { return View(); }
      public IActionResult RemoveProduct() { return View(); }
   }
}