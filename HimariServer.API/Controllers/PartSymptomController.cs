using HimariServer.Service.BusinessModels.PartSymptomModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HimariServer.Repository.Commons;
using Microsoft.AspNetCore.Authorization;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/part-symptoms")]
    [ApiController]
    public class PartSymptomController : BaseController
    {
        private readonly IPartSymptomService _partSymptomService;

        public PartSymptomController(IPartSymptomService partSymptomService)
        {
            _partSymptomService = partSymptomService;
        }
        [Authorize(Roles = "3,4")]
        [HttpPost]
        public Task<IActionResult> CreatePartSymptom([FromBody] CreatePartSymptomModel model)
        {
            return ValidateAndExecute(async () => await _partSymptomService.CreatePartSymptom(model));
        }
        [Authorize(Roles = "3,4")]
        [HttpGet("{id}")]
        public Task<IActionResult> GetPartSymptomById(int id)
        {
            return ValidateAndExecute(async () => await _partSymptomService.GetPartSymptomById(id));
        }
        [Authorize(Roles = "3,4")]
        [HttpGet]
        public Task<IActionResult> GetPartSymptoms([FromQuery] PaginationParameter paginationParameter, [FromQuery] bool newestFirst = true, [FromQuery] string? searchTerm = null)
        {
            return ValidateAndExecute(async () => await _partSymptomService.GetPartSymptomsPaginationAsync(paginationParameter, newestFirst, searchTerm));
        }
        [Authorize(Roles = "3,4")]
        [HttpPut]
        public Task<IActionResult> UpdatePartSymptom([FromBody] PartSymptomModel model)
        {

            return ValidateAndExecute(async () => await _partSymptomService.UpdatePartSymptom(model));
        }
        [Authorize(Roles = "3,4")]
        [HttpDelete("{id}")]
        public Task<IActionResult> DeletePartSymptomById(int id)
        {
            return ValidateAndExecute(async () => await _partSymptomService.DeletePartSymptomById(id));
        }
    }
}
