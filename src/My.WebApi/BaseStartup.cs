using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using My.CorrelationIdProvider;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace My.WebApi
{
    public abstract class BaseStartup
    {
        protected abstract Info ApiInfo { get; }

        public virtual IConfiguration Configuration { get; }

        protected BaseStartup(IConfiguration configuration, IHostingEnvironment env = null)
        {
            Configuration = configuration;
            ApiVersion.Version = configuration["ApiInfo:Version"] ?? "v1";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {

            services.AddLogging();
            ConfigureServicesDependencies(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ConfigureServiceSwaggerGen(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<ScopedLoggingMiddleware>();
            app.UseMiddleware<LogRequestMiddleware>();
            app.UseMiddleware<LogResponseMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            ConfigureSwagger(app, env);
            app.UseMvc();
        }

        protected virtual void ConfigureServicesDependencies(IServiceCollection services)
        {
            services.AddSingleton<ICorrelationIdProvider, CorrelationIdProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        protected virtual void ConfigureServiceSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiInfo.Version, ApiInfo);

                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        protected virtual void ConfigureSwagger(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{ApiInfo.Version}/swagger.json", $"{ApiInfo.Title} {ApiInfo.Version}");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
