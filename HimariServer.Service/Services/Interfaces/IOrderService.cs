using HimariServer.Repository.Commons;
using HimariServer.Repository.Enums;
using HimariServer.Service.BusinessModels.OrderModels;
using HimariServer.Service.BusinessModels.ResultModels;
using Microsoft.AspNetCore.Mvc;
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
        Task<BaseResponseModel> GetOrderByUserId(int userId, PaginationParameter paginationParameter, string? searchTerm,
        bool newestFirst = true,
        DeliveryStatus? deliveryStatus = null,
        PaymentStatus? paymentStatus = null);
        Task<BaseResponseModel> UpdateOrder(OrderUpdateModel orderUpdateModel);
        Task<BaseResponseModel> GetAllOrders(PaginationParameter paginationParameter);
        Task<BaseResponseModel> GetOrderByOrderId(int orderId);
        Task<BaseResponseModel> SearchOrders(string? searchTerm, PaginationParameter paginationParameter, bool newestFirst,
            DeliveryStatus? deliveryStatus, PaymentStatus? paymentStatus);
        Task<BaseResponseModel> GetStatistics(int? month, int? year);
    }
}
