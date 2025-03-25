namespace WhoisLookupAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.RateLimiting;
    using WhoisLookupAPI.Attributes;

    /// <summary>
    /// Serves as the base controller for all API controllers in the application.
    /// Provides common functionality and configurations for derived controllers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("standard")]
    [WhoisAPIExceptionFilter]
    public class BaseController : Controller
    {
    }
}
