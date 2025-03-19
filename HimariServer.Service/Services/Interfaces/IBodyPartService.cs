using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.BodyPartModels;
using HimariServer.Service.BusinessModels.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IBodyPartService
    {
        public Task<BaseResponseModel> GetBodyPartsPaginationAsync(PaginationParameter paginationParameter, bool newestFirst, string? searchTerm);
        public Task<BaseResponseModel> GetBodyPartByIdAsync(int id);
        public Task<BaseResponseModel> DeleteBodyPartByIdAsync(int id);
        public Task<BaseResponseModel> AddBodyPart(AddBodyPartModel model);
        public Task<BaseResponseModel> UpdateBodyPart(UpdateBodyPartModel model);
    }
}
