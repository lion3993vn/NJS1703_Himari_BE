using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HimariServer.Repository.Repositories.Implements
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly HimariServerContext _context;

        public ProductRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsContainProduct(int categoryId)
        {
            return await _context.Products.AnyAsync(x => x.CategoryId == categoryId && !x.IsDeleted);
        }

        public async Task<List<Product>> GetAllProduct()
        {
            return await _context.Products.Include(x => x.Brand)
                                                  .Include(x => x.ProductSymptoms)
                                                  .ThenInclude(x => x.PartSymptom)
                                                  .ThenInclude(x => x.BodyPart)
                                                  .Where(x => !x.IsDeleted)
                                                  .ToListAsync();
        }
    }
}
