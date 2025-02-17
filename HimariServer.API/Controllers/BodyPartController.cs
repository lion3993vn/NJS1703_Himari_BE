using HimariServer.Repository.Commons;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/body-parts")]
    [ApiController]
    public class BodyPartController : BaseController
    {
        private readonly IBodyPartService _bodyPartService;

        public BodyPartController(IBodyPartService bodyPartService)
        {
            _bodyPartService = bodyPartService;
        }

        [HttpGet]
        public Task<IActionResult> GetBodyParts(PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _bodyPartService.GetBodyPartsPaginationAsync(paginationParameter));
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetBodyPartById(int id)
        {
            return ValidateAndExecute(async () => await _bodyPartService.GetBodyPartByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteBodyPartById(int id)
        {
            return ValidateAndExecute(async () => await _bodyPartService.DeleteBodyPartByIdAsync(id));
        }


    }
}
