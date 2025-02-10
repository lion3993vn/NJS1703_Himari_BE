using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public Task<IActionResult> GetUserByEmail([FromQuery]string email)
        {
            return ValidateAndExecute(async () => await _userService.GetUserByEmailAsync(email));
        }
    }
}
