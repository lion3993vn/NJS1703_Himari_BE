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
        private readonly IClaimsService _claimsService;

        public AuthController(IUserService userService, IClaimsService claimsService)
        {
            _userService = userService;
            _claimsService = claimsService;
        }

        [HttpPost("login/google/oauth")]
        public Task<IActionResult> LoginWithGoogleOAuth([FromBody] string credential)
        {
            return ValidateAndExecute(() => _userService.LoginWithGoogleOAuth(credential));
        }
    }
}
