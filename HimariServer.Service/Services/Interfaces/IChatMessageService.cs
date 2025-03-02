using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.ResultModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HimariServer.Service.BusinessModels.ChatMessageModels;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task<BaseResponseModel> GetMessageChatByUserIdPaginated(int userId, PaginationParameter paginationParameter);
        Task<BaseResponseModel> GetProductRecommendationAsync(ChatRequestModel chatRequest);
    }
}
