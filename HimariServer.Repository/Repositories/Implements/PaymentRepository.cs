using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;
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
    }
}
