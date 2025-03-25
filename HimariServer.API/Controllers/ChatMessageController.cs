using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HimariServer.Repository.Commons;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "1,3,4")]
        [HttpGet("{userId}")]
        public Task<IActionResult> GetMessageChatByUserIdPaginated(int userId, [FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () =>
                await _chatMessageService.GetMessageChatByUserIdPaginated(userId, paginationParameter));
        }
    }
}
