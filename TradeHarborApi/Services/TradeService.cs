using TradeHarborApi.Models;
using TradeHarborApi.Repositories;

namespace TradeHarborApi.Services
{
    public class TradeService
    {
        private readonly TradesRepository _tradesRepo;
        private readonly AuthService _authService;

        public TradeService(TradesRepository tradesRepo, AuthService authService)
        {
            _tradesRepo = tradesRepo;
            _authService = authService;
        }

        public async Task<IEnumerable<TradePost>> GetTrades()
        {
            var tradePosts = await _tradesRepo.GetTrades();
            foreach (var post in tradePosts)
            {
                post.Timestamp = DateTime.SpecifyKind(post.Timestamp, DateTimeKind.Utc);
            }

            return tradePosts;
        }

        public async Task CreateTradePost(CreateTradePostRequest request)
        {
            request.UserId = _authService.GetUserIdFromJwt();
            request.Timestamp = DateTime.UtcNow;
            var asdf = await _tradesRepo.CreateTradePost(request);
        }

        public async Task<IEnumerable<FriendProfile>> GetFriendsForUser()
        {
            var userId = _authService.GetUserIdFromJwt();
            var friends = await _tradesRepo.GetFriendsForUser(userId);
            return friends;
        }

        public async Task<IEnumerable<FriendProfile>> GetAllUsers()
        {
            var users = await _tradesRepo.GetAllUsers();
            return users;
        }
    }
}
