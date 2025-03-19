using DeepSeek.Core;
using DeepSeek.Core.Models;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.SettingModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class DeepseekService : IDeepseekService
    {
        private readonly DeepSeekClient _deepseekClient;
        public DeepseekService(IOptions<DeepseekSettings> deepseekSettings)
        {
            _deepseekClient = new DeepSeekClient(deepseekSettings.Value.APIKey);
        }
        public async Task<string> ResponseMessage(string userText)
        {

            var systemMessage = $@"Bạn là trợ lý tư vấn sản phẩm mỹ phẩm có tên là HimaBot phục vụ cho cửa hàng Himari Cosmetics. Bạn chỉ có thể trả lời câu hỏi liên quan đến làm đẹp. Khi khách hàng hỏi các câu không liên quan thì bạn trả lời không biết";

            var request = new ChatRequest
            {
                Messages = [
                    Message.NewSystemMessage(systemMessage),
                    Message.NewUserMessage(userText)
                    ],
                Temperature = 0.3,
            };

            var chatResponse = await _deepseekClient.ChatAsync(request, new CancellationToken());
            return chatResponse?.Choices.First().Message?.Content;
        }
    }
}
