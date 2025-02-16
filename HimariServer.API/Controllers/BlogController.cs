using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Service.BusinessModels.BlogModels;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/blogs")] 
    [ApiController]
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
           _blogService = blogService;
        }
        [HttpGet]
        public Task<IActionResult> GetBlogs(PaginationParameter paginationParameter)
        {
            return ValidateAndExecute(async () => await _blogService.GetBlogsPaginationAsync(paginationParameter));
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetBlogById(int id)
        {
            return ValidateAndExecute(async () => await _blogService.GetBlogById(id));
        }

        [HttpPost]
        public Task<IActionResult> CreateBlog([FromBody] AddBlogModel blog)
        {
            return ValidateAndExecute(async () => await _blogService.AddBlog(blog));

        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateBlog(int id, [FromBody] UpdateBlogModel blog)
        {
            return ValidateAndExecute(async () => await _blogService.UpdateBlog(id, blog));
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteBlogById(int id)
        {
            return ValidateAndExecute(async () => await _blogService.DeleteBlogById(id));
        }
     
    }
}
