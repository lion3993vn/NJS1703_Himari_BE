using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/payos")]
    [ApiController]
    public class PayOSController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public PayOSController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> CreateProduct(WebhookType webhook)
        {
            try
            {
                await _orderService.ConfirmOrderPayment(webhook);
                return Ok(new
                {
                    success = true
                });
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    success = true
                });
            }
        }
    }
}
