using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;

namespace HimariServer.Repository.Repositories.Implements
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly HimariServerContext _context;

        public BrandRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }
    }
}
