using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.ChatMessageModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MimeKit;
using DeepSeek.AspNetCore;
using DeepSeek.Core;
using DeepSeek.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using HimariServer.Service.SettingModels;
using Microsoft.Extensions.Options;
using DeepSeek.Core;
using HimariServer.Repository.Entities;

namespace HimariServer.Service.Services.Implements
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly DeepSeekClient _client;
        public ChatMessageService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<OpenAISettings> openAISettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //var a = openAISettings.Value.ApiKey;
            _client = new DeepSeekClient(openAISettings.Value.ApiKey);
        }
        public async Task<BaseResponseModel> GetProductRecommendationAsync(ChatRequestModel chatRequest)
        {
            var bodypartSymptomPair = JsonConvert.SerializeObject(_unitOfWork.SymptomRepository.GetBodyPartSymptomPairs());

            var systemMessage = $@"Bạn là trợ lý tư vấn sản phẩm mỹ phẩm có tên là HimaBot phục vụ cho cửa hàng Himari Cosmetics. Bạn chỉ có thể trả lời câu hỏi liên quan đến làm đẹp. Khi khách hàng miêu tả triệu chứng, hãy đưa ra phản hồi có 2 phần được trả về dưới dạng JSON:
            chatMessage: Tư vấn cho khách hàng (Độ dài không quá 3 câu).
            keywords: Cặp dữ liệu gồm key (bodypart) và value (symptom).

            Dưới đây là danh sách dữ liệu (bodypart, symptom): {bodypartSymptomPair}.";
            var userMessage = $@"{chatRequest.Message}";
            var request = new ChatRequest
            {
                Messages = [
                    Message.NewSystemMessage(systemMessage),
                    Message.NewUserMessage(userMessage)
                    ]
            };

            var chatResponse = await _client.ChatAsync(request, new CancellationToken());
            if (chatResponse is null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status503ServiceUnavailable,
                    Message = MessageConstants.GET_CHAT_MESSAGES_FAILED
                };
            }
            var chatResponseJson = JsonConvert.DeserializeObject<APIChatResponseModel>(chatResponse?.Choices.First().Message?.Content);
            var productIds = await _unitOfWork.ProductRepository.GetProductIdsByPartSymptomAsync(chatResponseJson.Keywords.Symptom);
            var response = new ChatResponseModel
            {
                Content = chatResponseJson.ChatMessage,
                Products = productIds
            };
            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_CHAT_MESSAGES_SUCCESS,
                Data = response
            };
        }

        public async Task<BaseResponseModel> GetMessageChatByUserIdPaginated(int userId, PaginationParameter paginationParameter)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.USER_NOT_EXIST);
            }

            var chatMessages = await _unitOfWork.ChatMessageRepository.ToPaginationIncludeAsync(
                paginationParameter,
                filter: x => x.UserId == userId && !x.IsDeleted,
                include: query => query.Include(x => x.User),
                orderBy: query => query.OrderByDescending(x => x.CreatedDate)
            );

            var messageList = _mapper.Map<Pagination<ChatMessageModel>>(chatMessages);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_CHAT_MESSAGES_SUCCESS,
                Data = new ModelPaging
                {
                    Data = messageList,
                    MetaData = new
                    {
                        messageList.TotalCount,
                        messageList.PageSize,
                        messageList.CurrentPage,
                        messageList.TotalPages,
                        messageList.HasNext,
                        messageList.HasPrevious
                    }
                }
            };
        }
    }
}
