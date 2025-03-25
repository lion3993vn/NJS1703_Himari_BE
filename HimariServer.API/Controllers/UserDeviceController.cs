using HimariServer.Service.BusinessModels.UserDeviceModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/user-devices")]
    [ApiController]
    public class UserDeviceController : BaseController
    {
        private IUserDeviceService _userDeviceService;

        public UserDeviceController(IUserDeviceService userDeviceService)
        {
            _userDeviceService = userDeviceService;
        }

        [Authorize(Roles = "1,3,4")]
        [HttpPost]
        public Task<IActionResult> AddUserDevice([FromBody]CreateUserDeviceModel model)
        {
            return ValidateAndExecute(() => _userDeviceService.AddDeviceTokenByUserId(model));
        }

        [Authorize(Roles = "1,3,4")]
        [HttpDelete("{deviceToken}")]
        public Task<IActionResult> DeleteDeviceToken(string deviceToken)
        {
            return ValidateAndExecute(() => _userDeviceService.DeleteDeviceToken(deviceToken));
        }
    }
}
