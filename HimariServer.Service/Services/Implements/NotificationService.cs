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

        public async Task<BaseResponseModel> PushNotificationByUserId(int userId, NotificationRequestModel model)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
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
            noti.Type = NotificationType.USER;

            await _unitOfWork.NotificationRepository.AddAsync(noti);
            await _unitOfWork.SaveAsync(); // Ensure the notification is saved and has an ID

            var userNoti = new UserNotification
            {
                NotificationId = noti.Id,
                UserId = userId,
                IsRead = false
            };
            await _unitOfWork.UserNotificationRepository.AddAsync(userNoti);
            await _unitOfWork.SaveAsync(); // Ensure the user notification is saved

            var userDevice = await _unitOfWork.UserDeviceRepository.GetUserDeviceByUserId(userId);

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

        public async Task<BaseResponseModel> GetUnreadNotificationCount(int userId)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.USER_NOT_EXIST);
            }

            var notification = await _unitOfWork.UserNotificationRepository.GetUnreadNotificationCount(userId);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.COUNT_UNREAD_NOTI_SUCCESS,
                Data = notification
            };
        }

        public async Task<BaseResponseModel> MarkNotificationAsRead(int notificationId)
        {
            var userNoti = await _unitOfWork.UserNotificationRepository.GetByIdAsync(notificationId);

            if (userNoti == null)
            {
                throw new NotExistException(MessageConstants.USER_NOTI_NOT_EXIST);
            }

            userNoti.IsRead = true;
            _unitOfWork.UserNotificationRepository.UpdateAsync(userNoti);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.MARK_NOTI_AS_READ_SUCCESS
            };

        }

        public async Task<BaseResponseModel> MarkAllNotificationsAsRead(int userId)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.USER_NOT_EXIST);
            }

            var userNoti = await _unitOfWork.UserNotificationRepository.GetUnreadNotificationByUserId(userId);

            if (!userNoti.Any())
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = MessageConstants.NO_NOTI_MARK_AS_READ
                };
            }

            foreach (var item in userNoti)
            {
                item.IsRead = true;
                _unitOfWork.UserNotificationRepository.UpdateAsync(item);
            }

            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.MARK_ALL_NOTI_AS_READ_SUCCESS
            };
        }

        public async Task<BaseResponseModel> GetSystemNotifications(PaginationParameter paginationParameter, string keyword = null, bool newestFirst = true)
        {
            var notifications = await _unitOfWork.NotificationRepository.ToPaginationIncludeAsync(
                paginationParameter,
                filter: x => !x.IsDeleted &&
                          x.Type == NotificationType.SYSTEM &&
                          (string.IsNullOrEmpty(keyword) ||
                           x.Title.Contains(keyword) ||
                           x.TitleUnsign.Contains(keyword) ||
                           x.Message.Contains(keyword)),
                orderBy: query => newestFirst
                    ? query.OrderByDescending(x => x.CreatedDate)
                    : query.OrderBy(x => x.CreatedDate)
            );

            var listNoti = _mapper.Map<Pagination<SystemNotificationModel>>(notifications);

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

        public async Task<BaseResponseModel> PushNotification(NotificationRequestModel model)
        {
            var userDevices = await _unitOfWork.UserDeviceRepository.GetAllWithUser();

            var noti = _mapper.Map<Notification>(model);
            noti.TitleUnsign = StringUtils.ConvertToUnSign(model.Title);
            noti.Type = NotificationType.SYSTEM;

            await _unitOfWork.NotificationRepository.AddAsync(noti);
            await _unitOfWork.SaveAsync();

            var tokens = userDevices.Select(x => x.DeviceToken).ToList(); //tokens of all devices

            var userNotis = userDevices.DistinctBy(x => x.UserId).Select(x => new UserNotification
            {
                NotificationId = noti.Id,
                UserId = x.UserId,
                IsRead = false
            }).ToList();

            await _unitOfWork.UserNotificationRepository.AddRangeAsync(userNotis);
            await _unitOfWork.SaveAsync();

            var tokensNotValid = await FirebaseLibrary.SendRangeMessageFireBase(model.Title, model.Message, tokens);

            if (tokensNotValid.Any())
            {
                await RemoveTokenNotValid(tokensNotValid);
            }


            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PUSH_NOTI_SUCCESS
            };
        }

        private async Task RemoveTokenNotValid(List<string> tokens)
        {
            foreach(var token in tokens)
            {
                var userDevice = await _unitOfWork.UserDeviceRepository.GetByTokenDevice(token);

                _unitOfWork.UserDeviceRepository.SoftDeleteAsync(userDevice);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
