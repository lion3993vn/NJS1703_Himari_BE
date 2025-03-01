using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.ChatMessageModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HimariServer.Service.Services.Implements
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChatMessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
