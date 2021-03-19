using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HR_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Kestrel maximum request body size
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        // Handle requests up to 10 MB
                        options.Limits.MaxRequestBodySize = 10485760; // Bytes
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
