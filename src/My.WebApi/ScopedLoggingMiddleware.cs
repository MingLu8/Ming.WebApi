using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using My.CorrelationIdProvider;

namespace My.WebApi
{
    public class ScopedLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ScopedLoggingMiddleware> _logger;

        public ScopedLoggingMiddleware(RequestDelegate next, ILogger<ScopedLoggingMiddleware> logger)
        {
            this._next = next ?? throw new System.ArgumentNullException(nameof(next));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context, ICorrelationIdProvider correlationIdProvider)
        {
            if (context == null) throw new System.ArgumentNullException(nameof(context));

            try
            {
                var loggerState = new LoggerState
                {
                    [correlationIdProvider.CorrelationIdKey] = correlationIdProvider.GetCorrelationId()
                    //Add any number of properties to be logged under a single scope
                };

                using (_logger.BeginScope(loggerState))
                {
                    await _next(context);
                }
            }
            //To make sure that we don't loose the scope in case of an unexpected error
            catch (Exception ex) when (LogOnUnexpectedError(ex))
            {
            }
        }

        private bool LogOnUnexpectedError(Exception ex)
        {
            _logger.LogError(ex, "An unexpected exception occured!");
            return true;
        }
    }

    internal class LoggerState : Dictionary<string, object>
    {
        const string seperator = " => ";
        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var item in this)
                builder.Append($"{item.Key}:{item.Value}{seperator}");

            return builder.ToString().Replace(seperator, string.Empty);
        }
    }
}