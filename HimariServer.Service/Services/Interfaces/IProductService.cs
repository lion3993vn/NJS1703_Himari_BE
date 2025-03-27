using HimariServer.Repository.Commons;
using HimariServer.Repository.Enums;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.BusinessModels.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IProductService
    {
        Task<BaseResponseModel> CreateProduct(CreateProductModel product);
        Task<BaseResponseModel> DeleteProductById(int id);
        Task<BaseResponseModel> GetFeaturedProducts(PaginationParameter paginationParameter);
        Task<BaseResponseModel> GetProductById(int id);
        Task<BaseResponseModel> GetProductsByCategory(PaginationParameter paginationParameter, int categoryId, ProductSortOption sortOption = ProductSortOption.Newest);
        Task<BaseResponseModel> GetProductsPaginationAsync(PaginationParameter paginationParameter, ProductSortOption sortOption = ProductSortOption.Newest);
        Task<BaseResponseModel> UpdateProduct(UpdateProductModel product);
        Task<BaseResponseModel> GetProductsByBrand(PaginationParameter paginationParameter, int brandId);
        Task<BaseResponseModel> SearchProductsByKeyword(PaginationParameter paginationParameter, string? keyword);
    }
}
