using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;

namespace HimariServer.Repository.Repositories.Implements
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly HimariServerContext _context;

        public ProductRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }
    }
}
