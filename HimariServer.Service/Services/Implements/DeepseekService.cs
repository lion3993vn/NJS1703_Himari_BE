using DeepSeek.Core;
using DeepSeek.Core.Models;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.SettingModels;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class DeepseekService : IDeepseekService
    {
        private readonly DeepSeekClient _deepseekClient;
        private readonly string _systemMessage = @"Bạn là trợ lý tư vấn sản phẩm mỹ phẩm có tên là HimaBot phục vụ cho cửa hàng Himari Cosmetics. Bạn chỉ có thể trả lời câu hỏi liên quan đến làm đẹp. Khi khách hàng hỏi các câu không liên quan thì bạn trả lời không biết";

        public DeepseekService(IOptions<DeepseekSettings> deepseekSettings)
        {
            _deepseekClient = new DeepSeekClient(deepseekSettings.Value.APIKey);
        }

        public async Task<string> ResponseMessage(string userText)
        {
            var request = new ChatRequest
            {
                Messages = [
                    Message.NewSystemMessage(_systemMessage),
                    Message.NewUserMessage(userText)
                ],
                Temperature = 0.3,
            };

            var chatResponse = await _deepseekClient.ChatAsync(request, new CancellationToken());
            return chatResponse?.Choices.First().Message?.Content;
        }

        public async Task StreamResponseMessage(string userText, Func<string, Task> onMessageReceived)
        {
            var request = new ChatRequest
            {
                Messages = [
                    Message.NewSystemMessage(_systemMessage),
                    Message.NewUserMessage(userText)
                ],
                Temperature = 0.3,
                Stream = true // Enable streaming
            };

            
            // Use the DeepSeekClient's streaming API
            await foreach (var response in _deepseekClient.ChatStreamAsync(request, new CancellationToken()))
            {
                var content = response?.Delta?.Content;
                
                if (!string.IsNullOrEmpty(content))
                {
                    // Send the content chunk to the caller
                    await onMessageReceived(content);
                }
            }
            
            // Removed the duplicated full response send at the end
        }
    }
}
