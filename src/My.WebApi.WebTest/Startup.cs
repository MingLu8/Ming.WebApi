using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using My.WebApi;
using Swashbuckle.AspNetCore.Swagger;

namespace My.WebApi.WebTest
{
    public class Startup : BaseStartup
    {
        protected override Info ApiInfo => new Info
        {
            Version = ApiVersion.Version,
            Title = "ToDo API",
            Description = "A simple example ASP.NET Core Web API",
            TermsOfService = "None",
            //Contact = new Contact
            //{
            //    Name = "Shayne Boyer",
            //    Email = string.Empty,
            //    Url = "https://twitter.com/spboyer"
            //},
            //License = new License
            //{
            //    Name = "Use under LICX",
            //    Url = "https://example.com/license"
            //}
        };

        public Startup(IConfiguration configuration) : base(configuration)
        {
        }


    }
}
