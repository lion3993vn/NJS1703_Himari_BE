using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Service.BusinessModels.CategoryModels;
using HimariServer.Service.BusinessModels.OrderModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public Task<IActionResult> CreateOrder(OrderRequestModel model)
        {
            return ValidateAndExecute(async () => await _orderService.CreateOrder(model));
        }

        [Authorize(Roles = "1,3,4")]
        [HttpGet("{userId}")]
        public Task<IActionResult> GetOrderByUserId(int userId, [FromQuery] PaginationParameter paginationParameter, [FromQuery] string? searchTerm,
            [FromQuery] bool newestFirst = true,
            [FromQuery] DeliveryStatus? deliveryStatus = null,
            [FromQuery] PaymentStatus? paymentStatus = null)
        {
            return ValidateAndExecute(async () => await _orderService.GetOrderByUserId(userId, paginationParameter, searchTerm, newestFirst, deliveryStatus, paymentStatus));
        }

        [Authorize(Roles = "3,4")]
        [HttpPut]
        public Task<IActionResult> UpdateOrder(OrderUpdateModel orderUpdateModel)
        {
            return ValidateAndExecute(async () => await _orderService.UpdateOrder(orderUpdateModel));
        }

        //[HttpGet]
        //public Task<IActionResult> GetAllOrders([FromQuery] PaginationParameter paginationParameter)
        //{
        //    return ValidateAndExecute(async () => await _orderService.GetAllOrders(paginationParameter));
        //}
        [Authorize(Roles = "1,3,4")]
        [HttpGet("id/{orderId}")]
        public Task<IActionResult> GetOrderByOrderCode(int orderId)
        {
            return ValidateAndExecute(async () => await _orderService.GetOrderByOrderId(orderId));
        }

        [Authorize(Roles = "3,4")]
        [HttpGet]
        public Task<IActionResult> SearchOrders(
            [FromQuery] string? searchTerm,
            [FromQuery] PaginationParameter paginationParameter,
            [FromQuery] bool newestFirst = true,
            [FromQuery] DeliveryStatus? deliveryStatus = null,
            [FromQuery] PaymentStatus? paymentStatus = null)
        {
            return ValidateAndExecute(async () =>
                await _orderService.SearchOrders(searchTerm, paginationParameter, newestFirst, deliveryStatus, paymentStatus));
        }

        [Authorize(Roles = "3,4")]
        [HttpGet("statistics")]
        public Task<IActionResult> GetStatistics([FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            return ValidateAndExecute(async () =>
               await _orderService.GetStatistics(month, year));
        }
    }
}
