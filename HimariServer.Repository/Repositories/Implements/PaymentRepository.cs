using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
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
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly HimariServerContext _context;

        public PaymentRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments.FirstOrDefaultAsync(x => x.OrderId == orderId && !x.IsDeleted);
        }

        public async Task<List<Payment>?> GetPaymentPending()
        {
            return await _context.Payments.Include(x => x.Order).Where(x => x.Status == PaymentStatus.Pending && !x.IsDeleted).ToListAsync();
        }
    }
}
