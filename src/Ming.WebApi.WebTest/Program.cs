using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ming.WebApi.WebTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ILogger logger = null;
            try
            {
                var host = CreateWebHostBuilder(args).Build();
                logger = host.Services.GetRequiredService<ILogger<Program>>();
                logger?.LogInformation("Service starting...");
                host.Run();
                logger?.LogInformation("Service ended.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger?.LogCritical(ex, "Service terminated unexpectedly.");
            }
            finally
            {
                Thread.Sleep(10000);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: false)
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    configLogging.AddConsole((options) =>
                    {
                        options.IncludeScopes = Convert.ToBoolean(hostContext.Configuration["Logging:IncludeScopes"]);
                    });

                    //configLogging.AddApplicationInsights(hostContext.Configuration["Logging:ApplicationInsights:Instrumentationkey"]);
                })
                .UseStartup<Startup>();
    }
}
