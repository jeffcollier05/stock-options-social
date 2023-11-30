using TradeHarborApi.Models;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    public class TradeService
    {
        private readonly TradesRepository _repo;

        public TradeService(TradesRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TradePost>> GetTrades()
        {
            var tradePosts = await _repo.GetTrades();
            return tradePosts;
        }
    }
}
