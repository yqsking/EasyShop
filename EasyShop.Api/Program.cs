using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EasyShop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                  .ConfigureLogging(logger =>
                  {
                      logger.AddFilter("System", LogLevel.Warning);//¹ýÂËµôÃüÃû¿Õ¼ä
                      logger.AddFilter("Microsoft", LogLevel.Warning);
                      logger.AddLog4Net("log4net.config");
                  })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
