using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.BodyPartModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "3,4")]
        [HttpGet]
        public Task<IActionResult> GetBodyParts([FromQuery] PaginationParameter paginationParameter,
            [FromQuery] bool newestFirst = true,
            [FromQuery] string? searchTerm = null)
        {
            return ValidateAndExecute(async () => await _bodyPartService.GetBodyPartsPaginationAsync(paginationParameter, newestFirst, searchTerm));
        }

        [Authorize(Roles = "3,4")]
        [HttpGet("{id}")]
        public Task<IActionResult> GetBodyPartById(int id)
        {
            return ValidateAndExecute(async () => await _bodyPartService.GetBodyPartByIdAsync(id));
        }

        [Authorize(Roles = "3,4")]
        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteBodyPartById(int id)
        {
            return ValidateAndExecute(async () => await _bodyPartService.DeleteBodyPartByIdAsync(id));
        }

        [Authorize(Roles = "3,4")]
        [HttpPost]
        public Task<IActionResult> AddBodyPart([FromBody]AddBodyPartModel model)
        {
            return ValidateAndExecute(async () => await _bodyPartService.AddBodyPart(model));
        }

        [Authorize(Roles = "3,4")]
        [HttpPut]
        public Task<IActionResult> UpdateBodyPart([FromBody] UpdateBodyPartModel model)
        {
            return ValidateAndExecute(async () => await _bodyPartService.UpdateBodyPart(model));
        }

    }
}
