using Microsoft.AspNetCore.Mvc;
using Ming.CorrelationIdProvider;

namespace Ming.WebApi.WebTest.Controllers
{
    [VersionedRoute("api/[version]/[controller]")]
    [ApiController]
    public class CorrelationIdController : ControllerBase
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public CorrelationIdController(ICorrelationIdProvider correlationIdProvider)
        {
            _correlationIdProvider = correlationIdProvider;
        }

        [HttpGet("/getCorrelationIdKey")]
        public ActionResult<string> CorrelationIdKey()
        {
            return $"correlationIdKey:{_correlationIdProvider.CorrelationIdKey}";
        }

        // GET api/values
        [HttpGet("/withCorrelationIdHeader")]
        public ActionResult<string> WithCorrelationId([FromHeader(Name = CorrelationProvider.DefaultCorrelationIdHeaderKey)] string correlationId)
        {
            return $"correlationId:{_correlationIdProvider.GetCorrelationId()}";
        }

        [HttpGet("/withoutCorrelationIdHeader")]
        public ActionResult<string> WithoutCorrelationId()
        {
            return $"correlationId:{_correlationIdProvider.GetCorrelationId()}";
        }
    }
}
