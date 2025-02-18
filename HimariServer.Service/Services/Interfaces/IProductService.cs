using HimariServer.Repository.Commons;
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
        Task<BaseResponseModel> GetProductById(int id);
        Task<BaseResponseModel> GetProductsPaginationAsync(PaginationParameter paginationParameter);
        Task<BaseResponseModel> UpdateProduct(UpdateProductModel product);
    }
}
