using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Repositories.Implements
{
    public class UserNotificationRepository : GenericRepository<UserNotification>, IUserNotificationRepository
    {
        private readonly HimariServerContext _context;

        public UserNotificationRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserNotification>> GetUnreadNotificationByUserId(int userId)
        {
            return await _context.UserNotifications.Where(x => x.UserId == userId && !x.IsRead && !x.IsDeleted).ToListAsync();
        }

        public async Task<int> GetUnreadNotificationCount(int userId)
        {
            return await _context.UserNotifications.CountAsync(x => x.UserId == userId && !x.IsRead && !x.IsDeleted);
        }
    }
}
