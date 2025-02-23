using AutoMapper;
using HimariServer.Repository.Entities;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.BusinessModels.UserDeviceModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class UserDeviceService : IUserDeviceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserDeviceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponseModel> AddDeviceTokenByUserId(CreateUserDeviceModel model)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(model.UserId);
            if (user == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.USER_NOT_EXIST,
                };
            }

            var userDevice = await _unitOfWork.UserDeviceRepository.GetByTokenDevice(model.DeviceToken);
            if (userDevice != null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = MessageConstants.DEVICE_TOKEN_EXIST,
                };
            }

            var newUserDevice = _mapper.Map<UserDevice>(model);
            await _unitOfWork.UserDeviceRepository.AddAsync(newUserDevice);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.DEVICE_TOKEN_ADD_SUCCESS,
                Data = _mapper.Map<UserDeviceModel>(newUserDevice)
            };
        }

        public async Task<BaseResponseModel> DeleteDeviceToken(string token)
        {
            var userDevice = await _unitOfWork.UserDeviceRepository.GetByTokenDevice(token);
            if(userDevice == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.DEVICE_TOKEN_NOT_EXIST,
                };
            }

            _unitOfWork.UserDeviceRepository.SoftDeleteAsync(userDevice);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.DEVICE_TOKEN_DELETE_SUCCESS,
            };
        }
    }
}

