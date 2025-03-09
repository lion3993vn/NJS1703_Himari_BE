using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.OrderModels;
using HimariServer.Service.BusinessModels.ResultModels;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponseModel> CreateOrder(OrderRequestModel model);
        Task ConfirmOrderPayment(WebhookType webhook);
        Task<BaseResponseModel> GetOrderByUserId(int userId, PaginationParameter paginationParameter);
        Task<BaseResponseModel> UpdateOrder(OrderUpdateModel orderUpdateModel);
    }
}
