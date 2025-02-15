using AutoMapper;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.BusinessModels.UserModels;
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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.UsersRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                return new BaseResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = MessageConstants.GET_USER_BY_EMAIL_SUCCESS,
                    Data = _mapper.Map<UserModel>(user)
                };
            }
            else 
            {
                throw new NotExistException(MessageConstants.ACCOUNT_NOT_EXIST, "");
            }
            
        }
    }
}
