using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Service.BusinessModels.BlogModels;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.API.Controllers
{
    [Route("api/v1/blogs")] 
    [ApiController]
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
           _blogService = blogService;
        }
        [HttpGet]
        public Task<IActionResult> GetBlogs(
            [FromQuery] PaginationParameter paginationParameter, 
            [FromQuery] int? blogCategoryId = null , 
            [FromQuery] bool newestFirst = true, 
            [FromQuery] string? searchTerm = null)
        {
            return ValidateAndExecute(async () => await _blogService.GetBlogsPaginationAsync(paginationParameter, blogCategoryId,newestFirst, searchTerm));
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetBlogById(int id)
        {
            return ValidateAndExecute(async () => await _blogService.GetBlogById(id));
        }

        [Authorize(Roles = "3,4")]
        [HttpPost]
        public Task<IActionResult> CreateBlog([FromBody] AddBlogModel blog)
        {
            return ValidateAndExecute(async () => await _blogService.AddBlog(blog));

        }

        [Authorize(Roles = "3,4")]
        [HttpPut]
        public Task<IActionResult> UpdateBlog([FromBody] UpdateBlogModel blog)
        {
            return ValidateAndExecute(async () => await _blogService.UpdateBlog(blog));
        }

        [Authorize(Roles = "3,4")]
        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteBlogById(int id)
        {
            return ValidateAndExecute(async () => await _blogService.DeleteBlogById(id));
        }
     
    }
}
