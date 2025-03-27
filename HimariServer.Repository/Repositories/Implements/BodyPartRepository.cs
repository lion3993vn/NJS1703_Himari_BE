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
    public class BodyPartRepository : GenericRepository<BodyPart>, IBodyPartRepository
    {
        private readonly HimariServerContext _context;

        public BodyPartRepository(HimariServerContext context) : base(context)
        {
            _context = context;
        }

    }
}
