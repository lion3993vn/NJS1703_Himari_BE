using DeepSeek.Core;
using DeepSeek.Core.Models;
using GenerativeAI;
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
    public class GeminiService : IGeminiService
    {
        private readonly GenerativeModel _generativeModel;

        private static readonly string _systemMessage = @"# HimaBot - Trợ lý Himari Cosmetics

        ## Vai trò và danh tính
        - **Tên**: HimaBot
        - **Nhiệm vụ**: Trợ lý ảo phục vụ khách hàng tại Himari Cosmetics
        - **Lĩnh vực chuyên môn**: Chỉ các vấn đề liên quan đến làm đẹp và mỹ phẩm

        ## Nguyên tắc tương tác
        1. **Chỉ trả lời** các câu hỏi liên quan đến làm đẹp, mỹ phẩm, có thể giới thiệu bản thân, cách chữa trị các vấn đề làm đẹp, mỹ phẩm và triệu chứng cơ thể
        2. **Từ chối lịch sự** khi được hỏi về các chủ đề không liên quan: ""Xin lỗi, tôi chỉ có thể hỗ trợ các vấn đề về làm đẹp và mỹ phẩm.""
        3. **Trả lời ngắn gọn** tối đa 5 câu

        ## Tư vấn sản phẩm
        - Khi người dùng yêu cầu tư vấn sản phẩm cụ thể, phản hồi: ""Bạn đợi tí nhé, tôi sẽ tìm các sản phẩm phù hợp với yêu cầu của bạn""
        - Chỉ sử dụng câu trả lời này khi người dùng chủ động yêu cầu tư vấn sản phẩm

        => sau đây là câu hỏi của người dùng: ";

        private static readonly string _systemIntentMessage = @"Nếu người dùng cần tư vấn sản phẩm mỹ phẩm, làm đẹp, chữa các triệu chứng cụ thể, hãy trả lời là: 1, còn lại là 0. Không trả lời thêm.

        => sau đây là câu hỏi của người dùng: ";

        public GeminiService(IOptions<GeminiSettings> geminiSettings)
        {
            var googleAi = new GoogleAi(geminiSettings.Value.APIKey);

            _generativeModel = googleAi.CreateGenerativeModel("models/gemini-1.5-flash");
        }

        public async Task StreamResponseMessage(string userText, Func<string, Task> onMessageReceived)
        {

            await foreach (var chunk in _generativeModel.StreamContentAsync(_systemMessage + userText))
            {
                var content = chunk.Text;

                await onMessageReceived(content);
            }
        }

        public async Task<string> IntentMessage(string userText)
        {
            var response = await _generativeModel.GenerateContentAsync(_systemIntentMessage + userText);

            return response.Text;
        }
    }
}
