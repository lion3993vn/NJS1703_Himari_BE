using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public Task<IActionResult> GetProducts([FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _productService.GetProductsPaginationAsync(paginationParameter));
        }
        [HttpGet("category/{categoryId}")]
        public Task<IActionResult> GetProductsByCategory([FromQuery] PaginationParameter paginationParameter, int categoryId)
        {
            return ValidateAndExecute(async () => await _productService.GetProductsByCategory(paginationParameter, categoryId));
        }
        [HttpGet("featured")]
        public Task<IActionResult> GetFeaturedProducts([FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _productService.GetFeaturedProducts(paginationParameter));
        }
        [HttpGet("{id}")]
        public Task<IActionResult> GetProductById(int id)
        {
            return ValidateAndExecute(async () => await _productService.GetProductById(id));
        }

        [HttpGet("brand/{brandId}")]
        public Task<IActionResult> GetProductsByBrand([FromQuery] PaginationParameter paginationParameter, int brandId)
        {
            return ValidateAndExecute(async () => await _productService.GetProductsByBrand(paginationParameter, brandId));
        }

        [HttpGet("search")]
        public Task<IActionResult> SearchProducts([FromQuery] PaginationParameter paginationParameter, [FromQuery] string keyword)
        {
            return ValidateAndExecute(async () => await _productService.SearchProductsByKeyword(paginationParameter, keyword));
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteProductById(int id)
        {
            return ValidateAndExecute(async () => await _productService.DeleteProductById(id));
        }
        [HttpPut]
        public Task<IActionResult> UpdateProduct([FromBody] UpdateProductModel product)
        {
            return ValidateAndExecute(async () => await _productService.UpdateProduct(product));
        }
        [HttpPost]
        public Task<IActionResult> CreateProduct([FromBody] CreateProductModel product)
        {
            return ValidateAndExecute(async () => await _productService.CreateProduct(product));
        }
    }
}
