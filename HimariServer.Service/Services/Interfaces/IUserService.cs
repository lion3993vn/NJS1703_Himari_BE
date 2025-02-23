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
        public Task<BaseResponseModel> LoginWithGoogleOAuth(string credential);

        public Task<BaseResponseModel> RefreshToken(string jwtToken);
    }
}
