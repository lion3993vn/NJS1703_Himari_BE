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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly HimariServerContext _context;
        public OrderRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderByCodeAsync(int orderCode)
        {
            return await _context.Orders.Where(x => x.OrderCode == orderCode && !x.IsDeleted).FirstOrDefaultAsync();
        }
    }
}
