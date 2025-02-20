using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;

namespace HimariServer.Repository.Repositories.Implements
{
    public class SymptomRepository : GenericRepository<PartSymptom>, ISymptomRepository
    {
        private readonly HimariServerContext _context;

        public SymptomRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }
    }
}
