using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.SymptomModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/symptoms")]
    [ApiController]
    public class SymptomController : BaseController
    {
        private readonly ISymptomService _symptomService;
        public SymptomController(ISymptomService symptomService)
        {
            _symptomService = symptomService;
        }
        [HttpGet]
        public Task<IActionResult> GetSymptoms([FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _symptomService.GetSymptomsPaginationAsync(paginationParameter));
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetSymptomById(int id)
        {
            return ValidateAndExecute(async () => await _symptomService.GetSymptomById(id));
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteSymptomById(int id)
        {
            return ValidateAndExecute(async () => await _symptomService.DeleteSymptomById(id));
        }
        [HttpPut]
        public Task<IActionResult> UpdateSymptom([FromBody] SymptomModel symptom)
        {
            return ValidateAndExecute(async () => await _symptomService.UpdateSymptom(symptom));
        }
        [HttpPost]
        public Task<IActionResult> CreateSymptom([FromBody] CreateSymptomModel symptom)
        {
            return ValidateAndExecute(async () => await _symptomService.CreateSymptom(symptom));
        }
    }
}
