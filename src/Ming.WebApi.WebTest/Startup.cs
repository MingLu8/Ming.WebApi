using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ming.CorrelationIdProvider;
using Swashbuckle.AspNetCore.Swagger;

namespace Ming.WebApi.WebTest
{
    public class Startup
    {
        private Info _info;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _info = configuration.CreateApiInfo(env);
            ApiVersion.Version = _info.Version;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICorrelationIdProvider, CorrelationProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_info.Version, _info);
                var xmlFile = $"{typeof(Startup).Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseTraceLoggerMiddleware();
            app.UseHttpsRedirection();
            app.UseSwagger(_info);
            app.UseMvc();
        }
    }
}
