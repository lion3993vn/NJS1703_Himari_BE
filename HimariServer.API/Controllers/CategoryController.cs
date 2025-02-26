using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.CategoryModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public Task<IActionResult> GetCategories([FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _categoryService.GetCategoriesPaginationAsync(paginationParameter));
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetCategoryById(int id)
        {
            return ValidateAndExecute(async () => await _categoryService.GetCategoryByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteCategoryById(int id)
        {
            return ValidateAndExecute(async () => await _categoryService.DeleteCategoryByIdAsync(id));
        }

        [HttpPut]
        public Task<IActionResult> UpdateCategoryById([FromBody] CategoryUpdateModel model)
        {
            return ValidateAndExecute(async () => await _categoryService.UpdateCategory(model));
        }

        [HttpPost]
        public Task<IActionResult> CreateCategory(AddCategoryModel model)
        {
            return ValidateAndExecute(async () => await _categoryService.CreateCategory(model));
        }

        [HttpGet("parent")]
        public Task<IActionResult> GetParentCategories([FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _categoryService.GetParentCategoriesPaginationAsync(paginationParameter));
        }

        [HttpGet("parent/{parentId}/subcategories")]
        public Task<IActionResult> GetSubCategoriesByParentId(int parentId, [FromQuery] PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _categoryService.GetSubCategoriesByParentIdPaginationAsync(parentId, paginationParameter));
        }
    }
}
