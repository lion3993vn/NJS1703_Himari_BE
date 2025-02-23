using AutoMapper;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.NotificationModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponseModel> GetNotificationById(int id)
        {
            var noti = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
            if (noti == null)
            {
                throw new NotExistException("", MessageConstants.NOTI_NOT_EXIST);
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Data = _mapper.Map<NotificationModel>(noti),
                Message = MessageConstants.GET_NOTI_SUCCESS
            };
        }

        public Task<bool> PushListMessageFirebase(string title, string body, List<string> fcmTokens)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PushMessageFirebase(string title, string body, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
