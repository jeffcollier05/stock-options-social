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
        private readonly SocialService _socialService;

        public SocialController(SocialService socialService)
        {
            _socialService = socialService;
        }

        [HttpGet]
        public async Task<IEnumerable<TradePost>> GetTrades()
        {
            var tradePosts = await _socialService.GetTrades();
            return tradePosts;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTradePost([FromBody] CreateTradePostRequest request)
        {
            await _socialService.CreateTradePost(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTradePost([FromBody] DeleteTradePostRequest request)
        {
            await _socialService.DeleteTradePost(request);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<UserProfile>> GetAllUsers()
        {
            var users = await _socialService.GetAllUsers();
            return users;
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriend([FromBody] ModifyFriendPairRequest request)
        {
            await _socialService.RemoveFriend(request);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            var notifications = await _socialService.GetNotifications();
            return notifications;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNotification(DeleteNotificationRequest request)
        {
            await _socialService.DeleteNotification(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendRequest(CreateFriendRequestRequest request)
        {
            await _socialService.CreateFriendRequest(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(AcceptFriendRequestRequest request)
        {
            await _socialService.AcceptFriendRequest(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeclineFriendRequest(DeclineFriendRequestRequest request)
        {
            await _socialService.DeclineFriendRequest(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ReactToPost(PostReactionRequest request)
        {
            await _socialService.ReactToPost(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CommentOnPost(PostCommentRequest request)
        {
            await _socialService.CommentOnPost(request);
            return Ok();
        }
    }
}