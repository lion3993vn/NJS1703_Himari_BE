using HimariServer.Repository.Commons;
using HimariServer.Service.BusinessModels.NotificationModels;
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
        public Task<BaseResponseModel> PushNotificationByUserId(NotificationRequestModel model);
        public Task<BaseResponseModel> GetNotificationById(int id);
        public Task<BaseResponseModel> GetNotificationsByUserId(PaginationParameter paginationParameter, int userId, int type);
        public Task<BaseResponseModel> GetUnreadNotificationCount(int userId);
        public Task<BaseResponseModel> MarkNotificationAsRead(int notificationId);
        public Task<BaseResponseModel> MarkAllNotificationsAsRead(int userId);
    }
}
