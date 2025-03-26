using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.UserModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "3")]
        [HttpGet]
        public Task<IActionResult> GetUsers([FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _userService.GetUsers(paginationParameter));
        }

        [Authorize(Roles = "1,3,4")]
        [HttpPut]
        public Task<IActionResult> UpdateUser([FromBody] UpdateUserModel user)
        {
            return ValidateAndExecute(async () => await _userService.UpdateUser(user));
        }

        [Authorize(Roles = "1,3,4")]
        [HttpGet("{id}")]
        public Task<IActionResult> GetUser(int id)
        {
            return ValidateAndExecute(async () => await _userService.GetUserById(id));
        }

        [Authorize(Roles = "3")]
        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteUser(int id)
        {
            return ValidateAndExecute(async () => await _userService.DeleteUser(id));
        }

        [Authorize(Roles = "3")]
        [HttpPut("address")]
        public Task<IActionResult> UpdateUserAddress([FromBody] UpdateUserAddressModel userAddress)
        {
            return ValidateAndExecute(async () => await _userService.UpdateUserAddress(userAddress));
        }
    }
}
