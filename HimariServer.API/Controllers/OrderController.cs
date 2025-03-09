using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Service.BusinessModels.CategoryModels;
using HimariServer.Service.BusinessModels.OrderModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
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

        [HttpPost]
        public Task<IActionResult> CreateOrder(OrderRequestModel model)
        {
            return ValidateAndExecute(async () => await _orderService.CreateOrder(model));
        }
        [HttpGet("{userId}")]
        public Task<IActionResult> GetOrderByUserId(int userId, PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _orderService.GetOrderByUserId(userId, paginationParameter));
        }
        //orderid, delivery status, address
        [HttpPut]
        public Task<IActionResult> UpdateOrder(OrderUpdateModel orderUpdateModel)
        {
            return ValidateAndExecute(async () => await _orderService.UpdateOrder(orderUpdateModel));
        }

    }
}
