using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.NotificationModels;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public async Task<BaseResponseModel> PushNotificationByUserId(NotificationRequestModel model)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(model.UserId);
            if (user == null)
            {
                return new BaseResponseModel
                {
                    Message = MessageConstants.USER_NOT_EXIST,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var noti = _mapper.Map<Notification>(model);
            noti.TitleUnsign = StringUtils.ConvertToUnSign(model.Title);

            await _unitOfWork.NotificationRepository.AddAsync(noti);
            await _unitOfWork.SaveAsync(); // Ensure the notification is saved and has an ID

            var userNoti = new UserNotification
            {
                NotificationId = noti.Id,
                UserId = model.UserId,
                IsRead = false
            };
            await _unitOfWork.UserNotificationRepository.AddAsync(userNoti);
            await _unitOfWork.SaveAsync(); // Ensure the user notification is saved

            var userDevice = await _unitOfWork.UserDeviceRepository.GetUserDeviceByUserId(model.UserId);

            if (userDevice.Count == 0)
            {
                return new BaseResponseModel
                {
                    Message = MessageConstants.USER_DEVICE_NOT_FOUND,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            foreach (var item in userDevice)
            {
                await FirebaseLibrary.SendMessageFireBase(model.Title, model.Message, item.DeviceToken);
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PUSH_NOTI_USER_SUCCESS
            };
        }

        public async Task<BaseResponseModel> GetNotificationsByUserId(PaginationParameter paginationParameter, int userId, int type = 0)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.USER_NOT_EXIST);
            }

            if (!Enum.TryParse(type.ToString(), out NotificationType notificationType))
            {
                throw new ArgumentException(MessageConstants.ENUM_NOTI_NOT_VALID);
            }

            var notifications = await _unitOfWork.UserNotificationRepository.ToPaginationIncludeAsync(
                paginationParameter,
                filter: x => x.UserId == userId && !x.IsDeleted && x.Notification.Type == notificationType,
                include: query => query.Include(x => x.Notification)
                                        .Include(x => x.User),
                orderBy: query => query.OrderByDescending(x => x.CreatedDate)
            );

            var listNoti = _mapper.Map<Pagination<NotificationModel>>(notifications);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_NOTI_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listNoti,
                    MetaData = new
                    {
                        listNoti.TotalCount,
                        listNoti.PageSize,
                        listNoti.CurrentPage,
                        listNoti.TotalPages,
                        listNoti.HasNext,
                        listNoti.HasPrevious
                    }
                }
            };
        }
    }
}
