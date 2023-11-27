using Microsoft.AspNetCore.Mvc;
using TradeHarborApi.Services;

namespace TradeHarborApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TradesController : ControllerBase
    {
        private readonly TradeService _tradeService;

        public TradesController(TradeService tradeService)
        {
            _tradeService = tradeService;
        }


        [HttpGet]
        public async Task GetTrades()
        {
            await _tradeService.GetTrades();
        }
    }
}