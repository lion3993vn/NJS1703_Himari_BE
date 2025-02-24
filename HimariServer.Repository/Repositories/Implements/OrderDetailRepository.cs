using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;

namespace HimariServer.Repository.Repositories.Implements
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly HimariServerContext _context;

        public OrderDetailRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }
    }
}
