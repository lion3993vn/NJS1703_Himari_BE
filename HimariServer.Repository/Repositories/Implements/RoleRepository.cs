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
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly HimariServerContext _context;

        public RoleRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Role?> GetByRoleName(string role)
        {
            return await _context.Roles.Where(x => x.RoleName == "USER").FirstOrDefaultAsync();
        }
    }
}
