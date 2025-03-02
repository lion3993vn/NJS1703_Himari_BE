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
        public Task<IActionResult> CreateOrder(OrderResquestModel model)
        {
            return ValidateAndExecute(async () => await _orderService.CreateOrder(model));
        }
    }
}
