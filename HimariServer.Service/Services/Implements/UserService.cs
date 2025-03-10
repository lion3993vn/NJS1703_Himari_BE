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
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HimariServer.Repository.Commons;

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

        public async Task<BaseResponseModel> GetUserById(int id)
        {
            var existingUser = await _unitOfWork.UsersRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.USER_NOT_EXIST
                };
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_USER_BY_EMAIL_SUCCESS,
                Data = _mapper.Map<UserModel>(existingUser)
            };
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

                var accessToken = AuthenTokenUtils.GenerateAccessToken(newUser.Email, newUser, role.RoleName, _configuration);
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

        public async Task<BaseResponseModel> RefreshToken(string jwtToken)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authSigningKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                SecurityToken validatedToken;
                var principal = handler.ValidateToken(jwtToken, validationParameters, out validatedToken);
                var email = principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                if (email != null)
                {
                    var existUser = await _unitOfWork.UsersRepository.GetUserByEmailAsync(email);
                    if (existUser != null)
                    {
                        var accessToken = AuthenTokenUtils.GenerateAccessToken(email, existUser, existUser.Role.RoleName, _configuration);
                        var refreshToken = AuthenTokenUtils.GenerateRefreshToken(email, _configuration);
                        return new BaseResponseModel
                        {
                            StatusCode = StatusCodes.Status200OK,
                            Data = new AuthenModel
                            {
                                AccessToken = accessToken,
                                RefreshToken = refreshToken
                            },
                            Message = MessageConstants.TOKEN_REFRESH_SUCCESS_MESSAGE
                        };
                    }
                }
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = MessageConstants.USER_NOT_EXIST
                };
            }
            catch
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = MessageConstants.TOKEN_NOT_VALID
                };
            }
        }

        public async Task<BaseResponseModel> UpdateUser(UpdateUserModel user)
        {
            var existingUser = await _unitOfWork.UsersRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.USER_NOT_EXIST
                };
            }

            _mapper.Map(user, existingUser);

            _unitOfWork.UsersRepository.UpdateAsync(existingUser);
            await _unitOfWork.SaveAsync();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.USER_UPDATE_SUCCESS
            };

        }

        public async Task<BaseResponseModel> GetUsers(PaginationParameter paginationParameter)
        {
            var users = await _unitOfWork.UsersRepository.ToPaginationIncludeAsync(
                paginationParameter,
                filter: x => !x.IsDeleted && x.Role.RoleName != "ADMIN",
                include: query => query.Include(x => x.Role),
                orderBy: query => query.OrderByDescending(x => x.CreatedDate)
            );

            var userList = _mapper.Map<Pagination<UserModel>>(users);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_USER_SUCCESS,
                Data = new ModelPaging
                {
                    Data = userList,
                    MetaData = new
                    {
                        userList.TotalCount,
                        userList.PageSize,
                        userList.CurrentPage,
                        userList.TotalPages,
                        userList.HasNext,
                        userList.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> DeleteUser(int id)
        {
            var existingUser = await _unitOfWork.UsersRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.USER_NOT_EXIST
                };
            }

            _unitOfWork.UsersRepository.SoftDeleteAsync(existingUser);
            await _unitOfWork.SaveAsync();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.USER_DELETE_SUCCESS
            };
        }

        public async Task<BaseResponseModel> UpdateUserAddress(UpdateUserAddressModel userAddress)
        {
            var existingUser = await _unitOfWork.UsersRepository.GetByIdAsync(userAddress.Id);
            if (existingUser == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.USER_NOT_EXIST
                };
            }

            // Update only the address fields
            existingUser.Province = userAddress.Province;
            existingUser.District = userAddress.District;
            existingUser.Ward = userAddress.Ward;
            existingUser.AddressBonus = userAddress.AddressBonus;

            _unitOfWork.UsersRepository.UpdateAsync(existingUser);
            await _unitOfWork.SaveAsync();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.USER_ADDRESS_UPDATE_SUCCESS
            };
        }
    }
}
