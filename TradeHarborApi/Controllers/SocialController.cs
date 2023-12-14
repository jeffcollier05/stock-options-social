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
    /// <summary>
    /// API Controller for social-related actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SocialController : ControllerBase
    {
        private readonly SocialService _socialService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialController"/> class.
        /// </summary>
        /// <param name="socialService">The social service used for handling social-related actions.</param>
        public SocialController(SocialService socialService)
        {
            _socialService = socialService;
        }

        /// <summary>
        /// Retrieves a collection of trade posts.
        /// </summary>
        /// <returns>An asynchronous task that represents the operation. The task result is a collection of <see cref="TradePost"/>.</returns>
        [HttpGet]
        public async Task<IEnumerable<TradePost>> GetTrades()
        {
            var tradePosts = await _socialService.GetTrades();
            return tradePosts;
        }

        /// <summary>
        /// Creates a new trade post based on the provided request.
        /// </summary>
        /// <param name="request">The request containing information for creating a trade post.</param>
        /// <returns>An asynchronous task that represents the operation. The task result is an <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTradePost([FromBody] CreateTradePostRequest request)
        {
            await _socialService.CreateTradePost(request);
            return Ok();
        }

        /// <summary>
        /// Deletes a trade post based on the provided request.
        /// </summary>
        /// <param name="request">The request containing information for deleting a trade post.</param>
        /// <returns>An asynchronous task that represents the operation. The task result is an <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteTradePost([FromBody] DeleteTradePostRequest request)
        {
            await _socialService.DeleteTradePost(request);
            return Ok();
        }

        /// <summary>
        /// Retrieves a collection of user profiles.
        /// </summary>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is a collection of <see cref="UserProfile"/>.
        /// </returns>
        [HttpGet]
        public async Task<IEnumerable<UserProfile>> GetAllUsers()
        {
            var users = await _socialService.GetAllUsers();
            return users;
        }

        /// <summary>
        /// Removes a friend connection between two users based on the provided request.
        /// </summary>
        /// <param name="request">The request containing information for removing a friend connection.</param>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is an <see cref="IActionResult"/> indicating the result of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> RemoveFriend([FromBody] ModifyFriendPairRequest request)
        {
            await _socialService.RemoveFriend(request);
            return Ok();
        }

        /// <summary>
        /// Retrieves a collection of notifications.
        /// </summary>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is a collection of <see cref="Notification"/>.
        /// </returns>
        [HttpGet]
        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            var notifications = await _socialService.GetNotifications();
            return notifications;
        }

        /// <summary>
        /// Deletes a notification based on the provided request.
        /// </summary>
        /// <param name="request">The request containing information for deleting a notification.</param>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is an <see cref="IActionResult"/> indicating the result of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> DeleteNotification(DeleteNotificationRequest request)
        {
            await _socialService.DeleteNotification(request);
            return Ok();
        }

        /// <summary>
        /// Creates a friend request based on the provided request.
        /// </summary>
        /// <param name="request">The request containing information for creating a friend request.</param>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is an <see cref="IActionResult"/> indicating the result of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateFriendRequest(CreateFriendRequestRequest request)
        {
            await _socialService.CreateFriendRequest(request);
            return Ok();
        }

        /// <summary>
        /// Accepts a friend request based on the provided request.
        /// </summary>
        /// <param name="request">The request containing information for accepting a friend request.</param>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is an <see cref="IActionResult"/> indicating the result of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(AcceptFriendRequestRequest request)
        {
            await _socialService.AcceptFriendRequest(request);
            return Ok();
        }

        /// <summary>
        /// Declines a friend request based on the provided request.
        /// </summary>
        /// <param name="request">The request containing information for declining a friend request.</param>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is an <see cref="IActionResult"/> indicating the result of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> DeclineFriendRequest(DeclineFriendRequestRequest request)
        {
            await _socialService.DeclineFriendRequest(request);
            return Ok();
        }

        /// <summary>
        /// Reacts to a post based on the provided request.
        /// </summary>
        /// <param name="request">The request containing information for reacting to a post.</param>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is an <see cref="IActionResult"/> indicating the result of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> ReactToPost(PostReactionRequest request)
        {
            await _socialService.ReactToPost(request);
            return Ok();
        }

        /// <summary>
        /// Comments on a post based on the provided request.
        /// </summary>
        /// <param name="request">The request containing information for commenting on a post.</param>
        /// <returns>
        /// An asynchronous task that represents the operation. 
        /// The task result is an <see cref="IActionResult"/> indicating the result of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CommentOnPost(PostCommentRequest request)
        {
            await _socialService.CommentOnPost(request);
            return Ok();
        }
    }
}