using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Repositories.Interfaces
{
    public interface IUserNotificationRepository : IGenericRepository<UserNotification>
    {
        public Task<int> GetUnreadNotificationCount(int userId);
        public Task<List<UserNotification>> GetUnreadNotificationByUserId(int userId);
    }
}
