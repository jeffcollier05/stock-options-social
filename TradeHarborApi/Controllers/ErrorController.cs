using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TradeHarborApi.Controllers
{
    /// <summary>
    /// Controller for handling and logging errors in the API.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="logger">The logger for logging errors.</param>
        /// <param name="environment">The hosting environment.</param>
        public ErrorController(ILogger<ErrorController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Handles errors and returns an appropriate response.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the response.</returns>
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;
            var errorMessage = exception?.Message;

            _logger.LogError(errorMessage);
            var responseMessage = _environment.IsDevelopment() ? errorMessage : "There was an internal server error.";
            return StatusCode(500, responseMessage);
        }
    }
}
