using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQWatermark.Service.Abstract;
using RabbitMQWatermark.Service.Concrete;
using RabbitMQWatermark.Web.Models.Context;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
   options.UseInMemoryDatabase(databaseName: "ProductDB");
});

builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<IRabbitMQConsumerService, RabbitMQPublisherManager>();
builder.Services.AddSingleton<IImageService, ImageManager>();

builder.Services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });

builder.Services.AddHostedService<ImageWaterMarkProcessBackgroundManager>();

builder.Services.AddControllersWithViews();




var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   app.UseExceptionHandler("/Home/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();