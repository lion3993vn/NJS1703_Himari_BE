using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Service.BusinessModels.NotificationModels;
using HimariServer.Service.BusinessModels.UserModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [Authorize(Roles = "3,4")]
        [HttpPost("push")]
        public Task<IActionResult> PushMessage(NotificationRequestModel model)
        {
            return ValidateAndExecute(async () => await _notificationService.PushNotification(model));
        }
        [Authorize(Roles = "3,4")]
        [HttpPost("push/{userId}")]
        public Task<IActionResult> PushMessageByUserId(int userId, NotificationRequestModel model)
        {
            return ValidateAndExecute(async () => await _notificationService.PushNotificationByUserId(userId, model));
        }
        [Authorize(Roles = "1,3,4")]
        [HttpGet("user/{userId}")]
        public Task<IActionResult> GetNotificationsByUserId([FromQuery] PaginationParameter paginationParameter, int userId, [FromQuery] int type)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.GetNotificationsByUserId(paginationParameter, userId, type));
        }
        [Authorize(Roles = "1,3,4")]
        [HttpGet("unread/count")]
        public Task<IActionResult> GetUnreadNotificationCount([FromQuery] int userId)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.GetUnreadNotificationCount(userId));
        }
        [Authorize(Roles = "1,3,4")]
        [HttpPut("{notificationId}/mark-as-read")]
        public Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.MarkNotificationAsRead(notificationId));
        }
        [Authorize(Roles = "1,3,4")]
        [HttpPut("notifications/mark-all-as-read")]
        public Task<IActionResult> MarkAllNotificationsAsRead([FromBody] MarkAllAsReadRequest request)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.MarkAllNotificationsAsRead(request.UserId));
        }
        [Authorize(Roles = "1,3,4")]
        [HttpGet("system")]
        public Task<IActionResult> GetSystemNotifications(
            [FromQuery] PaginationParameter paginationParameter,
            [FromQuery] bool newestFirst = true, 
            [FromQuery] string? searchTerm = null)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.GetSystemNotifications(paginationParameter, newestFirst, searchTerm));
        }
    }
}
