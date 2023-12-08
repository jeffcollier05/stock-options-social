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
            var userId = _authService.GetUserIdFromJwt();
            var tradePosts = await _tradesRepo.GetTrades(userId);
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
            await _tradesRepo.CreateTradePost(request);
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

        public async Task AddFriend(ModifyFriendPairRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.AddFriend(request.FriendUserId, userId);
        }

        public async Task RemoveFriend(ModifyFriendPairRequest request)
        {
            var userId = _authService.GetUserIdFromJwt();
            await _tradesRepo.RemoveFriend(request.FriendUserId, userId);
        }
    }
}
