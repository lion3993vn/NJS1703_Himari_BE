using AutoMapper;
using Google.Apis.Auth;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.AuthenModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.BusinessModels.UserModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
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
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<BaseResponseModel> LoginWithGoogleOAuth(string credential)
        {
            string cliendId = _configuration["GoogleCredential:ClientId"];

            if (string.IsNullOrEmpty(cliendId))
            {
                throw new DefaultException("", MessageConstants.TOKEN_NOT_VALID);
            }

            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { cliendId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);
            if (payload == null)
            {
                throw new DefaultException("", MessageConstants.TOKEN_NOT_VALID);
            }

            var existUser = await _unitOfWork.UsersRepository.GetUserByEmailAsync(payload.Email);
            if (existUser != null)
            {
                if (existUser.IsDeleted)
                {
                    return new BaseResponseModel
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = MessageConstants.USER_HAS_BEEN_DELETE,
                    };
                }
                var accessToken = AuthenTokenUtils.GenerateAccessToken(existUser.Email, existUser, existUser.Role.RoleName, _configuration);
                var refreshToken = AuthenTokenUtils.GenerateRefreshToken(existUser.Email, _configuration);

                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = MessageConstants.LOGIN_SUCCESS_MESSAGE,
                    Data = new AuthenModel
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    },
                };
            }
            else
            {
                var role = await _unitOfWork.RoleRepository.GetByRoleName("USER");
                var newUser = new User
                {
                    Email = payload.Email,
                    FullName = payload.Name,
                    UnsignName = StringUtils.ConvertToUnSign(payload.Name),
                    AvatarUrl = payload.Picture,
                    GoogleId = payload.JwtId,
                    RoleId = role.Id,
                    IsVerify = payload.EmailVerified
                };

                await _unitOfWork.UsersRepository.AddAsync(newUser);
                _unitOfWork.Save();

                var accessToken = AuthenTokenUtils.GenerateAccessToken(newUser.Email, newUser,role.RoleName, _configuration);
                var refreshToken = AuthenTokenUtils.GenerateRefreshToken(newUser.Email, _configuration);

                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = MessageConstants.LOGIN_SUCCESS_MESSAGE,
                    Data = new AuthenModel
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    },
                };
            }
        }
    }
}
