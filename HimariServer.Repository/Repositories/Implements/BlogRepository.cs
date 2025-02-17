using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;

namespace HimariServer.Repository.Repositories.Implements
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        private readonly HimariServerContext _context;

        public BlogRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }
    }
}
