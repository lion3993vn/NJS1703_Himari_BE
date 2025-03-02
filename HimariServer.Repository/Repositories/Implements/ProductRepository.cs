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
        public async Task<List<int>> GetProductIdsByPartSymptomAsync(string partSymptomName)
        {
            return await _context.ProductSymptoms
                .Where(ps => ps.PartSymptom.Name == partSymptomName && !ps.IsDeleted)
                .Select(ps => ps.ProductId.Value)
                .ToListAsync();
        }
    }
}
