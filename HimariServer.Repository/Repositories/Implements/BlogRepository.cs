using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HimariServer.Repository.Repositories.Implements
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        private readonly HimariServerContext _context;

        public BlogRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsContainBlog(int blogCategoryId)
        {
            return await _context.Blogs.AnyAsync(x => x.BlogCategoryId == blogCategoryId && !x.IsDeleted);
        }
    }
}
