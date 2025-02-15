using HimariServer.Repository.Commons;
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
        public Task<IActionResult> GetCategories(PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _categoryService.GetCategoriesPaginationAsync(paginationParameter));
        }
    }
}
