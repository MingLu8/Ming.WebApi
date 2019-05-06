using Microsoft.AspNetCore.Http;
using Ming.CorrelationIdProvider;
using NSubstitute;
using Xunit;

namespace Ming.WebApi.Tests
{
    public class CorrelationIdProviderMiddlewareTests
    {
        [Fact]
        public async void AddCorrelationId_is_call_once()
        {
            var correlationIdProvider = Substitute.For<ICorrelationIdProvider>();

            var middleware = new CorrelationIdMiddleware(next: async (innerHttpContext) => { });

            await middleware.Invoke(new DefaultHttpContext(), correlationIdProvider);
            correlationIdProvider.Received(1).AddCorrelationId();
        }
    }
}
