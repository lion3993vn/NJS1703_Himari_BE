using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Repositories.Interfaces
{
    public interface ISymptomRepository : IGenericRepository<PartSymptom>
    {
        List<KeyValuePair<string, string>> GetBodyPartSymptomPairs();
    }
}
