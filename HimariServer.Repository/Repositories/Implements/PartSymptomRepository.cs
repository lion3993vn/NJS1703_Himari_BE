using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;

namespace HimariServer.Repository.Repositories.Implements
{
    public class PartSymptomRepository : GenericRepository<PartSymptom>, IPartSymptomRepository
    {
        private readonly HimariServerContext _context;

        public PartSymptomRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }
    }
}
