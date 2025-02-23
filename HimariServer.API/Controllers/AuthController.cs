using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login/google/oauth")]
        public Task<IActionResult> LoginWithGoogleOAuth([FromBody] string credential)
        {
            return ValidateAndExecute(() => _userService.LoginWithGoogleOAuth(credential));
        }

        [HttpPost("refresh-token")]
        public Task<IActionResult> RefreshToken([FromBody] string token)
        {
            return ValidateAndExecute(() => _userService.RefreshToken(token));
        }
    }
}
