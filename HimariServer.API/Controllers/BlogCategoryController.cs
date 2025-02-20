using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.BlogCategoryModels;
using HimariServer.Service.BusinessModels.BlogModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/blog-categories")]
    [ApiController]
    public class BlogCategoryController : BaseController
    {
        private readonly IBlogCategoryService _blogCategoryService;

        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
        }

        [HttpGet]
        public Task<IActionResult> GetBlogCategories(PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _blogCategoryService.GetBlogsCategoryPaginationAsync(paginationParameter));
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetBlogCategoryById(int id)
        {
            return ValidateAndExecute(async () => await _blogCategoryService.GetBlogCategoryById(id));
        }

        [HttpPost]
        public Task<IActionResult> CreateBlogCategory([FromBody] AddBlogCategoryModel blog)
        {
            return ValidateAndExecute(async () => await _blogCategoryService.AddBlogCategory(blog));

        }

        [HttpPut]
        public Task<IActionResult> UpdateBlog([FromBody] UpdateBlogCategoryModel blog)
        {
            return ValidateAndExecute(async () => await _blogCategoryService.UpdateBlogCategory(blog));
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteBlogById(int id)
        {
            return ValidateAndExecute(async () => await _blogCategoryService.DeleteBlogCategoryById(id));
        }
    }
}
