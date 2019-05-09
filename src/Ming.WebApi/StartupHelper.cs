using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using Ming.CorrelationIdProvider;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Ming.WebApi
{
    public static class StartupHelper
    {
        public static Info CreateApiInfo(this IConfiguration configuration, IHostingEnvironment env)
        {
            var info = configuration.GetSettings<Info>("ApiInfo") ?? new Info();
            info.Title = info.Title ?? env?.ApplicationName;
            info.Version = info.Version ?? ApiVersion.Version;
            return info;
        }

        public static void UseTraceLoggerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<ScopedLoggingMiddleware>();
            app.UseMiddleware<LogRequestMiddleware>();
            app.UseMiddleware<LogResponseMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static void UseSwagger(this IApplicationBuilder app, Info info)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{info.Version}/swagger.json", $"{info.Title} {info.Version}");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
