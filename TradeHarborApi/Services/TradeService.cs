using TradeHarborApi.Models;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    public class TradeService
    {
        private readonly TradesRepository _repo;
        private readonly AuthService _authService;

        public TradeService(TradesRepository repo, AuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        public async Task<IEnumerable<TradePost>> GetTrades()
        {
            var tradePosts = await _repo.GetTrades();
            foreach (var post in tradePosts)
            {
                post.Timestamp = DateTime.SpecifyKind(post.Timestamp, DateTimeKind.Utc);
            }

            return tradePosts;
        }

        public async Task CreateTradePost(CreateTradePostRequest request)
        {
            request.Id = _authService.GetUserIdFromJwt();
            request.Timestamp = DateTime.UtcNow;
            var asdf = await _repo.CreateTradePost(request);
        }
    }
}
