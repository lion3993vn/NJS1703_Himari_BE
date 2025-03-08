using HimariServer.Service.BusinessModels.PartSymptomModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/payments")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{orderCode}")]
        public Task<IActionResult> GetPaymentByOrderCode(int orderCode)
        {
            return ValidateAndExecute(async () => await _paymentService.GetPaymentInfoByOrderCode(orderCode));
        }
    }
}
