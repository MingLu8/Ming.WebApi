using System;
using Microsoft.Extensions.Configuration;

namespace My.WebApi
{
    public static class ConfigurationExtensions
    {
        public static T GetSettings<T>(this IConfiguration configuration, string sectionName)
        {
            var settings = Activator.CreateInstance<T>();
            configuration.Bind(sectionName, settings);
            return settings;
        }
    }
}
