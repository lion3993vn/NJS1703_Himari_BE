using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.PartSymptomModels;
using HimariServer.Service.BusinessModels.ResultModels;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IPartSymptomService
    {
        Task<BaseResponseModel> CreatePartSymptom(CreatePartSymptomModel model);
        Task<BaseResponseModel> GetPartSymptomById(int id);
        Task<BaseResponseModel> UpdatePartSymptom(PartSymptomModel model);
        Task<BaseResponseModel> DeletePartSymptomById(int id);
        Task<BaseResponseModel> GetPartSymptomsPaginationAsync(PaginationParameter paginationParameter);
    }
}
