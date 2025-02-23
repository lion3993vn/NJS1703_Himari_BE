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
    }
}
