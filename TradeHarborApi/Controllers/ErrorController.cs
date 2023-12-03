using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TradeHarborApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly IWebHostEnvironment _environment;

        public ErrorController(ILogger<ErrorController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

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
