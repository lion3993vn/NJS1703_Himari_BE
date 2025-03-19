using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IChatMessageService _chatMessageService;
        private readonly IDeepseekService _deepseekService;

        public ChatHub(IUnitOfWork unitOfWork, IChatMessageService chatMessageService, IDeepseekService deepseekService)
        {
            _unitOfWork = unitOfWork;
            _chatMessageService = chatMessageService;
            _deepseekService = deepseekService;
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
            
            // Update the bot message in the database with the complete response
            messageBot.Message = fullResponse.ToString();
            _unitOfWork.ChatMessageRepository.UpdateAsync(messageBot);
            _unitOfWork.Save();
            
            // Signal completion of the streaming - don't send the full message again
            await Clients.Caller.SendAsync("ReceiveStreamingMessage", "", true);
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
