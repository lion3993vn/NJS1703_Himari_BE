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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly HimariServerContext _context;

        public CategoryRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetSubCategories(int parentCategoryId)
        {
            return await _context.Categories.Where(x => x.ParentCategoryId == parentCategoryId).ToListAsync();
        }
    }
}
