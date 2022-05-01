using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using System.Security.Authentication;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Initialize Logger
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

            try
            {
                Log.Information("Application is starting.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Uses Serilog instead of default .NET Logger
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        // Kestrel maximum request body size
                        // Handle requests up to 10 MB
                        options.Limits.MaxRequestBodySize = 10485760; // Bytes

                        options.Limits.MinRequestBodyDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
                        options.Limits.MinResponseDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
                        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                        options.ConfigureHttpsDefaults(listenOptions =>
                        {
                            listenOptions.SslProtocols = SslProtocols.Tls12;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
