using HimariServer.Service.BusinessModels.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface INotificationService
    {
        public Task<bool> PushMessageFirebase(string title, string body, int userId);
        public Task<bool> PushListMessageFirebase(string title, string body, List<string> fcmTokens);
        public Task<BaseResponseModel> GetNotificationById(int id);
    }
}
