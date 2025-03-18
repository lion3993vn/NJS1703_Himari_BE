using HimariServer.Repository.Commons;
using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
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
            return await _context.Orders.Include(o => o.OrderDetails)
                                      .ThenInclude(od => od.Product)
                                      .Include(o => o.Payments)
                                      .Include(x => x.User)
                                      .Where(x => x.OrderCode == orderCode && !x.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalOrder(int? month, int? year)
        {
            return await _context.Orders.Where(x => !x.IsDeleted && (!year.HasValue || x.CreatedDate.Year == year) && (!month.HasValue || x.CreatedDate.Month == month)).CountAsync();
        }
        public async Task<int> GetNotStartedOrder(int? month, int? year)
        {
            return await _context.Orders.Include(o => o.Payments).Where(x => !x.IsDeleted && x.DeliveryStatus == DeliveryStatus.NotStarted && x.Payments.FirstOrDefault().Status != PaymentStatus.Failed && (!year.HasValue || x.CreatedDate.Year == year) && (!month.HasValue || x.CreatedDate.Month == month)).CountAsync();
        }
        public async Task<int> GetOrderByDeliveryStatus(int? month, int? year, DeliveryStatus status)
        {
            return await _context.Orders.Where(x => !x.IsDeleted && x.DeliveryStatus == status && (!year.HasValue || x.CreatedDate.Year == year) && (!month.HasValue || x.CreatedDate.Month == month)).CountAsync();
        }        
    }
}
