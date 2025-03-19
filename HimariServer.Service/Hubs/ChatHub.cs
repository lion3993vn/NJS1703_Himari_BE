using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var messageResponse = await ProcessMessageResponse(message);

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

        private async Task<string> ProcessMessageResponse(string message)
        {
            return await _deepseekService.ResponseMessage(message);
        }
    }
}
