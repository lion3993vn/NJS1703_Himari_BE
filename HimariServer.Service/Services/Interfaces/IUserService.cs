using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.BusinessModels.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IUserService
    {
        public Task<BaseResponseModel> GetUserById(int id);
        public Task<BaseResponseModel> LoginWithGoogleOAuth(string credential);
        public Task<BaseResponseModel> RefreshToken(string jwtToken);
        public Task<BaseResponseModel> UpdateUser(UpdateUserModel user);
        public Task<BaseResponseModel> GetUsers(PaginationParameter paginationParameter);
        public Task<BaseResponseModel> DeleteUser(int id);
        public Task<BaseResponseModel> UpdateUserAddress(UpdateUserAddressModel userAddress);
    }
}
