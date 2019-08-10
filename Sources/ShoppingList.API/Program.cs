using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace ShoppingList.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseIISIntegration()
                .ConfigureLogging(logging =>
                {
                    logging.AddAWSProvider();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .UseStartup<Startup>();
    }
}
