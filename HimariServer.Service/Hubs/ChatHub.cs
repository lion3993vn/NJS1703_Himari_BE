using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.Utils;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HimariServer.Service.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeepseekService _deepseekService;
        private readonly IChromaService _chromaService;
        private readonly IGeminiService _geminiService;

        public ChatHub(IUnitOfWork unitOfWork, IDeepseekService deepseekService, IChromaService chromaService, IGeminiService geminiService)
        {
            _unitOfWork = unitOfWork;
            _deepseekService = deepseekService;
            _chromaService = chromaService;
            _geminiService = geminiService;
        }

        public async Task SendMessage(int userId, string message)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.USER_NOT_EXIST);
            }

            var messageUser = new ChatMessage
            {
                UserId = userId,
                Message = message,
                Type = MessageType.USER
            };
            await _unitOfWork.ChatMessageRepository.AddAsync(messageUser);

            _unitOfWork.Save();

            var fullResponse = new StringBuilder();

            // Use streaming mode của gemini
            await _geminiService.StreamResponseMessage(message, async (partialResponse) =>
            {
                // Send each chunk to the client as it arrives
                await Clients.Caller.SendAsync("ReceiveStreamingMessage", partialResponse, false);

                // Accumulate the full response
                fullResponse.Append(partialResponse);
            });

            // 🔹 Kiểm tra nếu tin nhắn có yêu cầu giới thiệu sản phẩm
            if (await IsProductInquiry(message))
            {
                var listProduct = await _chromaService.QuerySimilarProducts(message);
                listProduct = listProduct.Take(2).ToList();

                if (listProduct.Any())
                {
                    await Clients.Caller.SendAsync("ReceiveProductRecommendations", listProduct);
                }
                else
                {
                    string noProduct = " Xin lỗi, tôi không tìm thấy sản phẩm phù hợp nhu cầu của bạn.";
                    fullResponse.Append(noProduct);
                    await Clients.Caller.SendAsync("ReceiveStreamingMessage", noProduct, false);
                }
            }
            var messageBot = new ChatMessage
            {
                UserId = userId,
                Message = fullResponse.ToString(),
                Type = MessageType.BOT
            };
            await _unitOfWork.ChatMessageRepository.AddAsync(messageBot);
            _unitOfWork.Save();

            await Clients.Caller.SendAsync("ReceiveStreamingMessage", "", true);
        }


        private async Task<bool> IsProductInquiry(string message)
        {
            var response = await _geminiService.IntentMessage(message);

            if (response == "1")
            {
                return true;
            }
            return false;
        }

        public async Task SendMessageWithoutStreaming(int userId, string message)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.USER_NOT_EXIST);
            }

            var messageUser = new ChatMessage
            {
                UserId = userId,
                Message = message,
                Type = MessageType.USER
            };
            await _unitOfWork.ChatMessageRepository.AddAsync(messageUser);

            var messageResponse = await _deepseekService.ResponseMessage(message);

            var messageBot = new ChatMessage
            {
                UserId = userId,
                Message = messageResponse,
                Type = MessageType.BOT
            };
            await _unitOfWork.ChatMessageRepository.AddAsync(messageBot);

            _unitOfWork.Save();

            await Clients.Caller.SendAsync("ReceiveMessage", messageResponse);
        }
    }
}
