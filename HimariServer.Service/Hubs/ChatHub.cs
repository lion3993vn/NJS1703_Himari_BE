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
        private readonly IChatMessageService _chatMessageService;
        private readonly IDeepseekService _deepseekService;
        private readonly IChromaService _chromaService;

        public ChatHub(IUnitOfWork unitOfWork, IChatMessageService chatMessageService, IDeepseekService deepseekService, IChromaService chromaService)
        {
            _unitOfWork = unitOfWork;
            _chatMessageService = chatMessageService;
            _deepseekService = deepseekService;
            _chromaService = chromaService;
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

            // Create a placeholder for the bot's response
            var messageBot = new ChatMessage
            {
                UserId = userId,
                Message = "", // Will be updated with the full response at the end
                Type = MessageType.BOT
            };
            await _unitOfWork.ChatMessageRepository.AddAsync(messageBot);
            _unitOfWork.Save();

            var fullResponse = new StringBuilder();

            // Use streaming mode
            await _deepseekService.StreamResponseMessage(message, async (partialResponse) =>
            {
                // Send each chunk to the client as it arrives
                await Clients.Caller.SendAsync("ReceiveStreamingMessage", partialResponse, false);

                // Accumulate the full response
                fullResponse.Append(partialResponse);
            });

            // 🔹 Kiểm tra nếu tin nhắn có yêu cầu giới thiệu sản phẩm
            if (IsProductInquiry(message))
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

            messageBot.Message = fullResponse.ToString();
            _unitOfWork.ChatMessageRepository.UpdateAsync(messageBot);
            _unitOfWork.Save();

            await Clients.Caller.SendAsync("ReceiveStreamingMessage", "", true);
        }


        private bool IsProductInquiry(string message)
        {
            string normalizedMessage = StringUtils.ConvertToUnSign(message.ToLower());

            string[] productKeywords = {
               "san pham", "gia", "mua", "tu van", "dat hang",
               "loai nao", "co khong", "tot nhat", "khuyen mai", "bao hanh",
               "giao hang", "phi ship", "co san", "hang moi", "giam gia",
               "con hang", "dat mua", "phan loai", "tinh nang", "mo ta",
               "so sanh", "chat luong", "bao lau", "chinh hang", "mau sac",
               "kich thuoc", "dung luong", "hieu nang", "thuong hieu", "test thu",
               "su dung", "cach dung", "huong dan", "cach chon", "uu dai",
               "doi tra", "che do", "ngung ban", "hot deal", "don hang"
            };

            return productKeywords.Any(keyword => normalizedMessage.Contains(keyword));
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
