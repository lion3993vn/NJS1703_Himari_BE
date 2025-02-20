using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.SymptomModels;
using HimariServer.Service.BusinessModels.ResultModels;

namespace HimariServer.Service.Services.Interfaces
{
    public interface ISymptomService
    {
        Task<BaseResponseModel> CreateSymptom(CreateSymptomModel symptom);
        Task<BaseResponseModel> DeleteSymptomById(int id);
        Task<BaseResponseModel> GetSymptomById(int id);
        Task<BaseResponseModel> GetSymptomsPaginationAsync(PaginationParameter paginationParameter);
        Task<BaseResponseModel> UpdateSymptom(SymptomModel symptom);
    }
}
