using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.BrandModels;
using HimariServer.Service.BusinessModels.ResultModels;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IBrandService
    {
        Task<BaseResponseModel> CreateBrand(CreateBrandModel brand);
        Task<BaseResponseModel> DeleteBrandById(int id);
        Task<BaseResponseModel> GetBrandById(int id);
        Task<BaseResponseModel> GetBrandsPaginationAsync(PaginationParameter paginationParameter, bool newestFirst, string? searchTerm);
        Task<BaseResponseModel> UpdateBrand(BrandModel brand);
    }
}
