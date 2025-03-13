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

        public ChatHub(IUnitOfWork unitOfWork, IChatMessageService chatMessageService)
        {
            _unitOfWork = unitOfWork;
            _chatMessageService = chatMessageService;
        }

        public async Task SendMessage(string message)
        {
            try
            {
                //var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
                //if (user == null)
                //{
                //    throw new NotExistException(MessageConstants.USER_NOT_EXIST);
                //}

                //var messageUser = new ChatMessage
                //{
                //    UserId = userId,
                //    Message = message,
                //    Type = MessageType.USER
                //};
                //await _unitOfWork.ChatMessageRepository.AddAsync(messageUser);

                var messageResponse = ProcessMessageResponse(message);
                //var messageBot = new ChatMessage
                //{
                //    UserId = userId,
                //    Message = messageResponse,
                //    Type = MessageType.BOT
                //};
                //await _unitOfWork.ChatMessageRepository.AddAsync(messageBot);

                //_unitOfWork.Save();

                await Clients.Caller.SendAsync("ReceiveMessage", messageResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string ProcessMessageResponse(string message)
        {
            if(message.Contains("vàng da"))
            {
                return "Vàng da là một loại bệnh da phổ biến, bạn nên đi khám ngay";
            }
            else
            {
                return "Tôi không hiểu bạn đang nói gì";
            }
        }
    }
}
