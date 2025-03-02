
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.BusinessModels.ChatMessageModels;
using HimariServer.Repository.Entities;
namespace HimariServer.API.Controllers
{
    [Route("api/v1/chat-messages")]
    [ApiController]
    public class ChatMessageController : BaseController
    {
        private readonly IChatMessageService _chatMessageService;

        public ChatMessageController(IChatMessageService ChatMessageService)
        {
            _chatMessageService = ChatMessageService;
        }

        [HttpGet("{userId}")]
        public Task<IActionResult> GetMessageChatByUserIdPaginated(int userId, [FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () =>
                await _chatMessageService.GetMessageChatByUserIdPaginated(userId, paginationParameter));
        }
        [HttpPost]
        public Task<IActionResult> GetProductRecommendationAsync([FromBody] ChatRequestModel chatRequest) {
            return ValidateAndExecute(async () => await _chatMessageService.GetProductRecommendationAsync(chatRequest));
        }
    }
}
