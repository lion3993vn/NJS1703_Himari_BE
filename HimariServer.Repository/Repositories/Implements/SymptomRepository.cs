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
        public List<KeyValuePair<string, string>> GetBodyPartSymptomPairs()
        {
            var result = _context.PartSymptoms
                .Where(ps => ps.BodyPart != null) 
                .Select(ps => new KeyValuePair<string, string>(ps.BodyPart.BodyPartName, ps.Name))
                .ToList();

            return result;
        }
    }
}
