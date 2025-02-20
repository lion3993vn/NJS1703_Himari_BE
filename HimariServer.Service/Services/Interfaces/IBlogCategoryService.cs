using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.BlogCategoryModels;
using HimariServer.Service.BusinessModels.BlogModels;
using HimariServer.Service.BusinessModels.ResultModels;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IBlogCategoryService
    {
        Task<BaseResponseModel> DeleteBlogCategoryById(int id);
        Task<BaseResponseModel> GetBlogCategoryById(int id);
        Task<BaseResponseModel> GetBlogsCategoryPaginationAsync(PaginationParameter paginationParameter);
        Task<BaseResponseModel> UpdateBlogCategory(UpdateBlogCategoryModel blog);
        Task<BaseResponseModel> AddBlogCategory(AddBlogCategoryModel blog);
    }
}
