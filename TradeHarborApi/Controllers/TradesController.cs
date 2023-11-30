using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TradeHarborApi.Models;
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
        public async Task<IEnumerable<TradePost>> GetTrades()
        {
            var tradePosts = await _tradeService.GetTrades();
            return tradePosts;
        }
    }
}