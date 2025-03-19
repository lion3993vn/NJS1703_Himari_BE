using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.BrandModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/brands")]
    [ApiController]
    public class BrandController : BaseController
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet]
        public Task<IActionResult> GetBrands([FromQuery] PaginationParameter paginationParameter, [FromQuery] bool newestFirst = true, [FromQuery] string? searchTerm = null)
        {
            return ValidateAndExecute(async () => await _brandService.GetBrandsPaginationAsync(paginationParameter, newestFirst, searchTerm));
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetBrandById(int id)
        {
            return ValidateAndExecute(async () => await _brandService.GetBrandById(id));
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteBrandById(int id)
        {
            return ValidateAndExecute(async () => await _brandService.DeleteBrandById(id));
        }
        [HttpPut]
        public Task<IActionResult> UpdateBrand([FromBody] BrandModel brand)
        {
            return ValidateAndExecute(async () => await _brandService.UpdateBrand(brand));
        }
        [HttpPost]
        public Task<IActionResult> CreateBrand([FromBody] CreateBrandModel brand)
        {
            return ValidateAndExecute(async () => await _brandService.CreateBrand(brand));
        }
    }
}
