using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.ProductSymptomModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/product-symptoms")]
    [ApiController]
    public class ProductSymptomController : BaseController
    {
        private readonly IProductSymptomService _productSymptomService;
        
        public ProductSymptomController(IProductSymptomService productSymptomService)
        {
            _productSymptomService = productSymptomService;
        }
        
        [HttpGet]
        public Task<IActionResult> GetProductSymptoms([FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _productSymptomService.GetProductSymptomsPaginationAsync(paginationParameter));
        }
        
        [HttpGet("{id}")]
        public Task<IActionResult> GetProductSymptomById(int id)
        {
            return ValidateAndExecute(async () => await _productSymptomService.GetProductSymptomById(id));
        }
        
        [HttpGet("product/{productId}")]
        public Task<IActionResult> GetProductSymptomsByProductId([FromQuery] PaginationParameter paginationParameter, int productId)
        {
            return ValidateAndExecute(async () => await _productSymptomService.GetProductSymptomsByProductId(paginationParameter, productId));
        }
        
        [HttpGet("part-symptom/{partSymptomId}")]
        public Task<IActionResult> GetProductSymptomsByPartSymptomId([FromQuery] PaginationParameter paginationParameter, int partSymptomId)
        {
            return ValidateAndExecute(async () => await _productSymptomService.GetProductSymptomsByPartSymptomId(paginationParameter, partSymptomId));
        }
        
        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteProductSymptomById(int id)
        {
            return ValidateAndExecute(async () => await _productSymptomService.DeleteProductSymptomById(id));
        }
        
        [HttpPut]
        public Task<IActionResult> UpdateProductSymptom([FromBody] UpdateProductSymptomModel productSymptom)
        {
            return ValidateAndExecute(async () => await _productSymptomService.UpdateProductSymptom(productSymptom));
        }
        
        [HttpPost]
        public Task<IActionResult> CreateProductSymptom([FromBody] CreateProductSymptomModel productSymptom)
        {
            return ValidateAndExecute(async () => await _productSymptomService.CreateProductSymptom(productSymptom));
        }

        [HttpPost("bulk")]
        public Task<IActionResult> CreateMultiProductSymptom([FromBody] CreateProductSymptomMutilModel multiModel)
        {
            return ValidateAndExecute(async () => await _productSymptomService.CreateMultiProductSymptom(multiModel));
        }
    }
}
