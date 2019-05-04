using System;
using Microsoft.AspNetCore.Mvc;

namespace Ming.WebApi
{
    /// <summary>
    /// Specifies an attribute route on a controller where [version] will be replace by version number defined in ApiVersion.Version.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class VersionedRouteAttribute : RouteAttribute
    {
        /// <summary>
        /// Creates a new <see cref="VersionedRouteAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public VersionedRouteAttribute(string template)
            : base(template.Replace("[version]", ApiVersion.Version))
        {

        }
    }
}