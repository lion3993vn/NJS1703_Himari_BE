using HimariServer.Service.BusinessModels.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<BaseResponseModel> GetRevenue();
        Task<BaseResponseModel> GetNewOrder();

        Task<BaseResponseModel> GetNewUser();
    }
}
