using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/dashboard")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("revenue")]
        public Task<IActionResult> GetRevenue()
        {
            return ValidateAndExecute(async () => await _dashboardService.GetRevenue());
        }

        [HttpGet("new-order")]
        public Task<IActionResult> GetNewOrder()
        {
            return ValidateAndExecute(async () => await _dashboardService.GetNewOrder());
        }

        [HttpGet("new-user")]
        public Task<IActionResult> GetNewUser()
        {
            return ValidateAndExecute(async () => await _dashboardService.GetNewUser());
        }

        [HttpGet("new-product")]
        public Task<IActionResult> GetNewProduct()
        {
            return ValidateAndExecute(async () => await _dashboardService.GetNewProduct());
        }

        [HttpGet("revenue-month")]
        public Task<IActionResult> GetRevenueWithListMonth()
        {
            return ValidateAndExecute(async () => await _dashboardService.GetRevenueWithListMonth());
        }

        [HttpGet("revenue-percent")]
        public Task<IActionResult> GetOrderWithRevenue()
        {
            return ValidateAndExecute(async () => await _dashboardService.GetOrderWithRevenue());
        }

        [HttpGet("product-low-quantity")]
        public Task<IActionResult> GetLowQuantityProduct()
        {
            return ValidateAndExecute(async () => await _dashboardService.GetLowQuantityProduct());
        }
    }
}
