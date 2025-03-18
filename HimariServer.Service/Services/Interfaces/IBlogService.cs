using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.BlogModels;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.BusinessModels.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IBlogService
    {
        Task<BaseResponseModel> DeleteBlogById(int id);
        Task<BaseResponseModel> GetBlogById(int id);
        Task<BaseResponseModel> GetBlogsPaginationAsync(PaginationParameter paginationParameter,
            int? blogCategoryId,
            bool newestFirst,
            string? searchTerm);
        Task<BaseResponseModel> UpdateBlog(UpdateBlogModel blog);
        Task<BaseResponseModel> AddBlog(AddBlogModel blog);

    }
}
