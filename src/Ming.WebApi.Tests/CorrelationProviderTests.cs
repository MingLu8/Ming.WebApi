using System;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Ming.WebApi.Tests
{
    public class CorrelationProviderTests
    {
        [Fact]
        public void GetCorrelation_return_correlation_from_request_header()
        {
            var configuration = Substitute.For<IConfiguration>();

            var correlationId = Guid.NewGuid().ToString("D");
            configuration[CorrelationProvider.CorrelationIdConfigKey].Returns(CorrelationProvider.DefaultCorrelationIdHeaderKey);

            var groupId = Guid.NewGuid().ToString("D");
            configuration[CorrelationProvider.CorrelationIdConfigKey].Returns(CorrelationProvider.DefaultCorrelationIdHeaderKey);

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext());
            httpContextAccessor.HttpContext.Request.Headers[CorrelationProvider.DefaultCorrelationIdHeaderKey] = correlationId;

            var correlationIdProvider = new CorrelationProvider(configuration, httpContextAccessor);
            correlationIdProvider.GetCorrelationId().Should().Be(correlationId);
        }

        [Fact]
        public void AddCorrelationId_with_correlationId_set_does_change_correlationId()
        {
            var correlationId = Guid.NewGuid().ToString("D");
            var configuration = Substitute.For<IConfiguration>();
            configuration[CorrelationProvider.CorrelationIdConfigKey].Returns(CorrelationProvider.DefaultCorrelationIdHeaderKey);

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext());
            httpContextAccessor.HttpContext.Request.Headers[CorrelationProvider.DefaultCorrelationIdHeaderKey] = correlationId;

            var correlationIdProvider = new CorrelationProvider(configuration, httpContextAccessor);
            correlationIdProvider.AddCorrelationId();

            httpContextAccessor.HttpContext.Request.Headers[CorrelationProvider.DefaultCorrelationIdHeaderKey].Should().BeEquivalentTo(correlationId);
        }

        [Fact]
        public void AddCorrelationId_without_existing_correlationId_set_correlationId()
        {
            var correlationId = Guid.NewGuid().ToString("D");
            var configuration = Substitute.For<IConfiguration>();
            configuration[CorrelationProvider.CorrelationIdConfigKey].Returns(CorrelationProvider.DefaultCorrelationIdHeaderKey);

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext());

            var correlationIdProvider = new CorrelationProvider(configuration, httpContextAccessor);
            correlationIdProvider.AddCorrelationId();

            httpContextAccessor.HttpContext.Request.Headers[CorrelationProvider.DefaultCorrelationIdHeaderKey].Should().NotBeNullOrEmpty();
        }
    }
}