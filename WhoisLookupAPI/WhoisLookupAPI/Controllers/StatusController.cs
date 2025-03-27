namespace WhoisLookupAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;

    /// <summary>
    /// Controller for checking the status of the API.
    /// </summary>
    public class StatusController : BaseController
    {
        /// <summary>
        /// Simple status check to verify if the API is running.
        /// </summary>
        /// <returns>Returns API status and timestamp.</returns>
        [HttpGet]
        public IActionResult GetStatus()
        {
            var response = new
            {
                status = "API is running",
                timestamp = DateTime.UtcNow
            };

            return Ok(response);
        }
    }
}
