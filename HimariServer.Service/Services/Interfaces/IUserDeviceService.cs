using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.BusinessModels.UserDeviceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IUserDeviceService
    {
        Task<BaseResponseModel> AddDeviceTokenByUserId(CreateUserDeviceModel model);
        Task<BaseResponseModel> DeleteDeviceToken(string token);
    }
}
