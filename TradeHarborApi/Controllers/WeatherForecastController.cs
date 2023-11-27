using Microsoft.AspNetCore.Mvc;

namespace TradeHarborApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public int GetNumber()
        {
            return 4;
        }
    }
}