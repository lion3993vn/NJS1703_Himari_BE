using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Repositories.Interfaces
{
    public interface IUserDeviceRepository : IGenericRepository<UserDevice>
    {
        Task<UserDevice> GetByTokenDevice(string token);
        Task<List<UserDevice>> GetUserDeviceByUserId(int userId);
    }
}
