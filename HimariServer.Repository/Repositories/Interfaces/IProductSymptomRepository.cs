using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Repositories.Interfaces
{
    public interface IProductSymptomRepository : IGenericRepository<ProductSymptom>
    {
        public Task<ProductSymptom?> FindByPartSymptomAndProduct(int partSymptomId, int productId);
    }
}
