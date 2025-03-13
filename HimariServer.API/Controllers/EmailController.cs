using HimariServer.Service.BusinessModels.EmailModels;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HimariServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public EmailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _mailService.SendEmailAsync(new MailRequest
            {
                ToEmail = "lion3993vn@gmail.com",
                Subject = "Chào mừng bạn đến với Himari!",
                Body = EmailUtils.WelcomeEmail("Hiếu"),
                Attachments = null
            });
            return Ok("Himari Email Service");
        }
    }
}
