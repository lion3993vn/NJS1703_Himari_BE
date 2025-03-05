using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.ResultModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<BaseResponseModel> UploadImageAsync(IFormFile file);
    }
}
