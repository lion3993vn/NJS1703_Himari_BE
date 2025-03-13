using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Service.BusinessModels.NotificationModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
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

        [HttpPost("push")]
        public Task<IActionResult> PushMessageByUserId(NotificationRequestModel model)
        {
            return ValidateAndExecute(async () => await _notificationService.PushNotificationByUserId(model));
        }

        [HttpGet("user/{userId}")]
        public Task<IActionResult> GetNotificationsByUserId([FromQuery] PaginationParameter paginationParameter, int userId, [FromQuery] int type)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.GetNotificationsByUserId(paginationParameter, userId, type));
        }

        [HttpGet("unread/count")]
        public Task<IActionResult> GetUnreadNotificationCount([FromQuery] int userId)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.GetUnreadNotificationCount(userId));
        }

        [HttpPut("{notificationId}/mark-as-read")]
        public Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.MarkNotificationAsRead(notificationId));
        }

        [HttpPut("notifications/mark-all-as-read")]
        public Task<IActionResult> MarkAllNotificationsAsRead([FromBody] MarkAllAsReadRequest request)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.MarkAllNotificationsAsRead(request.UserId));
        }
        [HttpGet("system")]
        public Task<IActionResult> GetSystemNotifications(
            [FromQuery] PaginationParameter paginationParameter,
            [FromQuery] string keyword = null,
            [FromQuery] bool newestFirst = true)
        {
            return ValidateAndExecute(async () =>
                await _notificationService.GetSystemNotifications(paginationParameter, keyword, newestFirst));
        }
    }
}
