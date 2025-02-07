using System.Diagnostics;
using Backend_reactNative_Shoppee_Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SWD392_Himari.Repository;

namespace Backend_reactNative_Shoppee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUserServices userServices, IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _userServices = userServices;
            _unitOfWork = unitOfWork;   
        }
        [HttpGet("get-all-user-no-cache")]
        public async Task<IActionResult> GetAllUserNoCache()
        {
            _unitOfWork.BeginTransaction();
            var listUser = await _userServices.getAllUser();
            _unitOfWork.CommitTransaction();

            return Ok(listUser);
        }


        [HttpGet("get-user-by-UserName")]
        public async Task<IActionResult> GetUserByUserId(string userId)
        {
            _unitOfWork.BeginTransaction();
            var user = await _userServices.getUserById(userId);
            _unitOfWork.CommitTransaction();
            return Ok(user);
        }


    }
}
