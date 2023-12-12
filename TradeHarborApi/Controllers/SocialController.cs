using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeHarborApi.Models;
using TradeHarborApi.Models.Notification;
using TradeHarborApi.Models.Post;
using TradeHarborApi.Models.PostFeatures;
using TradeHarborApi.Services;

namespace TradeHarborApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SocialController : ControllerBase
    {
        private readonly TradeService _tradeService;

        public SocialController(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet]
        public async Task<IEnumerable<TradePost>> GetTrades()
        {
            var tradePosts = await _tradeService.GetTrades();
            return tradePosts;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTradePost([FromBody] CreateTradePostRequest request)
        {
            await _tradeService.CreateTradePost(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTradePost([FromBody] DeleteTradePostRequest request)
        {
            await _tradeService.DeleteTradePost(request);
            return Ok();
        }

        // good up to here, keep checking all request to make sure [Required] is on
        [HttpGet]
        public async Task<IEnumerable<UserProfile>> GetAllUsers()
        {
            var users = await _tradeService.GetAllUsers();
            return users;
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriend([FromBody] ModifyFriendPairRequest request)
        {
            await _tradeService.RemoveFriend(request);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            var notifications = await _tradeService.GetNotifications();
            return notifications;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNotification(DeleteNotificationRequest request)
        {
            await _tradeService.DeleteNotification(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendRequest(CreateFriendRequestRequest request)
        {
            await _tradeService.CreateFriendRequest(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(AcceptFriendRequestRequest request)
        {
            await _tradeService.AcceptFriendRequest(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeclineFriendRequest(DeclineFriendRequestRequest request)
        {
            await _tradeService.DeclineFriendRequest(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ReactToPost(PostReactionRequest request)
        {
            await _tradeService.ReactToPost(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CommentOnPost(PostCommentRequest request)
        {
            await _tradeService.CommentOnPost(request);
            return Ok();
        }
    }
}