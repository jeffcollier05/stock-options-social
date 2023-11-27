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

        public async Task GetTrades()
        {
            var asdf = await _repo.GetTrades();
        }
    }
}
