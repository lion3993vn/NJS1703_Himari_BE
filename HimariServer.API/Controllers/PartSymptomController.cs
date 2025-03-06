using HimariServer.Service.BusinessModels.PartSymptomModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HimariServer.Repository.Commons;

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

        [HttpPost]
        public Task<IActionResult> CreatePartSymptom([FromBody] CreatePartSymptomModel model)
        {
            return ValidateAndExecute(async () => await _partSymptomService.CreatePartSymptom(model));
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetPartSymptomById(int id)
        {
            return ValidateAndExecute(async () => await _partSymptomService.GetPartSymptomById(id));
        }

        [HttpGet]
        public Task<IActionResult> GetPartSymptoms([FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _partSymptomService.GetPartSymptomsPaginationAsync(paginationParameter));
        }

        [HttpPut]
        public Task<IActionResult> UpdatePartSymptom([FromBody] PartSymptomModel model)
        {

            return ValidateAndExecute(async () => await _partSymptomService.UpdatePartSymptom(model));
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeletePartSymptomById(int id)
        {
            return ValidateAndExecute(async () => await _partSymptomService.DeletePartSymptomById(id));
        }
    }
}
