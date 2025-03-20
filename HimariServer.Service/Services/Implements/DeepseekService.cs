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
        private readonly string _systemMessage = @"Bạn là trợ lý có tên là HimaBot phục vụ cho cửa hàng Himari Cosmetics. Bạn chỉ có thể trả lời câu hỏi liên quan đến làm đẹp. Khi khách hàng hỏi các câu không liên quan thì bạn trả lời không biết. Trả lời ngắn gọn. Nếu người dùng muốn tứ vấn sản phẩm cụ thể thì trả lời là : 'Bạn đợi tí nhé, tôi sẽ tìm các sản phẩm phù hợp với yêu cầu của bạn'";
        private readonly string _systemFormatMessage = @"Nhiệm vụ của bạn là format lại câu hỏi của người dùng theo chuẩn như sau: 'Sản phẩm tên <sản phẩm> có mô tả như sau <mô tả> thuộc thương hiệu <brand> có thể chữa trị các triệu chứng <cái người dùng cần điều trị - để dùng mục đích gì> thuộc <bộ phận cơ thể>' . Không hỏi hoặc trả lời thêm, chỉ trả lời theo format đưa ra thôi, nếu người dùng không có đưa đủ thông tin thì bạn suy luận ra, nhớ bỏ dấu < và > trong câu trả lời, chỉnh sửa lại câu nếu sai chính tả";

        public DeepseekService(IOptions<DeepseekSettings> deepseekSettings)
        {
            _deepseekClient = new DeepSeekClient(deepseekSettings.Value.APIKey);
        }

        public async Task<string> FormatMessageUser(string userText)
        {
            var request = new ChatRequest
            {
                Messages = [
                    Message.NewSystemMessage(_systemFormatMessage),
                    Message.NewUserMessage(userText)
                    ],
            };

            var chatResponse = await _deepseekClient.ChatAsync(request, new CancellationToken());
            return chatResponse?.Choices.First().Message?.Content;
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
        }
    }
}
