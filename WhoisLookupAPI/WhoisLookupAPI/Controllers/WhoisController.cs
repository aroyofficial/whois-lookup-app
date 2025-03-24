namespace WhoisLookupAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.RateLimiting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using WhoisLookupAPI.Models.Request;
    using WhoisLookupAPI.Models.Response;
    using WhoisLookupAPI.Services.Interfaces;

    /// <summary>
    /// Controller for handling Whois API requests.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WhoisController : BaseController
    {
        private readonly IWhoisService _whoisService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisController"/> class.
        /// </summary>
        /// <param name="whoisService">Service responsible for processing Whois requests.</param>
        /// <param name="logger">Logger instance for logging API activity.</param>
        public WhoisController(IWhoisService whoisService)
        {
            _whoisService = whoisService ?? throw new ArgumentNullException(nameof(whoisService));
        }

        /// <summary>
        /// Retrieves Whois information based on the given request.
        /// </summary>
        /// <param name="request">The Whois request containing domain details.</param>
        /// <returns>An <see cref="WhoisRecord"/> object containing the Whois information.</returns>
        /// <response code="200">Returns the Whois information successfully.</response>
        /// <response code="400">Bad request if the request is invalid.</response>
        /// <response code="500">Internal server error if something goes wrong.</response>
        [HttpPost("lookup")]
        [ProducesResponseType(typeof(WhoisRecord), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [EnableRateLimiting("fixed")] // Optional: Enables rate limiting if configured
        public async Task<ActionResult<WhoisRecord>> GetWhoisInfo([FromBody] WhoisRequest request)
        {
            WhoisRecord whoisResponse = await _whoisService.GetWhoisInfo(request);
            return whoisResponse;
        }
    }
}
