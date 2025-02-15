using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<BaseResponseModel> GetCategoriesPaginationAsync(PaginationParameter paginationParameter);
    }
}
