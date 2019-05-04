using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Ming.WebApi.WebTest
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env) : base(configuration, env)
        {
            
        }


    }
}
