using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.ProductSymptomModels;
using HimariServer.Service.BusinessModels.ResultModels;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IProductSymptomService
    {
        Task<BaseResponseModel> GetProductSymptomsPaginationAsync(PaginationParameter paginationParameter);
        Task<BaseResponseModel> GetProductSymptomById(int id);
        Task<BaseResponseModel> CreateProductSymptom(CreateProductSymptomModel productSymptom);
        Task<BaseResponseModel> UpdateProductSymptom(UpdateProductSymptomModel productSymptom);
        Task<BaseResponseModel> DeleteProductSymptomById(int id);
        Task<BaseResponseModel> GetProductSymptomsByProductId(PaginationParameter paginationParameter, int productId);
        Task<BaseResponseModel> GetProductSymptomsByPartSymptomId(PaginationParameter paginationParameter, int partSymptomId);
        Task<BaseResponseModel> CreateMultiProductSymptom(CreateProductSymptomMutilModel multiModel);
    }
}
