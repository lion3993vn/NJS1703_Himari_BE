using HimariServer.Repository.DBContext;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using HimariServer.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HimariServer.Repository.Repositories.Implements
{
    public class PartSymptomRepository : GenericRepository<PartSymptom>, IPartSymptomRepository
    {
        private readonly HimariServerContext _context;

        public PartSymptomRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> IsContainPartSymptom(int bodyPartId)
        {
            return await _context.PartSymptoms.AnyAsync(x => x.BodyPartId == bodyPartId && !x.IsDeleted);
        }
    }
}
