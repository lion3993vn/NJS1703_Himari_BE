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
    public class UserDeviceRepository : GenericRepository<UserDevice>, IUserDeviceRepository
    {
        private readonly HimariServerContext _context;

        public UserDeviceRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserDevice>> GetAllWithUser()
        {
            return await _context.UserDevices.Include(x => x.User).Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<UserDevice?> GetByTokenDevice(string token)
        {
            return await _context.UserDevices.Where(x => x.DeviceToken == token && !x.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<List<UserDevice>> GetUserDeviceByUserId(int userId)
        {
            return await _context.UserDevices.Where(x => x.UserId == userId && !x.IsDeleted).ToListAsync();
        }
    }
}
